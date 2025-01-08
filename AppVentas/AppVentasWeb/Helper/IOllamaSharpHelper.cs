using OllamaSharp;

namespace AppVentasWeb.Helper
{
    public interface IOllamaSharpHelper
    {
        OllamaApiClient GetChatOllamaApiClient();

        Task<string> GetRespuestaOllamaAsync(string userMensaje, string schemaBd);
        Task<string> GetRespuestaOllamaFinalAsync(string userMensaje, string schemaBd);
        
    }
}
