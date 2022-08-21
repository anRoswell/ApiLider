using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DatumOC
    {
        public string IdCuaderno { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public string Carpeta { get; set; }
        public string IdCarpeta { get; set; }
    }

    public class ObtenerCuadernoResponse
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
        public List<DatumOC> Data { get; set; }
    }


}
