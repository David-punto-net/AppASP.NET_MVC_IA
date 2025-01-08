using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.Text;

namespace AppVentasWeb.Helper
{
    public class AzureOpenAIClientHelper : IAzureOpenAIClientHelper
    {
        private readonly Uri _endpoint;
        private readonly AzureKeyCredential _credential;
        private readonly string _model;

        public AzureOpenAIClientHelper(IConfiguration configuration)
        {
            _endpoint = new Uri(configuration["AzureOpenAi_Parametros:endpoint"]);
            _credential = new AzureKeyCredential(configuration["AzureOpenAi_Parametros:credential"]);
            _model = configuration["AzureOpenAi_Parametros:modelo"];
        }

        public ChatClient GetChatClient()
        {
            AzureOpenAIClient azureClient = new(_endpoint, _credential);

            ChatClient chatClient = azureClient.GetChatClient(_model);

            return chatClient;
        }

        public async Task<string> GetRespuestaIA_SQLQueryAsync(string userMensaje, string schemaInfo)
        {
            var chatClient = GetChatClient();

            ChatCompletion completion = await chatClient.CompleteChatAsync(
                [
                    new SystemChatMessage("Eres un generador de consulats SQL de SqlServer. Debes devolver solo consultas SQL, sin ninguna explicacion adicional, para este modelo de datos de las siguientes Tablas: " + 
                                            schemaInfo + " . NO debes generar querys que realizen cambios en la base de datos del tipo: INSERT,UPDATE,DELETE,DROP. La respuesta NO debe incluir caracteres como ```sql"),

                    //new SystemChatMessage("Eres un generador de consulats sql en sqlServer. Responde solamente entregando la consulta para un modelo de datos que consiste en las siguientes " +
                    //                      "tablas: AspNetUsers( Id, Rut, Nombres, Apellidos, Direccion, UserName), Sales(Id, Date, UserId, Remarks, OrderStatus), " +
                    //                      "SaleDetails( Id, SaleId, Remarks, ProductoId, Quantity), Productos(Id, Name, Description, Price, Stock), " +
                    //                      "Categorias(Id, Nombre), ProductCategories(Id, ProductoId, CategoriaId) " +
                    //                       " . NO debes generar querys que realizen cambios en la base de datos del tipo: INSERT,UPDATE,DELETE,DROP. La respuesta NO debe incluir caracteres como ```sql"),

                        new UserChatMessage(userMensaje)
                ]);

            if (completion?.Content != null && completion.Content.Count > 0)
            {
                StringBuilder respuesta = new StringBuilder();
                foreach (var content in completion.Content)
                {
                    respuesta.AppendLine(content.Text);
                }
                return respuesta.ToString();
            }
            else
            {
                return "";
            }
            //return completion.Content[0].Text;
            //return completion.Content.ToString();
        }

        public async Task<string> GetRespuestaIA_FinalAsync(string userMensaje, string datosJson)
        {
            var chatClient = GetChatClient();

            ChatCompletion completion = await chatClient.CompleteChatAsync(
                [

                    new SystemChatMessage("Eres un Agente IA experto en atencion al cliente. Debes responder basado en el siguiente json: " + datosJson),

                            new UserChatMessage(userMensaje)
                ]);

            if (completion?.Content != null && completion.Content.Count > 0)
            {
                StringBuilder respuesta = new StringBuilder();
                foreach (var content in completion.Content)
                {
                    respuesta.AppendLine(content.Text);
                }
                return respuesta.ToString();
            }
            else
            {
                return "";
            }

            //return completion.Content[0].Text;
            //return completion.Content.ToString();
        }
    }
}