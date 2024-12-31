using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Data.Entidades
{
    public class TemporalSale
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Producto Producto { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public float Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }
    }
}