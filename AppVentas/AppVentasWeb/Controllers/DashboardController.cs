using AppVentasWeb.Data;
using AppVentasWeb.Enum;
using AppVentasWeb.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppVentasWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly DataContex _context;
        private readonly IUserHelper _userHelper;

        public DashboardController(DataContex context, IUserHelper userHelper, IAzureOpenAIClientHelper azureOpenAIClientHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.UsersCount = _context.Users.Count();
            ViewBag.ProductsCount = _context.Productos.Count();
            ViewBag.NewOrdersCount = _context.Sales.Where(o => o.OrderStatus == OrderStatus.Nuevo).Count();
            ViewBag.ConfirmedOrdersCount = _context.Sales.Where(o => o.OrderStatus == OrderStatus.Confirmado).Count();

            return View(await _context.TemporalSales
                                    .Include(u => u.User)
                                    .Include(p => p.Producto).ToListAsync());
        }

    }
}





