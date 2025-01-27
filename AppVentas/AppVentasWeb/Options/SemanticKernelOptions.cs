using Azure.AI.OpenAI;
using System.ComponentModel.DataAnnotations;

namespace AppVentasWeb.Options
{
    public class SemanticKernelOptions
    {
        public string ChatModel { get; set; }
        public string CompletionsModel { get; set; }
        public string EmbeddingsModel { get; set; }

        [Required(AllowEmptyStrings = false)]
        public Uri Endpoint { get; init; }

        [Required(AllowEmptyStrings = false)]
        public string Key { get; init; }

        public AzureOpenAIClientOptions.ServiceVersion ServiceVersion { get; set; } = AzureOpenAIClientOptions.ServiceVersion.V2024_10_01_Preview;

    }
}
