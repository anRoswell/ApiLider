using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectCUI
    {
        public string Name { get; set; }
        public WebServiceMethodCUI WebServiceMethod { get; set; }
    }

    public class ParametersCUI
    {
        public string CUI { get; set; }
    }

    public class ExpPorCUI
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectCUI ExecutionObject { get; set; }
    }

    public class WebServiceMethodCUI
    {
        public string Name { get; set; }
        public ParametersCUI Parameters { get; set; }
    }


}
