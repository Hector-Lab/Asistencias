using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asistencia.Clases
{
    public class Banner
    {
        public int id { get; set; }
        public int Tipo { get; set; } //1 = Imagen 2 = Texto
        public String idRepositorio { get; set; }
        public String FechaLimite { get; set; }
        public String FechaAlta { get; set; }
        public String General { get; set; }
        public String idChecador { get; set; }
        public String Recurso { get; set; }
        public void toString()
        {
            Console.WriteLine(id + " - " + Tipo + " - " + idRepositorio + " - " + FechaLimite + " - " + FechaAlta + " - " + General + " - " + idChecador + " - " + Recurso);
        }
    }
}
