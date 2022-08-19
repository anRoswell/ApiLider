using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DatumCarR
    {
        public int IdCarpeta { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }

    public class MetadataCarR
    {
    }

    public class QMetadataCarR
    {
    }

    public class CarpetaResponse
    {
        public bool IsAuthenticated { get; set; }
        public object ResultValue { get; set; }
        public object InfoRule { get; set; }
        public object ExecutionObject { get; set; }
        public MetadataCarR Metadata { get; set; }
        public QMetadataCarR QMetadata { get; set; }
        public object ProcessData { get; set; }
        public object ValidationUI { get; set; }
        public bool Ok { get; set; }
        public object CodeError { get; set; }
        public object Error { get; set; }
        public object TrazaError { get; set; }
        public List<DatumCarR> Data { get; set; }
    }


}
