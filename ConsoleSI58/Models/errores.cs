//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Lider.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class errores
    {
        public decimal error_id { get; set; }
        public System.DateTime error_fecha { get; set; }
        public decimal error_usuario_id { get; set; }
        public string error_excepcion { get; set; }
        public string error_stackTrace { get; set; }
        public string error_estado { get; set; }
    
        public virtual usuarios usuarios { get; set; }
    }
}
