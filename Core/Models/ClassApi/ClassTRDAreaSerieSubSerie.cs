using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectTrdASS
    {
        public string Name { get; set; }
        public WebServiceMethodTrdASS WebServiceMethod { get; set; }
    }

    public class ParametersTrdASS
    {
        public string CodigoArea { get; set; }
        public string CodigoSerie { get; set; }
        public string CodigoSubSerie { get; set; }
    }

    public class TRDAreaSerieSubSerie
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectTrdASS ExecutionObject { get; set; }
    }

    public class WebServiceMethodTrdASS
    {
        public string Name { get; set; }
        public ParametersTrdASS Parameters { get; set; }
    }

}
