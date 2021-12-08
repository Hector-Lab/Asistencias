using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asistencia.Clases
{
    public class AsistenciaPantalla
    {

        //Datos Para mostrar en la pantalla
        public String HoraEntrada { get; set; }
        public String HoraSalida { get; set; }
        public String horaAsistencia { get; set; }
        public String EstatusPantalla { get; set; }
        public int EstatusAsistencia { get; set; }
        public int Estatus { get; set; }
        public int idGrupoPersona { get; set; }
        //Datos para hacer el bloqueo al insertar los horarios
        public String HoraEntradaSistema { get; set; }
        public String HoraSalidaSistema { get; set; }
    }
}
