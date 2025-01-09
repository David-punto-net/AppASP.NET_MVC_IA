using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Org.BouncyCastle.Utilities.Collections;


namespace AppVentasWeb.Helper
{
    public class OllamaIntegracion
    {

        public string resultado()
        {

            //"modelo1": "llama3.1:8b",
            //"modelo2": "llama3.2:3b"

            #pragma warning disable SKEXP0070 // Este tipo se incluye solo con fines de evaluación y está sujeto a cambios o a que se elimine en próximas actualizaciones. Suprima este diagnóstico para continuar.
            Kernel builder = Kernel.CreateBuilder()
                                .AddOllamaChatCompletion(
                                        modelId: "llama3.1:8b",
                                        endpoint: new Uri("11434"))
                                .Build();


            var aiChatService = builder.GetRequiredService<IChatCompletionService>();
            var chatHistory = new ChatHistory();


            var systemPrompt = "";

            chatHistory.AddAssistantMessage(systemPrompt);


            var userPrompt = "";

            chatHistory.AddUserMessage(userPrompt);

            chatHistory.Add(new ChatMessageContent(AuthorRole.User, userPrompt));

            var response = aiChatService.GetChatMessageContentsAsync(chatHistory).Result;

            chatHistory.AddAssistantMessage(response.ToString() ?? string.Empty);


            return response.ToString();

          
        }

        public string Prueba2()
        {

            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            builder.Services.AddOllamaChatCompletion("llama3.1:8b", new Uri("http://localhost:11434"));





            return "";
        }
    }
}
