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
    public partial class LogIn : Form
    {
        Controller control;
        public LogIn()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            String Password = campoPass.Text;
            String Nombre = campoNombre.Text;
            if (Password != "" && Nombre != "")
            {
                String valid = await control.VerificarChecador(Nombre, Password);
                if (valid == "1")
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(valid);
                }
            }
        }
        public void SetController(Controller cnt)
        {
            control = cnt;
        }
        private void LogIn_Load(object sender, EventArgs e)
        {
            campoNombre.Text = control.StorageNombre();
        }
    }
}
