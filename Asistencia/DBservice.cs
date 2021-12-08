using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Asistencia.Clases;
using System.Drawing;
using System.IO;
using System.Net;
//API
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Drawing.Imaging;
namespace Asistencia
{
    public class DBservice
    {
        public DBservice()
        {
        }
        public void CrearTablas()
        {
            //Creamos las tablas si no existen
            var db = new SqliteConnection($"Filename=data.db");
            SQLitePCL.Batteries.Init();
            db.Open();
            String query = "CREATE TABLE IF NOT EXISTS StorageValues (id INTEGER PRIMARY KEY AUTOINCREMENT, name NVARCHAR(2048) NOT NULL, value NVARCHAR(2048) NOT NULL)";
            SqliteCommand commad 
                = new SqliteCommand(query, db);
            commad.ExecuteReader();
            query = "CREATE TABLE IF NOT EXISTS Empleado (id INTEGER PRIMARY KEY AUTOINCREMENT,idEmpleado INTEGER, Nombre NVARCHAR(255) NOT NULL, Nfc_uid NVARCHAR(255), idChecador INTEGER, noEmpleado INTEGER, Cargo NVARCHAR(255), AreaAdministrativa NVARCHAR(255), NombrePlaza NVARCHAR(255), Trabajador NVARCHAR(255), Foto NVARCHAR(255))";
            SqliteCommand commad2 = new SqliteCommand(query, db);
            commad2.ExecuteReader();
            query = "CREATE TABLE IF NOT EXISTS Horario(id INTEGER PRIMARY KEY AUTOINCREMENT, Grupo INTEGER, GrupoDetalle INTEGER, PuestoEmpleado INTEGER, GrupoNombre NVARCHAR(255), Jornada INTEGER,Dia INTEGER, HoraEntrada NVARCHAR(255), HoraSalida NVARCHAR(255),Retardo INTEGER, Tolerancia INTEGER, Estatus INTEGER, idEmpleado INTEGER,LimiteFaltas INTEGER, LimiteRetardos INTEGER)";
            SqliteCommand commad3 = new SqliteCommand(query, db);
            commad3.ExecuteReader();
            query = "CREATE TABLE IF NOT EXISTS Asistencias (id INTEGER PRIMARY KEY AUTOINCREMENT, Fecha NVARCHAR(255), FechaTupla NVARCHAR(255), Usuario INTEGER, GrupoPersona INTEGER, idEmpleado INTEGER,MultipleHorario INTEGER)";
            SqliteCommand command4 = new SqliteCommand(query, db);
            command4.ExecuteReader();
            query = "CREATE TABLE IF NOT EXISTS AsistenciaDetalle (id INTEGER PRIMARY KEY AUTOINCREMENT, HoraEntrada NVARCHAR(255), HoraSalida NVARCHAR(255),EstatusAsistencia INTEGER, FechaTupla NVARCHAR(255), Tipo INTEGER, idAsistencia INTEGER)";
            SqliteCommand command5 = new SqliteCommand(query, db);
            command5.ExecuteReader();
            query = "CREATE TABLE IF NOT EXISTS Banner (id INTEGER PRIMARY KEY AUTOINCREMENT, Tipo INTEGER, idRepositorio INTEGER, FechaLimite NVARCHAR(255), FechaAlta NVARCHAR(255), General INTEGER, idChecador INTEGER, Recurso NVARCHAR(255), LocalName NVARCHAR(255))";
            SqliteCommand command6 = new SqliteCommand(query, db);
            command6.ExecuteReader();
            query = "CREATE TABLE IF NOT EXISTS HistorialAsistencia (id INTEGER PRIMARY KEY AUTOINCREMENT, HoraEntrada NVARCHAR(255), HoraSalida NVARCHAR(255), EstatusAsistencia INTEGER, FechaTupla NVARCHAR(255), Tipo INTEGER, idEmpleado INTEGER, MultipleHorario INTEGER, idGrupoPersona INTEGER)";
            SqliteCommand command7 = new SqliteCommand(query, db);
            command7.ExecuteReader();
            query = "CREATE TABLE IF NOT EXISTS Iconos (id INTEGER PRIMARY KEY AUTOINCREMENT, Ruta NVARCHAR(255), Nombre NVARCHAR(255))";
            SqliteCommand command8 = new SqliteCommand(query, db);
            command8.ExecuteReader();
            db.Close();
        }
        public void GuardarUID(String uid)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO StorageValues VALUES(null,@name,@value)";
            command.Parameters.AddWithValue("@name", "Storage:UID");
            command.Parameters.AddWithValue("@value", uid);
            command.ExecuteReader();
            db.Close();
        }
        public void GuardarID(String id)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO StorageValues VALUES(null,@name,@value)";
            command.Parameters.AddWithValue("@name", "Storage:idChecador");
            command.Parameters.AddWithValue("@value", id);
            command.ExecuteReader();
            db.Close();
        }
        public void GuardarNombre(String Nombre)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO StorageValues VALUES(null,@name,@value)";
            command.Parameters.AddWithValue("@name", "Storage:Nombre");
            command.Parameters.AddWithValue("@value", Nombre);
            command.ExecuteReader();
            db.Close();
        }
        public void GuardarSector(String Sector)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO StorageValues VALUES(null,@name,@value)";
            command.Parameters.AddWithValue("@name", "Storage:Sector");
            command.Parameters.AddWithValue("@value", Sector);
            command.ExecuteReader();
            db.Close();
        }
        public void GuardarCliente(String Cliente)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO StorageValues VALUES(null,@name,@value)";
            command.Parameters.AddWithValue("@name", "Storage:Cliente");
            command.Parameters.AddWithValue("@value", Cliente);
            command.ExecuteReader();
            db.Close();
        }
        public String ObtenerItem(String Nombre)
        {
            String value = "";
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT value FROM StorageValues where name = '" + Nombre + "'";
            SqliteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                value = result.GetString(0);
            }
            db.Close();
            return value;
        }
        public void GuardarEmpleado(Empleado empleado)
        {
            //Verificamos si es una insersion o un update
            //False es insertar a la tabla y true es par hacer update
            bool insert = VerificarInsercionEmpleado(empleado.idEmpleado);

            if (insert)
            {
                ActualizarDatosEmpleado(empleado);
            }
            else
            {
                InsertarEmpleado(empleado);
            }
        }
        public void GuardarHorario(Horario horario, String idEmpleado)
        {
            //Limpiamos el horario del empleado 
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO Horario VALUES(null,@Grupo,@GrupoDetalle,@PuestoEmpleado,@GrupoNombre,@Jornada,@Dia,@HoraEntrada,@HoraSalida,@Retardo,@Tolerancia,@Estatus,@idEmpleado,@LimiteFaltas,@LimiteRetardos)";
            command.Parameters.AddWithValue("@Grupo", horario.Grupo);
            command.Parameters.AddWithValue("@GrupoDetalle", horario.GrupoDetalle);
            command.Parameters.AddWithValue("@PuestoEmpleado", horario.PuestoEmpleado);
            command.Parameters.AddWithValue("@GrupoNombre", horario.GrupoNombre);
            command.Parameters.AddWithValue("@Jornada", horario.Jornada);
            command.Parameters.AddWithValue("@Dia", horario.Dia);
            command.Parameters.AddWithValue("@HoraEntrada", horario.HoraEntrada);
            command.Parameters.AddWithValue("@HoraSalida", horario.HoraSalida);
            command.Parameters.AddWithValue("@Retardo", horario.Retardo);
            command.Parameters.AddWithValue("@Tolerancia", horario.Tolerancia);
            command.Parameters.AddWithValue("@Estatus", horario.Estatus);
            command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
            command.Parameters.AddWithValue("@LimiteFaltas", horario.LimiteFaltas);
            command.Parameters.AddWithValue("@LimiteRetardos", horario.LimiteRetardos);
            command.ExecuteReader();
            db.Close();
        }
        public Empleado ObtenerEmpleado(String NFC)
        {
            Empleado empleado = null;
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM Empleado WHERE Nfc_uid = '" + NFC + "'";
            SqliteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                empleado = new Empleado();
                empleado.idEmpleado = result.GetString(1);
                empleado.Nombre = result.GetString(2);
                empleado.Nfc_uid = result.GetString(3);
                empleado.idChecador = result.GetString(4);
                empleado.noEmpleado = result.GetString(5);
                empleado.Cargo = result.GetString(6);
                empleado.AreaAdministrativa = result.GetString(7);
                empleado.NombrePlaza = result.GetString(8);
                empleado.Trabajador = result.GetString(9);
                empleado.Foto = result.GetString(10);
            }
            db.Close();
            return empleado;
        }
        public List<Horario> ObtenerHorario(String idEmpleado, String dia)
        {
            List<Horario> ListaHorario = new List<Horario>();
            Horario horario = null;
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM Horario WHERE idEmpleado = " + idEmpleado + " AND Dia = " + dia;
            Console.WriteLine("SELECT * FROM Horario WHERE idEmpleado = " + idEmpleado + " AND Dia = " + dia);
            SqliteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                horario = new Horario();
                horario.Grupo = result.GetString(1);
                horario.GrupoDetalle = result.GetString(2);
                horario.PuestoEmpleado = result.GetString(3);
                horario.GrupoNombre = result.GetString(4);
                horario.Jornada = result.GetString(5);
                horario.Dia = result.GetInt16(6);
                horario.HoraEntrada = result.GetString(7);
                horario.HoraSalida = result.GetString(8);

                horario.Retardo = result.GetInt16(9);
                horario.Tolerancia = result.GetInt16(10);
                horario.Estatus = result.GetString(11);
                horario.LimiteFaltas = result.GetInt32(13);
                horario.LimiteRetardos = result.GetInt32(14);
                ListaHorario.Add(horario);
            }
            db.Close();
            return ListaHorario;
        }
        public void DeleteData(String tableName)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "DELETE FROM " + tableName;
            command.ExecuteReader();
            db.Close();
        }
        public void ObtenerDatosEmpleadoTodos()
        {
            Console.WriteLine("Empesando la lectura de la base de datos");
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand commad = new SqliteCommand();
            commad.Connection = db;
            commad.CommandText = "SELECT * FROM Empleado;";
            var result = commad.ExecuteReader();
            db.Close();
        }
        //Estos son metodos de pruebas
        public void setEmpleado()
        {
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            //Insertamos el horario y el empleado 
            command.CommandText = "INSERT INTO Empleado VALUES(null,@idEmpleado,@Nombre,@Nfc_uid,@idChecador,@noEmpleado,@Cargo,@AreaAdministrativa,@NombrePlaza,@Trabajador,@Foto)";
            command.Parameters.AddWithValue("@idEmpleado", 1322);
            command.Parameters.AddWithValue("@Nombre", "Pedro Ramirez Ramirez");
            command.Parameters.AddWithValue("@Nfc_uid", "22479884");
            command.Parameters.AddWithValue("@idChecador", 9);
            command.Parameters.AddWithValue("@noEmpleado", 579);
            command.Parameters.AddWithValue("@Cargo", "Jefe de departamento");
            command.Parameters.AddWithValue("@AreaAdministrativa", "Administracion");
            command.Parameters.AddWithValue("@NombrePlaza", "Jefe de departamento");
            command.Parameters.AddWithValue("@Trabajador", 30);
            command.Parameters.AddWithValue("@Foto", "FOTO TEST");
            command.ExecuteReader();
            db.Close();
        }
        public void setHorarioEmpleado()
        {
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO Horario VALUES(null,@Grupo,@GrupoDetalle,@PuestoEmpleado,@GrupoNombre,@Jornada,@Dia,@HoraEntrada,@HoraSalida,@Retardo,@Tolerancia,@Estatus,@idEmpleado)";
            command.Parameters.AddWithValue("@Grupo", 1);
            command.Parameters.AddWithValue("@GrupoDetalle", 1);
            command.Parameters.AddWithValue("@PuestoEmpleado", 1280);
            command.Parameters.AddWithValue("@GrupoNombre", "Administradores");
            command.Parameters.AddWithValue("@Jornada", 1);
            command.Parameters.AddWithValue("@Dia", 3);
            command.Parameters.AddWithValue("@HoraEntrada", "9:00:00");
            command.Parameters.AddWithValue("@HoraSalida", "15:00:00");
            command.Parameters.AddWithValue("@Retardo", 15);
            command.Parameters.AddWithValue("@Tolerancia", 60);
            command.Parameters.AddWithValue("@Estatus", 1);
            command.Parameters.AddWithValue("@idEmpleado", 1322);
            command.ExecuteReader();
            db.Close();
        }
        //Fin de metodos de pruebas
        private bool VerificarInsercionEmpleado(String idEmpleado)
        {
            bool insert = false; //False es insertar a la tabla y true es par hacer update
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT idEmpleado FROM Empleado WHERE idEmpleado = @idEmpleado";
            command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
            SqliteDataReader result = command.ExecuteReader();
            while (result.Read())
            {
                insert = true;
            }
            db.Close();
            return insert;
        }
        public void LimpiarHorarioEmpleado(String idEmpleado)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "DELETE FROM Horario WHERE idEmpleado = @idEmpleado";
            command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
            command.ExecuteReader();
            db.Close();
        }
        private void InsertarEmpleado(Empleado empleado)
        {
            String nfc = "NULL";
            String idChecador = "NULL";
            String Foto = "NULL";
            if (!String.IsNullOrEmpty(empleado.Nfc_uid))
            {
                nfc = Convert.ToString(empleado.Nfc_uid);
            }
            if (!String.IsNullOrEmpty(empleado.idChecador))
            {
                idChecador = Convert.ToString(empleado.idChecador);
            }
            if (!String.IsNullOrEmpty(empleado.Foto))
            {
                Foto = Convert.ToString(empleado.Foto);
            }
            empleado.Nfc_uid = nfc;
            empleado.idChecador = idChecador;
            empleado.Foto = Foto;
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            //Insertamos el horario y el empleado 
            command.CommandText = "INSERT INTO Empleado VALUES(null,@idEmpleado,@Nombre,@Nfc_uid,@idChecador,@noEmpleado,@Cargo,@AreaAdministrativa,@NombrePlaza,@Trabajador,@Foto)";
            command.Parameters.AddWithValue("@idEmpleado", empleado.idEmpleado);
            command.Parameters.AddWithValue("@Nombre", empleado.Nombre);
            command.Parameters.AddWithValue("@Nfc_uid", nfc);
            command.Parameters.AddWithValue("@idChecador", idChecador);
            command.Parameters.AddWithValue("@noEmpleado", empleado.noEmpleado);
            command.Parameters.AddWithValue("@Cargo", empleado.Cargo);
            command.Parameters.AddWithValue("@AreaAdministrativa", empleado.AreaAdministrativa);
            command.Parameters.AddWithValue("@NombrePlaza", empleado.NombrePlaza);
            command.Parameters.AddWithValue("@Trabajador", empleado.Trabajador);
            command.Parameters.AddWithValue("@Foto", Foto);
            command.ExecuteReader();
            db.Close();
        }
        private void ActualizarDatosEmpleado(Empleado empleado)
        {
            String nfc = "NULL";
            String idChecador = "NULL";
            String Foto = "NULL";
            if (!String.IsNullOrEmpty(empleado.Nfc_uid))
            {
                nfc = Convert.ToString(empleado.Nfc_uid);
            }
            if (!String.IsNullOrEmpty(empleado.idChecador))
            {
                idChecador = Convert.ToString(empleado.idChecador);
            }
            if (!String.IsNullOrEmpty(empleado.Foto))
            {
                Foto = Convert.ToString(Convert.ToString(empleado.Foto));
            }
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "UPDATE Empleado SET Nombre = @Nombre, Nfc_uid = @Nfc_uid, idChecador = @idChecador, noEmpleado = @noEmpleado, Cargo = @Cargo, AreaAdministrativa = @AreaAdministrativa, NombrePlaza = @NombrePlaza, Trabajador = @Trabajador, Foto = @Foto WHERE idEmpleado = @idEmpleado";
            command.Parameters.AddWithValue("@idEmpleado", empleado.idEmpleado);
            command.Parameters.AddWithValue("@Nombre", empleado.Nombre);
            command.Parameters.AddWithValue("@Nfc_uid", nfc);
            command.Parameters.AddWithValue("@idChecador", idChecador);
            command.Parameters.AddWithValue("@noEmpleado", empleado.noEmpleado);
            command.Parameters.AddWithValue("@Cargo", empleado.Cargo);
            command.Parameters.AddWithValue("@AreaAdministrativa", empleado.AreaAdministrativa);
            command.Parameters.AddWithValue("@NombrePlaza", empleado.NombrePlaza);
            command.Parameters.AddWithValue("@Trabajador", empleado.Trabajador);
            command.Parameters.AddWithValue("@Foto", Foto);
            command.ExecuteReader();
            db.Close();
            Console.WriteLine("El empleado se actualizo");
        }
        public int RegistrarAsistencias(PresentacionAsistencia datosAsistencia, int multiple)
        {
            //Buscamos si hay alguna otra asistencia asignada
            bool found = false;
            bool nuevaInsercion = true;
            int idAsistencia = 0;
            int proceso = 0;
            string SalidaAnterior = "";
            List<DatosAsistencias> ListaAsistencia = ObtenerAsistenciasEmpleado(datosAsistencia.empleado.idEmpleado);
            if (ListaAsistencia.Count > 0)
            {
                //nuevaInsercion = false;

                // se busca en los detalles de las asistencias para hacer un update
                foreach (DatosAsistencias item in ListaAsistencia)
                {
                    idAsistencia = item.id;
                    List<AsistenciaDetalle> detallesAsistencia = obtenerAsistenciaDetalle(idAsistencia);
                    if (detallesAsistencia.Count > 0)
                    {
                        //buscamos si hay alguna coincidencia con algun otro registro
                        foreach (AsistenciaDetalle detalle in detallesAsistencia)
                        {
                            DateTime HoraEntradaDB = Convert.ToDateTime(detalle.HoraEntrada);
                            DateTime HoraSalidaDB = Convert.ToDateTime(detalle.HoraSalida);
                            DateTime HoraRegistro = Convert.ToDateTime(datosAsistencia.asistenciaDetalle.horaAsistencia);
                            DateTime HoraSalidaAnterior = new DateTime();
                            if (!String.IsNullOrEmpty(SalidaAnterior))
                            {
                                HoraSalidaAnterior = Convert.ToDateTime(SalidaAnterior);
                            }
                            if (HoraRegistro >= HoraEntradaDB && HoraRegistro <= HoraSalidaDB) // si es entrada
                            {
                                found = true;
                                proceso = -1;
                            }
                            else if (detallesAsistencia.Count == 1 && detalle.Tipo == 0) //Si solo tiene un horario y ya se registro la salida
                            {
                                found = true;
                                proceso = -1;
                            }
                            else if (String.IsNullOrEmpty(SalidaAnterior) && HoraRegistro >= HoraSalidaAnterior && HoraRegistro >= HoraSalidaDB)
                            {
                                found = true;
                                proceso = -1;
                            }
                        }
                    }
                }

            }
            if (nuevaInsercion && !found) // si no tiene Asistencias ni detalles
            {
                //Insertamos la asistenci
                InsertarAsistencia(datosAsistencia, multiple);
                proceso = 1;
            }
            else if (!nuevaInsercion && !found)
            {
                // Tiene mas de un horario de entrada en el dia agregar otro
                InsertarDetalleAsistencia(idAsistencia, datosAsistencia);
                proceso = 2;
            }
            return proceso;
        }
        public List<DatosAsistencias> ObtenerAsistenciasEmpleado(String idEmpleado)
        {
            String fecha = DateTime.Now.ToString("yyyy-MM-dd");
            List<DatosAsistencias> listaAsistencias = new List<DatosAsistencias>();
            DatosAsistencias asistencia;
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM Asistencias WHERE idEmpleado = @idEmpleado AND Fecha = @Fecha";
            command.Parameters.AddWithValue("@idEmpleado", idEmpleado);
            command.Parameters.AddWithValue("@Fecha", fecha);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                asistencia = new DatosAsistencias();
                asistencia.id = reader.GetInt32(0);
                asistencia.Fecha = reader.GetString(1);
                asistencia.FechaTupla = reader.GetString(2);
                asistencia.Usuario = reader.GetInt32(3);
                asistencia.idGrupoPersona = reader.GetInt32(4);
                asistencia.idEmpleado = reader.GetInt32(5);
                listaAsistencias.Add(asistencia);
            }
            db.Close();
            return listaAsistencias;
        }
        public void InsertarAsistencia(PresentacionAsistencia asistencia, int multiple)
        {
            String Fecha = DateTime.Now.ToString("yyyy-MM-dd");
            String FechaTupla = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int idAsistencia = -1;
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            //Fecha, FechaTupla, Usuario,GrupoPersona,idEmpleado
            command.CommandText = "INSERT INTO Asistencias VALUES(null,@Fecha,@FechaTupla,@Usuario,@GrupoPersona,@idEmpleado,@MultipleHorario)";
            command.Parameters.AddWithValue("@Fecha", Fecha);
            command.Parameters.AddWithValue("@FechaTupla", FechaTupla);
            command.Parameters.AddWithValue("@Usuario", 3800);
            command.Parameters.AddWithValue("@GrupoPersona", asistencia.asistenciaDetalle.idGrupoPersona);
            command.Parameters.AddWithValue("@idEmpleado", asistencia.empleado.idEmpleado);
            command.Parameters.AddWithValue("@MultipleHorario", multiple);
            command.ExecuteReader();
            command = null;
            command = new SqliteCommand();
            command.Connection = db;
            //Buscamos la insertsion
            command.CommandText = "SELECT MAX(id) FROM Asistencias";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                idAsistencia = reader.GetInt32(0);
            }
            command = null;
            command = new SqliteCommand();
            command.Connection = db;
            //Insertamos el detalle de la 
            command.CommandText = "INSERT INTO AsistenciaDetalle VALUES(null,@HoraEntrada,@HoraSalida,@EstatusAsistencia,@FechaTupla,@Tipo,@idAsistencia)";
            command.Parameters.AddWithValue("@HoraEntrada", asistencia.asistenciaDetalle.HoraEntradaSistema);
            command.Parameters.AddWithValue("@HoraSalida", asistencia.asistenciaDetalle.HoraSalidaSistema);
            command.Parameters.AddWithValue("@FechaTupla", FechaTupla);
            command.Parameters.AddWithValue("@Tipo", asistencia.asistenciaDetalle.Estatus);
            command.Parameters.AddWithValue("@idAsistencia", idAsistencia);
            command.Parameters.AddWithValue("@EstatusAsistencia", asistencia.asistenciaDetalle.EstatusAsistencia);
            command.ExecuteReader();
            db.Close();
        }
        public List<AsistenciaDetalle> obtenerAsistenciaDetalle(int idAsistencia)
        {
            List<AsistenciaDetalle> listaAsistenciaDetalles = new List<AsistenciaDetalle>();
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM AsistenciaDetalle WHERE idAsistencia = @idAsistencia";
            command.Parameters.AddWithValue("@idAsistencia", idAsistencia);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                AsistenciaDetalle detalle = new AsistenciaDetalle();
                detalle.id = reader.GetInt32(0);
                detalle.HoraEntrada = reader.GetString(1);
                detalle.HoraSalida = reader.GetString(2);
                detalle.FechaTupla = reader.GetString(3);
                detalle.Tipo = reader.GetInt32(4);
                detalle.idAsistencia = reader.GetInt32(5);
                listaAsistenciaDetalles.Add(detalle);
            }
            return listaAsistenciaDetalles;
        }
        public void InsertarDetalleAsistencia(int idAsistencia, PresentacionAsistencia detallesAsistencia)
        {
            String Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            SqliteConnection db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO AsistenciaDetalle VALUES(null,@HoraEntrada,@HoraSalida,@EstatusAsistencia,@FechaTupla,@Tipo,@idAsistencia)";
            command.Parameters.AddWithValue("@HoraEntrada", detallesAsistencia.asistenciaDetalle.HoraEntradaSistema);
            command.Parameters.AddWithValue("@HoraSalida", detallesAsistencia.asistenciaDetalle.HoraSalidaSistema);
            command.Parameters.AddWithValue("@FechaTupla", Fecha);
            command.Parameters.AddWithValue("@Tipo", detallesAsistencia.asistenciaDetalle.Estatus);
            command.Parameters.AddWithValue("@idAsistencia", idAsistencia);
            command.Parameters.AddWithValue("@EstatusAsistencia", detallesAsistencia.asistenciaDetalle.EstatusAsistencia);
            command.ExecuteReader();
            db.Close();
        }
        public List<AsistenciaDetalle> ObtenerDatosAsistenciaDetalles(int idAsistencia)
        {
            List<AsistenciaDetalle> listaDetalles = new List<AsistenciaDetalle>();
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM AsistenciaDetalle WHERE idAsistencia = @idAsistencia";
            command.Parameters.AddWithValue("@idAsistencia", idAsistencia);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                /*
                    public int id { get; set; }
                    public String HoraEntrada { get; set; }
                    public String HoraSalida { get; set; }
                    public int EstatusAsistencia { get; set; }
                    public String FechaTupla { get; set; }
                    public int Tipo { get; set; }
                    public int idAsistencia { get; set; }
                 */
                AsistenciaDetalle detalles = new AsistenciaDetalle();
                detalles.id = reader.GetInt32(0);
                detalles.HoraEntrada = reader.GetString(1);
                detalles.HoraSalida = reader.GetString(2);
                detalles.EstatusAsistencia = reader.GetInt32(3);
                detalles.FechaTupla = reader.GetString(4);
                detalles.Tipo = reader.GetInt32(5);
                detalles.idAsistencia = reader.GetInt32(6);
                detalles.HoraAsistencia = Convert.ToDateTime(detalles.FechaTupla).ToString("HH:mm:ss");
                listaDetalles.Add(detalles);
            }
            db.Close();
            return listaDetalles;
        }
        public List<DatosAsistencias> ObtenerDatosAsistencias()
        {
            List<DatosAsistencias> datosAsistencias = new List<DatosAsistencias>();
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM Asistencias";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                DatosAsistencias asistencias = new DatosAsistencias();
                asistencias.id = reader.GetInt32(0);
                asistencias.Fecha = reader.GetString(1);
                asistencias.FechaTupla = reader.GetString(2);
                asistencias.Usuario = reader.GetInt32(3);
                asistencias.idGrupoPersona = reader.GetInt32(4);
                asistencias.idEmpleado = reader.GetInt32(5);
                asistencias.MultipleHorario = reader.GetInt32(6);
                datosAsistencias.Add(asistencias);
                Console.WriteLine(reader.GetString(0));
            }
            db.Close();
            return datosAsistencias;
        }
        public bool insertarBanner(Banner banner, String name)
        {
            DateTime FechaActual = DateTime.Now;
            String FechaLimite = "";
            bool bannerValido = ValidarFechaLiminte(banner);
            if (String.IsNullOrEmpty(banner.idRepositorio))
            {
                banner.idRepositorio = "0";
            }
            if (String.IsNullOrEmpty(banner.FechaLimite))
            {
                banner.FechaLimite = "-1";
            }
            else
            {
                FechaLimite = banner.FechaLimite;
            }
            if (String.IsNullOrEmpty(banner.idChecador))
            {
                banner.idChecador = "0";
            }
            if (String.IsNullOrEmpty(banner.Recurso))
            {
                banner.Recurso = "-1";
            }
            if (bannerValido)
            {
                var db = new SqliteConnection($"Filename = data.db");
                db.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = db;
                command.CommandText = "INSERT INTO Banner VALUES(null,@Tipo,@idRepositorio,@FechaLimite,@FechaAlta,@General,@idChecador,@Recurso,@LocalName)";
                command.Parameters.AddWithValue("@Tipo", banner.Tipo);
                command.Parameters.AddWithValue("@idRepositorio", banner.idRepositorio);
                command.Parameters.AddWithValue("@FechaLimite", banner.FechaLimite);
                command.Parameters.AddWithValue("@FechaAlta", banner.FechaAlta);
                command.Parameters.AddWithValue("@General", banner.General);
                command.Parameters.AddWithValue("@idChecador", banner.idChecador);
                command.Parameters.AddWithValue("@Recurso", banner.Recurso);
                command.Parameters.AddWithValue("@LocalName", name);
                command.ExecuteReader();
                db.Close();
            }
            else
            {
                Console.WriteLine("El recurso: " + banner.Recurso + "\nExpiro");
            }
            return bannerValido;

        }
        public void InsertatChecadorDatos(Checador checador)
        {
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "";
        }
        public void InsertarChecadorUID(String checador, String uid)
        {
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "UPDATE StorageValues SET value = @value WHERE name = 'Storage:UID'";
            command.Parameters.AddWithValue("@value", uid);
            command.ExecuteReader();
            db.Close();
        }
        public void BorrarDatosAsistencia(DatosAsistencias asistencia, List<AsistenciaDetalle> listaDetallesAsistencias)
        {
            //Insertamos los datos de historial
            /*foreach (AsistenciaDetalle detalle in listaDetallesAsistencias)
            {
                var dbHistorial = new SqliteConnection($"Filename = data.db");
                dbHistorial.Open();
                var fechaActual = DateTime.Now;
                var Actual = fechaActual.Date.ToString("dd-MM-yyyy");
                SqliteCommand command = new SqliteCommand();
                command.Connection = dbHistorial;
                command.CommandText = "INSERT INTO HistorialAsistencia(null,@HoraEntrada,@HoraSalida,@EstatusAsistencia,@FechaTupla,@Tipo,@idEmpleado,@MultipleHorario,@idGrupoPersona)";
                command.Parameters.AddWithValue("@HoraEntrada", detalle.HoraEntrada);
                command.Parameters.AddWithValue("@HoraSalida", detalle.HoraSalida);
                command.Parameters.AddWithValue("@EstatusAsistencia", detalle.EstatusAsistencia);
                command.Parameters.AddWithValue("@FechaTupla", Actual);
                command.Parameters.AddWithValue("@Tipo", detalle.Tipo);
                command.Parameters.AddWithValue("@idEmpleado", asistencia.idEmpleado);
                command.Parameters.AddWithValue("@MultipleHorario", asistencia.MultipleHorario);
                command.Parameters.AddWithValue("@idGrupoPersona", asistencia.idGrupoPersona);
                Console.WriteLine("Horas Entrada: " + detalle.HoraEntrada);
                Console.WriteLine("Hora Salida; " + detalle.HoraSalida);
                Console.WriteLine("Estatus Asistencia: " + detalle.EstatusAsistencia);
                Console.WriteLine("Fecha Tupla: " + detalle.FechaTupla);
                Console.WriteLine("Tipo: " + detalle.Tipo);
                Console.WriteLine("idEmpleado: " + asistencia.idEmpleado);
                Console.WriteLine("Multiple: " + asistencia.MultipleHorario);
                Console.WriteLine("GrupoPersona: " + asistencia.idGrupoPersona);
                command.ExecuteReader();
                dbHistorial.Close();
            }*/
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand commadData = new SqliteCommand();
            commadData.Connection = db;
            //Eliminamos los de talles de la asitencia en la base de datos
            commadData.CommandText = "DELETE FROM AsistenciaDetalle WHERE idAsistencia = @id";
            commadData.Parameters.AddWithValue("@id", asistencia.id);
            commadData.ExecuteReader();
            commadData = null;
            //Eliminamos los datos de la asistencia en la basde de datos
            commadData = new SqliteCommand();
            commadData.Connection = db;
            commadData.CommandText = "DELETE FROM Asistencias WHERE id = @id";
            commadData.Parameters.AddWithValue("@id", asistencia.id);
            commadData.ExecuteReader();
            db.Close();



        }
        public List<RecursoBanner> ObtenerBannerChecador()
        {
            List<RecursoBanner> listaBanner = new List<RecursoBanner>();
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM Banner";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                String limite = reader.GetString(3);
                if (!limite.Equals("-1"))
                {
                    //Verificamos las fechas
                    DateTime bannerFecha = Convert.ToDateTime(limite);
                    DateTime FechaActula = DateTime.Now;
                    Console.WriteLine("Fecha Actual: " + FechaActula + " - Fecha Banner: " + bannerFecha);
                    if (bannerFecha > FechaActula)
                    {
                        RecursoBanner bannerData = new RecursoBanner();
                        bannerData.tipo = reader.GetInt32(1);
                        bannerData.nombre = reader.GetString(8);
                        if(bannerData.tipo == 1)
                        {
                            bannerData.img = (Bitmap)Bitmap.FromFile(Directory.GetCurrentDirectory() + "/" + bannerData.nombre);
                        }
                        listaBanner.Add(bannerData);
                    }

                }
                else
                {
                    RecursoBanner bannerData = new RecursoBanner();
                    bannerData.tipo = reader.GetInt32(1);
                    bannerData.nombre = reader.GetString(8);
                    if(bannerData.tipo == 1)
                    {
                        bannerData.img = (Bitmap)Bitmap.FromFile(Directory.GetCurrentDirectory() + "/" + bannerData.nombre);
                    }
                    listaBanner.Add(bannerData);
                }
            }
            db.Close();
            return listaBanner;
        }
        public bool ValidarFechaLiminte(Banner banner)
        {
            DateTime FechaActual = DateTime.Now;
            if (!String.IsNullOrEmpty(banner.FechaLimite))
            {
                DateTime FechaLimite = Convert.ToDateTime(banner.FechaLimite);
                if (FechaLimite > FechaActual)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public void GuardarAplicaSector(String aplicaSector)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO StorageValues VALUES(null,@name,@value)";
            command.Parameters.AddWithValue("@name", "Storage:AplicaSector");
            command.Parameters.AddWithValue("@value", aplicaSector);
            command.ExecuteReader();
            db.Close();
        }
        public void GuardarIconos(String ruta, String Nombre)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO Iconos VALUES (null,@Ruta,@Nombre)";
            command.Parameters.AddWithValue("@Ruta", ruta);
            command.Parameters.AddWithValue("@Nombre", Nombre);
            command.ExecuteReader();
            db.Close();
        }
        public Dictionary<String, String> ObtenerIconos()
        {
            Dictionary<String, String> listaIconos = new Dictionary<String, String>();
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM Iconos";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                listaIconos.Add(reader.GetString(2), reader.GetString(1));
                Console.WriteLine("Datos: " + reader.GetString(2) + " - " + reader.GetString(1));
            }
            return listaIconos;
        }
        public int validarEmpleados()
        {
            int NoEmpleados = 0;
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT count(id) as numero FROM empleado";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                NoEmpleados = reader.GetInt32(0);
            }
            return NoEmpleados;
        }
        public bool verificarHistorial(String empleado)
        {
            var fechaActual = DateTime.Now;
            var Actual = fechaActual.Date.ToString("dd-MM-yyyy");
            var db = new SqliteConnection($"Filename=data.db");
            String entrada = "", salida = "";
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT HoraEntrada, HoraSalida FROM HistorialAsistencia WHERE idEmpleado = @Empleado AND FechaTupla = @fecha";
            command.Parameters.AddWithValue("@Empleado", empleado);
            command.Parameters.AddWithValue("@fecha", Actual);
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                entrada = reader.GetString(0);
                salida = reader.GetString(1);
            }
            if (entrada != "" && salida != "")
            {
                DateTime entradaHistoria = Convert.ToDateTime(entrada);
                DateTime salidaHistoria = Convert.ToDateTime(salida);
                return (fechaActual >= entradaHistoria && fechaActual <= salidaHistoria);
            }
            else
            {
                return false; //true no se inserta false si se inserta
            }
        }
        public void GuardarNombreEmpresa(String empresa)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "INSERT INTO StorageValues VALUES(null,@name,@value)";
            command.Parameters.AddWithValue("@name", "Storage:Empresa");
            command.Parameters.AddWithValue("@value", empresa);
            command.ExecuteReader();
            db.Close();
        }
        public void EliminarTabla(string tabla)
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "DELETE FROM " + tabla;
            command.ExecuteReader();
            db.Close();
        }
        public void EliminarSectorDB()
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "DELETE FROM StorageValues WHERE name = 'Storage:Sector'";
            command.ExecuteReader();
            db.Close();
        }
        public void EliminaAplicaSector()
        {
            var db = new SqliteConnection($"Filename=data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "DELETE FROM StorageValues WHERE name = 'Storage:AplicaSector'";
            command.ExecuteReader();
            db.Close();
        }
        public int VerificarBanner()
        {
            int length = 0;
            List<RecursoBanner> listaBanner = new List<RecursoBanner>();
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM Banner";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                String limite = reader.GetString(3);
                if (!limite.Equals("-1"))
                {
                    //Verificamos las fechas
                    DateTime bannerFecha = Convert.ToDateTime(limite);
                    DateTime FechaActula = DateTime.Now;
                    Console.WriteLine("Fecha Actual: " + FechaActula + " - Fecha Banner: " + bannerFecha);
                    if (bannerFecha > FechaActula)
                    {
                        length++;
                    }
                }
                else
                {
                    length++;
                }
            }
            db.Close();
            return length;
        }
        public bool VerificarInsercionBanne(string idRepo) {
            bool found = false;
            if ( !String.IsNullOrEmpty(idRepo) )
            {
                var db = new SqliteConnection($"Filename = data.db");
                db.Open();
                SqliteCommand command = new SqliteCommand();
                command.Connection = db;
                command.CommandText = "SELECT * FROM Banner WHERE idRepositorio = @values";
                command.Parameters.AddWithValue("@values", idRepo);
                SqliteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    found = true;
                }
                db.Close();
            }
            return found;
        }
        /*public async List<RecursoBanner> CargarBannerV2(List<RecursoBanner> listaRecurso)
        {
            List<Banner> ListaBanner = new List<Banner>();
            var db = new SqliteConnection($"Filename = data.db");
            db.Open();
            SqliteCommand command = new SqliteCommand();
            command.Connection = db;
            command.CommandText = "SELECT * FROM Banner";
            SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Banner banner = new Banner();
                banner.Tipo = Int32.Parse(reader.GetString(1));
                banner.idRepositorio = reader.GetString(2);
                banner.FechaLimite = reader.GetString(3);
                banner.FechaAlta = reader.GetString(4);
                banner.General = reader.GetString(5);
                banner.idChecador = reader.GetString(6);
                banner.Recurso = reader.GetString(7);
                ListaBanner.Add(banner);
            }
            db.Close();
            Banner bannerAux = new Banner(); ;
            bool found = false;
            int index = 1;
            //Buscamos coincidencias dentro del temporal
            foreach(RecursoBanner recurso in listaRecurso)
            {
                foreach(Banner banner in ListaBanner)
                {
                    if(banner.Recurso == recurso.nombre)
                    {
                        bannerAux = banner;
                        found = true;
                    }
                }
                if (found)
                {
                    //Se tiene que obtener el recurso
                    
                    RecursoBanner bannerAdd = new RecursoBanner() { tipo = bannerAux.Tipo, nombre = bannerAux.Recurso, img = (await DescargarImagenAuxiliar(bannerAux.idRepositorio,index)) };
                    listaRecurso.Add(bannerAdd);
                    index++;
                }
            }
            return listaRecurso;
        }
        //Metodo para descargar imagenes
        public async Task<Bitmap> DescargarImagenAuxiliar(string idRepo,int index)
        {
            string cliente = ObtenerItem("Cliente");
            string direccionImagen = await ObtenerRecursoImagen(cliente,idRepo);
            try
            {
                WebClient web = new WebClient();
                Stream stream = web.OpenRead(direccionImagen);
                Bitmap bitmap = new Bitmap(stream);
                bitmap.Save(Directory.GetCurrentDirectory() + "/Auxiliar" +(index)+".png" , ImageFormat.Png);
                return bitmap;
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
                return null;
            }
        }
        public async Task<String> ObtenerRecursoImagen(string cl, string recurso)
        {
            try
            {
                string dir = "https://api.servicioenlinea.mx/api-movil/ObtenerRecurso";
                string Ruta = "";
                HttpClient cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var value = new Dictionary<String, String>
                {
                    {"cliente",cl },
                    {"recurso",recurso }
                };
                var content = new FormUrlEncodedContent(value);
                var data = await cliente.PostAsync(dir, content);
                Ruta = await data.Content.ReadAsStringAsync();
                cliente = null;
                content = null;
                data = null;
                return Ruta;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " Error de conexion!");
                return "null";
            }
        }
        */
    }

}
