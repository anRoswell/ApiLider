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
    
    public partial class grupos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public grupos()
        {
            this.grupoServicio = new HashSet<grupoServicio>();
            this.servicioAdquirido = new HashSet<servicioAdquirido>();
            this.serviciosSolicitados = new HashSet<serviciosSolicitados>();
        }
    
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int userTypesId { get; set; }
        public Nullable<decimal> valor { get; set; }
        public bool estado { get; set; }
        public int userIdcreatedAt { get; set; }
        public Nullable<int> userIdupdatedAt { get; set; }
        public System.DateTime createdAt { get; set; }
        public Nullable<System.DateTime> updatedAt { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<grupoServicio> grupoServicio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<servicioAdquirido> servicioAdquirido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<serviciosSolicitados> serviciosSolicitados { get; set; }
    }
}
