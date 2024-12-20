using AppVentasWeb.Data.Entidades;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Models
{
    public class RegionViewModel
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Región")]
        [Required(ErrorMessage = "Debe ingresar el {0}")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Nombre { get; set; }
        public int PaisId { get; set; }
        //public ICollection<Comuna> Comunas { get; set; }
    }
}
