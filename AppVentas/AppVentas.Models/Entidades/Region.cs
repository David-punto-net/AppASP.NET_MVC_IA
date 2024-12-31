using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AppVentas.Models.Entidades
{
    public class Region
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Región")]
        [Required(ErrorMessage = "Debe ingresar el {0}")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres")]
        public string Nombre { get; set; }

        [JsonIgnore]
        public Pais Pais { get; set; }

        public ICollection<Comuna> Comunas { get; set; }

        [Display(Name = "N° Comunas")]
        public int NumeroDeComunas => Comunas == null ? 0 : Comunas.Count;
    }
}