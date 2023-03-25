using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class DatumAE
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Sigla { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int? IdCiudad { get; set; }
    }

    public class Metadata
    {
    }

    public class QMetadata
    {
    }

    public class AreaEmpresaResponse
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
        public List<DatumAE> Data { get; set; }
    }


}
