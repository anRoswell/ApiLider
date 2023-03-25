using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ClassApi
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ExecutionObjectNB
    {
        public string Name { get; set; }
        public WebServiceMethodNB WebServiceMethod { get; set; }
    }

    public class ParametersNB
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public int IdCarpeta { get; set; }
        public double IdExpediente { get; set; }
    }

    public class NoteBook
    {
        public string Token { get; set; }
        public string AppKey { get; set; }
        public ExecutionObject ExecutionObject { get; set; }
    }

    public class WebServiceMethodNB
    {
        public string Name { get; set; }
        public ParametersNB Parameters { get; set; }
    }


}
