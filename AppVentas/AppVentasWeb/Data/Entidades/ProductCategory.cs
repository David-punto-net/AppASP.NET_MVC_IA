namespace AppVentasWeb.Data.Entidades
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }
        public Categoria Categoria { get; set; }
    }
}