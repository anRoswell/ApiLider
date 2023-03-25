using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.ClassApi
{
    public class ClassLogErrorApi
    {
        public int IdError { get; set; }
        public string Message { get; set; }
        public string CodigoError { get; set; }
        public string NombreArchivo { get; set; }
        public string CodigoArea { get; set; }
        public string NombreExpediente { get; set; }
        public string NombreCarpeta { get; set; }
    }
}
