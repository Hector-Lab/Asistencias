using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Asistencia.Clases;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using System.ComponentModel;
namespace Asistencia
{
    public class Controller
    {
        string tarea = "";
        readonly APIservice apiservice;
        readonly DBservice dbservice;
        readonly double screenW;
        readonly double screenH;
        BackgroundWorker worker;
        Random rn;
        public Controller()
        {
            rn = new Random();
            apiservice = new APIservice();
            dbservice = new DBservice();
            screenH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            screenW = 1000;
        }

        public int Comprobando()
        {
            int configurar = 0;
            if (!VerificarArchivos())
            {
                //Creamos el archivo de la base de datos
                if (CrearArchivos())
                {
                    configurar = 1;
                    dbservice.CrearTablas();
                }
                else
                {
                    configurar = -1;
                }
            }
            return configurar;
        }
        public Boolean VerificarArchivos()
        {
            //El archivo se llama temporal.db
            String current = Directory.GetCurrentDirectory(); // Se obtiene la direccion del proyecto y se le agrega el nombre del archivo
            current += "/data.db";
            Boolean found = File.Exists(current);
            return found;
        }
        public Boolean CrearArchivos()
        {
            try
            {
                String current = Directory.GetCurrentDirectory(); // Se obtiene la direccion del proyecto y se le agrega el nombre del archivo
                current += "/data.db";
                File.Create(current);
                return true;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                return false;
            }
        }
        public async Task<List<Cliente>> ListarClientes()
        {
            return await apiservice.ListaClientes();
        }
        public async Task<List<Sector>> ListarSectores(String cliente)
        {
            return await apiservice.ListaSectores(cliente);
        }
        public async Task<Boolean> EnviarConfiguracion(String cl, String sc, String pass, String Nombre, String nombreCliente)
        {
            String basicData = await apiservice.RegistrarChecador(cl, sc, pass, Nombre);
            if (basicData.Equals("-1") || basicData.Equals("0"))
            {
                return false;
            }
            else
            {
                String[] alldata = basicData.Split(',');
                String uid = alldata[0];
                String id = alldata[1];
                dbservice.GuardarUID(uid);
                dbservice.GuardarID(id);
                dbservice.GuardarNombre(Nombre);
                dbservice.GuardarSector(sc);
                dbservice.GuardarCliente(cl);
                dbservice.GuardarAplicaSector(sc == "-1" ? "0" : "1");
                dbservice.GuardarNombreEmpresa(nombreCliente);
                return true;
            }

        }
        public String StorageNombre()
        {
            return dbservice.ObtenerItem("Storage:Nombre");
        }
        public async Task<String> VerificarChecador(String nombre, String Pass)
        {
            //String nombre,String userPass, String Checador, String Cliente, String uid, String token
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            String valid = await apiservice.LogInChecador(nombre, Pass, Checador, cliente, uid);
            return valid;
        }
        //Este metodo se va a ejecutar cada 5 minutos (Agregar un proceso en segundo plano)
        public async void ObtenerEmpleados()
        {
            List<Empleado> empleados = new List<Empleado>();
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            String aplicaSector = dbservice.ObtenerItem("Storage:AplicaSector");
            try
            {
                if (aplicaSector != "0")
                {
                    
                    empleados = await apiservice.ListaEmpleados(cliente, uid, Checador);
                    Console.WriteLine(empleados.Count + " Numero de empleados");
                }
                else
                {
                    empleados = await apiservice.ObtenerEmpleadosGeneral(cliente, Checador);
                    Console.WriteLine("Numero de empleados: " + empleados.Count);
                }
                foreach (Empleado item in empleados)
                {
                    dbservice.GuardarEmpleado(item); // se guarda primero por si no tiene horario
                    List<Horario> ListaHorarios = await apiservice.ListaHorarios(cliente, Checador, item.idEmpleado);
                    dbservice.LimpiarHorarioEmpleado(item.idEmpleado); //Limpiamos el horario antiguo del empleado
                    foreach (Horario horario in ListaHorarios)
                    {
                        dbservice.GuardarHorario(horario, item.idEmpleado);
                    }
                }
                Console.WriteLine(empleados.Count + " : empleados actualizados");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public PresentacionAsistencia ObtenerHorarioEmpleado(String nfc_uid)
        {
            int proceso = 0;
            Empleado empleado = dbservice.ObtenerEmpleado(nfc_uid);
            PresentacionAsistencia asistenciaPantalla = new PresentacionAsistencia();
            if (empleado != null)
            {
                asistenciaPantalla.empleado = empleado; //Asignamos el empleado
                byte day = (byte)DateTime.Now.DayOfWeek;
                List<Horario> ListaHorario = dbservice.ObtenerHorario(empleado.idEmpleado, Convert.ToString(day));
                foreach (Horario item in ListaHorario)
                {
                    Console.WriteLine("Grupo: " + item.Grupo + " Detalle: " + item.GrupoDetalle + " - Dia:" + item.Dia);
                }
                if (ListaHorario.Count > 0)
                {
                    AsistenciaPantalla detalleAsistencia = VerificarAsistencia(ListaHorario); // Verificamso si es una asistencia un retardo o una salida
                    asistenciaPantalla.asistenciaDetalle = detalleAsistencia; //Asignamos los datos de sus asistencia
                    asistenciaPantalla.printAsistenciaDetall(); // Es solo para ver la infomacion del la asistencia
                    if (detalleAsistencia.Estatus != -1)
                    {
                        int multipleHorario = 1; //Normal
                        if (ListaHorario.Count > 1)
                        {
                            multipleHorario = 2;
                        }
                        bool existeHistoria = dbservice.verificarHistorial(empleado.idEmpleado);
                        if (existeHistoria)
                        {
                            proceso = -1;
                        }
                        else
                        {
                            proceso = dbservice.RegistrarAsistencias(asistenciaPantalla, multipleHorario);
                        }
                        if (proceso == -1)
                        {
                            Console.WriteLine("Ya hay una asistencia registrada");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("El empleado " + empleado.Nombre + " no tiene horario");
                }
            }
            else
            {
                Console.WriteLine("Empleado no encontrado!");
            }
            return asistenciaPantalla;
        }
        public void datosTest()
        {
            //dbservice.ObtenerDatosEmpleadoTodos();
            //dbservice.setEmpleado(); Horario
            //dbservice.DeleteData("Horario");
            //dbservice.DeleteData("Empleado");
            //dbservice.setHorarioEmpleado();
            //Obteniendo datos de la pantalla 
            /*String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            XML Egreso
            Cuentas por pagar COG
             */
        }
        public async Task<List<Empleado>> ListaEmpleados()
        {
            List<Empleado> listaEmpleados = new List<Empleado>();
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            try
            {
                listaEmpleados = await apiservice.ListaEmpleados(cliente, uid, Checador);
                return listaEmpleados;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return listaEmpleados;
            }
        }
        public void RegistrarEmpleado(Empleado empleado)
        {
            dbservice.GuardarEmpleado(empleado);
        }
        public async Task<List<Horario>> ListaHorarioEmpleado(String idEmpleado)
        {
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            List<Horario> ListaHorarios = await apiservice.ListaHorarios(cliente, Checador, idEmpleado);
            return ListaHorarios;
        }
        public void BorrarHorarioAnterior(String idEmpleado)
        {
            dbservice.LimpiarHorarioEmpleado(idEmpleado); //Limpiamos el horario antiguo del empleado
        }
        public void InsertarHorarioEmpleado(Horario horario, String idEmpleado)
        {
            dbservice.GuardarHorario(horario, idEmpleado);
        }
        //Falta agregar los limites de checado a la salida y entrada 
        public AsistenciaPantalla VerificarAsistencia(List<Horario> listaHorarios = null)
        {
            Console.WriteLine("Calculando Asistencia");
            //Datos para verificar a los superUsuarios
            int retardo = -1;
            int faltas = -1;
            //Datos de salida
            String HoraEntradaPantalla = "";
            String HoraSalidaPantalla = "";
            String HoraEntradaSistema = "";
            String HoraSalidaSistema = "";
            int EstatusAsistencia = 1;
            int Tipo = -1;
            String EstatusPantalla = "";
            String grupo = listaHorarios[0].Grupo;
            DateTime horaActual = DateTime.Now;
            if (listaHorarios != null)
            {
                Console.WriteLine("Grupo del empleado" + grupo);
            }
            else
            {
                //Verificacion de las asistencias con una lista  Este codigo solo es de pruebas
                List<Horario> horarios = new List<Horario>();

                Horario horarioUno = new Horario();
                horarioUno.HoraEntrada = "07:30:00";
                horarioUno.HoraSalida = "09:10:00";
                horarioUno.Tolerancia = 15;
                horarioUno.Retardo = 90;
                Horario horaDos = new Horario();
                horaDos.HoraEntrada = "13:10:00";
                horaDos.HoraSalida = "15:00:00";
                Horario horaTres = new Horario();
                horaTres.HoraEntrada = "17:30:00";
                horaTres.HoraSalida = "20:00:00";
                horaTres.Tolerancia = 10;
                horaTres.Retardo = 15;
                horaDos.Retardo = 60;
                horaDos.Tolerancia = 15;
                horarios.Add(horarioUno);
                horarios.Add(horaDos);
                horarios.Add(horaTres);
                listaHorarios = horarios;
                //8:00:00

            }
            foreach (Horario hora in listaHorarios)
            {
                DateTime horaEntrada = Convert.ToDateTime(hora.HoraEntrada);
                DateTime horaEntradaMinimo = horaEntrada.AddMinutes(-30);
                DateTime horaEntradaTolerancia = horaEntrada.AddMinutes(hora.Tolerancia);
                DateTime horaEntradaRetardo = horaEntradaTolerancia.AddMinutes(hora.Retardo);
                DateTime horaEntradaMaximo = horaEntradaRetardo.AddMinutes(30);
                DateTime horaSalida = Convert.ToDateTime(hora.HoraSalida);
                //Condiciones para el registro de las entradas
                if (horaActual >= horaEntradaMinimo && horaActual <= horaEntradaTolerancia)
                {
                    Console.WriteLine("Asistencias: " + horaEntrada + " - " + horaEntradaTolerancia + " - " + horaActual);
                    EstatusAsistencia = 1;
                    Tipo = 1;
                    EstatusPantalla = "Asistencias";
                    HoraEntradaPantalla = horaEntrada.ToString("HH:mm:ss");
                    HoraSalidaPantalla = horaSalida.ToString("HH:mm:ss");
                    HoraSalidaSistema = hora.HoraSalida;
                    HoraEntradaSistema = hora.HoraEntrada;
                    retardo = hora.LimiteRetardos;
                    faltas = hora.LimiteFaltas;
                }
                else if (horaActual >= horaEntradaTolerancia && horaActual <= horaEntradaRetardo)
                {
                    Console.WriteLine("Retardo: " + horaEntrada + " - " + horaEntradaRetardo + " - " + horaActual);
                    Tipo = 1;
                    EstatusAsistencia = 2;
                    EstatusPantalla = "Retardo";
                    HoraEntradaPantalla = horaEntrada.ToString("HH:mm:ss");
                    HoraSalidaPantalla = horaSalida.ToString("HH:mm:ss");
                    HoraSalidaSistema = hora.HoraSalida;
                    HoraEntradaSistema = hora.HoraEntrada;
                    retardo = hora.LimiteRetardos;
                    faltas = hora.LimiteFaltas;
                }
                else if (horaActual >= horaEntrada && horaActual <= horaSalida && horaActual <= horaEntradaMaximo)
                {
                    Console.WriteLine("Falta: " + horaEntrada + " - " + horaEntradaRetardo + " - " + horaSalida + " - M " + horaEntradaMaximo + " - " + horaActual);
                    Tipo = 1;
                    EstatusAsistencia = 3;
                    EstatusPantalla = "Falta";
                    HoraEntradaPantalla = horaEntrada.ToString("HH:mm:ss");
                    HoraSalidaPantalla = horaSalida.ToString("HH:mm:ss");
                    HoraSalidaSistema = hora.HoraSalida;
                    HoraEntradaSistema = hora.HoraEntrada;
                    retardo = hora.LimiteRetardos;
                    faltas = hora.LimiteFaltas;
                }
                else if (horaActual >= horaEntrada && horaActual <= horaSalida && horaActual > horaEntradaMaximo)
                {
                    Console.WriteLine(horaActual + "   No Checo su hora de entrada" + horaEntradaMaximo);
                    Tipo = 1;
                    EstatusAsistencia = 9;
                    EstatusPantalla = "Falta  (Checador Cerrado)";
                    HoraEntradaPantalla = horaEntrada.ToString("HH:mm:ss");
                    HoraSalidaPantalla = horaSalida.ToString("HH:mm:ss");
                    HoraSalidaSistema = hora.HoraSalida;
                    HoraEntradaSistema = hora.HoraEntrada;
                    retardo = hora.LimiteRetardos;
                    faltas = hora.LimiteFaltas;
                }
            }
            //Metodo para verificar las salidas
            if (Tipo == -1) // si esa vacio es por que no es entrada es salida
            {
                String horaSalidaAnterior = "";
                DateTime salidaAnterior = new DateTime(); //Tiene que sser despeus de los 30 minutos maximos
                //DateTime horaEntradaMinimo = new DateTime();
                foreach (Horario hora in listaHorarios)
                {
                    retardo = hora.LimiteRetardos;
                    faltas = hora.LimiteFaltas;
                    DateTime horaSalida = Convert.ToDateTime(hora.HoraSalida);
                    DateTime horaEntrada = Convert.ToDateTime(hora.HoraEntrada);
                    DateTime horaEntradaMinimo = horaEntrada.AddMinutes(-30);
                    DateTime horaSalidaMaximo = horaSalida.AddMinutes(30);
                    if (!String.IsNullOrEmpty(horaSalidaAnterior)) //Caso de multiples horarios
                    {
                        if (horaActual >= salidaAnterior && horaActual >= horaEntradaMinimo && horaActual < horaSalida)
                        {
                            //Console.WriteLine("Hora de salida intermedia (varios horarios) ");
                            Console.WriteLine("Salida Comida: ");
                            Console.WriteLine("Hora Actual: " + horaActual + " HoraSalidaAnterior: " + horaSalidaAnterior + " - Hora Entrada Minimo: " + horaEntradaMinimo);
                            Tipo = 2;
                            EstatusAsistencia = 1;
                            EstatusPantalla = "Salida";
                            HoraEntradaPantalla = salidaAnterior.ToString("HH:mm:ss");
                            HoraSalidaPantalla = horaEntrada.ToString("HH:mm:ss");
                            HoraEntradaSistema = salidaAnterior.ToString("HH:mm:ss");
                            HoraSalidaSistema = horaEntrada.ToString("HH:mm:ss");

                        }
                        else if (horaActual >= horaSalida && horaActual <= horaSalidaMaximo)
                        {
                            Console.WriteLine("Hora de salida normal (varios horarios)");
                            Tipo = 2;
                            EstatusAsistencia = 1;
                            EstatusPantalla = "Salida";
                            HoraEntradaPantalla = horaEntrada.ToString("HH:mm:ss");
                            HoraSalidaPantalla = horaSalida.ToString("HH:mm:ss");
                            HoraEntradaSistema = hora.HoraEntrada;
                            HoraSalidaSistema = hora.HoraSalida;

                        }
                        else if (horaActual > horaSalida && horaActual > horaSalidaMaximo && horaSalida > salidaAnterior)
                        {
                            Console.WriteLine("Hora de chacado se cerro Salida (varios horarios) " + horaSalidaMaximo);
                            Tipo = 2;
                            EstatusAsistencia = 9;
                            EstatusPantalla = "Salida";
                            HoraEntradaPantalla = horaEntrada.ToString("HH:mm:ss");
                            HoraSalidaPantalla = horaSalida.ToString("HH:mm:ss");
                            HoraEntradaSistema = hora.HoraEntrada;
                            HoraSalidaSistema = hora.HoraSalida;

                        }
                    }
                    else // Caso de un solo horario
                    {
                        if (horaActual >= horaSalida && horaActual <= horaSalidaMaximo)
                        {
                            Console.WriteLine("hora de salida normal un horario Hora" + horaSalidaMaximo);
                            Tipo = 2;
                            EstatusAsistencia = 1;
                            EstatusPantalla = "Salida";
                            HoraEntradaPantalla = horaEntrada.ToString("HH:mm:ss");
                            HoraSalidaPantalla = horaSalida.ToString("HH:mm:ss");
                            HoraEntradaSistema = hora.HoraEntrada;
                            HoraSalidaSistema = hora.HoraSalida;

                        }
                        else if (horaActual > horaSalida && horaActual > horaSalidaMaximo)
                        {
                            //Console.WriteLine("Hora de checado esta cerrado ( Salida ) un solo horario" + "Hora: " + horaSalidaMaximo);
                            Tipo = 2;
                            EstatusAsistencia = 9;
                            EstatusPantalla = "Salida (Checador Cerrado)";
                            HoraEntradaPantalla = horaEntrada.ToString("HH:mm:ss");
                            HoraSalidaPantalla = horaSalida.ToString("HH:mm:ss");
                            HoraEntradaSistema = hora.HoraEntrada;
                            HoraSalidaSistema = hora.HoraSalida;
                            Console.WriteLine(EstatusPantalla + ": " + HoraEntradaPantalla + " - " + HoraSalidaPantalla);

                        }
                    }
                    horaSalidaAnterior = hora.HoraSalida;
                    salidaAnterior = Convert.ToDateTime(horaSalidaAnterior);
                }
                if (Tipo == -1)
                {
                    DateTime horaEntrada = Convert.ToDateTime(listaHorarios.ElementAt<Horario>(0).HoraEntrada);
                    DateTime horaEntradaMinimo = horaEntrada.AddMinutes(-30);
                    Console.WriteLine("Aun no se avilita la hora de entrada" + listaHorarios.ElementAt<Horario>(0).HoraEntrada + " - " + horaActual);
                    Tipo = -1;
                    EstatusAsistencia = -1;
                    EstatusPantalla = "Entrada";
                    HoraEntradaPantalla = horaEntradaMinimo.ToString("HH:mm:ss");
                    HoraSalidaPantalla = listaHorarios.ElementAt<Horario>(0).HoraEntrada;
                    HoraEntradaSistema = HoraEntradaPantalla;
                    HoraSalidaSistema = HoraSalidaPantalla;
                }
            }
            AsistenciaPantalla pantalla = new AsistenciaPantalla();
            pantalla.Estatus = Tipo;
            Console.WriteLine("Mi configuracion: Faltas: " + faltas + " - " + retardo);
            if (retardo == 0 && faltas == 0)
            {
                pantalla.EstatusAsistencia = 1;
                pantalla.EstatusPantalla = (Tipo == 2 ? "Salida" : "Entrada");
            }
            else
            {
                pantalla.EstatusAsistencia = EstatusAsistencia;
                pantalla.EstatusPantalla = EstatusPantalla;
            }
            pantalla.HoraSalida = HoraSalidaPantalla;
            pantalla.HoraEntrada = HoraEntradaPantalla;
            pantalla.horaAsistencia = horaActual.ToString("HH:mm:ss");
            pantalla.idGrupoPersona = Convert.ToInt32(grupo);
            pantalla.HoraEntradaSistema = HoraEntradaSistema;
            pantalla.HoraSalidaSistema = HoraSalidaSistema;
            return pantalla;
        }
        public async void enviarAsistenciasGuardadas()
        {
            List<DatosAsistencias> listaAsistencias = dbservice.ObtenerDatosAsistencias();
            //Obtenemos el cliente
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            if (listaAsistencias.Count > 0)
            {
                //Recorremos la lista en busca de los detalles de la asistencia
                foreach (DatosAsistencias asistencia in listaAsistencias)
                {
                    List<AsistenciaDetalle> listaAsistenciaDetalles = dbservice.ObtenerDatosAsistenciaDetalles(asistencia.id);
                    bool complete = await apiservice.EnviarAsistencia(cliente, asistencia, listaAsistenciaDetalles);
                    //Si se inserto en la base de datos en la nube se borra y se va al historial
                    if (complete)
                    {
                        dbservice.BorrarDatosAsistencia(asistencia, listaAsistenciaDetalles);
                    }
                }
            }
            else
            {
                Console.WriteLine("Esta madre esta vacia!!");
            }
        }
        public async Task<List<RecursoBanner>> ObtenerBannerChecador()
        {
            //Obtenemos los datos del Checador de la base de datos
            List<RecursoBanner> RecursosBanner = new List<RecursoBanner>();
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            List<Banner> ListaBanner = await apiservice.ObtenerBannerChecador(cliente, uid, Checador);
            dbservice.DeleteData("Banner");
            if (ListaBanner.Count > 0 && ListaBanner.ElementAt<Banner>(0).id != -1)
            {
                Console.WriteLine("Actualizando Banner");
                return await obtenerRecursos(ListaBanner, cliente);
                //Recursos testing
                //return await ObtenerRecursosFull(ListaBanner, cliente);
            }
            else
            {
                Console.WriteLine("Banner Actualizado");
                return RecursosBanner;
            }

        }
        public async Task<List<RecursoBanner>> obtenerRecursos(List<Banner> listaBanner, String cliente)
        {
            //List<String> ListaRutas = new List<string>();
            List<RecursoBanner> ListaRecursor = new List<RecursoBanner>();
            int count = 1;
            //Recorremos el areglos para verificar los datos del banner
            foreach (Banner item in listaBanner)
            {
                String imgName = "Fondo" + count + ".png";
                Console.WriteLine("Recurso: " + item.Recurso);
                if (item.Tipo == 1)//Tipo Imagen
                {
                    //Procesamos la imagen 
                    String direccionImagem = await apiservice.ObtenerRecursoImagen(cliente, item.idRepositorio);
                    if (!direccionImagem.Equals("null"))
                    {
                        WebClient web = new WebClient();
                        Stream stream = web.OpenRead(direccionImagem);
                        Bitmap bitmap = new Bitmap(stream);
                        int percent = 100;
                        int imgWidth = bitmap.Width / percent;
                        //Variables Temporales
                        int imgW = bitmap.Width;
                        int imgH = bitmap.Height;
                        if (screenW < bitmap.Width)
                        {
                            Console.WriteLine(imgWidth + " - " + (screenW + 200));
                            while ((imgWidth < (screenW)))
                            {
                                if (percent > 1)
                                {
                                    imgWidth = bitmap.Width / percent;
                                    percent--;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            //Comenzamos el proceso de reduccion de calidad de la imagen
                            int resizedHeigth = bitmap.Height / percent;
                            int resizedWidth = bitmap.Width / percent;
                            Console.WriteLine(direccionImagem);
                            Console.WriteLine("Resize Heigth : " + resizedHeigth + " - Resize Width: " + resizedWidth);
                            Bitmap imageResize = new Bitmap(resizedWidth, resizedHeigth, PixelFormat.Format32bppRgb);
                            imageResize.SetResolution(resizedHeigth, resizedWidth);
                            using (Graphics graphics = Graphics.FromImage(imageResize))
                            {
                                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                                graphics.DrawImage(bitmap, new Rectangle(0, 0, resizedWidth, resizedHeigth), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                                MemoryStream memoryStream = new MemoryStream();
                                imageResize.Save(memoryStream, ImageFormat.Png);
                                bitmap = (Bitmap)Bitmap.FromStream(memoryStream);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No se Modifica el tamaño");
                            Console.WriteLine("Pantalla : " + screenW + " - " + screenH);
                            Console.WriteLine("Imagen: " + imgW + " - " + imgH);
                        }
                        try
                        {
                            Console.WriteLine("DescargaNormal");
                            bitmap.Save(Directory.GetCurrentDirectory() + "/" + imgName, ImageFormat.Png);
                            if (dbservice.insertarBanner(item, imgName))
                            {
                                RecursoBanner imgRecursos = new RecursoBanner() { nombre = imgName, tipo = item.Tipo, img = bitmap };
                                ListaRecursor.Add(imgRecursos);
                            }
                        }
                        catch (Exception e)
                        {
                            dbservice.GuardarRegistrosErrores("-1", "Error al descargar la imagen", DateTime.Now.ToString(), e.Message);
                            bool saved = false;
                            //La cambiamos por un while para hacer fuerza
                            int intentos = 0;
                            while (!saved)
                            {
                                string fecha = DateTime.Now.ToString();
                                try
                                {
                                    //Console.WriteLine("Error al escribir la imagen, escribir en el archivo auxiliar\n" + e.Message);
                                    imgName = "FondoTemp" + rn.Next(1000) + ".png";
                                    Console.WriteLine("Forzando: " + Directory.GetCurrentDirectory() + "\\" + imgName);
                                    bitmap.Save(Directory.GetCurrentDirectory() + "\\" + imgName, ImageFormat.Png);
                                    if (dbservice.insertarBanner(item, imgName))
                                    {
                                        RecursoBanner imgRecursos = new RecursoBanner() { nombre = imgName, tipo = item.Tipo, img = bitmap };
                                        ListaRecursor.Add(imgRecursos);
                                        saved = true;
                                    }
                                }
                                catch (Exception err)
                                {
                                    dbservice.GuardarRegistrosErrores("-1", "Error al descargar la imagen " + intentos + " - " + imgName, DateTime.Now.ToString(), err.Message);
                                    saved = false;
                                    intentos++;
                                    if(intentos> 10)
                                    {
                                        throw err;
                                    }
                                }
                            }


                        }

                        stream.Flush();
                        stream.Close();
                        web.Dispose();
                        count++;
                    }
                    else
                    {
                        Console.WriteLine("Recurso no encontrado: " + direccionImagem);
                    }
                }
                else
                {
                    if (dbservice.insertarBanner(item, item.Recurso))
                    {
                        Console.WriteLine("Es Texto: " + item.Recurso);
                        RecursoBanner txtBanner = new RecursoBanner() { nombre = item.Recurso, tipo = item.Tipo };
                        ListaRecursor.Add(txtBanner);
                    }
                }

            }
            return ListaRecursor;
        }
        //Metodo nuevo para descargar sin compresion
        public async Task<List<RecursoBanner>> ObtenerRecursosFull( List<Banner> listaBanner, String cliente)
        {
            List<RecursoBanner> ListaRecursor = new List<RecursoBanner>();
            int count = 1;
            foreach (Banner item in listaBanner)
            {
                String imgName = "Fondo" + count + ".png";
                Console.WriteLine("Verificando Recurso: " + item.Recurso);
                //Console.WriteLine(   item.idRepositorio == " " ? "SI":"NO" );
                if (!dbservice.VerificarInsercionBanne(item.idRepositorio))
                {
                    Console.WriteLine("El recurso se descarga");
                    if (item.Tipo == 1)
                    {
                        String direccionImagen = await apiservice.ObtenerRecursoImagen(cliente, item.idRepositorio);
                        if (!direccionImagen.Equals("null"))
                        {
                            WebClient web = new WebClient();
                            Stream stream = web.OpenRead(direccionImagen);
                            Bitmap bitmap = new Bitmap(stream);
                            try
                            {
                                Console.WriteLine("Descargar Normall");
                                bitmap.Save(Directory.GetCurrentDirectory() + "/" + imgName, ImageFormat.Png);
                                if (dbservice.insertarBanner(item, imgName))
                                {
                                    RecursoBanner imgRecurso = new RecursoBanner() { nombre = imgName, tipo = item.Tipo, img = bitmap };
                                    ListaRecursor.Add(imgRecurso);
                                }
                            }
                            catch (Exception err)
                            {
                                Console.WriteLine(err.Message);
                                imgName = "FondoTemporal" + count + ".png";
                                bitmap.Save(Directory.GetCurrentDirectory() + "/" + imgName, ImageFormat.Png);
                                if (dbservice.insertarBanner(item, imgName))
                                {
                                    RecursoBanner imgRecurso = new RecursoBanner() { nombre = imgName, tipo = item.Tipo, img = bitmap };
                                    ListaRecursor.Add(imgRecurso);
                                }
                            }
                            stream.Flush();
                            stream.Close();
                            web.Dispose();
                            count++;
                        }
                        else
                        {
                            Console.WriteLine("Recurso no encontrado");
                        }
                    }
                    else
                    {
                        if (dbservice.insertarBanner(item, item.Recurso))
                        {
                            Console.WriteLine("Es Texto: " + item.Recurso);
                            RecursoBanner txtBanner = new RecursoBanner() { nombre = item.Recurso, tipo = item.Tipo };
                            ListaRecursor.Add(txtBanner);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Recurso ya descargado");
                }


            }
            return ListaRecursor;
        }
        
        public async Task<Checador> RecuperarDatosChecador(String NombreChecador, String PasswordChecador, String Cliente)
        {
            Checador checador = await apiservice.RecuperarDatosChecador(NombreChecador, PasswordChecador, Cliente);
            return checador;
        }
        public void InsertarInformacionChecador(Checador checador, String nombreCliente)
        {
            dbservice.GuardarUID(checador.UID);
            dbservice.GuardarID(Convert.ToString(checador.id));
            dbservice.GuardarNombre(checador.Nombre);
            dbservice.GuardarSector(checador.Sector);
            dbservice.GuardarCliente(Convert.ToString(checador.Cliente));
            dbservice.GuardarAplicaSector(Convert.ToString(checador.AplicaSector));
            dbservice.GuardarNombreEmpresa(nombreCliente);
        }
        public async Task<String> obtenerLogoCliente(String cl)
        {
            //Hacemos el llamado de la API
            String datos = await apiservice.ObtenerLogoCliente(cl);
            return datos;
        }
        public void LimpiarBaseDatos()
        {
            dbservice.CrearTablas();
        }
        public async Task<String> obtenerBitacora()
        {
            String TareasCompletas = "";
            //Obtenemos los datos del checador
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            String nombre = dbservice.ObtenerItem("Storage:Nombre");
            //String cl,String Checador,String UID, String Nombre)
            List<Bitacora> listaBitacora = await apiservice.ObtenerBitacoraChecador(cliente, Checador, uid, nombre);
            //Recorremos la bitacora para ejecutar las tareas pendientes
            foreach (Bitacora bitacora in listaBitacora)
            {
                Console.WriteLine(bitacora.Descripcion);
                if (bitacora.id != -1)//Verificamos si exieten datos validos en la lista
                {
                    if (bitacora.Tarea == 1) // Se actualiza la base de datos
                    {
                        Console.WriteLine("Tarea en ejecucion: " + bitacora.Descripcion);
                        ObtenerEmpleados();                                                 //Se actualizan los empleados
                        await ObtenerBannerChecador();                                            //Se acualoza el banner
                        string respuesta = await apiservice.ActualizarConexion(cliente, Checador, uid, nombre);      //Se actualiza la conexion
                        Console.WriteLine("Se actualizo la conexion: " + respuesta);
                        respuesta = await apiservice.ActualizarBitacora(cliente, Convert.ToString(bitacora.idChecador), Convert.ToString(bitacora.id));
                        Console.WriteLine("Se actualizo la bitacora: " + respuesta);
                        TareasCompletas += bitacora.Tarea + " - " + bitacora.Descripcion + "completa";
                    } //Actualizacion de base de datos completa
                    if (bitacora.Tarea == 0)
                    {
                        DatosChecador checadorDatos = await apiservice.ActualizarConfigurarcion(cliente, Checador);
                        if (checadorDatos.id != -1)
                        {
                            Console.WriteLine("Nuevo Nombre" + checadorDatos.Nombre);
                            dbservice.DeleteData("StorageValues");
                            dbservice.GuardarID(Convert.ToString(checadorDatos.id));
                            dbservice.GuardarCliente(Convert.ToString(checadorDatos.Cliente));
                            dbservice.GuardarSector(Convert.ToString(checadorDatos.Sector) == "" ? "-1" : Convert.ToString(checadorDatos.Sector));
                            dbservice.GuardarNombre(checadorDatos.Nombre);
                            dbservice.GuardarUID(checadorDatos.UID);
                            await apiservice.ActualizarConexion(cliente, Convert.ToString(checadorDatos.id), checadorDatos.UID, checadorDatos.Nombre);
                            await apiservice.ActualizarBitacora(cliente, Convert.ToString(checadorDatos.id), Convert.ToString(bitacora.id));
                            TareasCompletas += bitacora.Tarea + " - " + bitacora.Descripcion + "completa";
                        }
                    } //Actualizacion de informacion
                    if (bitacora.Tarea == 2)
                    {
                        string[] bitacoraData = bitacora.Descripcion.Split('-');
                        String nombreTarea = bitacoraData[1];
                        String datosTarea = bitacoraData[0];
                        Console.WriteLine("Tarea en ejecucion: " + nombreTarea + "  Datos de la tarea: " + datosTarea);
                        string[] empleadoTarea = datosTarea.Split('_');
                        //ObtenerEmpleados();                                                 //Actualizar Empleado
                        TareaEmpleado(empleadoTarea[1]);
                        await apiservice.ActualizarConexion(cliente, Checador, uid, nombre);
                        await apiservice.ActualizarBitacora(cliente, Convert.ToString(bitacora.idChecador), Convert.ToString(bitacora.id));
                        TareasCompletas += bitacora.Tarea + " - " + bitacora.Descripcion + "completa";
                    } //Actualizacion de empleado
                    if (bitacora.Tarea == 3)
                    {
                        Console.WriteLine("Tarea en ejecucion: " + bitacora.Descripcion); //Actualizar Banner
                        await ObtenerBannerChecador();
                        await apiservice.ActualizarConexion(cliente, Checador, uid, nombre);
                        Console.WriteLine("Id de la bitacora: " + bitacora.id + " - " + bitacora.idChecador + " - " + cliente);
                        await apiservice.ActualizarBitacora(cliente, Convert.ToString(bitacora.idChecador), Convert.ToString(bitacora.id));
                        TareasCompletas += bitacora.Tarea + " - " + bitacora.Descripcion + "completa";
                    } //Actualizacion del banner
                    if (bitacora.Tarea == 10)
                    {
                        Console.WriteLine("Tarea 10");
                        if(worker == null)
                        {
                            worker = new BackgroundWorker();
                        }
                        
                        if (!worker.IsBusy)
                        {
                            tarea = Convert.ToString(bitacora.id);
                            worker.DoWork += enviarAsistenciaWorker;
                            worker.RunWorkerAsync();
                        }
                    }//Borrado del tablas
                }
                else
                {
                    //actualizamos la conexion para mostrar que el dispositivo este activo 
                    await apiservice.ActualizarConexion(cliente, Checador, uid, nombre);
                    Console.WriteLine("Bitacora vacia!!");
                }
            }
            return TareasCompletas;
        }
        public async void TareaEmpleado(String empleado)
        {
            try
            {
                //Obtenemos los datos del checador
                String cliente = dbservice.ObtenerItem("Storage:Cliente");
                String uid = dbservice.ObtenerItem("Storage:UID");
                String Checador = dbservice.ObtenerItem("Storage:idChecador");
                String nombre = dbservice.ObtenerItem("Storage:Nombre");
                String sector = dbservice.ObtenerItem("Storage:Sector");
                Empleado empleadoResult = await apiservice.ObtenerEmpleadoTarea(cliente, Checador, sector, uid, nombre, empleado);
                if (!empleadoResult.idEmpleado.Equals("-1")) // si el empleado es valido
                {
                    dbservice.GuardarEmpleado(empleadoResult); // se guarda primero por si no tiene horario
                    List<Horario> ListaHorarios = await apiservice.ListaHorarios(cliente, Checador, empleadoResult.idEmpleado);
                    dbservice.LimpiarHorarioEmpleado(empleadoResult.idEmpleado); //Limpiamos el horario antiguo del empleado
                    foreach (Horario horario in ListaHorarios)
                    {
                        dbservice.GuardarHorario(horario, empleadoResult.idEmpleado);
                    }
                }
                else
                {
                    Console.WriteLine("El empleado no existe: " + empleado);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message + " Error de conexion");

            }

        }
        public List<RecursoBanner> ObtenerBannerBaseLocal()
        {
            Console.WriteLine("Primero paso");
            return dbservice.ObtenerBannerChecador();
        }
        public void descargarIconos()
        {
            //Verificamos si la campeta existe
            String dirIconos = Directory.GetCurrentDirectory() + "/Iconos";
            if (!Directory.Exists(dirIconos))
            {
                //Lo creamos
                Directory.CreateDirectory(dirIconos);
            }
            Console.WriteLine("Descargando Iconos");
            /*String SucessIcon = "https://suinpac.com/repositorio/43/2021/08/31/U3VjY2Vzc184My5wbmc=";
            String ErrorIcon = "https://suinpac.com/repositorio/43/2021/08/31/RXJyb3JfOTIucG5n";
            String CloseIcon = "https://suinpac.com/repositorio/43/2021/08/31/Q2xvY2tfNzIucG5n";
            String Conexion = "https://suinpac.com/repositorio/43/2021/08/31/QWxlcnRfMjAucG5n";*/
            String[] Iconos = new string[]
            {
                "https://suinpac.com/repositorio/43/2021/08/31/U3VjY2Vzc184My5wbmc=",/*Sucess*/
                "https://suinpac.com/repositorio/43/2021/08/31/RXJyb3JfOTIucG5n",/*Error*/
                "https://suinpac.com/repositorio/43/2021/08/31/Q2xvY2tfNzIucG5n",/*Close*/
                "https://suinpac.com/repositorio/43/2021/08/31/QWxlcnRfMjAucG5n",/*Conexion*/
                "https://suinpac.com/repositorio/43/2021/09/02/TkZDRXJyb3JfMjkucG5n",/*NFC Error*/
            };
            String[] IconosNombre = new string[] { "Success.png", "Error.png", "Close.png", "Conexion.png", "NFCError.png" };
            int indexIconos = 0;
            foreach (String iconDir in Iconos)
            {
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(iconDir);
                Bitmap bitmap = new Bitmap(stream);
                bitmap.Save(Directory.GetCurrentDirectory() + "/Iconos/" + (IconosNombre[indexIconos]));
                dbservice.GuardarIconos(Directory.GetCurrentDirectory() + "/Iconos/" + (IconosNombre[indexIconos]), IconosNombre[indexIconos]);
                indexIconos++;
            }
        } //Agregar la validacion de conexion
        public Dictionary<String, String> CargarIconosDB()
        {
            return dbservice.ObtenerIconos();
        }
        public async void descargarRecursos()
        {
            String TareasCompletas = "";
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            String nombre = dbservice.ObtenerItem("Storage:Nombre");
            Console.WriteLine("Tarea en ejecucion: Descargar Recursos");
            ObtenerEmpleados();                                                 //Se actualizan los empleados
            await ObtenerBannerChecador();                                            //Se acualoza el banner
            descargarIconos();
            await apiservice.ActualizarConexion(cliente, Checador, uid, nombre);      //Se actualiza la conexion
            TareasCompletas = "Tarea en ejecucion: Descargar Recursos - completa";
            Console.WriteLine(TareasCompletas);
        }
        public int verificarEmpleados()
        {
            return dbservice.validarEmpleados();
        }
        public String ObtenerNombreCliente()
        {
            return dbservice.ObtenerItem("Storage:Empresa");
        }
        public void verificarEstructuraDartos()
        {
            List<AsistenciaDetalle> detalles = dbservice.ObtenerDatosAsistenciaDetalles(4);
            //Lo convertimos en json
            string datos = JsonConvert.SerializeObject(detalles);
            Console.WriteLine(datos);

        }
        public async void enviarAsistenciaWorker(object sender, DoWorkEventArgs e)
        {
            String cliente = dbservice.ObtenerItem("Storage:Cliente");
            String uid = dbservice.ObtenerItem("Storage:UID");
            String Checador = dbservice.ObtenerItem("Storage:idChecador");
            String nombre = dbservice.ObtenerItem("Storage:Nombre");
            enviarAsistenciasGuardadas();
            //Borramos la base de datos
            dbservice.EliminarTabla("Empleado");
            dbservice.EliminarTabla("Horario");
            //Verificamos si hay cambos en los sectores
            DatosChecador Verificar = await apiservice.ActualizarConfigurarcion(cliente, Checador);
            Console.WriteLine("Configuracion: " + Verificar.Sector);
            if(Verificar.Sector == -1 || Convert.ToString(Verificar.Sector) == "" )
            {
                //Es general Actualizamos la configururadcio
                dbservice.EliminarSectorDB();
                dbservice.EliminaAplicaSector();
                dbservice.GuardarSector("-1");
                dbservice.GuardarAplicaSector("0");
                ObtenerEmpleados();
            }
            else
            {
                //No es general Actualizamos el sector y el campo aplica sector
                dbservice.EliminarSectorDB();
                dbservice.EliminaAplicaSector();
                //Creamos los nuevos campos
                dbservice.GuardarAplicaSector("1");
                dbservice.GuardarSector(Convert.ToString(Verificar.Sector));
                ObtenerEmpleados();

            }
            //Actualizamos la conexion
            await apiservice.ActualizarBitacora(cliente, Checador ,"10");
            await apiservice.ActualizarConexion(cliente, Checador, uid, nombre);
        }
        public async Task<bool> VerificarCambioBanner()
        {
            bool found = false;
            //String cl,String Checador,String UID, String Nombre)
            List<Bitacora> listaBitacora = await apiservice.ObtenerBitacoraChecador(dbservice.ObtenerItem("Storage:Cliente"), dbservice.ObtenerItem("Storage:idChecador"), dbservice.ObtenerItem("Storage:UID"), dbservice.ObtenerItem("Storage:Nombre"));
            //Verificamos si hay existe la tarea 3 
            foreach( Bitacora item in listaBitacora ){
                if(item.Tarea == 3)
                {
                    found = true;
                }
            }
            return found;
        }
        public int VerificarCambios()
        {
            Console.WriteLine("Primero paso");
            return dbservice.VerificarBanner();
        }
        
    }
    
}
