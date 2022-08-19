using Lider.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Lider
{
    class Program
    {
        //Prueba de modificación
        public static Parametros parametersApi = null;
        public static List<initialDate> initialDate = null;
        public static DateTime StartDate { get; set; }
        public static DateTime EndDate { get; set; }
        public static List<urlApiI> EndPointApi { get; set; }
        static HttpClient client = new HttpClient();
        public log_error log { get; set; }

        public static TRDAreaSerieSubSerieResponse tRDAreaSerieSubSerieResponse = new TRDAreaSerieSubSerieResponse ();

        static void Main()
        {
            EndPointApi = GetUrlApi();
            parametersApi = GetParameters();
            initialDate = GetInitalDate();

            if (parametersApi.Count == 0)
            {
                // Enviar correo informativo
                Console.WriteLine("No existen parametros!!! por favor validar.");
            }
            else
            {
                RunAsync().GetAwaiter().GetResult();
            }
        }

        /// <summary>
        /// Parametrizamos clase HttpClient para la consulta a la API y llamamos a los diferentes metodos
        /// </summary>
        /// <returns>void</returns>
        static async Task RunAsync()
        {
            try
            {
                // Update port # in the following line.
                AddHeaders();
                Login login = new Login
                {
                    UData = parametersApi.ParametersApi[0].usuario,
                    AppKey = parametersApi.ParametersApi[0].password
                };

                LoginResponse loginResponse = await LoginServer(login);
                List<DataToSend> records = GetDataToSendApi();

                //

                // LLamamos a los diferentes metodos de la API necesarios para el envío de la información
                tRDAreaSerieSubSerieResponse = await TRDASSParameters(loginResponse);
                ResponseExpendiente resp = await CrearExpedienteParameters(loginResponse, records);
                Console.WriteLine(resp.Error);
                
                //CarpetaResponse carpetaResponse = await CarpetasParameters(loginResponse);
                //CuadernoRespone cuadernoRespone = await CuadernosParameters(loginResponse);

                //UserPorAreaResponse respUserPorArea = await CallUserPorAreaParametersAsync(loginResponse, 2585);
                //TdrEmpresaReponse tdrEmpresaReponse = await TRDEmpresaParameters(loginResponse);
                //ExpPorCUIResponse expPorCUIResponse = await ExpPorCUIParameters(loginResponse);
                //TdrEmpresaReponse tdrEmpresaReponseTemporal = await CrearDocumentoParameters(loginResponse);

                Console.ReadLine();
            }
            catch (Exception e)
            {
                log_error l = new log_error
                {
                    log_error1 = e.Message                    
                };
                SaveLogs(l);
            }            
        }

        #region PUENTE TO CALL API
        static async Task<UserPorAreaResponse> CallUserPorAreaParametersAsync(LoginResponse loginResponse, int IdArea)
        {
            ParametersUDA parameters = new ParametersUDA
            {
                IdArea = IdArea
            };

            UserPorAreaResponse usuarioPorArea = await UsuarioDeArea(loginResponse, parameters);
            return usuarioPorArea;
        }

        static async Task<TRDAreaSerieSubSerieResponse> TRDASSParameters(LoginResponse login)
        {
            ParametersTrdASS parameters = new ParametersTrdASS
            {
                CodigoArea = "500013110001",
                CodigoSerie = "270",
                CodigoSubSerie = "270-85"
            };

            TRDAreaSerieSubSerieResponse trdASS = await TrdASS(login, parameters);
            return trdASS;
        }

        static async Task<TdrEmpresaReponse> TRDEmpresaParameters(LoginResponse login)
        {
            ParametersTrdEmpresa parameters = new ParametersTrdEmpresa
            {
                IdAreaEmpresa = "2587"
            };

            TdrEmpresaReponse trdASS = await TrdPorEmpresa(login, parameters);
            return trdASS;
        }

        static async Task<CuadernoRespone> CuadernosParameters(LoginResponse login)
        {
            ParametersCuadernos parameters = new ParametersCuadernos
            {
                CUI = "2587"
            };

            CuadernoRespone trdASS = await Cuadernos(login, parameters);
            return trdASS;
        }

        static async Task<CarpetaResponse> CarpetasParameters(LoginResponse login)
        {
            ParametersFolder parameters = new ParametersFolder
            {
                
            };

            CarpetaResponse trdASS = await Carpetas(login, parameters);
            return trdASS;
        }

        static async Task<ExpPorCUIResponse> ExpPorCUIParameters(LoginResponse login)
        {
            ParametersCUI parameters = new ParametersCUI
            {
                CUI = "2587"
            };

            ExpPorCUIResponse trdASS = await ExpPorCUI(login, parameters);
            return trdASS;
        }

        static async Task<TdrEmpresaReponse> CrearDocumentoParameters(LoginResponse loginResponse)
        {
            ParametersCD parameters = new ParametersCD
            {
                IdExpediente = string.Empty,
                IdTipoDocumental = string.Empty,
                Nombre = string.Empty,
                Archivo = null,
                Metadatos = null,
                FechaCreacionDocumento = string.Empty,
                IdCarpeta = string.Empty,
                Autor = string.Empty,
                FechaTransmicion = string.Empty,
                FechaRecepcion = string.Empty,
                Descripcion = string.Empty,
                Version = string.Empty,
                IdAreaEmpresa = string.Empty,
                VerPublico = string.Empty,
                IdCuaderno = string.Empty
            };

            TdrEmpresaReponse trdASS = await CrearDocumento(loginResponse, parameters);
            return trdASS;
        }
        
        static async Task<ResponseExpendiente> CrearExpedienteParameters(LoginResponse loginResponse, List<DataToSend> records)
        {
            ParametersCE parameter = new ParametersCE
            {
                IdTablaRetencionDocumental = tRDAreaSerieSubSerieResponse.Data[0].IdTablaRetencionDocumental,
                IdVersionTablaRetencionDocumental = tRDAreaSerieSubSerieResponse.Data[0].IdVersionTablaDocumental,
                IdUsuarioResponsable = 407,
                Nombre = tRDAreaSerieSubSerieResponse.Data[0].CodigoTablaRetencionDocumental,
                Descripcion = null,
                FechaFinal = 1,
                NumeroRadicacionProceso = tRDAreaSerieSubSerieResponse.Data[0].CodigoTablaRetencionDocumental,
                IdCiudad = tRDAreaSerieSubSerieResponse.Data[0].IdCiudad,
                IdAreaEmpresa = tRDAreaSerieSubSerieResponse.Data[0].IdAreaEmpresa,
                Metadatos = new List<object> { }
            };

            return await CrearExpediente(loginResponse, parameter);
        }

        //static async Task<TdrEmpresaReponse> CrearExpedienteParameters(LoginResponse login)
        //{
        //    ParametersTrdEmpresa parameters = new ParametersTrdEmpresa
        //    {
        //        IdAreaEmpresa = "2587"
        //    };

        //    TdrEmpresaReponse trdASS = await CrearExpediente(login, parameters);
        //    return trdASS;
        //}

        #endregion

        #region CONSULTAMOS DATA A BASE DE DATOS PARA OBTENER DATA A ENVIAR
        /// <summary>
        /// Consultamos api y obtenemos los registros en el rango de fecha indicado
        /// </summary>
        /// <param name="login">Clase login con las variables necesarias para la consulta</param>
        /// <returns></returns>
        static List<DataToSend> GetDataToSendApi()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[GetDataToSendApi] 'consultar'");
            List<DataToSend> records = db.Database.SqlQuery<DataToSend>(query).ToList();

            // Sino devuelve null
            return records;
        }
        
        #endregion

        #region CONSULTAMOS A LAS API
        /// <summary>
        /// Realiza login y obtenemos token
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<LoginResponse> LoginServer(Login login)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            LoginResponse res = await response.Content.ReadAsAsync<LoginResponse>();

            // return URI of the created resource.
            return res;
        }

        /// <summary>
        /// Realizamos recorrido de los datos obtenidos 
        /// </summary>
        /// <param name="registers">Data obenida de la Api</param>
        static async Task<ResponseExpendiente> CrearExpediente(LoginResponse login, ParametersCE parameters)
        {
            // Aqui llamamos a la Api a enviarle la data
            try
            {
                CrearExpediente expediente = new CrearExpediente
                {
                    Token = login.Data.Token,
                    AppKey = parametersApi.ParametersApi[0].password
                };

                ExecutionObjectCE executionObject = new ExecutionObjectCE
                {
                    Name = "Expedientes"
                };

                WebServiceMethodCE webServiceMethod = new WebServiceMethodCE
                {
                    Name = "InsertExpediente"
                };
                executionObject.WebServiceMethod = webServiceMethod;
                webServiceMethod.Parameters = parameters;
                expediente.ExecutionObject = executionObject;

                //StartDate = initialDate[0].startDate;
                //EndDate = DateTime.Now;
                string endPoint = EndPointApi[1].patch;                

                HttpResponseMessage response = await client.PostAsJsonAsync(endPoint, expediente);
                response.EnsureSuccessStatusCode();

                ResponseExpendiente res = await response.Content.ReadAsAsync<ResponseExpendiente>();

                return res;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Realiza login y obtenemos token
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<TdrEmpresaReponse> TrdPorEmpresa(LoginResponse login, ParametersTrdEmpresa parameter)
        {
            TRDPOREMPRESA trdEmpresa = new TRDPOREMPRESA();
            trdEmpresa.Token = login.Data.Token;
            trdEmpresa.AppKey = parametersApi.ParametersApi[0].password;
            trdEmpresa.ExecutionObject.Name = "TRD";
            trdEmpresa.ExecutionObject.WebServiceMethod.Name = "GetTRDIdAreaEmpresa";
            trdEmpresa.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            TdrEmpresaReponse res = await response.Content.ReadAsAsync<TdrEmpresaReponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<TRDAreaSerieSubSerieResponse> TrdASS(LoginResponse login, ParametersTrdASS parameter)
        {
            TRDAreaSerieSubSerie trdSSS = new TRDAreaSerieSubSerie
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password
            };

            ExecutionObjectTrdASS executionObject = new ExecutionObjectTrdASS
            {
                Name = "TRD"
            };

            WebServiceMethodTrdASS webServiceMethod = new WebServiceMethodTrdASS
            {
                Name = "GetTRDFilter"
            };
            webServiceMethod.Parameters = parameter;
            executionObject.WebServiceMethod = webServiceMethod;
            trdSSS.ExecutionObject = executionObject;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[3].patch, trdSSS);
            response.EnsureSuccessStatusCode();

            TRDAreaSerieSubSerieResponse res = await response.Content.ReadAsAsync<TRDAreaSerieSubSerieResponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="loginResponse">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<UserPorAreaResponse> UsuarioDeArea(LoginResponse loginResponse, ParametersUDA parameter)
        {
            UsuarioDeArea usrDeArea = new UsuarioDeArea();
            usrDeArea.Token = loginResponse.Data.Token;
            usrDeArea.AppKey = parametersApi.ParametersApi[0].password;
            usrDeArea.ExecutionObject.Name = "Usuario";
            usrDeArea.ExecutionObject.WebServiceMethod.Name = "GetUsuarioArea";
            usrDeArea.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, loginResponse);
            response.EnsureSuccessStatusCode();

            UserPorAreaResponse res = await response.Content.ReadAsAsync<UserPorAreaResponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<TdrEmpresaReponse> CrearDocumento(LoginResponse login, ParametersCD parameter)
        {
            CrearDocumento crearDocumento = new CrearDocumento();
            crearDocumento.Token = login.Data.Token;
            crearDocumento.AppKey = parametersApi.ParametersApi[0].password;
            crearDocumento.ExecutionObject.Name = "Documentos";
            crearDocumento.ExecutionObject.WebServiceMethod.Name = "CrearDocumento";
            crearDocumento.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            TdrEmpresaReponse res = await response.Content.ReadAsAsync<TdrEmpresaReponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        

        static async Task<ExpPorCUIResponse> ExpPorCUI(LoginResponse login, ParametersCUI parameter)
        {
            ExpPorCUI expPorCUI = new ExpPorCUI();
            expPorCUI.Token = login.Data.Token;
            expPorCUI.AppKey = parametersApi.ParametersApi[0].password;
            expPorCUI.ExecutionObject.Name = "Documentos";
            expPorCUI.ExecutionObject.WebServiceMethod.Name = "CrearDocumento";
            expPorCUI.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            ExpPorCUIResponse res = await response.Content.ReadAsAsync<ExpPorCUIResponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<CarpetaResponse> Carpetas(LoginResponse login, ParametersFolder parameter)
        {
            Folder folder = new Folder();
            folder.Token = login.Data.Token;
            folder.AppKey = parametersApi.ParametersApi[0].password;
            folder.ExecutionObject.Name = "Carpetas";
            folder.ExecutionObject.WebServiceMethod.Name = "GetCarpetas";
            folder.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            CarpetaResponse res = await response.Content.ReadAsAsync<CarpetaResponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<CuadernoRespone> Cuadernos(LoginResponse login, ParametersCuadernos parameter)
        {
            Cuadernos cuadernos = new Cuadernos();
            cuadernos.Token = login.Data.Token;
            cuadernos.AppKey = parametersApi.ParametersApi[0].password;
            cuadernos.ExecutionObject.Name = "Cuaderno";
            cuadernos.ExecutionObject.WebServiceMethod.Name = "GetCuadernosCUI";
            cuadernos.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            CuadernoRespone res = await response.Content.ReadAsAsync<CuadernoRespone>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<CrearCuadernoResponse> CrearCuadernos(LoginResponse login, ParametersCC parameter)
        {
            CreacionCuadernos creacionCuadernos = new CreacionCuadernos();
            creacionCuadernos.Token = login.Data.Token;
            creacionCuadernos.AppKey = parametersApi.ParametersApi[0].password;
            creacionCuadernos.ExecutionObject.Name = "Cuaderno";
            creacionCuadernos.ExecutionObject.WebServiceMethod.Name = "CrearCuaderno";
            creacionCuadernos.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            CrearCuadernoResponse res = await response.Content.ReadAsAsync<CrearCuadernoResponse>();

            return res;
        }

        private static void AddHeaders()
        {
            string urlApiI = string.Concat(parametersApi.ParametersApi[0].urlApi);
            client.BaseAddress = new Uri(urlApiI);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        #endregion

        #region CONSULTAS PARAMETROS INICIALES
        /// <summary>
        /// Obtenemos parametros iniciales
        /// </summary>
        /// <returns>Registro obtenido de base de datos</returns>
        static Parametros GetParameters()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            var result = (from p in db.parametersApi
                            where p.estado == true
                            select p).ToList();

            Parametros param = new Parametros
            {
                ParametersApi = result,
                Count = result.Count()
            };
            return param;                
        }

        /// <summary>
        /// Obtenemos los fecha iniciales en la tabla initialDate
        /// </summary>
        /// <returns>Registro obtenido de la base de datos</returns>
        static List<initialDate> GetInitalDate()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
                
            List<initialDate> result = (from d in db.initialDate
                                        where d.estado == true 
                                        select d).ToList();

            return result;
        }

        /// <summary>
        /// Actualizamos columna startDate
        /// </summary>
        /// <returns>booleano</returns>
        static bool UpdateLastDate()
        {
            int id = initialDate[0].id;
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            initialDate initialDa = db.initialDate.First(d => d.estado == true & d.id == id);
            initialDa.startDate = EndDate;
            db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Obtenemos el listado de url a consultar en el Api Rest de ITBX
        /// </summary>
        /// <returns>resultado obtenido de la base de datos</returns>
        static List<urlApiI> GetUrlApi()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();

            List<urlApiI> result = (from u in db.urlApiI
                                    select u).ToList();
            return result;
        }

        /// <summary>
        /// Guardamos registros de logs
        /// </summary>
        /// <param name="logs">Guardamos registro de los logs obtenidos</param>
        static void SaveLogs(log_error log)
        {
            using (SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities())
            {
                db.log_error.Add(log);
                db.SaveChanges();
            }
        }
        #endregion

        #region CLASES
        /*** CLASES ***/
        public class Login
        {
            public string UData { get; set; }
            public string AppKey { get; set; }
        }

        public class Parametros
        {
            public List<parametersApi> ParametersApi { get; set; }
            public int Count { get; set; }
        }
        #endregion
    }
}
