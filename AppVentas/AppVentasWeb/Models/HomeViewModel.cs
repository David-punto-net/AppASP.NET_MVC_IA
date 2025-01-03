using AppVentasWeb.Common;
using AppVentasWeb.Data.Entidades;

namespace AppVentasWeb.Models
{
    public class HomeViewModel
    {
        public PaginatedList<Producto> Productos { get; set; }
        public float Quantity { get; set; }

        public ICollection<Categoria> Categorias { get; set; }
    }
}