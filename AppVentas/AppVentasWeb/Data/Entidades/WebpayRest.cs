using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Data.Entidades
{
    public class WebpayRest
    {
        public int Id { get; set; }
        public Sale Sale { get; set; }

        [MaxLength(200)]
        public string Vci { get; set; }

        public decimal? Amount { get; set; }

        [MaxLength(250)]
        public string Status { get; set; }

        [MaxLength(250)]
        public string BuyOrder { get; set; }

        [MaxLength(250)]
        public string SessionId { get; set; }

        [MaxLength(250)]
        public string Token { get; set; }

        [MaxLength(250)]
        public string CardDetail { get; set; }

        [MaxLength(250)]
        public string AccountingDate { get; set; }

        public DateTime? TransactionDate { get; set; }

        [MaxLength(250)]
        public string AuthorizationCode { get; set; }

        [MaxLength(250)]
        public string PaymentTypeCode { get; set; }

        public int? ResponseCode { get; set; }
        public decimal? InstallmentsAmount { get; set; }
        public int? InstallmentsNumber { get; set; }
        public decimal? Balance { get; set; }
    }
}