using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;

namespace AppVentasWeb.Controllers
{
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
            return View(await _context.Paises.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pais = await _context.Paises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pais == null)
            {
                return NotFound();
            }

            return View(pais);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pais pais)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pais);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pais);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pais = await _context.Paises.FindAsync(id);
            if (pais == null)
            {
                return NotFound();
            }
            return View(pais);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Pais pais)
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
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaisExists(pais.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pais);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pais = await _context.Paises
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pais == null)
            {
                return NotFound();
            }

            return View(pais);
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

        private bool PaisExists(int id)
        {
            return _context.Paises.Any(e => e.Id == id);
        }
    }
}
