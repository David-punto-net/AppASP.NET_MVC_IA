using OpenAI.Chat;

namespace AppVentasWeb.Helper
{
    public interface IAzureOpenAIClientHelper
    {
        ChatClient GetChatClient();

        Task<string> GetRespuestaIA_SQLQueryAsync(string userMensaje, string schemaInfo);

        Task<string> GetRespuestaIA_FinalAsync(string userMensaje, string datosJson);
    }
}
