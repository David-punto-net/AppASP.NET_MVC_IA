using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Enum;
using AppVentasWeb.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

namespace AppVentasWeb.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DataContex _context;
        private readonly IFlashMessage _flashMessage;
        private readonly IOrdersHelper _ordersHelper;

        public OrdersController(DataContex context, IFlashMessage flashMessage, IOrdersHelper ordersHelper)
        {
            _context = context;
            _flashMessage = flashMessage;
            _ordersHelper = ordersHelper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sales
            .Include(s => s.User)
            .Include(s => s.SaleDetails)
            .ThenInclude(sd => sd.Producto)
            .ToListAsync());
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dispatch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            if (sale.OrderStatus != OrderStatus.Nuevo)
            {
                _flashMessage.Danger("Solo se pueden despachar pedidos que estén en estado 'Nuevo'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Despachado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();

                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'Despachado'.");
            }
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Send(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            if (sale.OrderStatus != OrderStatus.Despachado)
            {
                _flashMessage.Danger("Solo se pueden enviar pedidos que estén en estado 'despachado'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Enviado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'enviado'.");
            }
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            if (sale.OrderStatus != OrderStatus.Enviado)
            {
                _flashMessage.Danger("Solo se pueden confirmar pedidos que estén en estado 'enviado'.");
            }
            else
            {
                sale.OrderStatus = OrderStatus.Confirmado;
                _context.Sales.Update(sale);
                await _context.SaveChangesAsync();
                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'confirmado'.");
            }
            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Cancel(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Sale sale = await _context.Sales.FindAsync(id);

            if (sale == null)
            {
                return NotFound();
            }
            if (sale.OrderStatus == OrderStatus.Cancelado)
            {
                _flashMessage.Danger("No se puede cancelar un pedido que esté en estado 'Cancelado'.");
            }
            else
            {
                await _ordersHelper.CancelOrderAsync(sale.Id);

                _flashMessage.Confirmation("El estado del pedido ha sido cambiado a 'Cancelado'.");
            }

            return RedirectToAction(nameof(Details), new { Id = sale.Id });
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyOrders()
        {
            return View(await _context.Sales
            .Include(s => s.User)
            .Include(s => s.SaleDetails)
            .ThenInclude(sd => sd.Producto)
            .Where(s => s.User.UserName == User.Identity.Name)
            .ToListAsync());
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyDetails(int? id)
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