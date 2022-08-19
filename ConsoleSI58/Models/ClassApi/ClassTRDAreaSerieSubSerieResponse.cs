using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DatumTRDSSS
    {
        [JsonProperty("Id Tabla Retencion Documental")]
        public int IdTablaRetencionDocumental { get; set; }

        [JsonProperty("Id Version Tabla Documental")]
        public int IdVersionTablaDocumental { get; set; }

        [JsonProperty("Codigo TablaRetencionDocumental")]
        public string CodigoTablaRetencionDocumental { get; set; }

        [JsonProperty("Nombre TablaRetencionDocumental")]
        public object NombreTablaRetencionDocumental { get; set; }

        [JsonProperty("Descripcion TablaRetencionDocumental")]
        public object DescripcionTablaRetencionDocumental { get; set; }

        [JsonProperty("Tiempo Anos TablaRetencionDocumental")]
        public int TiempoAnosTablaRetencionDocumental { get; set; }

        [JsonProperty("Procedimiento TablaRetencionDocumental")]
        public string ProcedimientoTablaRetencionDocumental { get; set; }

        [JsonProperty("Tiempo Meses TablaRetencionDocumental")]
        public int TiempoMesesTablaRetencionDocumental { get; set; }

        [JsonProperty("Tiempo Dias TablaRetencionDocumental")]
        public int TiempoDiasTablaRetencionDocumental { get; set; }

        [JsonProperty("Tiempo Anos A C TablaRetencionDocumental")]
        public int TiempoAnosACTablaRetencionDocumental { get; set; }

        [JsonProperty("Tiempo Dias A C TablaRetencionDocumental")]
        public int TiempoDiasACTablaRetencionDocumental { get; set; }

        [JsonProperty("Tiempo Meses A C TablaRetencionDocumental")]
        public int TiempoMesesACTablaRetencionDocumental { get; set; }

        [JsonProperty("Conservar TablaRetencionDocumental")]
        public bool ConservarTablaRetencionDocumental { get; set; }

        [JsonProperty("Medio Tecnico TablaRetencionDocumental")]
        public bool MedioTecnicoTablaRetencionDocumental { get; set; }

        [JsonProperty("Eliminar TablaRetencionDocumental")]
        public bool EliminarTablaRetencionDocumental { get; set; }

        [JsonProperty("Seleccionar TablaRetencionDocumental")]
        public bool SeleccionarTablaRetencionDocumental { get; set; }

        [JsonProperty("Configuracion TablaRetencionDocumental")]
        public string ConfiguracionTablaRetencionDocumental { get; set; }
        public int IdSerie { get; set; }
        public int IdSubserie { get; set; }
        public int IdAreaEmpresa { get; set; }
        public int IdCiudad { get; set; }
    }

    public class MetadataTRDSSS
    {
    }

    public class QMetadataTRDSSS
    {
    }

    public class TRDAreaSerieSubSerieResponse
    {
        public bool IsAuthenticated { get; set; }
        public object ResultValue { get; set; }
        public object InfoRule { get; set; }
        public object ExecutionObject { get; set; }
        public MetadataTRDSSS Metadata { get; set; }
        public QMetadataTRDSSS QMetadata { get; set; }
        public object ProcessData { get; set; }
        public object ValidationUI { get; set; }
        public bool Ok { get; set; }
        public object CodeError { get; set; }
        public object Error { get; set; }
        public object TrazaError { get; set; }
        public List<DatumTRDSSS> Data { get; set; }
    }

}
