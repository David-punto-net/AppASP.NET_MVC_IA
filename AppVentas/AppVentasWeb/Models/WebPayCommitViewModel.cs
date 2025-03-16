namespace AppVentasWeb.Models
{
    public class WebPayCommitViewModel
    {
        public string Vci { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public string BuyOrder { get; set; }
        public string SessionId { get; set; }

        public string TokenWebpay { get; set; }
        public object CardDetail { get; set; }
        public string AccountingDate { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string AuthorizationCode { get; set; }
        public string PaymentTypeCode { get; set; }
        public int? ResponseCode { get; set; }
        public decimal? InstallmentsAmount { get; set; }
        public int? InstallmentsNumber { get; set; }
        public decimal? Balance { get; set; }
    }
}
