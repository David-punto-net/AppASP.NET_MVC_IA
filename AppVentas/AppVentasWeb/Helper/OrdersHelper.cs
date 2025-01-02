using AppVentasWeb.Common;
using AppVentasWeb.Data;
using AppVentasWeb.Data.Entidades;
using AppVentasWeb.Enum;
using AppVentasWeb.Models;

namespace AppVentasWeb.Helper
{
    public class OrdersHelper : IOrdersHelper
    {
        private readonly DataContex _context;

        public OrdersHelper(DataContex context)
        {
            _context = context;
        }

        public async Task<Response> ProcessOrderAsync(ShowCartViewModel model)
        {
            Response response = await CheckInventoryAsync(model);
            if (!response.IsSuccess)
            {
                return response;
            }
            Sale sale = new()
            {
                Date = DateTime.UtcNow,
                User = model.User,
                Remarks = model.Remarks,
                SaleDetails = new List<SaleDetail>(),
                OrderStatus = OrderStatus.Nuevo
            };
            foreach (TemporalSale item in model.TemporalSales)
            {
                sale.SaleDetails.Add(new SaleDetail
                {
                    Producto = item.Producto,
                    Quantity = item.Quantity,
                    Remarks = item.Remarks,
                });
                Producto product = await _context.Productos.FindAsync(item.Producto.Id);

                if (product != null)
                {
                    product.Stock -= item.Quantity;
                    _context.Productos.Update(product);
                }

                _context.TemporalSales.Remove(item);
            }

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();
            return response;
        }

        private async Task<Response> CheckInventoryAsync(ShowCartViewModel model)
        {
            Response response = new() { IsSuccess = true };
            foreach (TemporalSale item in model.TemporalSales)
            {
                Producto product = await _context.Productos.FindAsync(item.Producto.Id);
                if (product == null)
                {
                    response.IsSuccess = false;
                    response.Message = $"El producto {item.Producto.Name}, ya no está disponible";
                    return response;
                }
                if (
                product.Stock < item.Quantity)
                {
                    response.IsSuccess = false;
                    response.Message = $"Lo sentimos no tenemos existencias suficientes del producto {item.Producto.Name}, para tomar su pedido. Por favor disminuir la cantidad o sustituirlo por otro.";
                    return response;
                }
            }
            return response;
        }
    }
}