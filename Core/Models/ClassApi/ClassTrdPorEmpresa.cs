using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectTRD
    {
        public string Name { get; set; }
        public WebServiceMethodTrd WebServiceMethod { get; set; }
    }

    public class ParametersTrdEmpresa
    {
        public string IdAreaEmpresa { get; set; }
    }

    public class TRDPOREMPRESA
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectTRD ExecutionObject { get; set; }
    }

    public class WebServiceMethodTrd
    {
        public string Name { get; set; }
        public ParametersTrdEmpresa Parameters { get; set; }
    }


}
