using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asistencia.Clases
{
    internal class DatosAsistenciaRellenar
    {
        String Fecha { get; set; }
        String FechaTupla { get; set; }
        int Usuario { get; set; }
        int GrupoPersona { get; set; } 
        int idEmpleado { get; set; }
        int MultipleHorario { get; set; }
        List<AsistenciaDetalle> ListaAsistenciaDetalle { get; set; }
        
    }
}
