using Core.Models;
using Core.Models.ClassApi;
using Core.Repository;
using Core.Service;
using Core.Utils;
using Infraestructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Lider
{
    class Program
    {
        //Prueba de modificación
        public static Parametros parametersApi = null;
        public static DatumAE respAreaEmpresaFilter = null;
        public static ClassGetFolderSql folderSelected = null;
        public static CrearCuadernoResponse cCResp = null;
        public static bool filtroAreaEmpresa = true;
        public static List<initialDate> initialDate = null;
        public static List<TiposDocumentale> tiposDocumentale = null;
        public static GetExpedienteResponse getExpedienteResponse = null;
        public static DateTime StartDate { get; set; }
        public static DateTime EndDate { get; set; }
        public static List<urlApiI> EndPointApi { get; set; }

        static HttpResponseMessage httpResponseMessage;
        public static List<ClassFindDespacho> despachos = null;
        public static string nombreDespacho = null;
        public static string nombreExpediente = null;
        public static string IdTipoDocumental = null;
        public static string IdArea = null;
        public static string urlApi = null;
        public static string IdCiudad = null;
        public static string IdUsuario = null;
        public static string IdTrd = null;
        public static string IdVersionTrd = null;
        public static string serie = null;
        public static string password = null;
        public static int contadorSeriSubSerie = 0;
        public static int totalSeriSubSerie = 0;
        public static int contadorFiles = 0;
        public static int totalFiles = 0;
        public static int contadorFolders = 0;
        public static int totalFolders = 0;
        public static int contadorNoteBooks = 0;
        public static int totalNoteBooks = 0;
        public static int contadorNoteBookList = 0;
        public static int? totalNoteBookList = 0;

        public static Series serieSeleccionada = null;
        public static AreaEmpresaResponse areaEmpresaResponse = new AreaEmpresaResponse();

        static void Main()
        {
            try
            {
                //SplitConsole.DrawScreen();
                EndPointApi = GetUrlApi();
                parametersApi = GetParameters();
                initialDate = GetInitalDate();

                if (parametersApi.Count == 0)
                    Console.WriteLine("No existen parametros en tablas de parametrización!!! por favor validar ().EndPointApi - parametersApi - initialDate");
                else
                    RunAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                ShowConsole(string.Concat(e.Message, " ", e.InnerException?.Message), "red");
                ShowConsole("Por favor presiones enter para salir!!!");
                LogError logError = new LogError
                {
                    Message = string.Concat(e.Message, " ", e.InnerException?.Message),
                    StackTrace = e.StackTrace,
                    Type = 999
                };
                
                Exception(logError);
                Console.Read();
            }
        }

        /// <summary>
        /// Parametrizamos clase HttpClient para la consulta a la API y llamamos a los diferentes metodos
        /// </summary>
        /// <returns>void</returns>
        public static async Task RunAsync()
        {
            password = parametersApi.ParametersApi[0].password;
            urlApi = parametersApi.ParametersApi[0].urlApi;
            SolicitarDespacho();
            SeleccionarSerie();
            //GetExpediente();
            //GetIdArea();

            // Update port # in the following line.

            List<DataToSend> records = GetDataToSendApi();
            if (records.Count>0)
            {
                LoginResponse loginResponse = await LoginAsync();
                ShowConsole("Obteniendo token!!!");
                //LoginResponse loginResponse = await LoginServer(login);

                if (loginResponse.Ok)
                {
                    ShowConsole($"Consultando los expedientes del despacho {nombreDespacho}", "green");
                    List<SerieSubSerieExpediente> serieSubserieExpediente = spBuscarExpdientesSerie();

                    if (serieSubserieExpediente.Count == 0)
                        throw new Exception($"Sin expedientes a procesar, con la serie/subserie {serieSeleccionada.Serie_SubSerie}, Despacho: {nombreDespacho}");

                    EquivalenciasIntegrador equivalencia_Integrador = Equivalencia_Integrador(nombreDespacho, serieSubserieExpediente[0].Serie_SubSerie);
                    if (equivalencia_Integrador == null)
                        throw new Exception("Sin registros en equivalencia_Integrador");                    

                    IdArea = equivalencia_Integrador.IdArea;
                    IdCiudad = equivalencia_Integrador.IdCiudad;
                    IdUsuario = equivalencia_Integrador.IdUsuario;
                    IdTrd = equivalencia_Integrador.IdTrd;
                    IdVersionTrd = equivalencia_Integrador.IdVersionTrd;

                    contadorSeriSubSerie = 0;
                    totalSeriSubSerie = serieSubserieExpediente.Count;
                    foreach (SerieSubSerieExpediente expediente in serieSubserieExpediente)
                    {
                        contadorSeriSubSerie++;                            
                        // Obtenemos la data para crear un expediente

                        
                        string ND = "N/D";


                        if (IdArea != ND && IdCiudad != ND &&
                            IdUsuario != ND && IdTrd != ND &&
                            IdVersionTrd != ND)
                        {
                            
                            nombreExpediente = expediente.Expediente;

                            IdArea = equivalencia_Integrador.IdArea;
                            #region Crear u obtener Expediente
                            ShowConsole($"Creando expediente - {nombreExpediente}");
                            CrearExpedienteResponse crearExpResp = await CrearExpedienteParameters(loginResponse, equivalencia_Integrador);


                            ShowConsole($"Obteniendo Id del expediente {crearExpResp.ValidationUI} !!!");
                            if (!crearExpResp.Ok)
                                ShowConsole(crearExpResp.ValidationUI, "yellow", "blue");

                            getExpedienteResponse = await GetExpediente(loginResponse, nombreExpediente);
                            #endregion

                            ShowConsole($"Obteniendo folder de la base de datos!!! - {nombreExpediente}");
                            List<ClassGetFolderSql> folders = GetFolderId(nombreExpediente);

                            if (folders.Count > 0)
                            {
                                totalFolders = folders.Count;
                                contadorFolders = 0;
                                foreach (ClassGetFolderSql folder in folders)
                                {
                                    contadorFolders++;
                                    folderSelected = folder;
                                    ShowConsole($"Obteniendo cuadernos de la base de datos!!! - {nombreExpediente}");
                                    List<ClassSpGetNoteBook> noteBooks = GetNotebook(nombreExpediente);
                                    if (noteBooks.Count > 0)
                                    {
                                        totalNoteBooks = noteBooks.Count;
                                        contadorNoteBooks = 0;
                                        foreach (var notebook in noteBooks)
                                        {
                                            contadorNoteBooks++;
                                            ShowConsole("Creando cuaderno - " + notebook.CuadernoId);
                                            cCResp = await NoteBookParameters(loginResponse, notebook, folder);

                                            // Por preguntar como relacionar cuadernos con documentos
                                            ShowConsole($"Obteniendo Id de Cuadernos - {notebook.CuadernoId}");
                                            ObtenerCuadernoResponse noteBookList = await NoteBookByCUIParameters(loginResponse);

                                            if (noteBookList.Data?.Count > 0)
                                            {
                                                totalNoteBookList = noteBookList.Data?.Count;
                                                contadorNoteBookList = 0;
                                                foreach (DatumOC nb in noteBookList.Data)
                                                {
                                                    contadorNoteBookList++;
                                                    ShowConsole($"\nConsultando archivos de la Base de Datos!!! - {nb.Nombre}");
                                                    List<ClassSpFile> spFile = GetFile(nombreExpediente, nb.Nombre);
                                                    if (spFile.Count > 0)
                                                    {
                                                        totalFiles = spFile.Count;
                                                        contadorFiles = 0;
                                                        foreach (var file in spFile)
                                                        {
                                                            try
                                                            {
                                                                contadorFiles++;
                                                                ShowConsole($"\nCreando documento {file.NombreImagen} - Carpeta: {nb.Carpeta}!!!");
                                                                TdrEmpresaReponse cDocResp = await CrearDocumentoParameters(loginResponse, file, nb);
                                                                if (!cDocResp.Ok)
                                                                {
                                                                    ShowConsole("\n" + cDocResp.CodeError + " " + file.NombreImagen + cDocResp.ValidationUI + " " + nb.Carpeta, "yellow");
                                                                    Log1 logFallido = new Log1
                                                                    {
                                                                        despacho = nombreDespacho,
                                                                        idExpediente = nombreExpediente,
                                                                        cuaderno = string.Concat(notebook.Cuaderno.ToString(), " ", notebook.CuadernoId.ToString()),
                                                                        instancia = "",
                                                                        estado = false,
                                                                        archivo = file.NombreImagen,
                                                                        observacion = string.Concat("Serie_SubSerie: ", serieSeleccionada.Serie_SubSerie, " ! IdArea: ", IdArea, " ! IdCiudad: ", IdCiudad, " ! IdUsuario: ", IdUsuario, " ! IdTrd: ", IdTrd, " ! NombreImagen: ", file.NombreImagen, " ! Response: ", cDocResp.ValidationUI, " ! Carpeta: ", nb.Carpeta)
                                                                    };
                                                                    Repository.SpGuardarLog(logFallido);
                                                                }
                                                                else
                                                                {
                                                                    ClassEstadoDocumento estadoDocumento = new ClassEstadoDocumento
                                                                    {
                                                                        IdImagen = file.IdImagen,
                                                                        Message = "Carga Exitosa!!!",
                                                                        NombreExpediente = nombreExpediente
                                                                    };

                                                                    Log1 logExitoso = new Log1
                                                                    {
                                                                        despacho = nombreDespacho,
                                                                        idExpediente = nombreExpediente,
                                                                        cuaderno = string.Concat(notebook.Cuaderno.ToString(), " ", notebook.CuadernoId.ToString()),
                                                                        instancia = "",
                                                                        estado = true,
                                                                        archivo = file.NombreImagen
                                                                    };


                                                                    ShowConsole("Se cargo el archivo exitosamente!!!", "green", "blue");
                                                                    Repository.SpGuardarLog(logExitoso);
                                                                    SpImgStatusInRepository(estadoDocumento);
                                                                }
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                ShowConsole("\n" + string.Concat(e.Message, " ", e.InnerException?.Message) + " " + file.NombreImagen + e.StackTrace + " " + nb.Carpeta, "yellow");
                                                                //Log(e.Message, e.StackTrace);
                                                                Log1 logFallido = new Log1
                                                                {
                                                                    despacho = nombreDespacho,
                                                                    idExpediente = nombreExpediente,
                                                                    cuaderno = string.Concat(notebook.Cuaderno, " ", notebook.CuadernoId),
                                                                    instancia = nb.Carpeta,
                                                                    archivo = file.NombreImagen,
                                                                    estado = false,
                                                                    observacion = string.Concat(e.Message, " ", e.InnerException?.Message, " ", e.StackTrace)
                                                                };

                                                                Repository.SpGuardarLog(logFallido);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ShowConsole("Sin notebook para registrar", "black", "white");
                                                Log1 logFallido = new Log1
                                                {
                                                    despacho = nombreDespacho,
                                                    idExpediente = nombreExpediente,
                                                    cuaderno = "Sin notebook para registrar",
                                                    instancia = null,
                                                    archivo = null,
                                                    estado = false,
                                                    observacion = "Sin notebook para registrar"
                                                };

                                                Repository.SpGuardarLog(logFallido);
                                            }

                                            SpShowLogExpediente();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ShowConsole("\n Data inconsistente!!!");
                            Log1 logFallido = new Log1
                            {
                                despacho = nombreDespacho,
                                idExpediente = nombreExpediente,
                                cuaderno = null,
                                instancia = null,
                                archivo = null,
                                observacion = string.Concat($"Data inconsistente - IdArea {equivalencia_Integrador.IdArea} - IdCiudad {equivalencia_Integrador.IdCiudad} - IdUsuario {equivalencia_Integrador.IdUsuario} - IdTrd {equivalencia_Integrador.IdTrd} - IdVersionTrd {equivalencia_Integrador.IdVersionTrd}")
                            };

                            Repository.SpGuardarLog(logFallido);
                        }
                    }
                    
                }
                
            }
            Console.ReadLine();
        }

        static void Log(string Message = "Data inconsistente!!!", string StackTrace = "", int Type = 999)
        {
            LogError l = new LogError
            {
                Message = Message,
                StackTrace = StackTrace,
                Type = Type
            };
            //Exception(l);
        }

        /// <summary>
        /// Hacemos login y obtenemos login
        /// </summary>
        /// <returns></returns>
        public static async Task<LoginResponse> LoginAsync()
        { 

            Login login = new Login
            {
                UData = parametersApi.ParametersApi[0].usuario,
                AppKey = parametersApi.ParametersApi[0].password
            };

            httpResponseMessage = await Http<Login>.Post(login, EndPointApi[0].patch, urlApi);
            LoginResponse loginResponse = await httpResponseMessage.Content.ReadAsAsync<LoginResponse>();
            return loginResponse;
        }

        static private DatumAE GetFilterAreaEmpresa()
        {
            List<DatumAE> data = areaEmpresaResponse.Data;
            DatumAE resp = data.Find(x => x.Codigo == IdArea);

            return resp;
        }

        static EquivalenciasIntegrador Equivalencia_Integrador(string despacho, string Serie)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [equ].[SpEquivalencia_Integrador] '{despacho}', '{Serie}'");
            EquivalenciasIntegrador records = db.Database.SqlQuery<EquivalenciasIntegrador>(query).FirstOrDefault();

            // Sino devuelve null
            return records;
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

        static async Task<TRDAreaSerieSubSerieResponse> TRDASSParameters(LoginResponse login, ClassSpSerieYSubSerie respClassSpSerieYSubSerie)
        {
            ParametersTrdASS parameters = new ParametersTrdASS
            {
                CodigoArea = IdArea,
                CodigoSerie = respClassSpSerieYSubSerie.CodigoSerie,
                CodigoSubSerie = respClassSpSerieYSubSerie.CodigoSubSerie
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

        static async Task<CrearCuadernoResponse> CuadernosParameters(LoginResponse login, ClassSpGetNoteBook notebooke, ClassGetFolderSql folder)
        {
            ParametersCC parameter = new ParametersCC
            {
                Nombre = notebooke.Cuaderno,
                Codigo = notebooke.CuadernoId,
                IdCarpeta = folder.IdCarpeta,
                IdExpediente = getExpedienteResponse.Data[0].IdExpediente
            };

            CrearCuadernoResponse trdASS = await Cuadernos(login, parameter);
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
        
        static async Task<CrearExpedienteResponse> CrearExpedienteParameters(LoginResponse loginResponse, EquivalenciasIntegrador equivalencia)
        {
            ParametersCE parameter = new ParametersCE
            {
                IdTablaRetencionDocumental = Convert.ToInt32(equivalencia.IdTrd),
                IdVersionTablaRetencionDocumental = Convert.ToInt32(equivalencia.IdVersionTrd),
                IdUsuarioResponsable = Convert.ToInt32(equivalencia.IdUsuario),
                Nombre = nombreExpediente,
                Descripcion = "Contrato Nro No. C01.PCCNTR.2053505",
                FechaFinal = "1",
                NumeroRadicacionProceso = nombreExpediente,
                IdCiudad = Convert.ToInt32(equivalencia.IdCiudad),
                IdAreaEmpresa = Convert.ToInt32(equivalencia.IdArea),
                VerPublico = true,
                Metadatos = new List<object> { }
            };

            return await CrearExpediente(loginResponse, parameter);
        }
        
        static async Task<CrearCuadernoResponse> NoteBookParameters(LoginResponse loginResponse, ClassSpGetNoteBook notebooke, ClassGetFolderSql folder)
        {
            ParametersCC parameter = new ParametersCC
            {
                Nombre = notebooke.Cuaderno,
                Codigo = notebooke.CuadernoId,
                IdCarpeta = folder.IdCarpeta,
                IdExpediente = getExpedienteResponse.Data[0].IdExpediente
            };

            return await Cuadernos(loginResponse, parameter);
        }
        
        static async Task<ObtenerCuadernoResponse> NoteBookByCUIParameters(LoginResponse loginResponse)
        {
            ParametersCBC parameter = new ParametersCBC
            {
                CUI = nombreExpediente
            };

            return await ObtenerCuadernos(loginResponse, parameter);
        }

        static async Task<TdrEmpresaReponse> CrearDocumentoParameters(LoginResponse loginResponse, ClassSpFile file, DatumOC notebook)
        {
            string base64 = GetFileToBase64(file.NombreImagen);
            ArchivoCD archivoCD = Api.CreateArchivoCD(file.NombreImagen, base64);
            List<ArchivoCD> archivoCDList = Api.CreateListArchivoCD(archivoCD);
            ParametersCD parameters = Api.CrearDocumentoParameters(getExpedienteResponse, file, archivoCDList, notebook, folderSelected, IdArea);
            TdrEmpresaReponse trdASS = await CrearDocumento(loginResponse, parameters);
            return trdASS;
        }

        #endregion

        #region CONSULTAMOS A SQL SERVER
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

        static List<ClassGetFolderSql> GetFolderId(string nombreExpediente)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[spFolderId] '{nombreExpediente}'");
            List<ClassGetFolderSql> records = db.Database.SqlQuery<ClassGetFolderSql>(query).ToList();

            // Sino devuelve null
            return records;
        }

        static List<ClassSpGetNoteBook> GetNotebook(string nombreExpediente)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"[api].[SpNoteBook] '{nombreExpediente}'");
            List<ClassSpGetNoteBook> records = db.Database.SqlQuery<ClassSpGetNoteBook>(query).ToList();

            // Sino devuelve null
            return records;
        }

        static List<ClassSpFile> GetFile(string nombreExpediente, string nombre)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpFiles] '{nombreExpediente}', '{nombre}', '{serieSeleccionada.Serie_SubSerie}'"); 
            List<ClassSpFile> records = db.Database.SqlQuery<ClassSpFile>(query).ToList();

            // Sino devuelve null
            return records;
        }
        
        static List<ClassSpFile> Upd(string nombreExpediente)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpFiles] '{nombreExpediente}'");
            List<ClassSpFile> records = db.Database.SqlQuery<ClassSpFile>(query).ToList();

            // Sino devuelve null
            return records;
        }

        /// <summary>
        /// Guardamos registros de logs
        /// </summary>
        /// <param name="logs">Guardamos registro de los logs obtenidos</param>
        static void Exception(LogError log)
        {
            try
            {
                SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
                string query = string.Concat($"EXEC [api].[SpLogException] '{log.Message}', '{log.StackTrace}'");
                db.Database.ExecuteSqlCommand(query);
                //List<ClassSpFile> records = db.Database.SqlQuery<ClassSpFile>(query).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }            
        }

        static void SpImgStatusInRepository(ClassEstadoDocumento estadoDocumento)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpImgStatusInRepository] '{estadoDocumento.IdImagen}', '{nombreExpediente}', '{estadoDocumento.Message}'");
            db.Database.ExecuteSqlCommand(query);
            //List<ClassSpFile> records = db.Database.SqlQuery<ClassSpFile>(query).ToList();
        }

        static List<ClassValidateExpediente> SpValidarNumeroExpediente()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpValidarNumeroExpediente] '{nombreExpediente}'");
            List<ClassValidateExpediente> records = db.Database.SqlQuery<ClassValidateExpediente>(query).ToList();
            return records;
            //List<ClassSpFile> records = db.Database.SqlQuery<ClassSpFile>(query).ToList();
        }
        
        static void SpShowLogExpediente()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpLogTd] '{nombreExpediente}', 'sintd'");
            List<ClassSpLogTd> records = db.Database.SqlQuery<ClassSpLogTd>(query).ToList();

            ShowConsole("Logs TipoDocumental", "orange");
            foreach (var log in records)
            {
                Console.WriteLine(string.Concat(log.IdImagen, " - ", log.NombreImagen, " - ", log.Expediente, " - ", log.Imagen_repositorio, " - ", log.TipoDocumental));

            }
            //List<ClassSpFile> records = db.Database.SqlQuery<ClassSpFile>(query).ToList();
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
            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            HttpResponseMessage response = await Http<Login>.Post(login, EndPointApi[0].patch, urlApi);
            response.EnsureSuccessStatusCode();

            LoginResponse res = await response.Content.ReadAsAsync<LoginResponse>();

            // return URI of the created resource.
            return res;
        }

        /// <summary>
        /// Realizamos recorrido de los datos obtenidos 
        /// </summary>
        /// <param name="registers">Data obenida de la Api</param>
        static async Task<CrearExpedienteResponse> CrearExpediente(LoginResponse login, ParametersCE parameters)
        {
            // Aqui llamamos a la Api a enviarle la data
            try
            {
                CrearExpediente expediente = new CrearExpediente
                {
                    Token = login.Data.Token,
                    AppKey = parametersApi.ParametersApi[0].password,
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

                //HttpResponseMessage response = await client.PostAsJsonAsync(endPoint, expediente);
                HttpResponseMessage response = await Http<CrearExpediente>.Post(expediente, EndPointApi[1].patch, urlApi);
                response.EnsureSuccessStatusCode();

                CrearExpedienteResponse res = await response.Content.ReadAsAsync<CrearExpedienteResponse>();

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
            TRDPOREMPRESA trdEmpresa = new TRDPOREMPRESA
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password
            };
            trdEmpresa.ExecutionObject.Name = "TRD";
            trdEmpresa.ExecutionObject.WebServiceMethod.Name = "GetTRDIdAreaEmpresa";
            trdEmpresa.ExecutionObject.WebServiceMethod.Parameters = parameter;

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            HttpResponseMessage response = await Http<LoginResponse>.Post(login, EndPointApi[0].patch, urlApi);
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

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[3].patch, trdSSS);
            HttpResponseMessage response = await Http<TRDAreaSerieSubSerie>.Post(trdSSS, EndPointApi[3].patch, urlApi);
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
            UsuarioDeArea usrDeArea = new UsuarioDeArea
            {
                Token = loginResponse.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password
            };
            usrDeArea.ExecutionObject.Name = "Usuario";
            usrDeArea.ExecutionObject.WebServiceMethod.Name = "GetUsuarioArea";
            usrDeArea.ExecutionObject.WebServiceMethod.Parameters = parameter;

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, loginResponse);
            HttpResponseMessage response = await Http<LoginResponse>.Post(loginResponse, EndPointApi[0].patch, urlApi);
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
                   

            WebServiceMethodCD webServiceMethod = new WebServiceMethodCD
            {
                Name = "CrearDocumento",
                Parameters = parameter
            };

            ExecutionObjectCD executionObject = new ExecutionObjectCD
            {
                Name = "Documentos",
                WebServiceMethod = webServiceMethod
            };

            CrearDocumento crearDocumento = new CrearDocumento
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password,
                ExecutionObject = executionObject
            };

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[6].patch, crearDocumento);
            HttpResponseMessage response = await Http<CrearDocumento>.Post(crearDocumento, EndPointApi[6].patch, urlApi);
            response.EnsureSuccessStatusCode();

            TdrEmpresaReponse res = await response.Content.ReadAsAsync<TdrEmpresaReponse>();

            return res;
        }

        static async Task<GetExpedienteResponse> GetExpediente(LoginResponse login, string CUI)
        {
            ParametersGE parameters = Api.ExpedienteParameters(login, CUI);
            GetExpediente getExpediente = Api.GetExpedienteApi(login, parameters, password);

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[11].patch, getExpediente);
            HttpResponseMessage response = await Http<GetExpediente>.Post(getExpediente, EndPointApi[11].patch, urlApi);
            response.EnsureSuccessStatusCode();

            GetExpedienteResponse res = await response.Content.ReadAsAsync<GetExpedienteResponse>();

            if (res.Data.Count == 0)
                throw new Exception($"No se encontro ConfiguracionTRD!!! - Name: GetExpedienteCUI - CUI: {getExpediente.ExecutionObject.WebServiceMethod.Parameters.CUI} - patch {EndPointApi[11].patch} - urlApi: {urlApi}");
            
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(res.Data[0].ConfiguracionTRD);
            tiposDocumentale = myDeserializedClass.TiposDocumentales;
            return res;
        }

        static async Task<ExpPorCUIResponse> ExpPorCUI(LoginResponse login, ParametersCUI parameter)
        {
            ExpPorCUI expPorCUI = new ExpPorCUI
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password
            };
            expPorCUI.ExecutionObject.Name = "Documentos";
            expPorCUI.ExecutionObject.WebServiceMethod.Name = "CrearDocumento";
            expPorCUI.ExecutionObject.WebServiceMethod.Parameters = parameter;

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            HttpResponseMessage response = await Http<LoginResponse>.Post(login, EndPointApi[0].patch, urlApi);
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
            FolderApi folder = new FolderApi
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password
            };
            folder.ExecutionObject.Name = "Carpetas";
            folder.ExecutionObject.WebServiceMethod.Name = "GetCarpetas";
            folder.ExecutionObject.WebServiceMethod.Parameters = parameter;

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[0].patch, login);
            HttpResponseMessage response = await Http<LoginResponse>.Post(login, EndPointApi[0].patch, urlApi);
            response.EnsureSuccessStatusCode();

            CarpetaResponse res = await response.Content.ReadAsAsync<CarpetaResponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<CrearCuadernoResponse> Cuadernos(LoginResponse login, ParametersCC parameter)
        {
            WebServiceMethodCC webServiceMethod = new WebServiceMethodCC
            {
                Name = "CrearCuaderno",
                Parameters = parameter,
            };

            ExecutionObjectCC executionObject = new ExecutionObjectCC
            {
                Name = "Cuaderno",
                WebServiceMethod = webServiceMethod
            };

            CreacionCuadernos creacionCuadernos = new CreacionCuadernos
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password,
                ExecutionObject = executionObject
            };

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[13].patch, creacionCuadernos);
            HttpResponseMessage response = await Http<CreacionCuadernos>.Post(creacionCuadernos, EndPointApi[13].patch, urlApi);
            response.EnsureSuccessStatusCode();

            CrearCuadernoResponse res = await response.Content.ReadAsAsync<CrearCuadernoResponse>();

            return res;
        }

        /// <summary>
        /// TrdASS
        /// </summary>
        /// <param name="login">Clase login con los datos de acceso</param>
        /// <returns></returns>
        static async Task<ObtenerCuadernoResponse> ObtenerCuadernos(LoginResponse login, ParametersCBC parameter)
        {
            WebServiceMethodCBC webServiceMethod = new WebServiceMethodCBC
            {
                Name = "GetCuadernosCUI",
                Parameters = parameter,
            };

            ExecutionObjectCBC executionObject = new ExecutionObjectCBC
            {
                Name = "Cuaderno",
                WebServiceMethod = webServiceMethod
            };

            CuadernoByCUI creacionCuadernos = new CuadernoByCUI
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password,
                ExecutionObject = executionObject
            };

            //HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[12].patch, creacionCuadernos);
            HttpResponseMessage response = await Http<CuadernoByCUI>.Post(creacionCuadernos, EndPointApi[12].patch, urlApi);
            ObtenerCuadernoResponse res = await response.Content.ReadAsAsync<ObtenerCuadernoResponse>();
            return res;
        }

        //private static void AddHeaders()
        //{
        //    string urlApiI = string.Concat(parametersApi.ParametersApi[0].urlApi);
        //    client.BaseAddress = new Uri(urlApiI);
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //}
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

        static List<ClassFindDespacho> FindDespacho(string despacho)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpFindDespacho] '{despacho}'");
            List<ClassFindDespacho> records = db.Database.SqlQuery<ClassFindDespacho>(query).ToList();

            // Sino devuelve null
            return records;
        }

        static List<Series> SpBuscarSeries(string despacho)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpBuscarSeries] '{despacho}'");
            List<Series> records = db.Database.SqlQuery<Series>(query).ToList();

            // Sino devuelve null
            return records;
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

        static ClassSpSerieYSubSerie GetSerieYSubSerie()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string codigo = respAreaEmpresaFilter.Codigo;
            string query = string.Concat($"[api].[SpSeries] '{codigo}', '{nombreExpediente}'");
            ClassSpSerieYSubSerie record = db.Database.SqlQuery<ClassSpSerieYSubSerie>(query).First();

            // Sino devuelve null
            return record;
        }

        static List<SerieSubSerieExpediente> spBuscarExpdientesSerie()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[spBuscarExpedientesSeries] '{nombreDespacho}', '{serieSeleccionada.Serie_SubSerie}'");
            List<SerieSubSerieExpediente> record = db.Database.SqlQuery<SerieSubSerieExpediente>(query).ToList();

            // Sino devuelve null
            return record;
        }
        #endregion


        #region LINQ
        static string GetTipoDocumental(ClassSpFile file)
        {
            string resp = tiposDocumentale.Find(x => x.Nombre.ToLower().Trim() == file.TipoDocumental.ToLower().Trim())?.IdTipoDocumental;
            if (string.IsNullOrEmpty(resp))
            {
                ShowConsole($"Tipo Documental **{file.TipoDocumental}** no permito", "red");
                ClassEstadoDocumento estadoDocumento = new ClassEstadoDocumento
                {
                    IdImagen = file.IdImagen,
                    Message = "sintd",
                    NombreExpediente = nombreExpediente
                };

                SpImgStatusInRepository(estadoDocumento);
            }
            else
            {
                ClassEstadoDocumento estadoDocumento = new ClassEstadoDocumento
                {
                    IdImagen = file.IdImagen,
                    Message = "contd",
                    NombreExpediente = nombreExpediente
                };

                SpImgStatusInRepository(estadoDocumento);
            }
            return resp;
        }

        #endregion

        #region METODOS GENERALES
        static string GetFileToBase64(string fileName)
        {
            string pathRed = parametersApi.ParametersApi[1].urlApi;
            //string path = Directory.GetCurrentDirectory() + string.Format("{0} {1}", {pathRed}, $"/{fileName}");
            string path = string.Concat(pathRed, "/",nombreExpediente,$"/{fileName}");
            Byte[] bytes = File.ReadAllBytes(path);
            string strArchivoBase64 = Convert.ToBase64String(bytes);
            return strArchivoBase64;
        }

        #endregion

        #region CONSOLE
        static void ShowConsole(string mensaje, string option = "black", string color = "white")
        {
            Console.Clear();
            Consola.ShowEquivalenciaIntegrador(nombreDespacho, serieSeleccionada.Serie_SubSerie, IdArea, IdCiudad, IdUsuario, IdTrd, IdVersionTrd);
            Consola.ShowMostrarExpediente(nombreExpediente);
            Consola.ShowSeriSubSerie(contadorSeriSubSerie, totalSeriSubSerie);
            Consola.ShowFolders(contadorFolders, totalFolders);
            Consola.ShowNoteBooks(contadorNoteBooks, totalNoteBooks);
            Consola.ShowNoteBookList(contadorNoteBookList, totalNoteBookList);
            Consola.ShowFiles(contadorFiles, totalFiles);

            Consola.ShowFiles(contadorFiles, totalFiles);

            Console.SetCursorPosition(0, 17);
            switch (option)
            {
                case "black":
                    Console.Write("\n" + mensaje);
                    break;
                case "green":
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n" + mensaje);
                    break;
                case "red":
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n" + mensaje);
                    break;
                case "DarkMagenta":
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("\n" + mensaje);
                    break;
                case "yellow":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("\n" + mensaje);
                    break;
                case "gray":
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("\n" + mensaje);
                    break;
            }
            Console.ResetColor();
        }
        #endregion

        #region MENU
        static void SolicitarDespacho()
        {
            string nombreDespachoDigitado = string.Empty;
            do
            {
                nombreDespachoDigitado = Menu.MenuSolicitarDespacho();
                despachos = FindDespacho(nombreDespachoDigitado);
                if (despachos.Count <= 0)
                    Console.WriteLine("Expediente invalido, por favor digite nuevamente...\n");
            } while (despachos.Count == 0);
            nombreDespacho = nombreDespachoDigitado;
        }

        static void SeleccionarSerie()
        {
            List<Series> series = SpBuscarSeries(nombreDespacho);
            if (despachos.Count <= 0)
                throw new Exception("Serie no encontrada!!!");
            
            do
            {
                string serie = Menu.MenuSeleccionarSerie(series);
                serieSeleccionada = series.Find(x => x.Codigo == Convert.ToInt32(serie));
            } while (serieSeleccionada == null);
        }

        static void GetExpediente()
        {
            do
            {
                nombreExpediente = Menu.MenuExpediente();
            } while (nombreExpediente == null);
        }

        static void GetIdArea()
        {
            do
            {
                nombreExpediente = Menu.MenuIdArea();
            } while (nombreExpediente == null);
        }
        #endregion

        #region
        public static SID_PROTOCOL2Entities SingletonDB()
        {
            SID_PROTOCOL2Entities db = null;

            if (db == null)
                db = new SID_PROTOCOL2Entities();

            return db;
        }
        #endregion
    }
}
