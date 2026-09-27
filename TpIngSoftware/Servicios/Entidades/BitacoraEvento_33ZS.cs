using System;
using System.Collections.Generic;
using System.Data; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios
{
    public enum ModuloSistema_33ZS
    {
        Usuario,
        Ventas,
        Compras,
        Maestro,
        Perfiles,
        Bitacora,
        Respaldos
    }
    public enum TipoEvento_33ZS
    {
        Login,
        Logout,
        CrearUsuario,
        ModificarUsuario,
        CambiarClave,
        BloquearUsuario,
        DesbloquearUsuario,
        ActivarUsuario,
        DesactivarUsuario,
        ConsultarBitacora,
        CambiarIdioma,
        CrearRol,
        ModificarRol,
        EliminarRol,
        CrearFamilia,
        ModificarFamilia,
        EliminarFamilia,
        RealizarBackup,
        RealizarRestore,
        RecalcularDV
    }

    public class BitacoraEvento_33ZS
    {
        public BitacoraEvento_33ZS() { }

        public BitacoraEvento_33ZS(DataRow dr)
        {
            Id_Evento_33ZS = Convert.ToInt32(dr[0]);
            Login_33ZS = dr[1].ToString();
            Fecha_33ZS = Convert.ToDateTime(dr[2]);
            Hora_33ZS = Convert.ToDateTime(dr[3]); 
            Modulo_33ZS = dr[4].ToString();
            NombreEvento_33ZS = dr[5].ToString();
            Criticidad_33ZS = Convert.ToInt32(dr[6]);
        }

        public BitacoraEvento_33ZS(int idEvento, string login, DateTime fecha, DateTime hora, string modulo, string nombreEvento, int criticidad)
        {
            Id_Evento_33ZS = idEvento;
            Login_33ZS = login;
            Fecha_33ZS = fecha;
            Hora_33ZS = hora;
            Modulo_33ZS = modulo;
            NombreEvento_33ZS = nombreEvento;
            Criticidad_33ZS = criticidad;
        }

        private int _idEvento;
        private string _login;
        private DateTime _fecha;
        private DateTime _hora;
        private string _modulo;
        private string _nombreEvento;
        private int _criticidad;

        public int Id_Evento_33ZS
        {
            get { return _idEvento; }
            set { _idEvento = value; }
        }

        public string Login_33ZS
        {
            get { return _login; }
            set { _login = value; }
        }

        public DateTime Fecha_33ZS
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public DateTime Hora_33ZS
        {
            get { return _hora; }
            set { _hora = value; }
        }

        public string Modulo_33ZS
        {
            get { return _modulo; }
            set { _modulo = value; }
        }

        public string NombreEvento_33ZS
        {
            get { return _nombreEvento; }
            set { _nombreEvento = value; }
        }

        public int Criticidad_33ZS
        {
            get { return _criticidad; }
            set { _criticidad = value; }
        }
    }
}
