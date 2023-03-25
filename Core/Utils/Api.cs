
namespace Core.Service
{
    using Core.Models;
    using Core.Models.ClassApi;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public static class Api
    {
        //#region RESPONSE
        //public static 
        //#endregion

        #region AreaEmpresa
        public static ParametersAE AreaEmpresaParameters(LoginResponse loginResponse)
        {
            ParametersAE parameter = new ParametersAE
            {

            };
            return parameter;
        }

        public static AreaEmpresa AreaEmpresa(LoginResponse login, ParametersAE parameters, string password)
        {
            AreaEmpresa areaEmpresa = new AreaEmpresa
            {
                Token = login.Data.Token,
                AppKey = password
            };
            ExecutionObjectAE executionObject = new ExecutionObjectAE
            {
                Name = "Areas"
            };

            WebServiceMethodAE webServiceMethod = new WebServiceMethodAE
            {
                Name = "GetAreas"
            };

            executionObject.WebServiceMethod = webServiceMethod;
            webServiceMethod.Parameters = parameters;
            areaEmpresa.ExecutionObject = executionObject;
            return areaEmpresa;
        }
        #endregion

        #region GET EXPEDIENTE
        public static ParametersGE ExpedienteParameters(LoginResponse loginResponse, string CUI)
        {
            ParametersGE parameter = new ParametersGE
            {
                CUI = CUI
            };

            return parameter;
        }

        public static GetExpediente GetExpedienteApi(LoginResponse login, ParametersGE parameters, string password)
        {
            GetExpediente getExpediente = new GetExpediente
            {
                Token = login.Data.Token,
                AppKey = password
            };

            ExecutionObjectGE executionObject = new ExecutionObjectGE
            {
                Name = "Expedientes"
            };

            WebServiceMethodGE webServiceMethod = new WebServiceMethodGE
            {
                Name = "GetExpedienteCUI"
            };

            webServiceMethod.Parameters = parameters;
            executionObject.WebServiceMethod = webServiceMethod;
            getExpediente.ExecutionObject = executionObject;

            return getExpediente;
        }
        #endregion

        #region CrearDocumentoParameters
        public static ArchivoCD CreateArchivoCD(string fileName, string base64)
        {
            FileField fileField = new FileField
            {
                FormatType = "Base64",
                DestinationFile = new DestinationFile { }
            };

            ArchivoCD archivoCD = new ArchivoCD()
            {
                Archivo = fileName,
                DatosArchivo = base64,
                EsAdjunto = false,
                FileField = fileField
            };

            return archivoCD;
        }

        public static List<ArchivoCD> CreateListArchivoCD(ArchivoCD archivoCD)
        {
            List<ArchivoCD> archivoCDList = new List<ArchivoCD>
            {
                archivoCD
            };

            return archivoCDList;
        }

        public static ParametersCD CrearDocumentoParameters(GetExpedienteResponse getExpedienteResponse, ClassSpFile file, List<ArchivoCD> archivoCDList, DatumOC notebook, ClassGetFolderSql folderSelected, string IdArea)
        {
            ParametersCD parameters = new ParametersCD
            {
                IdExpediente = getExpedienteResponse.Data[0].IdExpediente,
                IdTipoDocumental = file.IdTipoDocumental,
                Nombre = file.NombreImagen,
                Archivo = archivoCDList,
                Metadatos = null,
                FechaCreacionDocumento = DateTime.Now.ToString("dd/MM/yyyy"),
                IdCarpeta = folderSelected.IdCarpeta,
                Autor = "Siglo21",
                FechaTransmicion = DateTime.Now.ToString("dd/MM/yyyy"),
                FechaRecepcion = DateTime.Now.ToString("dd/MM/yyyy"),
                Descripcion = "Descripcion teste 01",
                Version = "001",
                IdAreaEmpresa = Convert.ToInt32(IdArea),
                VerPublico = false,
                IdCuaderno = notebook.IdCuaderno,
                IndiceOrdenamiento = file.OrdenImagen
            };

            return parameters;
        }
        #endregion
    }
}
