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
    
    public partial class serviciosSolicitadosDocumentos
    {
        public int id { get; set; }
        public int serviciosSolicitadosId { get; set; }
        public int documentsId { get; set; }
        public int serviceId { get; set; }
        public string documentsName { get; set; }
        public string documentsUrl { get; set; }
        public string documentsExt { get; set; }
        public int documentsSize { get; set; }
        public int estado { get; set; }
        public int userIdcreatedAt { get; set; }
        public System.DateTime createdAt { get; set; }
        public Nullable<int> userIdupdateAt { get; set; }
        public Nullable<System.DateTime> updateAt { get; set; }
    
        public virtual documents documents { get; set; }
        public virtual servicios servicios { get; set; }
        public virtual serviciosSolicitados serviciosSolicitados { get; set; }
    }
}
