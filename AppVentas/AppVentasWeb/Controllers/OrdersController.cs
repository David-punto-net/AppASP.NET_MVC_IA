using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppVentasWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly DataContex _context;

        public OrdersController(DataContex context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Sales
            .Include(s => s.User)
            .Include(s => s.SaleDetails)
            .ThenInclude(sd => sd.Producto)
            .ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Sale sale = await _context.Sales
            .Include(s => s.User)
            .Include(s => s.SaleDetails)
            .ThenInclude(sd => sd.Producto)
            .ThenInclude(p => p.ProductImages)
            .FirstOrDefaultAsync(s => s.Id == id);
            if (sale == null)
            {
                return NotFound();
            }
            return View(sale);
        }
    }
}