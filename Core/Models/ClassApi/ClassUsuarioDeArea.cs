using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectUDA
    {
        public string Name { get; set; }
        public WebServiceMethodUDA WebServiceMethod { get; set; }
    }

    public class ParametersUDA
    {
        public int IdArea { get; set; }
    }

    public class UsuarioDeArea
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObjectUDA ExecutionObject { get; set; }
    }

    public class WebServiceMethodUDA
    {
        public string Name { get; set; }
        public ParametersUDA Parameters { get; set; }
    }


}
