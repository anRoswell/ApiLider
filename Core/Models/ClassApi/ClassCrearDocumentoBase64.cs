using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ArchivoCD
    {
        public string DatosArchivo { get; set; }
        public bool EsAdjunto { get; set; }
        public string Archivo { get; set; }
        public FileField FileField { get; set; }
    }

    public class DestinationFile
    {
    }

    public class ExecutionObjectCD
    {
        public string Name { get; set; }
        public WebServiceMethodCD WebServiceMethod { get; set; }
    }

    public class FileField
    {
        public string FormatType { get; set; }
        public DestinationFile DestinationFile { get; set; }
    }

    public class ParametersCD
    {
        public string IdExpediente { get; set; }
        public string IdTipoDocumental { get; set; }
        public string Nombre { get; set; }
        public List<ArchivoCD> Archivo { get; set; }
        public List<object> Metadatos { get; set; }
        public string FechaCreacionDocumento { get; set; }
        public string IdCarpeta { get; set; }
        public string Autor { get; set; }
        public string FechaTransmicion { get; set; }
        public string FechaRecepcion { get; set; }
        public string Descripcion { get; set; }
        public string Version { get; set; }
        public int IdAreaEmpresa { get; set; }
        public bool VerPublico { get; set; }
        public string IdCuaderno { get; set; }
        public string IndiceOrdenamiento { get; set; }
    }

    public class CrearDocumento
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectCD ExecutionObject { get; set; }
    }

    public class WebServiceMethodCD
    {
        public string Name { get; set; }
        public ParametersCD Parameters { get; set; }
    }
}
