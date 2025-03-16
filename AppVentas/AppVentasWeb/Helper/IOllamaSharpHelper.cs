using OllamaSharp;

namespace AppVentasWeb.Helper
{
    public interface IOllamaSharpHelper
    {
        OllamaApiClient GetChatOllamaApiClient(int IdModelo);

        Task<string> GetRespuestaOllamaAsync(string userMensaje, string schemaInfo);
        Task<string> GetRespuestaOllamaFinalAsync(string userMensaje, string DatosBd);

    }
}
