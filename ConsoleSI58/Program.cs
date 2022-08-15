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

        static void Main()
        {
            EndPointApi = GetUrlApi();
            parametersApi = GetParameters();
            initialDate = GetInitalDate();

            if (parametersApi.Count == 0)
            {
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

                LoginResponse result = await LoginServer(login);
                List<DataToSend> records = GetDataToSendApi();
                await CrearExpediente(result, records);

                Console.WriteLine("Consulta exitosa, bye!!!");
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

        

        #region CONSULTAMOS DATA A BASE DE DATOS
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
        static async Task CrearExpediente(LoginResponse login, List<DataToSend> records)
        {
            // Aqui llamamos a la Api a enviarle la data
            try
            {
                CREAREXPEDIENTE expediente = new CREAREXPEDIENTE();
                expediente.Token = login.Data.Token;
                expediente.AppKey = parametersApi.ParametersApi[0].password;                
                expediente.ExecutionObject.Name = "Expedientes";
                expediente.ExecutionObject.WebServiceMethod.Name = "InsertExpediente";
                expediente.ExecutionObject.WebServiceMethod.Parameters = records[0];

                string token = login.Data.Token;
                //StartDate = initialDate[0].startDate;
                EndDate = DateTime.Now;
                string endPoint = EndPointApi[1].patch;

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await client.PostAsJsonAsync(endPoint, expediente);
                response.EnsureSuccessStatusCode();

                ResponseExpendiente res = await response.Content.ReadAsAsync<ResponseExpendiente>();
            }
            catch (Exception e)
            {
                throw;
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
            TRDAreaSerieSubSerie trdSSS = new TRDAreaSerieSubSerie();
            trdSSS.Token = login.Data.Token;
            trdSSS.AppKey = parametersApi.ParametersApi[0].password;
            trdSSS.ExecutionObject.Name = "TRD";
            trdSSS.ExecutionObject.WebServiceMethod.Name = "GetTRDFilter";
            trdSSS.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            TRDAreaSerieSubSerieResponse res = await response.Content.ReadAsAsync<TRDAreaSerieSubSerieResponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<UserPorArea> UsuarioDeArea(LoginResponse login, ParametersUDA parameter)
        {
            UsuarioDeArea usrDeArea = new UsuarioDeArea();
            usrDeArea.Token = login.Data.Token;
            usrDeArea.AppKey = parametersApi.ParametersApi[0].password;
            usrDeArea.ExecutionObject.Name = "Usuario";
            usrDeArea.ExecutionObject.WebServiceMethod.Name = "GetUsuarioArea";
            usrDeArea.ExecutionObject.WebServiceMethod.Parameters = parameter;

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            response.EnsureSuccessStatusCode();

            UserPorArea res = await response.Content.ReadAsAsync<UserPorArea>();

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
                
            var result = (from d in db.initialDate
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
    }
}
