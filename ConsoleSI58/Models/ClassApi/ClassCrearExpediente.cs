// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
using System.Collections.Generic;

public class ExecutionObjectCE
{
        public string Name { get; set; }
        public WebServiceMethodCE WebServiceMethod { get; set; }
    }

    public class ParametersCE
    {
        public int IdTablaRetencionDocumental { get; set; }
        public int IdVersionTablaRetencionDocumental { get; set; }
        public int IdUsuarioResponsable { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int FechaFinal { get; set; }
        public bool VerPublico { get; set; }
        public string NumeroRadicacionProceso { get; set; }
        public int IdCiudad { get; set; }
        public int IdAreaEmpresa { get; set; }
        public List<object> Metadatos { get; set; }
    }

    public class CrearExpediente
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectCE ExecutionObject { get; set; }
    }

    public class WebServiceMethodCE
{
        public string Name { get; set; }
        public ParametersCE Parameters { get; set; }
    }

