using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppVentasWeb.Helper
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCategoriasAsync();

        Task<IEnumerable<SelectListItem>> GetComboPaisesAsync();

        Task<IEnumerable<SelectListItem>> GetComboReginesAsync(int paisId);

        Task<IEnumerable<SelectListItem>> GetComboComunasAsync(int regionesId);

        Task<IEnumerable<SelectListItem>> GetComboCiudadesAsync(int comunasId);
    }
}