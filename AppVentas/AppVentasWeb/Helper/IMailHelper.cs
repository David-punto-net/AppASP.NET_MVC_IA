using AppVentasWeb.Common;

namespace AppVentasWeb.Helper
{
    public interface IMailHelper
    {
        Response SendMail(string toName, string toEmail, string subject, string body);
    }
}
