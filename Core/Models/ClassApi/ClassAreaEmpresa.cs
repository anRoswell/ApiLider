using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectAE
    {
        public string Name { get; set; }
        public WebServiceMethodAE WebServiceMethod { get; set; }
    }

    public class ParametersAE
    {
    }

    public class AreaEmpresa
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectAE ExecutionObject { get; set; }
    }

    public class WebServiceMethodAE
    {
        public string Name { get; set; }
        public ParametersAE Parameters { get; set; }
    }


}
