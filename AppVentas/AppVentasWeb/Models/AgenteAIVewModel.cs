namespace AppVentasWeb.Models
{
    public class AgenteAIVewModel
    {
        public string Userimput { get; set; }
        public string RespuestaAsistente { get; set; }
        public float Temperature { get; set; } = 0.7f;
        public int MaxTokens { get; set; } = 100;
        public List<string> Historial { get; set; } = new List<string>();
    }
}