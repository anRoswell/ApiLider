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
    
    public partial class pagosOnLine
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int costoXPeriodoId { get; set; }
        public int estadoPago { get; set; }
        public string ip { get; set; }
        public string sistemaOperativo { get; set; }
        public string respestaTransaccion { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    }
}
