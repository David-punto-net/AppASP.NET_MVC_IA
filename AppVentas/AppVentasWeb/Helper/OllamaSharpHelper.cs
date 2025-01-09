using AppVentasWeb.Data;
using Microsoft.EntityFrameworkCore;
using OllamaSharp;
using System.Text.Json;

namespace AppVentasWeb.Helper
{
    public class OllamaSharpHelper : IOllamaSharpHelper
    {
        private readonly Uri _endpoint;
        private readonly string _model;
        private readonly DataContex _context;

        public OllamaSharpHelper(IConfiguration configuration, DataContex context)
        {
            _context = context;
            _endpoint = new Uri(configuration["LlamaAI_Parametros:endpoint"]);
            _model = configuration["LlamaAI_Parametros:modelo"];
        }

        public OllamaApiClient GetChatOllamaApiClient(int IdModelo)
        {
            OllamaApiClient ollama = new OllamaApiClient(_endpoint);

            if (IdModelo == 1)
            {
                ollama.SelectedModel = "llama3.1:8b";
            }
            else
            {
                ollama.SelectedModel = "llama3.2:3b";
            }

            return ollama;
        }

        private string GetDatabaseSchemaJson()
        {
            var excludedTables = new HashSet<string>
            {
                "AspNetRoles",
                "AspNetRoleClaims",
                "AspNetUserClaims",
                "AspNetUserLogins",
                "AspNetUserRoles",
                "AspNetUserTokens"
            };

            var schema = _context.Model.GetEntityTypes()
                .Where(entity => !excludedTables.Contains(entity.GetTableName()))
                .Select(entity => new
                {
                    Tabla = entity.GetTableName(),
                    Columnas = entity.GetProperties().Select(property => new
                    {
                        Name = property.Name,
                        tipo = property.GetColumnType()
                    })
                })
                .ToList();

            string jsonSchema = JsonSerializer.Serialize(schema, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            return jsonSchema;
        }

        public async Task<string> GetRespuestaOllamaAsync(string userMensaje)
        {
            var schemaBd = GetDatabaseSchemaJson();

            string msjSystem = "Eres un generador de querys SQL. Devuelve solo T-SQL, sin ninguna explicación adicional, para las siguientes tablas: "
                + schemaBd + ". NO debes generar querys que realizen cambios en la base de datos del tipo: INSERT,UPDATE,DELETE,DROP. La respuesta NO debe incluir caracteres como ```sql";

            var ollama = GetChatOllamaApiClient(1);
            var chat = new Chat(ollama, msjSystem);

            string respuesta = await chat.SendAsync(userMensaje).StreamToEndAsync();

            if (respuesta.Contains("```") || respuesta.Contains("sql"))
            {
                respuesta = respuesta.Replace("```", string.Empty).Replace("sql", string.Empty);
            }

            return respuesta;
        }

        public async Task<string> GetRespuestaOllamaFinalAsync(string userMensaje, string datosbd)
        {
            string msjSystem = "Eres un agente IA experto en información de la empresa. Proporcionas respuestas precisas y útiles, y tu enfoque principal es brindar una experiencia amigable y eficiente. debes responder basado en estos datos: " + datosbd;

            var ollama = GetChatOllamaApiClient(2);
            var chat = new Chat(ollama, msjSystem);

            string respuesta2 = await chat.SendAsync(userMensaje).StreamToEndAsync();

            return respuesta2;
        }
    }
}