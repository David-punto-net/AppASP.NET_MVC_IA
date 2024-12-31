using System.ComponentModel.DataAnnotations;

namespace AppVentas.Models.Entidades
{
    public class ProductImage
    {
        public int Id { get; set; }
        public Producto Producto { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        //TODO: Pending to change to the correct path
        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
        ? $"https://localhost:7057/images/noimage.png"
        : $"https://appventa.blob.core.windows.net/productos/{ImageId}";
    }
}