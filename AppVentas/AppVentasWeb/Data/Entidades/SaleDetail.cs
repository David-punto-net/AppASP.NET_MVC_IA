using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Data.Entidades
{
    public class SaleDetail
    {
        public int Id { get; set; }
        public Sale Sale { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        public Producto Producto { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => Producto == null ? 0 : (decimal)Quantity * Producto.Price;
    }
}