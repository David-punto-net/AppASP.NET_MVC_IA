using AppVentasWeb.Data.Entidades;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppVentasWeb.Helper
{
    public interface ICombosHelper
    {
        Task<IEnumerable<SelectListItem>> GetComboCategoriasAsync();

        Task<IEnumerable<SelectListItem>> GetComboCategoriasAsync(IEnumerable<Categoria> filter);

        Task<IEnumerable<SelectListItem>> GetComboPaisesAsync();

        Task<IEnumerable<SelectListItem>> GetComboRegionesAsync(int paisId);

        Task<IEnumerable<SelectListItem>> GetComboComunasAsync(int regionesId);

        Task<IEnumerable<SelectListItem>> GetComboCiudadesAsync(int comunasId);
    }
}