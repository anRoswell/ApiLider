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
    
    public partial class servicioJuridicoDelete
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public servicioJuridicoDelete()
        {
            this.servicioJuridicoDetalleDelete = new HashSet<servicioJuridicoDetalleDelete>();
        }
    
        public int id { get; set; }
        public int userId { get; set; }
        public int servicioId { get; set; }
        public int servicioAdquiridoId { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }
        public int stateServiceId { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual stateService stateService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<servicioJuridicoDetalleDelete> servicioJuridicoDetalleDelete { get; set; }
    }
}
