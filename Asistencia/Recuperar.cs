using Asistencia.Clases;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asistencia
{
    public partial class Recuperar : Form
    {
        int cliente;
        String infoCliente = "";
        Asistencia.Controller control;
        Checador checador;
        String nombreCliente = "";
        public Recuperar()
        {
            InitializeComponent();
        }

        private async void btnCheck_Click(object sender, EventArgs e)
        {
            //Hacemos el llamado al API
            String nombre = txtName.Text;
            String Pass = txttPass.Text;
            if (!(String.IsNullOrEmpty(nombre) && String.IsNullOrEmpty(Pass)))
            {
                checador = await control.RecuperarDatosChecador(txtName.Text, txttPass.Text, Convert.ToString(cliente));
                if (checador.id == -1) //Datos no encontrados
                {
                    lblChecador.Text = "Checador no encontrado\nRevise la informacion del checador e intente otra vez";
                    lblLogo.Image = null;
                    btnGuardar.Enabled = false;
                }
                else
                {
                    //Obtenemos el logo del cliente
                    String imgDir = await control.obtenerLogoCliente(Convert.ToString(cliente));
                    if (!imgDir.Equals("null"))
                    {
                        Bitmap logo = null;
                        byte[] byteBuffer = Convert.FromBase64String(imgDir);
                        MemoryStream memoryStream = new MemoryStream(byteBuffer);
                        memoryStream.Position = 0;
                        logo = (Bitmap)Bitmap.FromStream(memoryStream);
                        lblLogo.Image = logo;
                        lblChecador.Text = "Resumen de configuracion :\n" + infoCliente + "\nSector: " + checador.NombreSector + "\nChecador: " + checador.Nombre;
                    }
                    else
                    {
                        lblChecador.Text = "Resumen de configuracion :\n" + infoCliente + "\nSector: " + checador.NombreSector + "\nChecador: " + checador.Nombre + "\nLogotipo no encontrado";
                    }
                    btnGuardar.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Asegurese de llenar los campos correctamente");
                btnGuardar.Enabled = false;
            }
        }

        private void Recuperar_Load(object sender, EventArgs e)
        {

        }

        public void setClientes(List<Cliente> listaClientes)
        {
            foreach (Cliente item in listaClientes)
            {
                comboCliente.Items.Add(item);
                comboCliente.DisplayMember = "Descripcion";
                comboCliente.ValueMember = "id";
            }
        }

        private void comboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cliente SelectedCliente = (Cliente)comboCliente.SelectedItem;
            cliente = SelectedCliente.id;
            nombreCliente = SelectedCliente.Descripcion;
        }
        public void setController(Asistencia.Controller cnt)
        {
            control = cnt;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Creamos la base de datos
            //int created = control.Comprobando();
            //if(!(created == -1))
            //{
            
            control.LimpiarBaseDatos();
            if(checador.AplicaSector == null)
            {
                checador.AplicaSector = "0";
            }
            checador.Sector = (checador.Sector == null ? "0" : "1");
            
            // se cre la base de datos
            control.InsertarInformacionChecador(checador, nombreCliente);
            this.DialogResult = DialogResult.OK;
            /*}
            else
            {
                MessageBox.Show("Error al crear la base de datos", "Erro de escritura", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Al precionar el boton se guardan los datos en la base local*/
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            control.descargarRecursos();
        }
    }
}
