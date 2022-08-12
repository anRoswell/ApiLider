using ConsoleSI58.Models;
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
        public static Parametros parameters = null;
        public static List<initialDate> initialDate = null;
        public static DateTime StartDate { get; set; }
        public static DateTime EndDate { get; set; }
        public static List<urlApiItbx> UrlApiItbx { get; set; }
        static HttpClient client = new HttpClient();
        public logs log { get; set; }

        static void Main()
        {
            UrlApiItbx = GeturlApiItbx();
            parameters = GetParameters();
            initialDate = GetInitalDate();
            if (parameters.count == 0)
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
                client.BaseAddress = new Uri(parameters.ParametersPbx[0].uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Login login = new Login
                {
                    email = parameters.ParametersPbx[0].email,
                    password = parameters.ParametersPbx[0].password
                };
                LoginResponse result = await LoginServer(login);
                PbxRegisters resp = await GetRegistroPBXApiRest(result);
                Console.WriteLine("Consulta exitosa, bye!!!");
                //Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                logs l = new logs
                {
                    description = e.Message                    
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
            HttpResponseMessage response = await client.PostAsJsonAsync(
                UrlApiItbx[0].patch, login);
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
        static async Task<PbxRegisters> GetRegistroPBXApiRest(LoginResponse login)
        {
            string token = login.access_token;
            StartDate = initialDate[0].startDate;
            EndDate = DateTime.Now;

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + login.access_token);

            var response = await client.GetAsync(UrlApiItbx[2].patch + string.Format("?startdate={0}&enddate={1}", StartDate.ToString("yyyy-MM-dd HH:mm:ss"), EndDate.ToString("yyyy-MM-dd HH:mm:ss")));

            // Si el servicio responde correctamente
            if (response.IsSuccessStatusCode)
            {
                // Lee el response y lo deserializa como un Product
                PbxRegisters resp  = await response.Content.ReadAsAsync<PbxRegisters>();
                if (resp.Total != 0)
                    SaveRegisters(resp);
                UpdateLastDate();
                return resp;
            }
            // Sino devuelve null
            return await Task.FromResult<PbxRegisters>(null);
        }

        /// <summary>
        /// Realizamos recorrido de los datos obtenidos 
        /// </summary>
        /// <param name="registers">Data obenida de la Api</param>
        static void SaveRegisters(PbxRegisters registers)
        {
            if (registers != null)
            {
                using (SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities())
                {
                    List<registroPbx> result2 = registers.Results.Where(b => serviciosToSave.Any(a => b.queue.Equals(a.codigo))).ToList();
                    if(result2.Count > 0)
                    {
                        foreach (var pbx in result2)
                        {
                            var result = from p in db.registroPbx
                                         where p.id == pbx.id
                                         select p;
                            int count = result.Count();
                            if (count == 0)
                            {
                                db.registroPbx.Add(pbx);
                                db.SaveChanges();
                            }
                        }
                    }                    
                }
            }
        }

        /// <summary>
        /// Obtenemos parametros iniciales
        /// </summary>
        /// <returns>Registro obtenido de base de datos</returns>
        static Parametros GetParameters()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            var result = (from p in db.parametersPbx
                            where p.estado == true
                            select p).ToList();

            Parametros param = new Parametros
            {
                ParametersPbx = result,
                count = result.Count()
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
            solucionesIntegrales58Entities db = new solucionesIntegrales58Entities();
            initialDate initialDa = db.initialDate.First(d => d.estado == true & d.id == id);
            initialDa.startDate = EndDate;
            db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Obtenemos el listado de url a consultar en el Api Rest de ITBX
        /// </summary>
        /// <returns>resultado obtenido de la base de datos</returns>
        static List<urlApiItbx> GeturlApiItbx()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();

            List<urlApiItbx> result = (from u in db.urlApiItbx
                                        select u).ToList();
            return result;
        }

        /// <summary>
        /// Guardamos registros de logs
        /// </summary>
        /// <param name="logs">Guardamos registro de los logs obtenidos</param>
        static void SaveLogs(logs log)
        {
            using (SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities())
            {
                db.logs.Add(log);
                db.SaveChanges();
            }
        }


        /*** CLASES ***/
        public class Login
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class LoginResponse
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        public partial class PbxRegisters
        {
            [JsonProperty("total")]
            public long Total { get; set; }

            [JsonProperty("results")]
            public registroPbx[] Results { get; set; }
        }

        public class Parametros
        {
            public List<parametersPbx> ParametersPbx { get; set; }
            public int count { get; set; }
        }
    }
}
