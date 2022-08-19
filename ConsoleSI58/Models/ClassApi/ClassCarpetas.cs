using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectFolder
    {
        public string Name { get; set; }
        public WebServiceMethodFolder WebServiceMethod { get; set; }
    }

    public class ParametersFolder
    {
    }

    public class Folder
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectFolder ExecutionObject { get; set; }
    }

    public class WebServiceMethodFolder
    {
        public string Name { get; set; }
        public ParametersFolder Parameters { get; set; }
    }


}
