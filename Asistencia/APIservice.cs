using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Asistencia.Clases;
using Newtonsoft.Json;
namespace Asistencia
{

    public class APIservice
    {
        readonly DBservice dBservice;
        public APIservice()
        {
            dBservice = new DBservice();
        }
        public async Task<List<Cliente>> ListaClientes()
        {
            List<Cliente> ListaClientes = new List<Cliente>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/clientes";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    { "Cliente","41" }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                ListaClientes = await data.Content.ReadAsAsync<List<Cliente>>();
                if (ListaClientes.ElementAt<Cliente>(0).id == -1)
                {
                    ListaClientes = new List<Cliente>();
                }
                cliente = null;
                content = null;
                data = null;
                return ListaClientes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                dBservice.GuardarRegistrosErrores("-1","Error al Obtener los clientes", DateTime.Now.ToString(),e.Message);
                return ListaClientes;
            }
        }
        public async Task<List<Sector>> ListaSectores(String cl)
        {
            List<Sector> ListaSectores = new List<Sector>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/ChecadorSectores";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    {"Cliente", cl},
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                
                ListaSectores = await data.Content.ReadAsAsync<List<Sector>>();
                if (ListaSectores.ElementAt<Sector>(0).id == -1)
                {
                    ListaSectores = new List<Sector>();
                }
                cliente = null;
                content = null;
                data = null;
                return ListaSectores;
            }
            catch (Exception error)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al obtener los sectores", DateTime.Now.ToString(), error.Message);
                Console.WriteLine(error.Message + " Error de conexion");
                return ListaSectores;
            }
        }
        public async Task<String> RegistrarChecador(String cl, String sc, String pass, String Nombre)
        {
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/ConfigurarChecador";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    {"Cliente",cl },
                    {"Sector",sc },
                    {"Nombre",Nombre },
                    {"UserPass",pass }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                String Result = await data.Content.ReadAsStringAsync();
                cliente = null;
                content = null;
                data = null;
                return Result;
            }
            catch (Exception err)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al configurar el checador :/", DateTime.Now.ToString(), err.Message);
                Console.WriteLine(err.Message + " No se pudo registrar el Checador - Error de conexion");
                return "-1";
            }
        }
        public async Task<String> LogInChecador(String nombre, String userPass, String Checador, String Cliente, String uid)
        {
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/auth-Checador";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    {"Password",userPass },
                    {"Checador",Checador },
                    {"NombreChecador",nombre },
                    {"Cliente",Cliente },
                    {"uid",uid }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                String Result = await data.Content.ReadAsStringAsync();
                cliente = null;
                content = null;
                data = null;
                return Result;
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al recuperar el checador :/", DateTime.Now.ToString(), e.Message);
                Console.WriteLine(e.Message + " Error de Conexion");
                return "0";
            }
        }
        public async Task<List<Empleado>> ListaEmpleados(String Cliente, String uid, String Checador)
        {
            List<Empleado> ListaEmpleados = new List<Empleado>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/datosEmpleados";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                ///cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    {"Cliente",Cliente },
                    {"idChecador",Checador }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);

                ListaEmpleados = await data.Content.ReadAsAsync<List<Empleado>>();
                if (ListaEmpleados.ElementAt<Empleado>(0).idEmpleado == "-1")
                {
                    ListaEmpleados = new List<Empleado>();
                }
                cliente = null;
                content = null;
                data = null;
                return ListaEmpleados;
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al obtener la lista de los empleados :/", DateTime.Now.ToString(), e.Message);
                Console.WriteLine(e.Message);
                return ListaEmpleados;
            }
        }
        public async Task<List<Horario>> ListaHorarios(String Cliente, String Checador, String Empleado)
        {
            List<Horario> ListaHorarios = new List<Horario>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/horarioEmpleado";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    {"Cliente",Cliente },
                    {"Checador",Checador },
                    {"Empleado",Empleado }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                string info = await data.Content.ReadAsStringAsync();
                Console.WriteLine(info);
                ListaHorarios = await data.Content.ReadAsAsync<List<Horario>>();
                if (ListaHorarios.ElementAt<Horario>(0).Grupo == "-1")
                {
                    ListaHorarios = new List<Horario>();
                }
                cliente = null;
                content = null;
                data = null;
                return ListaHorarios;
            }
            catch (Exception e)
            {

                dBservice.GuardarRegistrosErrores("-1", "Error al recuperar el horario del empleado  :/ -" + Empleado, DateTime.Now.ToString(), e.Message);
                //Console.WriteLine(e.Message);
                Console.WriteLine("Error al obtener el Horario");
                return ListaHorarios;
            }
        }
        //Cambiar por un boleano para eliminar de la base de datos en local
        public async Task<Boolean> EnviarAsistencia(String cl, DatosAsistencias asistencia, List<AsistenciaDetalle> detalles)
        {
            try
            {
                var jsonStringDetalles = JsonConvert.SerializeObject(detalles);
                var dir = "https://api.servicioenlinea.mx/api-movil/RegistrarAsistenciaChecador";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<string, string>
                {
                    {"cliente",cl},
                    {"Fecha",asistencia.Fecha },
                    {"FechaTupla",asistencia.FechaTupla },
                    {"idGrupoPersona",Convert.ToString(asistencia.idGrupoPersona)},
                    {"MultipleHorario",Convert.ToString(asistencia.MultipleHorario)},
                    {"Detalles",jsonStringDetalles.ToString()}

                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                String result = await data.Content.ReadAsStringAsync();
                cliente = null;
                content = null;
                data = null;
                Console.WriteLine(result);
                return result.Equals("1");

            }
            catch (Exception er)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al enviar la sistencia :/ " + Convert.ToString(asistencia.idGrupoPersona), DateTime.Now.ToString(), er.Message);
                Console.WriteLine(er.Message + " Error de conexion");
                return false;
            }
        }
        public async Task<List<Banner>> ObtenerBannerChecador(String cl, String uid, String Checador)
        {
            List<Banner> ListaBanner = new List<Banner>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/ActualizarBanner";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var values = new Dictionary<String, String>
                {
                    { "Cliente",cl },
                    { "uid",uid },
                    { "Checador",Checador }
                };
                var content = new FormUrlEncodedContent(values);
                var data = await cliente.PostAsync(dir, content);
                List<Banner> listaBanner = await data.Content.ReadAsAsync<List<Banner>>();
                //Validamos los datos de recepcion
                Banner validBanner = listaBanner.ElementAt<Banner>(0);
                if (validBanner.id == -1)
                {
                    //El banner no es valido

                }
                cliente = null;
                content = null;
                data = null;
                return listaBanner;
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al obtener el banner del checador" , DateTime.Now.ToString(), e.Message);
                Console.WriteLine(e.Message);
                Console.WriteLine("Datos: \n" + e.Data);
                return ListaBanner;
            }

        }
        public async Task<String> ObtenerRecursoImagen(string cl, string recurso)
        {
            try
            {
                string dir = "https://api.servicioenlinea.mx/api-movil/ObtenerRecurso";
                string Ruta = "";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var value = new Dictionary<String, String>
                {
                    {"cliente",cl },
                    {"recurso",recurso }
                };
                var content = new FormUrlEncodedContent(value);
                var data = await cliente.PostAsync(dir, content);
                Ruta = await data.Content.ReadAsStringAsync();
                cliente = null;
                content = null;
                data = null;
                return Ruta;
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al obtener la imagen :/ ", DateTime.Now.ToString(), e.Message);
                Console.WriteLine(e.Message + " Error de conexion!");
                return "null";
            }
        }
        public async Task<Checador> RecuperarDatosChecador(String name, String password, String cl)
        {
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/login-checador";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var value = new Dictionary<String, String>
                {
                    {"cliente",cl },
                    {"nombre",name},
                    {"password",password }
                };
                var content = new FormUrlEncodedContent(value);
                var data = await cliente.PostAsync(dir, content);
                Checador checador = await data.Content.ReadAsAsync<Checador>();
                cliente = null;
                content = null;
                data = null;
                return checador;
            }
            catch (Exception er)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al recuperar los datos del checador ", DateTime.Now.ToString(), er.Message);
                Console.WriteLine(er.Message + "Error de conexion");
                Checador ch = new Checador() { id = -1 };
                return ch;

            }
        }
        public async Task<String> ObtenerLogoCliente(String cl)
        {
            try
            {
                string dir = "https://api.servicioenlinea.mx/api-movil/logo-checador";
                HttpClient Cliente = new HttpClient();
                Cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var value = new Dictionary<string, string>
                {
                    {"cliente",cl }
                };
                var content = new FormUrlEncodedContent(value);
                var data = await Cliente.PostAsync(dir, content);
                String img64 = await data.Content.ReadAsStringAsync();
                Cliente = null;
                content = null;
                data = null;
                return img64;
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al obtener el logo del cliente :/ ", DateTime.Now.ToString(), e.Message);
                Console.WriteLine(e.Message + "Error de conexion");
                return "null";
            }

        }
        public async Task<List<Bitacora>> ObtenerBitacoraChecador(String cl, String Checador, String UID, String Nombre)
        {
            List<Bitacora> listaBitacora = new List<Bitacora>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/obtenerBitacora";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var value = new Dictionary<String, String>
                {
                    {"Checador",Checador },
                    {"Cliente",cl },
                    {"UID",UID },
                    {"Nombre",Nombre }
                };
                var content = new FormUrlEncodedContent(value);
                var data = await cliente.PostAsync(dir, content);
                String erro = await data.Content.ReadAsStringAsync();
                Console.WriteLine(erro);
                listaBitacora = await data.Content.ReadAsAsync<List<Bitacora>>();
                if (listaBitacora.ElementAt<Bitacora>(0).id == -1)
                {
                    listaBitacora = new List<Bitacora>();
                }
                cliente = null;
                content = null;
                data = null;
                return listaBitacora;
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al obtener la bitacora del checador :/ ", DateTime.Now.ToString(), e.Message);
                Console.WriteLine(e.Message);
                return listaBitacora;
            }
        }
        public async Task<String> ActualizarConexion(String cl, String Checador, String UID, String Nombre)
        {
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/actualizarConexion";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var values = new Dictionary<String, String>
                {
                    {"Cliente",cl },
                    {"Nombre",Nombre },
                    {"Checador",Checador },
                    {"UID",UID }
                };
                var data = new FormUrlEncodedContent(values);
                var result = await cliente.PostAsync(dir, data);
                String conexion = await result.Content.ReadAsStringAsync();
                if (!conexion.Contains("null"))
                {
                    Console.WriteLine("Conexion Actualizada!!");
                }
                else
                {
                    Console.WriteLine("La conexion no se actualizo\nEspere hasta el siguiente ciclo");
                }
                cliente = null;
                values = null;
                result = null;
                return "OK";
            }
            catch (Exception error)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al actualizar la conexion del checador :/ ", DateTime.Now.ToString(), error.Message);
                Console.WriteLine(error.Message + "Error de conexion!");
                return "Error de conexion";
            }
        }
        public async Task<String> ActualizarBitacora(String cl, String Checador, String idTarea)
        {
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/TareaCompleta";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var data = new Dictionary<String, String>
                {
                    {"idTarea",idTarea },
                    {"Cliente",cl },
                    {"Checador",Checador }
                };
                var values = new FormUrlEncodedContent(data);
                var result = await cliente.PostAsync(dir, values);
                String complete = await result.Content.ReadAsStringAsync();
                Console.WriteLine(complete);
                if (complete.Contains("1"))
                {
                    Console.WriteLine("Actualizacion completa");
                }
                else
                {
                    Console.WriteLine("No se puedu actualizar la bitacora");
                }
                cliente = null;
                values = null;
                result = null;
                return "OK";
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al actualizar la bitacora :/ ", DateTime.Now.ToString(), e.Message);
                Console.WriteLine(e.Message + "Error de conexion!");
                return "Error de conexion";
            }
        }
        public async Task<Empleado> ObtenerEmpleadoTarea(String cl, String checador, String sector, String uid, String nombre, String empleado)
        {
            Empleado empleadoResult = new Empleado();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/TareaEmpleado";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var data = new Dictionary<String, String>
                {
                    {"Cliente",cl},
                    {"Checador",checador},
                    {"Sector",sector},
                    {"UID",uid},
                    {"Nombre",nombre},
                    {"Empleado",empleado }
                };
                var values = new FormUrlEncodedContent(data);
                var result = await cliente.PostAsync(dir, values);
                List<Empleado> empleados = await result.Content.ReadAsAsync<List<Empleado>>();
                empleadoResult = empleados.ElementAt<Empleado>(0);
                cliente = null;
                values = null;
                result = null;
                return empleadoResult;
            }
            catch (Exception err)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al obtener el empleado Tarea :/ ", DateTime.Now.ToString(), err.Message);
                Console.WriteLine(err + " Error de coexion!");
                empleadoResult.idEmpleado = "-1";
                return empleadoResult;
            }
        }
        public async Task<DatosChecador> ActualizarConfigurarcion(String cl, String checador)
        {
            DatosChecador checadorData = new DatosChecador() { id = -1 };
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/ActualizarConfiguracion";
                HttpClient cliente = new HttpClient();
                var data = new Dictionary<String, String> {
                    {"cliente",cl },
                    {"idChecador",checador }
                };
                var values = new FormUrlEncodedContent(data);
                var result = await cliente.PostAsync(dir, values);
                List<DatosChecador> listaDatos = await result.Content.ReadAsAsync<List<DatosChecador>>();
                cliente = null;
                values = null;
                result = null;
                return listaDatos.ElementAt<DatosChecador>(0);
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error al actualizar la configuracion del checador", DateTime.Now.ToString(), e.Message);
                Console.WriteLine(e);
                return checadorData;
            }
        }
        public async Task<List<Empleado>> ObtenerEmpleadosGeneral(String cl, String checador)
        {
            List<Empleado> listaEmpleados = new List<Empleado>();
            try
            {
                String dir = "https://api.servicioenlinea.mx/api-movil/EmpleadosGeneral";
                HttpClient cliente = new HttpClient();
                var data = new Dictionary<String, String>{
                    { "Cliente",cl},
                    { "idChecador", checador}
                };
                var values = new FormUrlEncodedContent(data);
                var result = await cliente.PostAsync(dir, values);
                listaEmpleados = await result.Content.ReadAsAsync<List<Empleado>>();
                Console.WriteLine(listaEmpleados.Count);
                cliente = null;
                values = null;
                result = null;
                return listaEmpleados;
            }
            catch (Exception e)
            {
                dBservice.GuardarRegistrosErrores("-1", "Error la obtener todos los empleados", DateTime.Now.ToString(), e.Message);
                Console.WriteLine("Error al obtener los empleados");
                return listaEmpleados;
            }
        }
    }
}
