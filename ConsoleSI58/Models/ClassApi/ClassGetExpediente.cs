using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectGE
    {
        public string Name { get; set; }
        public WebServiceMethodGE WebServiceMethod { get; set; }
    }

    public class ParametersGE
    {
        public string CUI { get; set; }
    }

    public class GetExpediente
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectGE ExecutionObject { get; set; }
    }

    public class WebServiceMethodGE
    {
        public string Name { get; set; }
        public ParametersGE Parameters { get; set; }
    }




}
