//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infraestructure
{
    using System;
    using System.Collections.Generic;
    
    public partial class Log
    {
        public int id { get; set; }
        public int IdError { get; set; }
        public string ValidadorUi { get; set; }
        public string CodigoError { get; set; }
        public string NombreArchivo { get; set; }
        public string CodigoArea { get; set; }
        public string NombreExpediente { get; set; }
        public string NombreCarpeta { get; set; }
        public Nullable<bool> status { get; set; }
        public System.DateTime createdAt { get; set; }
        public Nullable<System.DateTime> updatedAt { get; set; }
    }
}
