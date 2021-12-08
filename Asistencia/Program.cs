using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Asistencia
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            String current = Directory.GetCurrentDirectory(); // Se obtiene la direccion del proyecto y se le agrega el nombre del archivo
            current = current + "/data.db";
            if (!File.Exists(current))
            {
                //las configuraciones no existen
                Application.Run(new ConfigForm());
            }
            else
            {
                Controller control = new Controller();
                MainScreen mainScreen = new MainScreen();
                mainScreen.setController(control);
                Application.Run(mainScreen);
            }
        }
    }
}
