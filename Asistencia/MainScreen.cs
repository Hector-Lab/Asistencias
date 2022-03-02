﻿using Asistencia.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Speech.Synthesis;

#pragma warning disable CS0246 // El nombre del tipo o del espacio de nombres 'USB' no se encontró (¿falta una directiva using o una referencia de ensamblado?)
#pragma warning restore CS0246 // El nombre del tipo o del espacio de nombres 'USB' no se encontró (¿falta una directiva using o una referencia de ensamblado?)
namespace Asistencia
{
    public partial class MainScreen : Form
    {
        string vozMensaje = "";
        string urlFoto = "";
        Image noEncontrado;
        bool bannerTick = false;
        string NFCUID = "";
        int relayChanels = -1;
        int currentDeviceIndex = -1;
        int selectedChanel = 2; //Canales del rele los que tengo son 1 y 2
        int relayDevices = -1;
        bool[] estadosTornoquete;
        //Los colores se pueden obtener de la base de datos y convertirlos aqui a rgb
        Color asistencia = Color.FromArgb(30, 187, 80, 1);
        Color retardo = Color.FromArgb(225, 218, 35);
        Color falta = Color.FromArgb(187, 44, 30);
        Color festivoCubrePermiso = Color.FromArgb(35, 109, 224);
        Color sinSueldo = Color.FromArgb(255, 87, 51);
        Color automatico = Color.FromArgb(107, 167, 107);
        bool ImagenOcupada = false;
        bool actualizacionCompleta = true;
        byte opacity = 255;
        List<RecursoBanner> ListaNombresImg;
        Controller control;
        bool change = false;
        List<String> meses;
        int index = 0;
        int red = 0;
        int blue = 0;
        int green = 0;
        bool textoFijo = false;
        bool textoCambio = false;
        bool bannerActualizado = false;
        bool actualizarEmpleados = false;
        bool enviandoDatos = false;
        String[] ArregloNFC = new String[] { "966bce7c", "9670e5ac", "fda9fcfc", "d392c20c", "0f1b7cae", "961ff1ac", "0ddb9a6c", "967245fc", "fc8bc17c", "96a23f9c", "9670396c", "d94da1dc", "fd6c169c", "0f3b57ce", "0ba07c39", "efe34e0c", "0eb976ce", "fd1e2a0c", "966137bc", "fd56b1dc", "620acbec", "0f2ce9ce", "fc6e543c", "fd6a236c", "fa16d6fc", "fd43830c", "0f3ac13e", "0ef4a25e", "0f2cbb2e", "962d747c", "9671f25c", "da0b55ec", "fa2b79fc", "a3ecae1c", "da0beeec", "fa3d682c", "efdaef4c", "d9f2918c", "d94d449c", "fce819cc", "0f1aa18e", "fa12b84c", "1f107de2", "0ec4be4e", "d9f5fa5c", "0ec3c9ce", "d94ce36c", "fd44218c", "d94bf75c", "fc9b118c", "fc8f87ec", "969111ec", "9629e41c", "9635d68c", "d97ac13c", " 9620285c", "9686b83c", "9623638c", "9699d6cc", "fd8898ac", " 0dd5101c", "9616dfec", "0e94ef7e", "fdd2180c", "fcf6058c", "fca7b05c", " fc9e510c", " d94d213c", "968232dc", " 0f39442e", "968dd8dc", "0dd9f54c", "18e83273", "9663b97c", "961e7d5c", "0bccc049", "0f19f9fe", "fd6d4ecc", "fa143f5c", "0ef2053e", " fd4a3d3c", "efda6ebc", " 924a31f4", "9636713c", "fd6ee52c", "d97bfc7c", "fd35663c", "0e80debe", " fce8008c", "fd79c78c", "fd32056c", "fdaef34c", "96a00cfc", " 0edbf21e", "fc82736c", "9635d37c", "fd3a260c", "9661952c", "fda29a3c", "d94283ac", "962c5eac", " 712a5b8c", "fa2ade9c", "1f9c3de2", "ef8ae1bc", "96768aac", "fa03621c", "d97d8d8c", "d950ffcc", "efb5284c", "fa059c3c", "fc84fb6c", "967af45c", "0eb95d3e", "9634d32c", "968c813c", "fced1fac", "0ed20b0e", " 0ee02e3e", "fd02549c", "fcf1ae8c", "fc7ab44c", "968f8bbc", "963151ac", " fd682a4c", "d94c925c", " fa12fa8c", "fd554a0c", "fd4a9d8c", "0eefd07e", "fd1a218c", "0eb93a2e", "fd50a9ac", "df45e99c", "fc89653c", "9622c38c", " 03f687e9", " d97aa36c", " fdaca2ec", "fc9b152c", " 0eb9f80e", "fd7cab7c", "fa32571c", "fd01ab9c", "fd71d66c", "0ee7e2ee", "fcc54ecc", "fd4284fc", "96a43b1c", " 03ad6e29", "fa27511c", "fa043e4c", "fcccfefc", "fdca7e7c", "fd54d0ec", "0ea1ae3e", "fdcd9ccc", "fd823a5c", "0ea7204e", "fd93063c", "d2a0024c", "0eac2d3e", "fa33f86c", "96369b9c", "0eb9411e", " 0f1ad49e", "fd6e403c", "fd31157c", "fd8c856c", "fd44215c", " fd7dff5c", "fd220f3c", "d979ebac", " fcec829c", "fdc8cb4c", "efe1739c", "efe3469c", " 1f4d34a2", "fcadaf7c", "f9fe86cc", "a4fa990c", "fd69179c", "1f6252b2", "963085ec", "18ee88c3", " 96347f2c", "961f295c", "fa168f1c", "fdbb238c", "de83a27c", "9662ff7c", "963244fc", "fd54752c", " 18ef8663", "96a01bdc", " 1e8dac11", " fa24cc5c", "fdab85ac", "d38c528c" };
        int contadorNFC = 0;
        Bitmap currentImage;
        Dictionary<String, String> listaIconos;
        NFCReader nfc;
        SpeechSynthesizer sintetizer = new SpeechSynthesizer();
        bool NFCConnect = false;
        public MainScreen()
        {
            nfc = new NFCReader();
           sintetizer.SetOutputToDefaultAudioDevice();
            //RelayManager.Init();
            meses = new List<string>();
            meses.Add("Enero");
            meses.Add("Febrero");
            meses.Add("Marzo");
            meses.Add("Abril");
            meses.Add("Mayo");
            meses.Add("Junio");
            meses.Add("Julio");
            meses.Add("Agosto");
            meses.Add("Septiembre");
            meses.Add("Octubre");
            meses.Add("Noviembe");
            meses.Add("Dicembre");
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;
            int heigth = Screen.PrimaryScreen.Bounds.Height;
            int width = Screen.PrimaryScreen.Bounds.Width;
            this.MinimumSize = new Size(width, heigth);
            int middliHeigth = heigth / 2;
            int middliWidth = width / 2;
            //Calculamos la posicion de los relojes y del mensaje
            lblMensaje.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, lblMensaje.Size.Height);
            int middleCenterW = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (lblBackReloj.Size.Width / 2);
            int middleCneterH = (Screen.PrimaryScreen.WorkingArea.Height / 2) - (lblBackReloj.Size.Height / 2);
            lblBackReloj.Size = new Size(middleCenterW, middleCneterH);
            //intentando calcular le diseño de la pantalla
            //Primer groupbox
            groupBox1.Location = new Point(10, 10);
            groupBox1.Size = new Size(middliWidth, heigth + 100);
            //Div1.Location = new Point(0, 0);
            //Div1.Size = new Size(middliWidth, heigth + 99);
            //Segundo groupbox
            groupBox2.Location = new Point(middliWidth + 10, 0);
            groupBox2.Size = new Size(middliWidth, heigth - 15);
            tableLayoutPanel1.Size = new Size(groupBox2.Size.Width - 25, groupBox2.Size.Height);
            tableLayoutPanel1.BackColor = Color.Transparent;
            groupBox2.BackColor = Color.Transparent;
            lblEmpresa.MinimumSize = new Size(middliWidth, lblEmpresa.Size.Height);
            lblEmpresa.Location = new Point(0, heigth - 85);
            lblMensaje.Visible = false;
            //groupBox2.BackColor = asistencia;
            BlockInput(true);
            //Console.WriteLine("WidthB1: " + groupBox1.Size.Width + " - HeigthB1: " + groupBox1.Size.Height);
            //Console.WriteLine("WidthB2: "+groupBox2.Size.Width + " - HeigthB2: " + groupBox2.Size.Height);
            ListaNombresImg = new List<RecursoBanner>();

            //Descargamos los iconos del sistema
            //Verificamos que hayan reles conectados al dispositivo
            /*if (RelayManager.DevicesCount() > 0)
            {
                Console.WriteLine("Numero de Reles" + RelayManager.DevicesCount().ToString());
                Console.WriteLine("Abriendo los dispostivo");
                RelayManager.OpenDevice(0);
                relayChanels = RelayManager.ChannelsCount();
                currentDeviceIndex = RelayManager.CurrentDeviceIndex();
                Console.WriteLine("Numero de Canales: " + relayChanels + " :::: Dispositivo Actual: " + currentDeviceIndex);
                estadosTornoquete = new bool[relayChanels];
            }
            else
            {
                Console.Write("No hay reles conectos");
            }*/
            noEncontrado = Image.FromFile(Directory.GetCurrentDirectory() + "/Resources/notfound.png");
        }
        //Metodo para bloquear el teclado
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool BlockInput(bool fBlockIt);
        //Fin del metodo para bloquear el teclado
        private void button1_Click(object sender, EventArgs e)
        {
            //control.ObtenerEmpleados();
            //control.datosTest();
            //lbl.Text = "Esto es una prueba";
            //control.VerificarAsistencia();
            //List<Horario> horarios = new List<Horario>();
            //control.VerificarAsistencia(horarios);
            //Prueba de descargar archivos
            //control.VerificarAsistencia();
        }
        private async void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Aqui ejecutamos el codigo del proceso en segundo plano
            BackgroundWorker worker = sender as BackgroundWorker;
            //Console.WriteLine(ImagenOcupada + "Imagen Ocupada  ?");
            //Verificamos si hay cambios en el banner
            if (actualizarEmpleados)
            {
                control.ObtenerEmpleados();
            }
            control.enviarAsistenciasGuardadas();
            if (!actualizarEmpleados)
            {
                actualizacionCompleta = false;
                String tareas = await control.obtenerBitacora();
                actualizacionCompleta = true;
                if (tareas.Contains("3"))
                {
                    bannerActualizado = true;
                }
            }
            actualizarEmpleados = false;
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Se lanza al terminar la el proceso en segundo plano
            Console.WriteLine("Proceso de actualizacion completo");
            if (e.Cancelled)
            {
                Console.WriteLine("La actualizacion se cancelo");
            }
            else if (e.Error != null)
            {
                Console.WriteLine("Error al actualizar " + e.Error.Message);
            }
            else
            {
                Console.WriteLine("Actualizacion Completa");
            }

        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //Aqui se muesta el proceso de actualizacion
            Console.WriteLine(e.ProgressPercentage.ToString() + "%");
        }
        private void tActualizar_Tick(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                Console.WriteLine("Iniciando Actualizacion");
                backgroundWorker1.RunWorkerAsync();
            }
            //Verificamos la valides del banner

            if (control.VerificarCambios() != ListaNombresImg.Count)
            {
                //Limpiamos los datos del arreglo
                ListaNombresImg.Clear();
                resetBanner();
            }
        }
        private void Reloj_Tick(object sender, EventArgs e)
        {
            //Falta el block input
            if (bannerActualizado)
            {
                resetBanner();
            }
            lblReloj.Text = FormatoFecha();
            lblBackReloj.Text = DateTime.Now.ToString("hh:mm:ss");
            //Verificando el nfc_uid
            if (NFCUID != "")
            {
                //Volteamos el NFC
                calcularAsistenciasV2(NFCUID);
            }
            if (!NFCConnect)
            {
                //nfc = null;
                //nfc = new NFCReader(); 
                //ReconectNFC();
            }
        }
        private void MainScreen_Load(object sender, EventArgs e)
        {
            ReconectNFC();
            Render.Enabled = true;
            var fechaActual = DateTime.Now;
            var Actual = fechaActual.Date.ToString("dd-MM-yyyy");
            var siguiten = Convert.ToDateTime(Actual).AddDays(1).ToString();
            lblEmpresa.Text = control.ObtenerNombreCliente();
            configurarBanner();
            listaIconos = control.CargarIconosDB();
            actualizarEmpleados = (control.verificarEmpleados() == 0);
            if (actualizarEmpleados)
            {
                backgroundWorker1.RunWorkerAsync();
            }
            Console.WriteLine(listaIconos.Count + " - Iconos");
            if (listaIconos.Count <= 0)
            {
                control.descargarIconos();
                listaIconos = control.CargarIconosDB();
            }

        }
        public async void configurarBanner()
        {
            elementosVisibles(false);
            //Obtenemos la lista del banner en la base de datos local
            ListaNombresImg = control.ObtenerBannerBaseLocal();
            if (ListaNombresImg.Count == 0)
            {
                Console.WriteLine("Banner desde la base la API");
                //Obtenemos la informaciondn del banner desde el API
                ListaNombresImg = await control.ObtenerBannerChecador();
                Console.WriteLine("Banner desde la base la API");
                if (ListaNombresImg.Count > 0)
                {
                    cambiarImagenBanner(); //Si hay un banner
                    Render.Enabled = true;
                }
                else
                {
                    //si no hay un banner se muestra solo el reloj
                    Render.Enabled = false;
                    RelojTick();
                }
            }
            else
            {
                Console.WriteLine("Banner desde la base de datos local");
                Render.Enabled = true;
            }
            //ontrol.ObtenerEmpleados();
        }
        private void Render_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tick - mi index: " + index);

            if (textoFijo)
            {
                Console.WriteLine("Tick del proceso de texto");
                textoFijo = false;
                lblMensaje.Visible = false;
                //lblMensaje.Enabled = true;
                lblMensaje.BackColor = Color.Black;
                lblMensaje.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height + 200);
                RenderTexto.Enabled = false;
            }
            if (bannerTick)
            {
                if (actualizacionCompleta)
                {
                    if (ListaNombresImg.Count > 0)
                    {
                        if (index >= ListaNombresImg.Count)
                        {
                            index = 0;
                        }
                        RecursoBanner imgBanner = ListaNombresImg.ElementAt<RecursoBanner>(index < 0 ? 0 : index);
                        String Nombre = imgBanner.nombre;
                        int tipo = imgBanner.tipo;
                        if (tipo == 1) // Es una Imagen
                        {
                            //Empieza el proceso de cambio de imagen
                            lblBackReloj.Visible = true;
                            procesoImagenV2(imgBanner.img);
                        }
                        else if (tipo == 2)//Es Texto
                        {
                            //Empieza el proceso de cambio a texto
                            lblBackReloj.Visible = false;
                            RenderTexto.Enabled = true;
                            lblMensaje.Visible = true;
                            //Calculamos la altura del texto
                            lblMensaje.BackColor = Color.Black;
                            int tamaño = (Nombre.Length / 70) + 2;
                            lblMensaje.MinimumSize = new Size(lblMensaje.Width, tamaño * 70);
                            procesoTextov2(Nombre);
                        }
                    }
                    else
                    {
                        RelojTick();
                    }
                }
                else
                {
                    RelojTick();
                }
                if (index >= ListaNombresImg.Count)
                {
                    index = 0;
                }
                else
                {
                    index++;
                }
            }
            else
            {
                RelojTick();
            }
            bannerTick = !bannerTick;
        }
        private void bannerStart_Tick(object sender, EventArgs e)
        {
            Render.Enabled = true;
            elementosVisibles(false);
            bannerStart.Enabled = false;
        }
        private String FormatoFecha()
        {
            String mesRsult = "";
            int mesActual = DateTime.Now.Month;
            int indexMeses = 1;
            foreach (String mes in meses)
            {
                if (mesActual == indexMeses)
                {
                    mesRsult = mes;
                    break;
                }
                indexMeses++;
            }
            String fecha = DateTime.Now.Day + " de " + mesRsult + " del año " + DateTime.Now.Year + " " + DateTime.Now.ToString("hh:mm:ss");
            return fecha;
        }
        private void limpiarPantalla()
        {

            txtNombre.ResetText();
            txtArea.ResetText();
            txtCargo.ResetText();
            txtNoEmpleado.ResetText();
            lblHorario.ResetText();
            lblTipo.ResetText();
            lblRegistro.ResetText();
            EmpleadoFoto.Image = noEncontrado;
            EmpleadoFoto.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        private void cambiarImagenBanner()
        {

            //this.WindowState = FormWindowState.Maximized;
            //Obtenemos el objeto banner
            if (index >= 0 && index < ListaNombresImg.Count)
            {
                RecursoBanner recurso = ListaNombresImg.ElementAt<RecursoBanner>(index);
                if (recurso.tipo == 1)
                {
                    textoCambio = false;
                }
                else if (recurso.tipo == 2)
                {
                    textoCambio = true;
                    this.BackgroundImage = null;
                    //Calculamos el tamaño del textbox
                    lblMensaje.BackColor = Color.Black;
                    int tamaño = (recurso.nombre.Length / 70) + 2;
                    lblMensaje.MinimumSize = new Size(lblMensaje.Width, tamaño * 70);
                    lblBackReloj.Visible = false;
                }
            }
        }
        private Bitmap aplicarFiltro(Image img, byte alpah = 255)
        {
            Bitmap bmpNew = GetARGB(img);
            BitmapData bmpData = bmpNew.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            IntPtr ptr = bmpData.Scan0;
            byte[] bytebuffer = new byte[bmpData.Stride * bmpNew.Height];
            Marshal.Copy(ptr, bytebuffer, 0, bytebuffer.Length);
            for (int k = 3; k < bytebuffer.Length; k += 4)
            {
                bytebuffer[k] = alpah;
            }
            Marshal.Copy(bytebuffer, 0, ptr, bytebuffer.Length);
            bmpNew.UnlockBits(bmpData);
            bytebuffer = null;
            return bmpNew;
        }
        private void torniquete_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("El rele se apaga y se desabilita el timer");
            //Solo va a cerrar los reles
            //RelayManager.Close(currentDeviceIndex,selectedChanel);
            //RelayManager.Close(currentDeviceIndex, 2);
            estadosTornoquete[currentDeviceIndex] = false;
            torniquete.Enabled = false;
        }
        private void Enviar_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Enviando asistencias");
            control.enviarAsistenciasGuardadas();
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private Bitmap GetARGB(Image img)
        {
            Bitmap copy = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(copy))
            {
                graphics.DrawImage(img, new Rectangle(0, 0, copy.Width, copy.Height), new Rectangle(0, 0, copy.Width, copy.Height), GraphicsUnit.Pixel);
                graphics.Flush();
            }
            return copy;
        }
        private void RelojView_Tick(object sender, EventArgs e)
        {
            Reloj.Enabled = true;
            Render.Enabled = true;
        }
        public void elementosVisibles(bool mostrar)
        {
            if (!mostrar)
            {
                BackColor = Color.Black;
            }
            else
            {
                BackColor = Color.WhiteSmoke;
            }
            groupBox1.Visible = mostrar;
            groupBox2.Visible = mostrar;
            lblMensaje.Visible = false;
            lblMensaje.Visible = !mostrar;

        }
        public void setController(Controller cnt)
        {
            control = cnt;
        }
        public void procesoTextov2(String Mensaje)
        {
            //Asignamos el color del fondo al label 
            lblMensaje.ForeColor = Color.Green;
            lblMensaje.Text = Mensaje;
            this.BackgroundImage = null;
            //Verificamos el margen maximo del label
            int margenTexto = lblMensaje.Height * -1;
            if (lblMensaje.Location.Y > margenTexto && !textoFijo) // si aun no llega al limite de la pantalla y la vandera del texto fijo es false
            {
                //lo desplazamos verticalmente
                Point newLocation = new Point();
                newLocation.Y = lblMensaje.Location.Y - 5;
                newLocation.X = lblMensaje.Location.X;
                lblMensaje.Location = newLocation;
            }
            else
            {
                if (!textoFijo)
                {
                    // se centra el label en la pantalla
                    red = lblMensaje.ForeColor.R;
                    blue = lblMensaje.ForeColor.B;
                    green = lblMensaje.ForeColor.G;
                    int altura = (Mensaje.Length / 70) >= 1 ? (Mensaje.Length / 70) : (Mensaje.Length / 70) + 1;
                    Size minimunSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, (altura) * 70);
                    lblMensaje.MinimumSize = minimunSize;
                    lblMensaje.Size = minimunSize;
                    //Calculamos el centro de la pantalla con respeto al label
                    int cordx = (Screen.PrimaryScreen.WorkingArea.Width / 2) - (lblMensaje.Width / 2);
                    int cordy = (Screen.PrimaryScreen.WorkingArea.Height / 2) - (lblMensaje.Height / 2);
                    lblMensaje.Location = new Point(cordx, cordy);
                    textoFijo = true;
                }
                else
                {
                    RenderTexto.Enabled = false;
                }

            }
        }
        public void resetBanner()
        {
            Console.WriteLine("Reseteando banner");
            //El metod se activa despues de actualizar los datos del banner del checador
            //Obtenemor los datos del banner de la base de datos
            //Verificamos halla datos en el banner
            index = 0;
            //Verificamos si el primer recurso es texto para dar formato
            lblMensaje.Visible = false;
            lblMensaje.ForeColor = Color.Green;
            lblMensaje.Location = new Point(0, Screen.PrimaryScreen.WorkingArea.Height + 50);
            red = lblMensaje.ForeColor.R;
            blue = lblMensaje.ForeColor.B;
            green = lblMensaje.ForeColor.G;
            opacity = 0;
            textoFijo = false;
            change = true;
            this.BackgroundImage = null;
            ListaNombresImg.Clear();
            ListaNombresImg = control.ObtenerBannerBaseLocal();
            bannerActualizado = false;
            actualizacionCompleta = true;
            lblBackReloj.Visible = true;
        }
        public void procesoImagenV2(Bitmap Nombre)
        {
            //No se Aplica el filtro de las imagenes
            Console.WriteLine(index + " : Nombre de la imagen " + Nombre);
            //Asignamos el banner
            this.BackgroundImage = Nombre;
            lblBackReloj.Visible = false;
        }
        public void NFCTrigger()
        {
            bool ConnectionActive = nfc.Connect();
            Console.WriteLine("Se detecto la tarjeta: " + ConnectionActive);
            
            if (ConnectionActive)
            {
                NFCUID = nfc.GetCardUID();
            }
        }
        public void NFCRemoved()
        {
            nfc.Disconnect();
        }
        private void calcularAsistenciasV2(string data)
        {
            if (!data.Equals("00000063"))// Este es el error de la tarjeta removida antes de terminar de leer el uid
            {
                Render.Enabled = false;
                limpiarPantalla();
                elementosVisibles(true);
                //Cambiamos el fondo del frame a null
                this.BackgroundImage = null;
                PresentacionAsistencia datosAsistecia = control.ObtenerHorarioEmpleado(data);
                if (datosAsistecia.empleado != null)
                {
                    urlFoto = datosAsistecia.empleado.Foto;
                    //Foto del empleado
                    if (!String.IsNullOrEmpty(urlFoto) && urlFoto.ToString() != "NULL")
                    {
                        try
                        {
                            EmpleadoFoto.ImageLocation = urlFoto;
                            if (!backgroundWorker2.IsBusy)
                            {
                                backgroundWorker2.RunWorkerAsync();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("No hay conexion a internet");
                            EmpleadoFoto.SizeMode = PictureBoxSizeMode.CenterImage;
                            LmpiarEmpleadoFoto();
                        }

                    }
                    else
                    {
                        EmpleadoFoto.Image = noEncontrado;
                    }
                    txtNombre.Text = datosAsistecia.empleado.Nombre;
                    Console.WriteLine(datosAsistecia.empleado.Nombre);
                    txtArea.Text = datosAsistecia.empleado.AreaAdministrativa;
                    txtCargo.Text = datosAsistecia.empleado.Cargo;
                    txtNoEmpleado.Text = datosAsistecia.empleado.noEmpleado;
                    if (datosAsistecia.asistenciaDetalle != null)
                    {
                        lblHorario.Text = "Entrada:  " + datosAsistecia.asistenciaDetalle.HoraEntrada + " - Salida: " + datosAsistecia.asistenciaDetalle.HoraSalida;
                        lblTipo.ForeColor = asistencia;
                        lblTipo.Text = datosAsistecia.asistenciaDetalle.EstatusPantalla;
                        lblRegistro.Text = "Hora de registro: " + datosAsistecia.asistenciaDetalle.horaAsistencia;
                        if (lblTipo.Equals("Salida (Checador Cerrado)") || lblTipo.Equals("Falta  (Checador Cerrado)"))
                        {
                            lblAsistencia.Image = Bitmap.FromFile(listaIconos["Success.png"]);
                        }
                        else
                        {
                            lblAsistencia.Image = Bitmap.FromFile(listaIconos["Close.png"]);
                        }
                        lblAsistencia.Text = "Asistencia registrada exitosamente";

                        if (!backgroundWorker3.IsBusy)
                        {
                            try
                            {
                                vozMensaje = "Asistencia Registrada";
                                backgroundWorker3.RunWorkerAsync();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }

                    }
                    else
                    {
                        var bitmap = Bitmap.FromFile(listaIconos["Conexion.png"]);
                        lblAsistencia.Image = bitmap;
                        lblAsistencia.Text = "El empleado  " + datosAsistecia.empleado.Nombre + " no tiene un horario asignado";
                    }
                    /*if(estadosTornoquete.Length > 0)
                    { 
                        //Se habre el rele
                        estadosTornoquete[currentDeviceIndex] = true;
                        RelayManager.Open(currentDeviceIndex, selectedChanel);
                        RelayManager.Open(currentDeviceIndex, 1);
                        torniquete.Enabled = true;
                    }
                    else
                    {
                        //No se habre el rele
                        Console.WriteLine("No tengo reles conectados");
                    }*/


                }
                else
                {
                    lblAsistencia.Image = Bitmap.FromFile(listaIconos["Error.png"]);
                    LmpiarEmpleadoFoto();
                    lblAsistencia.Text = "Empleado no encontrado";
                    if (!backgroundWorker3.IsBusy)
                    {
                        try
                        {
                            vozMensaje = "Empleado no encontrado";
                            backgroundWorker3.RunWorkerAsync();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                }
                /*lbl.Text = data;
                 lblEntrada.Text = datosAsistecia.asistenciaDetalle.HoraEntrada;
                 lblSalida.Text = datosAsistecia.asistenciaDetalle.HoraSalida;
                 lblEstatus.Text = datosAsistecia.asistenciaDetalle.EstatusPantalla;
                 lblHora.Text = datosAsistecia.asistenciaDetalle.horaAsistencia;*/
                NFC_stop.Enabled = true;
            }
            else
            {
                //Mensajes de Error
                lblTipo.Text = "Error al leer la tajeta";
                lblAsistencia.Image = Bitmap.FromFile(listaIconos["NFCError.png"]);
                if (!backgroundWorker3.IsBusy)
                {
                    try {
                        vozMensaje = "Mantenga la tarjeta sobre el lector por unos momentos!";
                        backgroundWorker3.RunWorkerAsync();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            NFCUID = "";
        }
        private void NFC_stop_Tick(object sender, EventArgs e)
        {
            Render.Enabled = true;
            NFC_stop.Enabled = false;
            elementosVisibles(false);
            EmpleadoFoto.Image = noEncontrado;
            if (ListaNombresImg.ElementAt<RecursoBanner>((index - 1) < 0 ? 0 : index - 1).tipo == 1)
            {
                if (!bannerTick)
                {
                    procesoImagenV2(ListaNombresImg.ElementAt<RecursoBanner>((index - 1) < 0 ? 0 : index - 1).img);
                }
                else
                {
                    RelojTick();
                }
            }
            else
            {
                if (!bannerTick)
                {
                    lblRegistro.Visible = true;
                }
            }
        }
        private void RenderTexto_Tick(object sender, EventArgs e)
        {
            // es por que se adelanta el index
            procesoTextov2(ListaNombresImg.ElementAt<RecursoBanner>(index - 1 < 0 ? 0 : index - 1).nombre);
        }
        private void NFCDisconected()
        {
            Console.WriteLine("Se desconecto el nfc");
            nfc.Disconnect();
            NFCConnect = false;
            BlockInput(false);
        }
        public void LmpiarEmpleadoFoto()
        {
            EmpleadoFoto.SizeMode = PictureBoxSizeMode.CenterImage;
            EmpleadoFoto.Image = noEncontrado;
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            EmpleadoFoto.Load(urlFoto);
        }
        private void RelojTick()
        {
            //Solo se va usar un timer para cambar el render
            RelojView.Enabled = true;
            Reloj.Enabled = true;
            this.BackgroundImage = null;
            lblBackReloj.Visible = true;
            Render.Enabled = true;
        }
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            sintetizer.Speak(vozMensaje);
        }
        private void MainScreen_MouseClick(object sender, MouseEventArgs e)
        {
            //NOTE: Probando voz en
            Console.WriteLine( "NFC: " + ArregloNFC[contadorNFC]);
            NFCUID = ArregloNFC[contadorNFC];
            contadorNFC++;
        }
        private async void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Iniciando Actualizion del banner");
            await control.ObtenerBannerChecador();
            bannerActualizado = true;
        }
        private void ReconectNFC()
        {
            try
            {   
                nfc.CardInserted += new NFCReader.CardEventHandler(NFCTrigger);
                nfc.CardEjected += new NFCReader.CardEventHandler(NFCRemoved);
                nfc.DeviceDisconnected += new NFCReader.CardEventHandler(NFCDisconected);
                nfc.Watch();
                NFCConnect = true;
            }
            catch (Exception error)
            {
                NFCConnect = false;
            }
        }
    }
}