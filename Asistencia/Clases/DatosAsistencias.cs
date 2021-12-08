using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asistencia.Clases
{
    public class DatosAsistencias
    {
        public int id { get; set; }
        public String Fecha { get; set; }
        public String FechaTupla { get; set; }
        public int Usuario { get; set; }
        public int idGrupoPersona { get; set; }
        public int idEmpleado { get; set; }
        public int MultipleHorario { get; set; }
    }
}
