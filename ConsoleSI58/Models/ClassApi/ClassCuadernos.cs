using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectCuadernos
    {
        public string Name { get; set; }
        public WebServiceMethodCuadernos WebServiceMethod { get; set; }
    }

    public class ParametersCuadernos
    {
        public string CUI { get; set; }
    }

    public class Cuadernos
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectCuadernos ExecutionObject { get; set; }
    }

    public class WebServiceMethodCuadernos
    {
        public string Name { get; set; }
        public ParametersCuadernos Parameters { get; set; }
    }


}
