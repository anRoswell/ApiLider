using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class CrearExpedienteResponse
    {
        public bool IsAuthenticated { get; set; }
        public object ResultValue { get; set; }
        public object InfoRule { get; set; }
        public object ExecutionObject { get; set; }
        public Metadata Metadata { get; set; }
        public QMetadata QMetadata { get; set; }
        public object ProcessData { get; set; }
        public string ValidationUI { get; set; }
        public bool Ok { get; set; }
        public string CodeError { get; set; }
        public object Error { get; set; }
        public object TrazaError { get; set; }
        public object Data { get; set; }
    }




}
