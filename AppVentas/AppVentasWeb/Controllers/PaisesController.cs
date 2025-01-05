using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Helper;
using AppVentasWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;
using static AppVentasWeb.Helper.ModalHelper;

namespace AppVentasWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaisesController : Controller
    {
        private readonly DataContex _context;
        private readonly IFlashMessage _flashMessage;

        public PaisesController(DataContex context, IFlashMessage flashMessage)
        {
            _context = context;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paises
                .Include(c => c.Regiones)
                .ThenInclude(s => s.Comunas)
                .ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pais = await _context.Paises
                .Include(c => c.Regiones)
                .ThenInclude(r => r.Comunas)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pais == null)
            {
                return NotFound();
            }

            return View(pais);
        }

        public async Task<IActionResult> DetailsRegion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Regiones
                .Include(r => r.Comunas)
                .ThenInclude(r => r.Ciudades)
                .Include(r => r.Pais)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        public async Task<IActionResult> DetailsComuna(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comuna = await _context.Comunas
                .Include(r => r.Ciudades)
                .Include(r => r.Region)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (comuna == null)
            {
                return NotFound();
            }

            return View(comuna);
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddRegion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Pais pais = await _context.Paises.FindAsync(id);

            if (pais == null)
            {
                return NotFound();
            }

            RegionViewModel model = new()
            {
                PaisId = pais.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRegion(RegionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Region region = new()
                    {
                        Nombre = model.Nombre,
                        Pais = await _context.Paises.FindAsync(model.PaisId),
                        Comunas = new List<Comuna>()
                    };

                    _context.Add(region);
                    await _context.SaveChangesAsync();

                    Pais pais = await _context.Paises
                                    .Include(c => c.Regiones)
                                    .ThenInclude(s => s.Comunas)
                                    .FirstOrDefaultAsync(c => c.Id == model.PaisId);

                    _flashMessage.Info("Registro creado.");

                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllRegiones", pais) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una región con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddRegion", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddComuna(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Region region = await _context.Regiones.FindAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            ComunaViewModel model = new()
            {
                RegionId = region.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComuna(ComunaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Comuna comuna = new()
                    {
                        Nombre = model.Nombre,
                        Region = await _context.Regiones.FindAsync(model.RegionId),
                        Ciudades = new List<Ciudad>()
                    };

                    _context.Add(comuna);
                    await _context.SaveChangesAsync();

                    Region region = await _context.Regiones
                                    .Include(c => c.Comunas)
                                    .ThenInclude(s => s.Ciudades)
                                    .FirstOrDefaultAsync(c => c.Id == model.RegionId);

                    _flashMessage.Info("Registro creado.");

                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllComunas", region) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una Comuna con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddComuna", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddCiudad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Comuna comuna = await _context.Comunas.FindAsync(id);

            if (comuna == null)
            {
                return NotFound();
            }

            CiudadViewModel model = new()
            {
                ComunaId = comuna.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCiudad(CiudadViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Ciudad ciudad = new()
                    {
                        Nombre = model.Nombre,
                        Comuna = await _context.Comunas.FindAsync(model.ComunaId),
                    };

                    _context.Add(ciudad);
                    await _context.SaveChangesAsync();

                    Comuna comuna = await _context.Comunas
                                 .Include(c => c.Ciudades)
                                 .FirstOrDefaultAsync(c => c.Id == model.ComunaId);

                    _flashMessage.Info("Registro creado.");

                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllCiudades", comuna) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una Ciudad con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddCiudad", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditRegion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Regiones
             .Include(c => c.Pais)
             .FirstOrDefaultAsync(m => m.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            RegionViewModel model = new()
            {
                Id = region.Id,
                Nombre = region.Nombre,
                PaisId = region.Pais.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRegion(int id, RegionViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Region region = await _context.Regiones.FindAsync(model.Id);
                    region.Nombre = model.Nombre;

                    _context.Update(region);

                    Pais pais = await _context.Paises
                    .Include(c => c.Regiones)
                    .ThenInclude(s => s.Comunas)
                    .FirstOrDefaultAsync(c => c.Id == model.PaisId);

                    await _context.SaveChangesAsync();

                    _flashMessage.Info("Registro actualizado.");

                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllRegiones", pais) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una Región con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditRegion", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditComuna(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comuna = await _context.Comunas
             .Include(c => c.Ciudades)
              .Include(c => c.Region)
             .FirstOrDefaultAsync(m => m.Id == id);

            if (comuna == null)
            {
                return NotFound();
            }

            ComunaViewModel model = new()
            {
                Id = comuna.Id,
                Nombre = comuna.Nombre,
                RegionId = comuna.Region.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComuna(int id, ComunaViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Comuna comuna = await _context.Comunas.FindAsync(model.Id);
                    comuna.Nombre = model.Nombre;

                    _context.Update(comuna);

                    Region region = await _context.Regiones
                    .Include(c => c.Comunas)
                    .ThenInclude(s => s.Ciudades)
                    .FirstOrDefaultAsync(c => c.Id == model.RegionId);

                    await _context.SaveChangesAsync();

                    _flashMessage.Info("Registro actualizado.");

                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllComunas", region) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una Comuna con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditComuna", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> EditCiudad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ciudad = await _context.Ciudades
             .Include(c => c.Comuna)
             .FirstOrDefaultAsync(m => m.Id == id);

            if (ciudad == null)
            {
                return NotFound();
            }

            CiudadViewModel model = new()
            {
                Id = ciudad.Id,
                Nombre = ciudad.Nombre,
                ComunaId = ciudad.Comuna.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCiudad(int id, CiudadViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Ciudad ciudad = await _context.Ciudades.FindAsync(model.Id);
                    ciudad.Nombre = model.Nombre;

                    _context.Update(ciudad);

                    Comuna comuna = await _context.Comunas
                                        .Include(c => c.Ciudades)
                                        .FirstOrDefaultAsync(c => c.Id == model.ComunaId);

                    await _context.SaveChangesAsync();

                    _flashMessage.Info("Registro actualizado.");

                    return Json(new { isValid = true, html = ModalHelper.RenderRazorViewToString(this, "_ViewAllCiudades", comuna) });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe una Ciudad con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "EditCiudad", model) });
        }

        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {
            Pais pais = await _context.Paises.FirstOrDefaultAsync(c => c.Id == id);
            if (pais == null)
            {
                return NotFound();
            }
            try
            {
                _context.Paises.Remove(pais);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar el país porque tiene registros relacionados.");
            }
            return RedirectToAction(nameof(Index));
        }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteRegion(int id)
        {
            Region region = await _context.Regiones
            .Include(s => s.Pais)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (region == null)
            {
                return NotFound();
            }
            try
            {
                _context.Regiones.Remove(region);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la Región porque tiene registros relacionados.");
            }
            return RedirectToAction(nameof(Details), new { id = region.Pais.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteComuna(int id)
        {
            Comuna comuna = await _context.Comunas
                     .Include(c => c.Region)
                     .FirstOrDefaultAsync(m => m.Id == id);

            if (comuna == null)
            {
                return NotFound();
            }

            try
            {
                _context.Comunas.Remove(comuna);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la Comuna porque tiene registros relacionados.");
            }
            return RedirectToAction(nameof(DetailsRegion), new { id = comuna.Region.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> DeleteCiudad(int id)
        {
            Ciudad ciudad = await _context.Ciudades
                     .Include(c => c.Comuna)
                     .FirstOrDefaultAsync(m => m.Id == id);

            if (ciudad == null)
            {
                return NotFound();
            }

            try
            {
                _context.Ciudades.Remove(ciudad);
                await _context.SaveChangesAsync();
                _flashMessage.Info("Registro borrado.");
            }
            catch
            {
                _flashMessage.Danger("No se puede borrar la Cioudad porque tiene registros relacionados.");
            }
            return RedirectToAction(nameof(DetailsComuna), new { id = ciudad.Comuna.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new Pais());
            }
            else
            {
                Pais pais = await _context.Paises.FindAsync(id);
                if (pais == null)
                {
                    return NotFound();
                }
                return View(pais);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, Pais country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id == 0) //Insert
                    {
                        _context.Add(country);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro creado.");
                    }
                    else //Update
                    {
                        _context.Update(country);
                        await _context.SaveChangesAsync();
                        _flashMessage.Info("Registro actualizado.");
                    }
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAll", _context.Paises.Include(c => c.Regiones).ThenInclude(s => s.Comunas).ToList())
                    });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger("Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(exception.Message);
                }
            }
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddOrEdit", country) });
        }
    }
}