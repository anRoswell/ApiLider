using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lider.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Current
    {
        public object ResultValue { get; set; }
        public object InfoRule { get; set; }
        public object Model { get; set; }
        public int Id { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public object ExtraMessage { get; set; }
        public object Type { get; set; }
        public bool IsBusiness { get; set; }
    }

    public class DataCCR
    {
    }

    public class ExCCR
    {
        public Data Data { get; set; }
        public List<object> InfoRowsProcess { get; set; }
        public object Validation { get; set; }
        public string Message { get; set; }
        public bool ViewInUI { get; set; }
        public object Ex { get; set; }
        public bool IsProcesed { get; set; }
        public Current Current { get; set; }
        public object InnerException { get; set; }
        public string StackTrace { get; set; }
        public object HelpLink { get; set; }
        public string Source { get; set; }
        public int HResult { get; set; }
    }

    public class ExecutionObjectCCR
    {
        public object MethodNameUI { get; set; }
        public object Token { get; set; }
        public object AppKey { get; set; }
        public string ReturnQueryType { get; set; }
        public object EventName { get; set; }
        public object ControlName { get; set; }
        public bool IsRule { get; set; }
        public List<object> ObjectValues { get; set; }
        public List<object> ObjectsToRun { get; set; }
        public object AccessKey { get; set; }
        public WebServiceMethod WebServiceMethod { get; set; }
        public object Model { get; set; }
        public object ExtraObject { get; set; }
        public List<object> ValuesToReplace { get; set; }
        public List<object> TagsToReplace { get; set; }
        public List<object> ValuesToSet { get; set; }
        public bool IsTest { get; set; }
        public object Asynchronous { get; set; }
        public int ResultType { get; set; }
        public bool SetResultTypeToChilds { get; set; }
        public object BaseInfoQueryParams { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public List<object> Traza { get; set; }
        public object PaginationSettings { get; set; }
        public object Parameters { get; set; }
        public object TaskSchedulerObject { get; set; }
        public object FlowTemplate { get; set; }
        public object ExportImportItem { get; set; }
        public object DestinationTemplate { get; set; }
        public string CrudAction { get; set; }
        public object IsInternalExecution { get; set; }
        public object CurrentQuery { get; set; }
        public object Label { get; set; }
        public string Name { get; set; }
        public string GUID { get; set; }
        public string ObjectType { get; set; }
        public object Value { get; set; }
        public bool Active { get; set; }
        public bool SaveinProcess { get; set; }
        public object FilterMetadata { get; set; }
    }

    public class MetadataCCR
    {
    }

    public class ParametersCCR
    {
        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public int IdCarpeta { get; set; }
        public int IdExpediente { get; set; }
    }

    public class ProcessData
    {
        public bool ReturnProcessModel { get; set; }
        public bool DisableReturnErrorTrace { get; set; }
        public bool DisableSaveExecuteObject { get; set; }
        public bool DisableSaveTrace { get; set; }
        public SecurityModel SecurityModel { get; set; }
        public ExecutionObject ExecutionObject { get; set; }
        public string Tipo { get; set; }
        public string Tiempo { get; set; }
        public string Traza { get; set; }
        public string Validaciones { get; set; }
        public string ErroresInternos { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public ExCCR Ex { get; set; }
        public object ObjetoValidaciones { get; set; }
        public Result Result { get; set; }
    }

    public class QMetadataCCR
    {
    }

    public class Result
    {
        public string Error { get; set; }
        public string ObjetoValidaciones { get; set; }
        public string AppGUID { get; set; }
        public string AppName { get; set; }
        public string ExecutionObject { get; set; }
        public string ExecutionObjectType { get; set; }
        public string ExecutionObjectName { get; set; }
        public string ExecutionObjectGUID { get; set; }
        public string SubExecutionObjectType { get; set; }
        public string SubExecutionObjectName { get; set; }
        public string SubExecutionObjectGUID { get; set; }
        public string Model { get; set; }
        public string ProcessModel { get; set; }
    }

    public class CrearCuadernoResponse
    {
        public bool IsAuthenticated { get; set; }
        public object ResultValue { get; set; }
        public object InfoRule { get; set; }
        public object ExecutionObject { get; set; }
        public Metadata Metadata { get; set; }
        public QMetadata QMetadata { get; set; }
        public ProcessData ProcessData { get; set; }
        public string ValidationUI { get; set; }
        public bool Ok { get; set; }
        public string CodeError { get; set; }
        public string Error { get; set; }
        public string TrazaError { get; set; }
        public object Data { get; set; }
    }

    public class SecurityModel
    {
        public int IdEmpresa { get; set; }
        public string Empresa { get; set; }
        public int IdAreaEmpresa { get; set; }
        public string AreaEmpresa { get; set; }
        public int IdSedeEmpresa { get; set; }
        public string SedeEmpresa { get; set; }
        public string Grupos { get; set; }
        public int IdRole { get; set; }
        public string Role { get; set; }
        public int IdTipoBusquedaAreaEmpresa { get; set; }
        public bool TodosPermisosExpedientes { get; set; }
        public bool TodosPermisosDocumentosExpedientes { get; set; }
        public bool TodosPermisosPaginas { get; set; }
        public bool TodosPermisosMenus { get; set; }
        public bool TodosPermisosQuerys { get; set; }
        public bool TodosPermisosReglas { get; set; }
        public bool UsuarioEsParaAccesoAnonimo { get; set; }
        public bool RoleEsParaAccesoAnonimo { get; set; }
        public string IpServer { get; set; }
        public string IpClient { get; set; }
        public string Pais { get; set; }
        public string UserName { get; set; }
        public string User { get; set; }
        public object Password { get; set; }
        public int UserId { get; set; }
        public string Identificacion { get; set; }
        public int IdTipoIdentificacion { get; set; }
        public string Email { get; set; }
        public string CodigoGrupo { get; set; }
        public string CodigoUsuario { get; set; }
        public string CodigoRole { get; set; }
        public string CodigoAreaEmpresa { get; set; }
        public string IdAreasEmpresa { get; set; }
        public string AreasEmpresa { get; set; }
        public object AreasEmpresaUsuario { get; set; }
        public int IdDominioUsuarios { get; set; }
        public object DominioUsuarios { get; set; }
        public bool PermiteFirmar { get; set; }
        public object ImagenFirma { get; set; }
        public int IdTipoFirma { get; set; }
        public string UrlSitio { get; set; }
        public string NombreBiblioteca { get; set; }
        public string UsuarioSitio { get; set; }
        public string PasswordSitio { get; set; }
        public string DominioSitio { get; set; }
        public string UsuarioServicioFirma { get; set; }
        public string PasswordServicioFirma { get; set; }
        public string TokenSegundaAuth { get; set; }
        public object Configuracion { get; set; }
        public string Url { get; set; }
        public object Imei { get; set; }
        public string Token { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int SessionTime { get; set; }
        public object SessionUnitTime { get; set; }
        public string MaquinaCreacion { get; set; }
        public bool ConsutarIp { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string OperatingSystem { get; set; }
        public bool IsAnonymous { get; set; }
        public int Id { get; set; }
        public string AppKey { get; set; }
        public int IdGuidAcceso { get; set; }
        public string IdGrupos { get; set; }
    }

    public class WebServiceMethodCCR
    {
        public object PaginationSettings { get; set; }
        public ParametersCCR Parameters { get; set; }
        public object Label { get; set; }
        public string Name { get; set; }
        public string GUID { get; set; }
        public string ObjectType { get; set; }
        public object Value { get; set; }
        public bool Active { get; set; }
        public bool SaveinProcess { get; set; }
        public object FilterMetadata { get; set; }
    }


}
