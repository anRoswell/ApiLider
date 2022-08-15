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
                string urlApiI = string.Concat(parametersApi.ParametersApi[0].urlApi);
                client.BaseAddress = new Uri(urlApiI);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Login login = new Login
                {
                    UData = parametersApi.ParametersApi[0].usuario,
                    AppKey = parametersApi.ParametersApi[0].password
                };

                LoginResponse result = await LoginServer(login);
                
                //await GetRegistroPBXApiRest(result, result);
                Console.WriteLine("Consulta exitosa, bye!!!");
                //Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log_error l = new log_error
                {
                    log_error1 = e.Message                    
                };
                SaveLogs(l);
            }            
        }

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
        /// Consultamos api y obtenemos los registros en el rango de fecha indicado
        /// </summary>
        /// <param name="login">Clase login con las variables necesarias para la consulta</param>
        /// <returns></returns>
        static async Task<datos_finales> GetDataToSendApi(LoginResponse login)
        {
            

           

            // Sino devuelve null
            return null;
        }

        /// <summary>
        /// Realizamos recorrido de los datos obtenidos 
        /// </summary>
        /// <param name="registers">Data obenida de la Api</param>
        static async Task SaveRegistersAsync(LoginResponse login, datos_finales registers)
        {
            // Aqui llamamos a la Api a enviarle la data
            try
            {
                string token = login.Data.Token;
                StartDate = initialDate[0].startDate;
                EndDate = DateTime.Now;

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
                response.EnsureSuccessStatusCode();

                JsonToSendApi res = await response.Content.ReadAsAsync<JsonToSendApi>();

                // return URI of the created resource.
                
            }
            catch (Exception e)
            {
                throw;
            }
        }

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
