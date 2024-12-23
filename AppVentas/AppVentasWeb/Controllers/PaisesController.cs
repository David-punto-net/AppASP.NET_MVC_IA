using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using System.Diagnostics.Metrics;
using AppVentasWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace AppVentasWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaisesController : Controller
    {
        private readonly DataContex _context;

        public PaisesController(DataContex context)
        {
            _context = context;
        }

      
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paises
                .Include(c => c.Regiones)
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

     
        public async Task<IActionResult> DetailsCiudad(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comuna = await _context.Ciudades
                .Include(r => r.Comuna)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (comuna == null)
            {
                return NotFound();
            }

            return View(comuna);
        }

  
        [HttpGet]
        public IActionResult Create()
        {
            Pais pais = new()
            {
                Regiones = new List<Region>()
            };

            return View(pais);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pais pais)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pais);

                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(pais);
        }

 
        [HttpGet]
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
                    return RedirectToAction(nameof(Details), new { Id = model.PaisId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una región con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(model);
        }

       
        [HttpGet]
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
                    return RedirectToAction(nameof(DetailsRegion), new { Id = model.RegionId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Comuna con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(model);
        }

     
        [HttpGet]
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
                    return RedirectToAction(nameof(DetailsComuna), new { Id = model.ComunaId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Ciuad con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(model);
        }

      
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pais = await _context.Paises
                       .Include(c => c.Regiones)
                       .FirstOrDefaultAsync(m => m.Id == id);

            if (pais == null)
            {
                return NotFound();
            }
            return View(pais);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pais pais)
        {
            if (id != pais.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pais);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(pais);
        }

       
        [HttpGet]
        public async Task<IActionResult> EditRegion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var region = await _context.Regiones.FindAsync(id);
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
                    Region region = new()
                    {
                        Id = model.Id,
                        Nombre = model.Nombre,
                        Pais = await _context.Paises.FindAsync(model.PaisId),
                        Comunas = new List<Comuna>()
                    };

                    _context.Update(region);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Details), new { Id = model.PaisId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Región con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(model);
        }

     
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
                    Comuna comuna = new()
                    {
                        Id = model.Id,
                        Nombre = model.Nombre,
                        Region = await _context.Regiones.FindAsync(model.RegionId),
                        Ciudades = new List<Ciudad>()
                    };

                    _context.Update(comuna);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(DetailsRegion), new { Id = model.RegionId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Comuna con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(model);
        }

       
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
                    Ciudad ciudad = new()
                    {
                        Id = model.Id,
                        Nombre = model.Nombre,
                        Comuna = await _context.Comunas.FindAsync(model.ComunaId),
                    };

                    _context.Update(ciudad);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(DetailsComuna), new { Id = model.ComunaId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Ciudad con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(model);
        }

   
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pais = await _context.Paises
                .Include(c => c.Regiones)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pais == null)
            {
                return NotFound();
            }

            return View(pais);
        }

      
        [HttpGet]
        public async Task<IActionResult> DeleteRegion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.Regiones
                     //.Include(c => c.Comunas)
                     .Include(c => c.Pais)
                     .FirstOrDefaultAsync(m => m.Id == id);

            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

       
        public async Task<IActionResult> DeleteComuna(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comuna = await _context.Comunas
                     .Include(c => c.Region)
                     .FirstOrDefaultAsync(m => m.Id == id);

            if (comuna == null)
            {
                return NotFound();
            }

            return View(comuna);
        }

       
        public async Task<IActionResult> DeleteCiudad(int? id)
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

            return View(ciudad);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pais = await _context.Paises.FindAsync(id);
            if (pais != null)
            {
                _context.Paises.Remove(pais);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
        [HttpPost, ActionName("DeleteRegion")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRegionConfirmed(int id)
        {
        
            var region = await _context.Regiones
                     .Include(c => c.Pais)
                     .FirstOrDefaultAsync(m => m.Id == id);

            if (region != null)
            {
                _context.Regiones.Remove(region);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { Id = region.Pais.Id });
        }

        
        [HttpPost, ActionName("DeleteComuna")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteComunaConfirmed(int id)
        {

            var comuna = await _context.Comunas
                     .Include(c => c.Region)
                     .FirstOrDefaultAsync(m => m.Id == id);

            if (comuna != null)
            {
                _context.Comunas.Remove(comuna);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsRegion), new { Id = comuna.Region.Id });
        }

        [HttpPost, ActionName("DeleteCiudad")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCiudadConfirmed(int id)
        {

            var ciudad = await _context.Ciudades
                     .Include(c => c.Comuna)
                     .FirstOrDefaultAsync(m => m.Id == id);

            if (ciudad != null)
            {
                _context.Ciudades.Remove(ciudad);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsComuna), new { Id = ciudad.Comuna.Id });
        }


    }
}