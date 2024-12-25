using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Rut")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Rut { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Nombres { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Apellidos { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caractéres.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to put the correct paths
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
        ? $"https://localhost:44338/images/noimage.png"
        : $"https://appventa.blob.core.windows.net/users/{ImageId}";

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }


        [Display(Name = "País")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar un país.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int PaisId { get; set; }

        public IEnumerable<SelectListItem> Paises { get; set; }

        [Display(Name = "Región")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una región.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int RegionId { get; set; }

        public IEnumerable<SelectListItem> Regiones { get; set; }

        [Display(Name = "Comúna")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una comúna.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ComunaId { get; set; }

        public IEnumerable<SelectListItem> Comunas { get; set; }

        [Display(Name = "Ciuadad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debes de seleccionar una ciudad.")]
        public int CiudadId { get; set; }

        public IEnumerable<SelectListItem> Ciudades { get; set; }
    }
}