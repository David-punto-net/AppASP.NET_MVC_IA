
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AppVentas.Models.Models
{
    public class AddProductImageViewModel
    {
        public int ProductoId { get; set; }

        [Display(Name = "Imagen")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public IFormFile? ImageFile { get; set; }

    }
}
