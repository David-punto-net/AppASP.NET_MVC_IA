using AppVentasWeb.Data.Entidades;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Models
{
    public class ShowCartViewModel
    {
        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        public ICollection<TemporalSale> TemporalSales { get; set; }

        public ICollection<WebPayCommitViewModel> WebpayViewModels { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Cantidad")]
        public float Quantity => TemporalSales == null ? 0 : TemporalSales.Sum(ts => ts.Quantity);

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Valor")]
        public decimal Value => TemporalSales == null ? 0 : TemporalSales.Sum(ts => ts.Value);
    }
}