﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SID_PROTOCOL2Entities : DbContext
    {
        public SID_PROTOCOL2Entities()
            : base("name=SID_PROTOCOL2Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bitacora> Bitacora { get; set; }
        public virtual DbSet<initialDate> initialDate { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Log_Motivos> Log_Motivos { get; set; }
        public virtual DbSet<Accounts> Accounts { get; set; }
        public virtual DbSet<ArchivosZip> ArchivosZip { get; set; }
        public virtual DbSet<auditorias> auditorias { get; set; }
        public virtual DbSet<CALIDAD> CALIDAD { get; set; }
        public virtual DbSet<CALIDADLOTES> CALIDADLOTES { get; set; }
        public virtual DbSet<campos> campos { get; set; }
        public virtual DbSet<CAMPOSPROCESOSTIPOSDEDOCUMENTO> CAMPOSPROCESOSTIPOSDEDOCUMENTO { get; set; }
        public virtual DbSet<camposxtipodoc> camposxtipodoc { get; set; }
        public virtual DbSet<datos> datos { get; set; }
        public virtual DbSet<datos_finales> datos_finales { get; set; }
        public virtual DbSet<DATOSRAD> DATOSRAD { get; set; }
        public virtual DbSet<Despachos> Despachos { get; set; }
        public virtual DbSet<imagenes> imagenes { get; set; }
        public virtual DbSet<listas> listas { get; set; }
        public virtual DbSet<lotes> lotes { get; set; }
        public virtual DbSet<PROCESOS> PROCESOS { get; set; }
        public virtual DbSet<RAD_ALIST> RAD_ALIST { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        public virtual DbSet<series> series { get; set; }
        public virtual DbSet<tiposdedocumentos> tiposdedocumentos { get; set; }
        public virtual DbSet<TIPOSDELOTES> TIPOSDELOTES { get; set; }
        public virtual DbSet<TQM_LOCK> TQM_LOCK { get; set; }
        public virtual DbSet<TQM_LOCK_CALIDAD> TQM_LOCK_CALIDAD { get; set; }
        public virtual DbSet<usuarios> usuarios { get; set; }
        public virtual DbSet<valoreslistas> valoreslistas { get; set; }
        public virtual DbSet<Equivalencias_Carpetas> Equivalencias_Carpetas { get; set; }
        public virtual DbSet<Equivalencias_Integrador> Equivalencias_Integrador { get; set; }
        public virtual DbSet<Equivalencias_TiposDocumentales> Equivalencias_TiposDocumentales { get; set; }
        public virtual DbSet<Folder> Folder { get; set; }
        public virtual DbSet<parametersApi> parametersApi { get; set; }
        public virtual DbSet<urlApiI> urlApiI { get; set; }
        public virtual DbSet<datos_2> datos_2 { get; set; }
        public virtual DbSet<EQUIVALENCIAS> EQUIVALENCIAS { get; set; }
        public virtual DbSet<errores> errores { get; set; }
        public virtual DbSet<DB_Errors> DB_Errors { get; set; }
        public virtual DbSet<Excepcion> Excepcion { get; set; }
    
        public virtual ObjectResult<GET_GROUP_EXPEDIENTE_Result> GET_GROUP_EXPEDIENTE(Nullable<int> lOTE_ID)
        {
            var lOTE_IDParameter = lOTE_ID.HasValue ?
                new ObjectParameter("LOTE_ID", lOTE_ID) :
                new ObjectParameter("LOTE_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_GROUP_EXPEDIENTE_Result>("GET_GROUP_EXPEDIENTE", lOTE_IDParameter);
        }
    
        public virtual ObjectResult<GetDataToSendApi_Result> GetDataToSendApi(string action)
        {
            var actionParameter = action != null ?
                new ObjectParameter("action", action) :
                new ObjectParameter("action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetDataToSendApi_Result>("GetDataToSendApi", actionParameter);
        }
    
        public virtual int SpExampleApi(string action)
        {
            var actionParameter = action != null ?
                new ObjectParameter("action", action) :
                new ObjectParameter("action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpExampleApi", actionParameter);
        }
    
        public virtual int SpExcepcion(string message, string stackTrace, Nullable<int> type)
        {
            var messageParameter = message != null ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(string));
    
            var stackTraceParameter = stackTrace != null ?
                new ObjectParameter("StackTrace", stackTrace) :
                new ObjectParameter("StackTrace", typeof(string));
    
            var typeParameter = type.HasValue ?
                new ObjectParameter("type", type) :
                new ObjectParameter("type", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpExcepcion", messageParameter, stackTraceParameter, typeParameter);
        }
    
        public virtual int SpFiles(string dato_indice4, string nombre)
        {
            var dato_indice4Parameter = dato_indice4 != null ?
                new ObjectParameter("dato_indice4", dato_indice4) :
                new ObjectParameter("dato_indice4", typeof(string));
    
            var nombreParameter = nombre != null ?
                new ObjectParameter("Nombre", nombre) :
                new ObjectParameter("Nombre", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpFiles", dato_indice4Parameter, nombreParameter);
        }
    
        public virtual ObjectResult<SpFindDespacho_Result> SpFindDespacho(string despacho)
        {
            var despachoParameter = despacho != null ?
                new ObjectParameter("Despacho", despacho) :
                new ObjectParameter("Despacho", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SpFindDespacho_Result>("SpFindDespacho", despachoParameter);
        }
    
        public virtual ObjectResult<SpFolderId_Result> SpFolderId(string dato_indice4)
        {
            var dato_indice4Parameter = dato_indice4 != null ?
                new ObjectParameter("dato_indice4", dato_indice4) :
                new ObjectParameter("dato_indice4", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SpFolderId_Result>("SpFolderId", dato_indice4Parameter);
        }
    
        public virtual int SpImgStatusInRepository(Nullable<int> imagen_id, string imagen_expediente, string imagen_repositorio)
        {
            var imagen_idParameter = imagen_id.HasValue ?
                new ObjectParameter("imagen_id", imagen_id) :
                new ObjectParameter("imagen_id", typeof(int));
    
            var imagen_expedienteParameter = imagen_expediente != null ?
                new ObjectParameter("imagen_expediente", imagen_expediente) :
                new ObjectParameter("imagen_expediente", typeof(string));
    
            var imagen_repositorioParameter = imagen_repositorio != null ?
                new ObjectParameter("imagen_repositorio", imagen_repositorio) :
                new ObjectParameter("imagen_repositorio", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpImgStatusInRepository", imagen_idParameter, imagen_expedienteParameter, imagen_repositorioParameter);
        }
    
        public virtual int SpLogApi(Nullable<int> idError, string validadorUi, string codigoError, string nombreArchivo, string codigoArea, string nombreExpediente, string nombreCarpeta)
        {
            var idErrorParameter = idError.HasValue ?
                new ObjectParameter("IdError", idError) :
                new ObjectParameter("IdError", typeof(int));
    
            var validadorUiParameter = validadorUi != null ?
                new ObjectParameter("ValidadorUi", validadorUi) :
                new ObjectParameter("ValidadorUi", typeof(string));
    
            var codigoErrorParameter = codigoError != null ?
                new ObjectParameter("CodigoError", codigoError) :
                new ObjectParameter("CodigoError", typeof(string));
    
            var nombreArchivoParameter = nombreArchivo != null ?
                new ObjectParameter("NombreArchivo", nombreArchivo) :
                new ObjectParameter("NombreArchivo", typeof(string));
    
            var codigoAreaParameter = codigoArea != null ?
                new ObjectParameter("CodigoArea", codigoArea) :
                new ObjectParameter("CodigoArea", typeof(string));
    
            var nombreExpedienteParameter = nombreExpediente != null ?
                new ObjectParameter("NombreExpediente", nombreExpediente) :
                new ObjectParameter("NombreExpediente", typeof(string));
    
            var nombreCarpetaParameter = nombreCarpeta != null ?
                new ObjectParameter("NombreCarpeta", nombreCarpeta) :
                new ObjectParameter("NombreCarpeta", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpLogApi", idErrorParameter, validadorUiParameter, codigoErrorParameter, nombreArchivoParameter, codigoAreaParameter, nombreExpedienteParameter, nombreCarpetaParameter);
        }
    
        public virtual ObjectResult<SpLogTd_Result> SpLogTd(string imagen_expediente, string imagen_repositorio)
        {
            var imagen_expedienteParameter = imagen_expediente != null ?
                new ObjectParameter("imagen_expediente", imagen_expediente) :
                new ObjectParameter("imagen_expediente", typeof(string));
    
            var imagen_repositorioParameter = imagen_repositorio != null ?
                new ObjectParameter("imagen_repositorio", imagen_repositorio) :
                new ObjectParameter("imagen_repositorio", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SpLogTd_Result>("SpLogTd", imagen_expedienteParameter, imagen_repositorioParameter);
        }
    
        public virtual int SpNoteBook(string dato_indice4)
        {
            var dato_indice4Parameter = dato_indice4 != null ?
                new ObjectParameter("dato_indice4", dato_indice4) :
                new ObjectParameter("dato_indice4", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpNoteBook", dato_indice4Parameter);
        }
    
        public virtual ObjectResult<SpSeries_Result> SpSeries(string dato_indice2, string dato_indice4)
        {
            var dato_indice2Parameter = dato_indice2 != null ?
                new ObjectParameter("dato_indice2", dato_indice2) :
                new ObjectParameter("dato_indice2", typeof(string));
    
            var dato_indice4Parameter = dato_indice4 != null ?
                new ObjectParameter("dato_indice4", dato_indice4) :
                new ObjectParameter("dato_indice4", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SpSeries_Result>("SpSeries", dato_indice2Parameter, dato_indice4Parameter);
        }
    
        public virtual ObjectResult<SpValidarNumeroExpediente_Result> SpValidarNumeroExpediente(string expediente)
        {
            var expedienteParameter = expediente != null ?
                new ObjectParameter("expediente", expediente) :
                new ObjectParameter("expediente", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SpValidarNumeroExpediente_Result>("SpValidarNumeroExpediente", expedienteParameter);
        }
    
        public virtual ObjectResult<string> CONSULTAR_ARCHIVOS_ZIP(Nullable<int> iD_LOTE, string iNSTANCIA, string cUADERNO, string tIPO)
        {
            var iD_LOTEParameter = iD_LOTE.HasValue ?
                new ObjectParameter("ID_LOTE", iD_LOTE) :
                new ObjectParameter("ID_LOTE", typeof(int));
    
            var iNSTANCIAParameter = iNSTANCIA != null ?
                new ObjectParameter("INSTANCIA", iNSTANCIA) :
                new ObjectParameter("INSTANCIA", typeof(string));
    
            var cUADERNOParameter = cUADERNO != null ?
                new ObjectParameter("CUADERNO", cUADERNO) :
                new ObjectParameter("CUADERNO", typeof(string));
    
            var tIPOParameter = tIPO != null ?
                new ObjectParameter("TIPO", tIPO) :
                new ObjectParameter("TIPO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("CONSULTAR_ARCHIVOS_ZIP", iD_LOTEParameter, iNSTANCIAParameter, cUADERNOParameter, tIPOParameter);
        }
    
        public virtual ObjectResult<CONSULTAR_DESPACHOS_X_MUNICIPIO_Result> CONSULTAR_DESPACHOS_X_MUNICIPIO(string mUNICIPIO)
        {
            var mUNICIPIOParameter = mUNICIPIO != null ?
                new ObjectParameter("MUNICIPIO", mUNICIPIO) :
                new ObjectParameter("MUNICIPIO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CONSULTAR_DESPACHOS_X_MUNICIPIO_Result>("CONSULTAR_DESPACHOS_X_MUNICIPIO", mUNICIPIOParameter);
        }
    
        public virtual ObjectResult<CONSULTAR_DOCUMENTOS_EXPEDIENTE_Result> CONSULTAR_DOCUMENTOS_EXPEDIENTE(Nullable<int> lOTE_ID)
        {
            var lOTE_IDParameter = lOTE_ID.HasValue ?
                new ObjectParameter("LOTE_ID", lOTE_ID) :
                new ObjectParameter("LOTE_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CONSULTAR_DOCUMENTOS_EXPEDIENTE_Result>("CONSULTAR_DOCUMENTOS_EXPEDIENTE", lOTE_IDParameter);
        }
    
        public virtual ObjectResult<CONSULTAR_EXPEDIENTES_Result> CONSULTAR_EXPEDIENTES(string dEMANDANTE, string dEMANDADO, string rADICADO, string dESPACHO)
        {
            var dEMANDANTEParameter = dEMANDANTE != null ?
                new ObjectParameter("DEMANDANTE", dEMANDANTE) :
                new ObjectParameter("DEMANDANTE", typeof(string));
    
            var dEMANDADOParameter = dEMANDADO != null ?
                new ObjectParameter("DEMANDADO", dEMANDADO) :
                new ObjectParameter("DEMANDADO", typeof(string));
    
            var rADICADOParameter = rADICADO != null ?
                new ObjectParameter("RADICADO", rADICADO) :
                new ObjectParameter("RADICADO", typeof(string));
    
            var dESPACHOParameter = dESPACHO != null ?
                new ObjectParameter("DESPACHO", dESPACHO) :
                new ObjectParameter("DESPACHO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CONSULTAR_EXPEDIENTES_Result>("CONSULTAR_EXPEDIENTES", dEMANDANTEParameter, dEMANDADOParameter, rADICADOParameter, dESPACHOParameter);
        }
    
        public virtual ObjectResult<CONSULTAR_EXPEDIENTES_TODOS_Result> CONSULTAR_EXPEDIENTES_TODOS(string dESPACHO)
        {
            var dESPACHOParameter = dESPACHO != null ?
                new ObjectParameter("DESPACHO", dESPACHO) :
                new ObjectParameter("DESPACHO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CONSULTAR_EXPEDIENTES_TODOS_Result>("CONSULTAR_EXPEDIENTES_TODOS", dESPACHOParameter);
        }
    
        public virtual ObjectResult<string> CONSULTAR_MUNICIPIOS()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("CONSULTAR_MUNICIPIOS");
        }
    
        public virtual ObjectResult<string> CONSULTAR_RUTA_ZIP_CD_PLANO(Nullable<int> lOTE_ID, string nOMBRE_ARCHIVO)
        {
            var lOTE_IDParameter = lOTE_ID.HasValue ?
                new ObjectParameter("LOTE_ID", lOTE_ID) :
                new ObjectParameter("LOTE_ID", typeof(int));
    
            var nOMBRE_ARCHIVOParameter = nOMBRE_ARCHIVO != null ?
                new ObjectParameter("NOMBRE_ARCHIVO", nOMBRE_ARCHIVO) :
                new ObjectParameter("NOMBRE_ARCHIVO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("CONSULTAR_RUTA_ZIP_CD_PLANO", lOTE_IDParameter, nOMBRE_ARCHIVOParameter);
        }
    
        public virtual ObjectResult<string> CONSULTAR_RUTA_ZIP_EXPEDIENTE(Nullable<int> lOTE_ID)
        {
            var lOTE_IDParameter = lOTE_ID.HasValue ?
                new ObjectParameter("LOTE_ID", lOTE_ID) :
                new ObjectParameter("LOTE_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("CONSULTAR_RUTA_ZIP_EXPEDIENTE", lOTE_IDParameter);
        }
    
        public virtual ObjectResult<REPORTE_PRODUCCION_Result> REPORTE_PRODUCCION(Nullable<System.DateTime> fECHAINICIAL, Nullable<System.DateTime> fECHAFINAL, string dESPACHO, string mUNICIPIO)
        {
            var fECHAINICIALParameter = fECHAINICIAL.HasValue ?
                new ObjectParameter("FECHAINICIAL", fECHAINICIAL) :
                new ObjectParameter("FECHAINICIAL", typeof(System.DateTime));
    
            var fECHAFINALParameter = fECHAFINAL.HasValue ?
                new ObjectParameter("FECHAFINAL", fECHAFINAL) :
                new ObjectParameter("FECHAFINAL", typeof(System.DateTime));
    
            var dESPACHOParameter = dESPACHO != null ?
                new ObjectParameter("DESPACHO", dESPACHO) :
                new ObjectParameter("DESPACHO", typeof(string));
    
            var mUNICIPIOParameter = mUNICIPIO != null ?
                new ObjectParameter("MUNICIPIO", mUNICIPIO) :
                new ObjectParameter("MUNICIPIO", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<REPORTE_PRODUCCION_Result>("REPORTE_PRODUCCION", fECHAINICIALParameter, fECHAFINALParameter, dESPACHOParameter, mUNICIPIOParameter);
        }
    
        public virtual ObjectResult<SpEquivalencia_Integrador_Result> SpEquivalencia_Integrador(string despacho, string serie)
        {
            var despachoParameter = despacho != null ?
                new ObjectParameter("despacho", despacho) :
                new ObjectParameter("despacho", typeof(string));
    
            var serieParameter = serie != null ?
                new ObjectParameter("Serie", serie) :
                new ObjectParameter("Serie", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SpEquivalencia_Integrador_Result>("SpEquivalencia_Integrador", despachoParameter, serieParameter);
        }
    }
}
