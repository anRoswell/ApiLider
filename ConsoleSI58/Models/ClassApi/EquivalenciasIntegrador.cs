using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models.ClassApi
{
    public class EquivalenciasIntegrador
    {
        public int Id { get; set; }
        public string Despacho { get; set; }
        public string Serie_Subserie { get; set; }
        public int IdArea { get; set; }
        public int IdCiudad { get; set; }
        public int IdUsuario { get; set; }
        public int IdTrd { get; set; }
        public int IdVersionTrd { get; set; }
    }
}
