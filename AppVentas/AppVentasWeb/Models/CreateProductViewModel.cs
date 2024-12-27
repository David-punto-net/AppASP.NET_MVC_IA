using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Models
{
    public class CreateProductViewModel : EditProductoViewModel
    {
        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una categoría.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CategoriaId { get; set; }

        public IEnumerable<SelectListItem> Categorias { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
    }
}