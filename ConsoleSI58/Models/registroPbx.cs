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
    
    public partial class registroPbx
    {
        public int idPbx { get; set; }
        public Nullable<System.DateTime> time { get; set; }
        public string src { get; set; }
        public Nullable<int> queue { get; set; }
        public string agent { get; set; }
        public string @event { get; set; }
        public string waittime { get; set; }
        public string duration { get; set; }
        public Nullable<int> ingoing { get; set; }
        public Nullable<int> outgoing { get; set; }
        public string overflow { get; set; }
        public string id { get; set; }
        public string nit { get; set; }
    }
}
