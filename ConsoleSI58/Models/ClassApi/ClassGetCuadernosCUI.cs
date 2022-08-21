using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectCBC
    {
        public string Name { get; set; }
        public WebServiceMethodCBC WebServiceMethod { get; set; }
    }

    public class ParametersCBC
    {
        public string CUI { get; set; }
    }

    public class CuadernoByCUI
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectCBC ExecutionObject { get; set; }
    }

    public class WebServiceMethodCBC
    {
        public string Name { get; set; }
        public ParametersCBC Parameters { get; set; }
    }


}
