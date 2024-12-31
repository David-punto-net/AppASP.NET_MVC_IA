using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Helper;
using AppVentasWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AppVentasWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContex _context;
        private readonly IUserHelper _userHelper;

        public HomeController(ILogger<HomeController> logger, DataContex context, IUserHelper userHelper)
        {
            _logger = logger;
            _context = context;
            _userHelper= userHelper;
        }

        public async Task<IActionResult> Index()
        {
            List<Producto> products = await _context.Productos
            .Include(p => p.ProductImages)
            .Include(p => p.ProductCategories)
            .OrderBy(p => p.Description)
            .ToListAsync();
            HomeViewModel model = new() { Productos = products };
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user != null)
            {
                model.Quantity = await _context.TemporalSales
                                        .Where(ts => ts.User.Id == user.Id)
                                        .SumAsync(ts => ts.Quantity);
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        public async Task<IActionResult> Add(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            Producto producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            User user = await _userHelper.GetUserAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            TemporalSale temporalSale = new()
            {
                Producto = producto,
                Quantity = 1,
                User = user
            };
            _context.TemporalSales.Add(temporalSale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}