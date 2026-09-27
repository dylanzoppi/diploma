using System;
using System.Data;

namespace Servicios
{
    public class Usuario_33ZS
    {
        public Usuario_33ZS() { }

        public Usuario_33ZS(DataRow dr)
        {
            DNI_33ZS = dr["DNI"].ToString();
            Apellidos_33ZS = dr["Apellidos"].ToString();
            Nombre_33ZS = dr["Nombre"].ToString();
            Login_33ZS = dr["Login"].ToString();
            Password_33ZS = dr["Password"].ToString();
            Rol_33ZS = dr["Rol"].ToString();
            Email_33ZS = dr["Email"].ToString();
            Bloqueo_33ZS = bool.Parse(dr["Bloqueo"].ToString());
            Activo_33ZS = bool.Parse(dr["Activo"].ToString());
            Idioma_33ZS = dr.Table.Columns.Contains("Idioma") && dr["Idioma"] != DBNull.Value
                          ? dr["Idioma"].ToString()
                          : "ESP";
        }

        public Usuario_33ZS(string dni, string apellidos, string nombre, string login, string password, string rol, string email, bool bloqueo = false, bool activo = true)
        {
            DNI_33ZS = dni;
            Apellidos_33ZS = apellidos;
            Nombre_33ZS = nombre;
            Login_33ZS = login;
            Password_33ZS = password;
            Rol_33ZS = rol;
            Email_33ZS = email;
            Bloqueo_33ZS = bloqueo;
            Activo_33ZS = activo;
        }

        private string _dni;
        private string _apellidos;
        private string _nombre;
        private string _login;
        private string _password;
        private string _rol;
        private string _email;
        private bool _bloqueo = false;
        private bool _activo = true;
        private string _idioma = "ESP";

        public string DNI_33ZS
        {
            get { return _dni; }
            set { _dni = value; }
        }

        public string Apellidos_33ZS
        {
            get { return _apellidos; }
            set { _apellidos = value; }
        }

        public string Nombre_33ZS
        {
            get { return _nombre; }
            set { _nombre = value; }
        }

        public string Login_33ZS
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Password_33ZS
        {
            get { return _password; }
            set { _password = value; }
        }

        public string Rol_33ZS
        {
            get { return _rol; }
            set { _rol = value; }
        }

        public string Email_33ZS
        {
            get { return _email; }
            set { _email = value; }
        }

        public bool Bloqueo_33ZS
        {
            get { return _bloqueo; }
            set { _bloqueo = value; }
        }

        public bool Activo_33ZS
        {
            get { return _activo; }
            set { _activo = value; }
        }

        public string Idioma_33ZS
        {
            get { return _idioma; }
            set { _idioma = value; }
        }
    }
}
