using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObject
    {
        public string Name { get; set; }
        public WebServiceMethod WebServiceMethod { get; set; }
    }

    public class Parameters
    {
        public string IdTablaRetencionDocumental { get; set; }
        public string Nombre { get; set; }

        [JsonProperty("IdVersionTablaRetencionDocumental ")]
        public string IdVersionTablaRetencionDocumental { get; set; }
        public string IdUsuarioResponsable { get; set; }
        public string Descripcion { get; set; }
        public string FechaFinal { get; set; }
        public string VerPublico { get; set; }
        public string NumeroRadicacionProceso { get; set; }
        public string IdCiudad { get; set; }
        public string IdAreaEmpresa { get; set; }
        public string Metadatos { get; set; }
    }

    public class JsonToSendApi
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObject ExecutionObject { get; set; }
    }

    public class WebServiceMethod
    {
        public string Name { get; set; }
        public Parameters Parameters { get; set; }
    }


}
