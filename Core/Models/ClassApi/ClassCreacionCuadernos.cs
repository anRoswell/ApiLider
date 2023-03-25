using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectCC
    {
        public string Name { get; set; }
        public WebServiceMethodCC WebServiceMethod { get; set; }
    }

    public class ParametersCC
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string IdCarpeta { get; set; }
        public string IdExpediente { get; set; }
    }

    public class CreacionCuadernos
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectCC ExecutionObject { get; set; }
    }

    public class WebServiceMethodCC
    {
        public string Name { get; set; }
        public ParametersCC Parameters { get; set; }
    }
}
