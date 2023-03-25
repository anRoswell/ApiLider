using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DatumUXAR
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
    }

    public class MetadataUXAR
    {
    }

    public class QMetadataUXAR
    {
    }

    public class UserPorAreaResponse
    {
        public bool IsAuthenticated { get; set; }
        public object ResultValue { get; set; }
        public object InfoRule { get; set; }
        public object ExecutionObject { get; set; }
        public MetadataUXAR Metadata { get; set; }
        public QMetadataUXAR QMetadata { get; set; }
        public object ProcessData { get; set; }
        public object ValidationUI { get; set; }
        public bool Ok { get; set; }
        public object CodeError { get; set; }
        public object Error { get; set; }
        public object TrazaError { get; set; }
        public List<DatumUXAR> Data { get; set; }
    }


}
