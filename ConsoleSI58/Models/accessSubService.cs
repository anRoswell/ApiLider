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
    
    public partial class accessSubService
    {
        public int id { get; set; }
        public int servicioId { get; set; }
        public bool juridico { get; set; }
        public bool auxilioExequial { get; set; }
        public bool auxSocialDesempleo { get; set; }
        public bool planCiguenia { get; set; }
        public bool state { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    }
}
