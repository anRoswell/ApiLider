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
    
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Token { get; set; }
        public System.DateTime Expires { get; set; }
        public System.DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public Nullable<System.DateTime> Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
    
        public virtual Accounts Accounts { get; set; }
    }
}
