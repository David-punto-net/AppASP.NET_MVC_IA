using System.ComponentModel.DataAnnotations;

namespace AppVentas.Models.Models
{
    public class ComunaViewModel
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Comuna")]
        [Required(ErrorMessage = "Debe ingresar el {0}")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Nombre { get; set; }
        public int RegionId { get; set; }

    }
}
