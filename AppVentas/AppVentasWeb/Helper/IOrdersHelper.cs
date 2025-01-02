using AppVentasWeb.Common;
using AppVentasWeb.Models;

namespace AppVentasWeb.Helper
{
    public interface IOrdersHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel model);

        Task<Response> CancelOrderAsync(int id);
    }
}