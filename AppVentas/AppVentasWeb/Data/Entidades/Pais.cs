using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Data.Entidades
{
    public class Pais
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage ="Debe ingresar el {0}")]
        [StringLength(50, ErrorMessage ="El campo {0} debe tener máximo {1} caractéres")]
        [Display(Name ="País")]
        public string Nombre { get; set; }
    }
}
