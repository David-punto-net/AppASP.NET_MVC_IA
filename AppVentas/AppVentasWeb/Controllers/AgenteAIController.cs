using AppVentasWeb.Data;
using AppVentasWeb.Helper;
using AppVentasWeb.Models;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;
using System.Text.Json;

namespace AppVentasWeb.Controllers
{
    public class AgenteAIController : Controller
    {
        private readonly DataContex _context;
        private readonly IUserHelper _userHelper;
        private readonly IAzureOpenAIClientHelper _azureOpenAIClientHelper;


        public AgenteAIController(DataContex context, IUserHelper userHelper, IAzureOpenAIClientHelper azureOpenAIClientHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _azureOpenAIClientHelper = azureOpenAIClientHelper;
        }

        private string GetDatabaseSchema()
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

            //string jsonSchema = JsonSerializer.Serialize(schema, new JsonSerializerOptions
            //{
            //    WriteIndented = true 
            //});


            var stringBuilder = new StringBuilder();

            foreach (var table in schema)
            {
                stringBuilder.Append($"{table.Tabla}(");
                foreach (var column in table.Columnas)
                {
                    stringBuilder.Append($"{column.Name} {column.tipo}, ");
                }
                // Remove the last comma and space
                if (table.Columnas.Any())
                {
                    stringBuilder.Length -= 2;
                }
                stringBuilder.Append(") ");
            }

            return stringBuilder.ToString();
     
            //return jsonSchema;
        }


        [HttpGet]
        public IActionResult AgenteAI()
        {
            return View(new AgenteAIVewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AgenteAI(AgenteAIVewModel model)
        {
            if (ModelState.IsValid)
            {
                string userInput = model.Userimput;
                string schemaJson = GetDatabaseSchema();

                string repuestaIASql = await _azureOpenAIClientHelper.GetRespuestaIA_SQLQueryAsync(userInput, schemaJson);


                var datosJson = await GetEntitiesAsJsonAsync(repuestaIASql);


                var respuestaAI = await _azureOpenAIClientHelper.GetRespuestaIA_FinalAsync(userInput, datosJson);

                model.RespuestaAsistente = Markdown.ToHtml(respuestaAI);


            }

            return View(model);
        }

        public async Task<string> GetEntitiesAsJsonAsync(string repuestaIASql)
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = repuestaIASql;
                _context.Database.OpenConnection();

                using (var result = await command.ExecuteReaderAsync())
                {
                    var dataTable = new DataTable();
                    dataTable.Load(result);

                    var lista = dataTable.AsEnumerable().Select(row => dataTable.Columns.Cast<DataColumn>().ToDictionary(column => column.ColumnName, column => row[column])).ToList();

                    return JsonSerializer.Serialize(lista);
                }
            }
        }

    }
}
