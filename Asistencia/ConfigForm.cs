using Asistencia.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asistencia
{
    public partial class ConfigForm : Form
    {
        readonly Controller controller;
        Recuperar login;
        MainScreen mainScreen;
        int cliente = -1;
        int sector = -1;
        String infoCliente;
        String infoSector;
        String Nombre;
        List<Cliente> ListaClientes;
        public ConfigForm()
        {
            controller = new Controller();
            InitializeComponent();
            /*log = new LogIn();
            log.SetController(controller);
            if(log.ShowDialog(this) == DialogResult.OK) // metodo para verificar si el formulario se cumplio
            {
                Console.WriteLine("Verificacion hecha");
            }*/
        }

        private async void ConfigForm_Load(object sender, EventArgs e)
        {
            List<Cliente> listaClientes = await controller.ListarClientes();
            ListaClientes = listaClientes;
            if (ListaClientes.Count > 0)
            {
                foreach (Cliente item in listaClientes)
                {
                    cmbCliente.Items.Add(item);
                    cmbCliente.DisplayMember = "Descripcion";
                    cmbCliente.ValueMember = "id";
                }
            }
            else
            {
                MessageBox.Show("Error de conexion", "Error al cargar los clientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            //cliente = int.Parse(cmbCliente.SelectedValue.ToString());
            Cliente SelectedCliente = (Cliente)cmbCliente.SelectedItem;
            cliente = SelectedCliente.id;
            infoCliente = "Cliente: " + SelectedCliente.id + " - " + SelectedCliente.Descripcion + "\n";
            lblResumen.Text = "Resumen de configuracion :\n" + infoCliente + "" + infoSector + " " + Nombre;
            CargarSectoresCliente();

        }
        public async void CargarSectoresCliente()
        {
            cmbSector.Text = "Cargando Sectores";
            List<Sector> ListaSectores = await controller.ListarSectores(cliente + "");
            foreach (Sector item in ListaSectores)
            {
                Console.WriteLine(item.Nombre + " - " + item.id);
                cmbSector.Items.Add(item);
                cmbSector.DisplayMember = "Nombre";
                cmbSector.ValueMember = "id";
            }
            cmbSector.Enabled = true;
        }

        private void cmbSector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sector sc = (Sector)cmbSector.SelectedItem;
            Console.WriteLine(sc.id + " - " + sc.Nombre);
            sector = sc.id;
            infoSector = "Sector: " + sc.Nombre + " - " + sc.Descripcion + "\n";

            lblResumen.Text = "Resumen de configuracion :\n" + infoCliente + "" + infoSector + " " + Nombre;
        }

        private void lblNombre_TextChanged(object sender, EventArgs e)
        {
            Nombre = "Nombre: " + lblNombre.Text + "\n";

            lblResumen.Text = "Resumen de configuracion :\n" + infoCliente + "" + infoSector + " " + Nombre;
        }
        public async void enviarDatos()
        {
            //String cl, String sc,, String pass,String Nombre
            String userPass = lblPass.Text;
            String confirm = lblConfirmPass.Text;
            String checadorName = lblNombre.Text;
            if (String.IsNullOrEmpty(userPass) | String.IsNullOrEmpty(confirm) | String.IsNullOrEmpty(checadorName))
            {
                MessageBox.Show("Campos vacios");
            }
            else if (userPass == confirm)
            {
                try
                {
                    //Creamos los archivos de la base de datos
                    // 0 = "Los archivos si existen 1 = Los archivos se crearon -1 = Error al crear el archivo
                    controller.LimpiarBaseDatos();
                    bool complete = await controller.EnviarConfiguracion(Convert.ToString(cliente), Convert.ToString(sector), userPass, checadorName, infoCliente);
                    if (complete)
                    {
                        //Pasar  a la pantalla pincipal    
                        this.Hide();
                        mainScreen = new MainScreen();
                        mainScreen.setController(controller);
                        mainScreen.Show();
                    }
                    else
                    {
                        MessageBox.Show("Error al registrar checador", "Error al registrar el checador\nAsegúrese de tener una conexion de internet");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden");
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            enviarDatos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // se la interfaz para la recuperacion de informacion del checador
            login = new Recuperar();
            login.setClientes(ListaClientes);
            login.setController(controller);
            this.Hide();
            login.ShowDialog();
            if (login.DialogResult == DialogResult.OK)
            {
                // se cierra el formulario de configuracion y se muestra la pantalla principal
                mainScreen = new MainScreen();
                mainScreen.setController(controller);
                mainScreen.Show();
            }
            else if (login.DialogResult == DialogResult.Cancel)
            {
                Console.WriteLine("Se cancelo el proceso de recuperacion de los datos");
                login.Dispose();
                this.Show();
            }
        }
    }
    public class Valor
    {
        public String Value { get; set; }
        public int Index { get; set; }

    }
}
