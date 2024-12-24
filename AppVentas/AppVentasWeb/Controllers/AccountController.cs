using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Enum;
using AppVentasWeb.Helper;
using AppVentasWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppVentasWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly DataContex _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;

        public AccountController(IUserHelper userHelper, DataContex context, ICombosHelper combosHelper, IBlobHelper blobHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
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
                Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
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
            AddUserViewModel model = new()
            {
                Id = Guid.Empty.ToString(),
                Paises = await _combosHelper.GetComboPaisesAsync(),
                Regiones = await _combosHelper.GetComboRegionesAsync(0),
                Comunas = await _combosHelper.GetComboComunasAsync(0),
                Ciudades = await _combosHelper.GetComboCiudadesAsync(0),
                UserType = UserType.User,
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
                    return View(model);
                }
                LoginViewModel loginViewModel = new()
                {
                    Password = model.Password,
                    RememberMe = false,
                    Username = model.Username
                };
                var result2 = await _userHelper.LoginAsync(loginViewModel);
                if (result2.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
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
    }
}