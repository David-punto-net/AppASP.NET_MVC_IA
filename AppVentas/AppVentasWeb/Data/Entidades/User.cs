using AppVentasWeb.Enum;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Data.Entidades
{
    public class User : IdentityUser
    {
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

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
        ? $"https://localhost:44338/images/noimage.png"
        : $"https://appventa.blob.core.windows.net/users/{ImageId}"; 

        [Display(Name = "Tipo")]
        public UserType UserType { get; set; }

        [Display(Name = "Ciudad")]
        public Ciudad Ciudad { get; set; }

        [Display(Name = "Usuario")]
        public string FullName => $"{Nombres} {Apellidos}";

        [Display(Name = "Usuario")]
        public string FullNameWithDocument => $"{Nombres} {Apellidos} - {Rut}";
    }
}