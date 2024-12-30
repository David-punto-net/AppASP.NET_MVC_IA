using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
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

        public HomeController(ILogger<HomeController> logger, DataContex context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Producto>? productos = await _context.Productos
            .Include(p => p.ProductImages)
            .Include(p => p.ProductCategories)
            .OrderBy(p => p.Description)
            .ToListAsync();

            List<ProductsHomeViewModel> productsHome = new() { new ProductsHomeViewModel() };
            int i = 1;
            foreach (Producto? producto in productos)
            {
                if (i == 1)
                {
                    productsHome.LastOrDefault().Product1 = producto;
                }
                if (i == 2)
                {
                    productsHome.LastOrDefault().Product2 = producto;
                }
                if (i == 3)
                {
                    productsHome.LastOrDefault().Product3 = producto;
                }
                if (i == 4)
                {
                    productsHome.LastOrDefault().Product4 = producto;
                    productsHome.Add(new ProductsHomeViewModel());
                    i = 0;
                }
                i++;
            }
            return View(productsHome);
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
    }
}