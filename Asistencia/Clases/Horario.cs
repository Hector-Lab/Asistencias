using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asistencia.Clases
{
    public class Horario
    {
        public String Grupo { get; set; }
        public String GrupoDetalle { get; set; }
        public String PuestoEmpleado { get; set; }
        public String GrupoNombre { get; set; }
        public String Jornada { get; set; }
        public int Dia { get; set; }
        public String HoraEntrada { get; set; }
        public String HoraSalida { get; set; }
        public int Tolerancia { get; set; }
        public int Retardo { get; set; }
        public String Estatus { get; set; }
        public int LimiteFaltas { get; set; }
        public int LimiteRetardos { get; set; }
    }
}
