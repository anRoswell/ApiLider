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
    
    public partial class series
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public series()
        {
            this.lotes = new HashSet<lotes>();
        }
    
        public decimal serie_id { get; set; }
        public string serie_nombre { get; set; }
        public string serie_rutaplanos { get; set; }
        public string serie_rutaimagenes { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<lotes> lotes { get; set; }
    }
}
