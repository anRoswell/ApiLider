using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Datum
    {
        public string IdExpediente { get; set; }
        public string IdTablaRetencionDocumental { get; set; }
        public string IdVersionTablaRetencionDocumental { get; set; }
        public string Codigo { get; set; }
        public int IdUsuarioResponsable { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public object FechaFinal { get; set; }
        public string TipoBodega { get; set; }
        public object PalabrasClave { get; set; }
        public object FechaArchivoCentral { get; set; }
        public object FechaArchivoHistorico { get; set; }
        public object FechaCierre { get; set; }
        public int IdAreaEmpresa { get; set; }
        public bool VerPublico { get; set; }
        public string Ciudad { get; set; }
        public string UsuarioResponsable { get; set; }
        public string GrupoTrabajo { get; set; }
        public string CodigoGrupoTrabajo { get; set; }
        public string CodigoTRD { get; set; }
        public string NombreSerie { get; set; }
        public string CodigoSerie { get; set; }
        public string NombreSubSerie { get; set; }
        public string CodigoSubSerie { get; set; }
        public string ConfiguracionTRD { get; set; }
    }

    public class Root
    {
        public List<TiposDocumentale> TiposDocumentales { get; set; }
        public List<object> Metadatos { get; set; }
    }
    
    public class TiposDocumentale
    {
        public string IdTipoDocumental { get; set; }
        public string Nombre { get; set; }
    }

    public class GetExpedienteResponse
    {
        public bool IsAuthenticated { get; set; }
        public object ResultValue { get; set; }
        public object InfoRule { get; set; }
        public object ExecutionObject { get; set; }
        public Metadata Metadata { get; set; }
        public QMetadata QMetadata { get; set; }
        public object ProcessData { get; set; }
        public object ValidationUI { get; set; }
        public bool Ok { get; set; }
        public object CodeError { get; set; }
        public object Error { get; set; }
        public object TrazaError { get; set; }
        public List<Datum> Data { get; set; }
    }

}
