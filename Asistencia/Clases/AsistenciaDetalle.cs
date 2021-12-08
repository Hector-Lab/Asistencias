using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asistencia.Clases
{
    public class AsistenciaDetalle
    {
        public int id { get; set; }
        public String HoraEntrada { get; set; }
        public String HoraSalida { get; set; }
        public int EstatusAsistencia { get; set; }
        public String FechaTupla { get; set; }
        public int Tipo { get; set; }
        public int idAsistencia { get; set; }
        public String HoraAsistencia { get; set; }
    }
}
