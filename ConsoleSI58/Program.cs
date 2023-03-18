﻿using Lider.Models;
using Lider.Models.ClassApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
        public static DatumAE respAreaEmpresaFilter = null;
        public static ClassGetFolderSql folderSelected = null;
        public static CrearCuadernoResponse cCResp = null;
        public static bool filtroAreaEmpresa = true;
        public static List<initialDate> initialDate = null;
        public static List<TiposDocumentale> tiposDocumental = null;
        public static GetExpedienteResponse getExpedienteResponse = null;
        public static DateTime StartDate { get; set; }
        public static DateTime EndDate { get; set; }
        public static List<urlApiI> EndPointApi { get; set; }
        static HttpClient client = new HttpClient();
        public LogError log { get; set; }
        public static SID_PROTOCOL2Entities db = null;
        public static string nombreDespacho = null;
        public static string nombreExpediente = null;
        public static string IdTipoDocumental = null;
        public static string codigoArea = null;
        public static List<ClassFindDespacho> despachos = null;

        
        public static AreaEmpresaResponse areaEmpresaResponse = new AreaEmpresaResponse();

        static void Main()
        {
            try
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
            catch (Exception e)
            {
                ShowConsole(e.Message, "red");
                ShowConsole("Por favor presiones enter para salir!!!");
                Console.Read();
                LogError l = new LogError
                {
                    Message = e.Message,
                    StackTrace = e.StackTrace,
                    Type = 999
                };
                Exception(l);
            }
}

        /// <summary>
        /// Parametrizamos clase HttpClient para la consulta a la API y llamamos a los diferentes metodos
        /// </summary>
        /// <returns>void</returns>
        static async Task RunAsync()
        {
            Menu();            
            

            AddHeaders();
            Login login = new Login
            {
                UData = parametersApi.ParametersApi[0].usuario,
                AppKey = parametersApi.ParametersApi[0].password
            };

            ShowConsole("Obteniendo token!!!");
            LoginResponse loginResponse = await LoginServer(login);

            if (loginResponse.Ok)
            {
                // Pedimos despacho con sus expedientes
                

                int contador = 0;
                int total = despachos.Count;
                ShowConsole($"Consultando los expedientes del despacho {nombreDespacho}", "green");
                foreach (ClassFindDespacho expediente in despachos)
                {
                    contador++;
                    ShowConsole($"Procesando expediente {contador} de {total}: {nombreDespacho} - Expediente: {expediente.Expediente} - Serie: {expediente.Serie}", "green");
                    // Obtenemos la data para crear un expediente
                    EquivalenciasIntegrador equivalencia_Integrador = Equivalencia_Integrador(nombreDespacho, expediente.Serie);

                    if (equivalencia_Integrador != null)
                    {
                        string ND = "N/D";
                        if (equivalencia_Integrador.IdArea != ND && equivalencia_Integrador.IdCiudad != ND &&
                            equivalencia_Integrador.IdUsuario != ND && equivalencia_Integrador.IdTrd != ND &&
                            equivalencia_Integrador.IdVersionTrd != ND)
                        {
                            nombreExpediente = expediente.Expediente;
                            codigoArea = equivalencia_Integrador.IdArea;

                            ShowConsole("Recorriendo expedientes de la BD!!!");

                            #region Crear u obtener Expediente
                            ShowConsole("Creando expediente Api!!! - {0}", nombreExpediente);
                            CrearExpedienteResponse crearExpResp = await CrearExpedienteParameters(loginResponse, equivalencia_Integrador);

                            ShowConsole("Obteniendo Id del expediente Api!!!");
                            getExpedienteResponse = await ExpedienteParameters(loginResponse, nombreExpediente);
                            #endregion

                            ShowConsole("Obteniendo folder de la base de datos!!! - {0}", nombreExpediente);
                            List<ClassGetFolderSql> folders = GetFolderId(nombreExpediente);
                            if (folders.Count > 0)
                            {
                                foreach (ClassGetFolderSql folder in folders)
                                {
                                    folderSelected = folder;
                                    ShowConsole("Obteniendo cuadernos de la base de datos!!! - {0}", nombreExpediente);
                                    List<ClassSpGetNoteBook> noteBooks = GetNotebook(nombreExpediente);
                                    if (noteBooks.Count > 0)
                                    {
                                        foreach (var notebook in noteBooks)
                                        {
                                            ShowConsole("Creando cuaderno Api!!! - {0}", notebook.CuadernoId);
                                            cCResp = await NoteBookParameters(loginResponse, notebook, folder);

                                            // Por preguntar como relacionar cuadernos con documentos
                                            ShowConsole("Obteniendo Id de Cuadernos Api!!! - {0}", notebook.CuadernoId);
                                            ObtenerCuadernoResponse noteBookList = await NoteBookByCUIParameters(loginResponse);

                                            if (noteBookList.Data.Count > 0)
                                            {
                                                foreach (DatumOC nb in noteBookList.Data)
                                                {
                                                    ShowConsole("Consultando archivos de la Base de Datos!!! - {0}", nb.Nombre);
                                                    List<ClassSpFile> spFile = GetFile(nombreExpediente, nb.Nombre);
                                                    if (spFile.Count > 0)
                                                    {
                                                        foreach (var file in spFile)
                                                        {
                                                            try
                                                            {
                                                                ShowConsole("Creando documento Api!!!");
                                                                TdrEmpresaReponse cDocResp = await CrearDocumentoParameters(loginResponse, file, nb);
                                                                if (!cDocResp.Ok)
                                                                {
                                                                    ShowConsole("\n" + cDocResp.CodeError + " " + file.NombreImagen + cDocResp.ValidationUI + " " + nb.Carpeta, "yellow");
                                                                    Log(cDocResp.QMetadata.ToString(), cDocResp.TrazaError.ToString());
                                                                }
                                                                else
                                                                {
                                                                    ShowConsole("Se cargo el archivo exitosamente!!!", "red");
                                                                    ClassEstadoDocumento estadoDocumento = new ClassEstadoDocumento
                                                                    {
                                                                        IdImagen = file.IdImagen,
                                                                        Message = "Carga Exitosa!!!",
                                                                        NombreExpediente = nombreExpediente
                                                                    };

                                                                    SpImgStatusInRepository(estadoDocumento);
                                                                }
                                                            }
                                                            catch (Exception e)
                                                            {
                                                                ShowConsole("\n" + e.Message + " " + file.NombreImagen + e.StackTrace + " " + nb.Carpeta, "yellow");
                                                                Log(e.Message, e.StackTrace);
                                                            }
                                                            
                                                        }
                                                    }
                                                }
                                            }

                                            SpShowLogExpediente();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Se genera un log
                            ShowConsole("\n Data inconsistente!!!");
                            Log("Data inconsistente!!!");
                        }
                    }
                    
                }


                //}
                //else
                //{
                //    ClassLogErrorApi logErrorApi = new ClassLogErrorApi
                //    {
                //        CodigoError = null,
                //        CodigoArea = null,
                //        NombreArchivo = null,
                //        NombreCarpeta = null,
                //        NombreExpediente = null,
                //        Message = loginResponse.Error.ToString(),
                //        IdError = 5
                //    };
                //    LogApi(logErrorApi);
                //    Main();
                //}
            }
            else
            {
                ShowConsole("Registros no encontrados en base de datos!!!");
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
            Exception(l);
        }

        static private DatumAE GetFilterAreaEmpresa()
        {
            List<DatumAE> data = areaEmpresaResponse.Data;
            DatumAE resp = data.Find(x => x.Codigo == codigoArea);

            return resp;
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
                CodigoArea = codigoArea,
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

        static async Task<ExpPorCUIResponse> ExpPorCUIParameters(LoginResponse login)
        {
            ParametersCUI parameters = new ParametersCUI
            {
                CUI = "2587"
            };

            ExpPorCUIResponse trdASS = await ExpPorCUI(login, parameters);
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
        
        static async Task<AreaEmpresaResponse> AreaEmpresaParameters(LoginResponse loginResponse)
        {
            ParametersAE parameter = new ParametersAE
            {
                
            };

            return await AreaEmpresa(loginResponse, parameter);
        }
        
        static async Task<GetExpedienteResponse> ExpedienteParameters(LoginResponse loginResponse, string CUI)
        {
            ParametersGE parameter = new ParametersGE
            {
                CUI = CUI
            };

            return await GetExpediente(loginResponse, parameter);
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
            ArchivoCD archivoCD = CreateArchivoCD(file.NombreImagen, base64);
            List<ArchivoCD> archivoCDList = new List<ArchivoCD>
            {
                archivoCD
            };
            string tipoDocumental = GetTipoDocumental(file);

            if (string.IsNullOrEmpty(tipoDocumental))
                throw new Exception("Tipo documental no encontrasdo!!!");

            ParametersCD parameters = new ParametersCD
            {
                IdExpediente = getExpedienteResponse.Data[0].IdExpediente,
                IdTipoDocumental = tipoDocumental,
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
                IdAreaEmpresa = Convert.ToInt32(codigoArea),
                VerPublico = false,
                IdCuaderno = notebook.IdCuaderno
            };

            TdrEmpresaReponse trdASS = await CrearDocumento(loginResponse, parameters);
            return trdASS;
        }

        #endregion

        #region CONSULTAMOS A SQL SERVER

        static void Singleton()
        {
            if (db == null)
                db = new SID_PROTOCOL2Entities();   
        }

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
            string query = string.Concat($"EXEC [api].[SpFiles] '{nombreExpediente}', '{nombre}'");
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
        
        static void LogApi(ClassLogErrorApi exception)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpLogApi] 'logapi', '{exception.IdError}', '{exception.Message}', '{exception.CodigoError}', '{exception.NombreArchivo}', '{exception.CodigoArea}', '{exception.NombreExpediente}', '{exception.NombreCarpeta}'");
            db.Database.ExecuteSqlCommand(query);
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
                string query = string.Concat($"EXEC [api].[SpLogApi]  '0', 'excepcion', '{log.Message}', '9', '{log.Type}', '{log.StackTrace}'");
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
            List<ClassSpFile> records = db.Database.SqlQuery<ClassSpFile>(query).ToList();
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

        static List<ClassFindDespacho> FindDespacho(string despacho)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [api].[SpFindDespacho] '{despacho}'");
            List<ClassFindDespacho> records = db.Database.SqlQuery<ClassFindDespacho>(query).ToList();

            // Sino devuelve null
            return records;
        }

        static EquivalenciasIntegrador Equivalencia_Integrador(string despacho, string Serie)
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string query = string.Concat($"EXEC [equ].[SpEquivalencia_Integrador] '{despacho}', '{Serie}'");
            EquivalenciasIntegrador records = db.Database.SqlQuery<EquivalenciasIntegrador>(query).FirstOrDefault();

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
                webServiceMethod.Parameters = parameters;
                executionObject.WebServiceMethod = webServiceMethod;
                expediente.ExecutionObject = executionObject;

                //StartDate = initialDate[0].startDate;
                //EndDate = DateTime.Now;
                string endPoint = EndPointApi[1].patch;                

                HttpResponseMessage response = await client.PostAsJsonAsync(endPoint, expediente);
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

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[6].patch, crearDocumento);
            response.EnsureSuccessStatusCode();

            TdrEmpresaReponse res = await response.Content.ReadAsAsync<TdrEmpresaReponse>();

            return res;
        }

        static async Task<AreaEmpresaResponse> AreaEmpresa(LoginResponse login, ParametersAE parameters)
        {
            AreaEmpresa areaEmpresa = new AreaEmpresa
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password
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

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[2].patch, areaEmpresa);
            response.EnsureSuccessStatusCode();

            AreaEmpresaResponse res = await response.Content.ReadAsAsync<AreaEmpresaResponse>();

            return res;
        }

        static async Task<GetExpedienteResponse> GetExpediente(LoginResponse login, ParametersGE parameters)
        {
            GetExpediente getExpediente = new GetExpediente
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password
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

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[11].patch, getExpediente);
            response.EnsureSuccessStatusCode();

            GetExpedienteResponse res = await response.Content.ReadAsAsync<GetExpedienteResponse>();

            //
            if (res.Data.Count > 0)
            {
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(res.Data[0].ConfiguracionTRD);
                tiposDocumental = myDeserializedClass.TiposDocumentales;
                return res;
            }

            return null;
            
        }

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
            FolderApi folder = new FolderApi
            {
                Token = login.Data.Token,
                AppKey = parametersApi.ParametersApi[0].password
            };
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

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[13].patch, creacionCuadernos);
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

            HttpResponseMessage response = await client.PostAsJsonAsync(EndPointApi[12].patch, creacionCuadernos);
            response.EnsureSuccessStatusCode();

            ObtenerCuadernoResponse res = await response.Content.ReadAsAsync<ObtenerCuadernoResponse>();

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

        static ClassSpSerieYSubSerie GetSerieYSubSerie()
        {
            SID_PROTOCOL2Entities db = new SID_PROTOCOL2Entities();
            string codigo = respAreaEmpresaFilter.Codigo;
            string query = string.Concat($"[api].[SpSeries] '{codigo}', '{nombreExpediente}'");
            ClassSpSerieYSubSerie record = db.Database.SqlQuery<ClassSpSerieYSubSerie>(query).First();

            // Sino devuelve null
            return record;
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

        #region LINQ
        static string GetTipoDocumental(ClassSpFile file)
        {
            string resp = tiposDocumental.Find(x => x.Nombre.ToLower().Trim() == file.TipoDocumental.ToLower().Trim())?.IdTipoDocumental;
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
        
        static ArchivoCD CreateArchivoCD(string fileName, string base64)
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

        #endregion

        #region CONSOLE
        static void ShowConsole(string mensaje, string option = "black", string color = "black")
        {
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
                case "oragen":
                    Console.Write("\n" + mensaje);
                    break;
                case "yellow":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("\n" + mensaje);
                    break;
                case "orange":
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("\n" + mensaje);
                    break;
            }
            Console.ResetColor();
        }
        #endregion

        #region MENU
        static void Menu()
        {
            Console.WriteLine("                   ********************************************");
            Console.WriteLine("                   |     BIENVENIDO A DATA TRANSFER           |");
            Console.WriteLine("                   | Por favor ingrese los datos solicitado:  |");
            Console.WriteLine("                   |                                          |");
            Console.WriteLine("                   ********************************************");
            Console.WriteLine("\n");

            do
            {
                Console.WriteLine("Ingrese nombre del despacho:");
                nombreDespacho = Console.ReadLine();
                despachos = FindDespacho(nombreDespacho);
                //resp = SpValidarNumeroExpediente();
                if (despachos.Count<=0)
                {
                    Console.WriteLine("Expediente invalido, por favor digite nuevamente...\n");
                }
            } while (despachos.Count==0);

            //SpShowLogExpediente();

            //do
            //{
            //    Console.WriteLine("Ingrese código del área");
            //    codigoArea = Console.ReadLine();
            //} while (codigoArea.Length != 12);

        }
        #endregion
    }
}
