﻿using AppVentasWeb.Enum;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Data.Entidades
{
    public class Sale
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime Date { get; set; }

        public User User { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Comentarios")]
        public string? Remarks { get; set; }

        [Display(Name = "Estado")]
        public OrderStatus OrderStatus { get; set; }
        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<WebpayRest> WebpayRests { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        [Display(Name = "Líneas")]
        public int Lines => SaleDetails == null ? 0 : SaleDetails.Count;

        [DisplayFormat(DataFormatString = "{0:N2}")]
        [Display(Name = "Cantidad")]
        public float Quantity => SaleDetails == null ? 0 : SaleDetails.Sum(sd => sd.Quantity);

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value => SaleDetails == null ? 0 : SaleDetails.Sum(sd => sd.Value);
    }
}