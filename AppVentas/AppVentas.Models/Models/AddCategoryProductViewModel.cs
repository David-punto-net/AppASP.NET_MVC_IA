using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AppVentas.Models.Models
{
    public class AddCategoryProductViewModel
    {
        public int ProductoId { get; set; }

        [Display(Name = "Categoría")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes seleccionar una categoría.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}