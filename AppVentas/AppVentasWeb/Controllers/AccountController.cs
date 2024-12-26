using AppVentasWeb.Common;
using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Enum;
using AppVentasWeb.Helper;
using AppVentasWeb.Migrations;
using AppVentasWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Security.Policy;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace AppVentasWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContex _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IMailHelper _mailHelper;

        public AccountController(IUserHelper userHelper, DataContex context, ICombosHelper combosHelper,
            IBlobHelper blobHelper, IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _mailHelper = mailHelper;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Ha superado el máximo número de intentos, su cuenta está bloqueada, intente de nuevo en 5 minutos.");
                }
                else if (result.IsNotAllowed)
                {
                    ModelState.AddModelError(string.Empty, "El usuario no ha sido habilitado, debes de seguir las instrucciones del correo enviado para poder habilitarte en elSistema.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NotAuthorized()
        {
            return View();
        }

        public async Task<IActionResult> Register()
        {
            var userType = UserType.User;

            if (User.IsInRole(UserType.Admin.ToString()) && User.Identity.IsAuthenticated)
            {
                userType = UserType.Admin;
            }

            AddUserViewModel model = new()
            {
                Id = Guid.Empty.ToString(),
                Paises = await _combosHelper.GetComboPaisesAsync(),
                Regiones = await _combosHelper.GetComboRegionesAsync(0),
                Comunas = await _combosHelper.GetComboComunasAsync(0),
                Ciudades = await _combosHelper.GetComboCiudadesAsync(0),

                UserType = userType //UserType.User
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }
                model.ImageId = imageId;

                User user = await _userHelper.AddUserAsync(model);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Este correo ya está siendo usado.");
                    model.Paises = await _combosHelper.GetComboPaisesAsync();
                    model.Regiones = await _combosHelper.GetComboRegionesAsync(model.PaisId);
                    model.Comunas = await _combosHelper.GetComboComunasAsync(model.RegionId);
                    model.Ciudades = await _combosHelper.GetComboCiudadesAsync(model.ComunaId);
                    return View(model);
                }

                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendMail(

                $"{model.Nombres} {model.Apellidos}",
                model.Username,
                "Shopping - Confirmación de Email",
                $"<h1>Shopping - Confirmación de Email</h1>" +
                $"Para habilitar el usuario por favor hacer click en el siguiente link: , " +
                $"<hr/><br/><p><a href = \"{tokenLink}\">Confirmar Email Aquí</a></p>");

                if (response.IsSuccess)
                {
                    //TODO: BORRAR --  string token  y await _userHelper  se debe hacer por el correo
                    string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    await _userHelper.ConfirmEmailAsync(user, token);
                    //*******************************************************************************

                    if (User.IsInRole(UserType.Admin.ToString()))
                    {
                        ViewBag.Message = "Las instrucciones para habilitar al Administrador han sido enviadas al correo.";
                        return View(model);
                    }
                    ViewBag.Message = "Las instrucciones para habilitar el usuario han sido enviadas al correo.";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, response.Message);
            }

            model.Paises = await _combosHelper.GetComboPaisesAsync();
            model.Regiones = await _combosHelper.GetComboRegionesAsync(model.PaisId);
            model.Comunas = await _combosHelper.GetComboComunasAsync(model.RegionId);
            model.Ciudades = await _combosHelper.GetComboCiudadesAsync(model.ComunaId);
            return View(model);
        }

        public async Task<JsonResult> GetRegionesAsync(int paisId)
        {
            var pais = await _context.Paises
            .Include(c => c.Regiones)
            .FirstOrDefaultAsync(c => c.Id == paisId);

            if (pais == null)
            {
                return null;
            }
            return Json(pais.Regiones.OrderBy(d => d.Nombre));
        }

        public async Task<JsonResult> GetComunasAsync(int regionId)
        {
            var regiones = await _context.Regiones
                          .Include(c => c.Comunas)
                           .FirstOrDefaultAsync(c => c.Id == regionId);

            if (regiones == null)
            {
                return null;
            }
            return Json(regiones.Comunas.OrderBy(d => d.Nombre));
        }

        public async Task<JsonResult> GetCiudadesAsync(int comunaId)
        {
            var comuna = await _context.Comunas
                     .Include(c => c.Ciudades)
                     .FirstOrDefaultAsync(m => m.Id == comunaId);

            if (comuna == null)
            {
                return null;
            }
            return Json(comuna.Ciudades.OrderBy(d => d.Nombre));
        }

        public async Task<IActionResult> ChangeUser()
        {
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }

            EditUserViewModel model = new()
            {
                Direccion = user.Direccion,
                Nombres = user.Nombres,
                Apellidos = user.Apellidos,
                PhoneNumber = user.PhoneNumber,
                ImageId = user.ImageId,

                Ciudades = await _combosHelper.GetComboCiudadesAsync(user.Ciudad.Comuna.Id),
                CiudadId = user.Ciudad.Id,

                Comunas = await _combosHelper.GetComboComunasAsync(user.Ciudad.Comuna.Region.Id),
                ComunaId = user.Ciudad.Comuna.Id,

                Regiones = await _combosHelper.GetComboRegionesAsync(user.Ciudad.Comuna.Region.Pais.Id),
                RegionId = user.Ciudad.Comuna.Region.Id,

                Paises = await _combosHelper.GetComboPaisesAsync(),
                PaisId = user.Ciudad.Comuna.Region.Pais.Id,

                Id = user.Id,
                Rut = user.Rut
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = model.ImageId;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                }
                User user = await _userHelper.GetUserAsync(User.Identity.Name);
                user.Nombres = model.Nombres;
                user.Apellidos = model.Apellidos;
                user.Direccion = model.Direccion;
                user.PhoneNumber = model.PhoneNumber;
                user.ImageId = imageId;
                user.Ciudad = await _context.Ciudades.FindAsync(model.CiudadId);
                user.Rut = model.Rut;

                await _userHelper.UpdateUserAsync(user);
                return RedirectToAction("Index", "Home");
            }

            model.Paises = await _combosHelper.GetComboPaisesAsync();
            model.Regiones = await _combosHelper.GetComboRegionesAsync(model.PaisId);
            model.Comunas = await _combosHelper.GetComboComunasAsync(model.RegionId);
            model.Ciudades = await _combosHelper.GetComboCiudadesAsync(model.ComunaId);
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.OldPassword == model.NewPassword)
                {
                    ModelState.AddModelError(string.Empty, "Debes ingresar una contraseña diferente");
                    return View(model);
                }

                User? user = await _userHelper.GetUserAsync(User.Identity.Name);
                if (user != null)
                {
                    IdentityResult? result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "OK";
                        return RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            User user = await _userHelper.GetUserAsync(new Guid(userId));

            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
            {
                return NotFound();
            }
            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "El email no corresponde a ningún usuario registrado.");
                    return View(model);
                }
                string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
                string link = Url.Action(
                "ResetPassword",
                "Account",
                new { token = myToken }, protocol: HttpContext.Request.Scheme);
                _mailHelper.SendMail(
                $"{user.FullName}",
                model.Email,
                "Shopping - Recuperación de Contraseña",
                $"<h1>Shopping - Recuperación de Contraseña</h1>" +
                $"Para recuperar la contraseña haga click en el siguiente enlace:" +
                $"<p><a href = \"{link}\">Reset Password</a></p>");

                ViewBag.Message = "Las instrucciones para recuperar la contraseña han sido enviadas a su correo.";
                return View();
            }
            return View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await _userHelper.GetUserAsync(model.UserName);
            if (user != null)
            {
                IdentityResult result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Contraseña cambiada con éxito.";
                    return View();
                }
                ViewBag.Message = "Error cambiando la contraseña.";
              
            return View(model);
            }
            ViewBag.Message = "Usuario no encontrado.";
            return View(model);
        }
    }
}