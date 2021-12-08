using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asistencia.Clases
{
    public class PresentacionAsistencia
    {
        public AsistenciaPantalla asistenciaDetalle { get; set; }
        public Empleado empleado { get; set; }

        public void printAsistenciaDetall()
        {
            Console.WriteLine("Hora Entrada: " + asistenciaDetalle.horaAsistencia +
                " Hora Salida: " + asistenciaDetalle.HoraSalida + " Hora Asistencia: " + asistenciaDetalle.horaAsistencia
                + "Tipo: " + asistenciaDetalle.EstatusPantalla + " Estatus: " + asistenciaDetalle.Estatus + " Grupo: " + asistenciaDetalle.idGrupoPersona);
        }
    }
}
