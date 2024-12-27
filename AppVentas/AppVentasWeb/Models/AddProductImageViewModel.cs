using AppVentasWeb.Data.Entidades;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Models
{
    public class AddProductImageViewModel
    {
        public int ProductoId { get; set; }

        [Display(Name = "Imagen")]
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public IFormFile? ImageFile { get; set; }

    }
}
