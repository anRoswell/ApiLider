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
    
    public partial class camposxtipodoc
    {
        public decimal campotipodoc_id { get; set; }
        public decimal campotipodoc_tipodoc_id { get; set; }
        public decimal campotipodoc_campo_id { get; set; }
        public Nullable<int> campotipodoc_campoorden { get; set; }
        public decimal campotipo_esrequeridounvalor { get; set; }
        public string campotipo_tipodevalidacion { get; set; }
        public string campotipo_valoresadmitidos { get; set; }
        public string campotipo_cadenaconexionbd { get; set; }
        public string campotipo_nombretabla { get; set; }
        public string campotipo_tipodetabla { get; set; }
        public string campotipo_nombrecampoentabla { get; set; }
        public decimal campotipo_asumeunvaloranterior { get; set; }
        public decimal campotipo_modificablevalorasum { get; set; }
        public decimal campotipo_diccionario_id { get; set; }
        public decimal campotipo_esfechadedocumento { get; set; }
        public decimal campotipo_dic_campopadre_id { get; set; }
        public decimal campotipo_id_grupo { get; set; }
        public decimal campotipo_id_masdeuno { get; set; }
        public decimal campotipo_siocr { get; set; }
        public decimal campotipo_siocr_enlinea { get; set; }
        public decimal campotipo_siocr_posxsi { get; set; }
        public decimal campotipo_siocr_posysi { get; set; }
        public decimal campotipo_siocr_posxid { get; set; }
        public decimal campotipo_siocr_posyid { get; set; }
        public decimal campotipo_sibcr { get; set; }
        public decimal campotipo_sibcr_enlinea { get; set; }
        public string campotipo_sibcr_estandar { get; set; }
        public decimal campotipo_sibcr_posxsi { get; set; }
        public decimal campotipo_sibcr_posysi { get; set; }
        public decimal campotipo_sibcr_posxid { get; set; }
        public decimal campotipo_sibcr_posyid { get; set; }
        public Nullable<short> campotipo_siboton { get; set; }
        public decimal campotipo_sifirmadigital { get; set; }
        public decimal campotipo_sifirmadigitalglobal { get; set; }
        public decimal campotipo_sifirmaelectronica { get; set; }
        public string campotipo_sifedetalle { get; set; }
    
        public virtual campos campos { get; set; }
    }
}