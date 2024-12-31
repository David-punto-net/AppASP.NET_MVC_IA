using AppVentasWeb.Data.Entidades;

namespace AppVentasWeb.Models
{
    public class HomeViewModel
    {
        public ICollection<Producto> Productos { get; set; }
        public float Quantity { get; set; }
    }
}