using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppVentasWeb.Data.Entidades
{
    public class Comuna
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Comuna")]
        [Required(ErrorMessage = "Debe ingresar el {0}")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Nombre { get; set; }

        [JsonIgnore]
        public Region Region { get; set; }

        public ICollection<Ciudad> Ciudades { get; set; }

        [Display(Name = "N° Ciudades")]
        public int NumeroDeCiudades => Ciudades == null ? 0 : Ciudades.Count;
    }
}