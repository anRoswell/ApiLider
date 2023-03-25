using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObject
    {
        public string Name { get; set; }
        public WebServiceMethod WebServiceMethod { get; set; }
    }

    public class DataToSend
    {
        [Required]
        public int IdTablaRetencionDocumental { get; set; }
        public string Nombre { get; set; }
        [JsonProperty("IdVersionTablaRetencionDocumental ")]
        public int IdVersionTablaRetencionDocumental { get; set; }
        public int IdUsuarioResponsable { get; set; }
        public string Descripcion { get; set; }
        public string FechaFinal { get; set; }
        public int VerPublico { get; set; }
        public string NumeroRadicacionProceso { get; set; }
        public int IdCiudad { get; set; }
        public int IdAreaEmpresa { get; set; }
        public string[] Metadatos { get; set; }
    }

    public class CREAREXPEDIENTE
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObject ExecutionObject { get; set; }
    }

    public class WebServiceMethod
    {
        public string Name { get; set; }
        public DataToSend Parameters { get; set; }
    }

    public class ResponseExpendiente
    {
        public bool Ok { get; set; }
        public string Data { get; set; }
        public string Error { get; set; }
    }
}
