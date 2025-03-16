namespace AppVentasWeb.DTOs;

public class TransbankRequestDTO
{
    public string Buy_order { get; set; }
    public string Session_id { get; set; }
    public decimal Amount { get; set; }
    public string Return_url { get; set; }
}

public class TransbankReversarAnular
{
    public string? Token { get; set; }
    public decimal Amount { get; set; }
}
