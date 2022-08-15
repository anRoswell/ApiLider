using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public string Token { get; set; }
        public string TiempoGeneral { get; set; }
    }

    public class Metadata
    {
        public string TiempoServer1 { get; set; }
        public string TiempoServer2 { get; set; }
        public string TiempoServer3 { get; set; }
    }

    public class QMetadata
    {
    }

    public class LoginResponse
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
        public Data Data { get; set; }
    }


}
