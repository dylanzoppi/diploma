using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace Servicios
{
    public class SessionManager_33ZS : Sujeto_33ZS
    {
        private static SessionManager_33ZS _instancia_33ZS;
        private static readonly object _lock_33ZS = new object();

        public Usuario_33ZS UsuarioActual_33ZS { get; private set; }
        public DateTime FechaInicio_33ZS { get; private set; }


        private string _idiomaActual_33ZS = "ESP";
        private Dictionary<string, string> _traducciones_33ZS;

        private SessionManager_33ZS()
        {
        }

        public static SessionManager_33ZS GetInstance_33ZS()
        {
            if (_instancia_33ZS == null)
            {
                lock (_lock_33ZS)
                {
                    if (_instancia_33ZS == null)
                        _instancia_33ZS = new SessionManager_33ZS();
                }
            }

            return _instancia_33ZS;
        }

        public static void Login_33ZS(Usuario_33ZS usuario)
        {
            if (usuario == null)
                throw new Exception("No se puede iniciar sesión sin un usuario válido.");

            lock (_lock_33ZS)
            {
                SessionManager_33ZS sesion = GetInstance_33ZS();

                if (sesion.UsuarioActual_33ZS != null)
                    throw new Exception("Ya existe una sesión iniciada.");

                sesion.UsuarioActual_33ZS = usuario;
                sesion.FechaInicio_33ZS = DateTime.Now;
            }
        }

        public static void Logout_33ZS()
        {
            lock (_lock_33ZS)
            {
                SessionManager_33ZS sesion = GetInstance_33ZS();

                if (sesion.UsuarioActual_33ZS == null)
                    throw new Exception("No hay una sesión iniciada.");
                sesion.UsuarioActual_33ZS = null;
            }
        }

        public static bool HaySesionActiva_33ZS()
        {
            return _instancia_33ZS != null && _instancia_33ZS.UsuarioActual_33ZS != null;
        }

        public string Traducir_33ZS(string clave)
        {
            if (string.IsNullOrWhiteSpace(clave))
                return string.Empty;

            string[] partes = clave.Split('|');
            string claveBase = partes[0];

            if (_traducciones_33ZS != null && _traducciones_33ZS.ContainsKey(claveBase))
            {
                string texto = _traducciones_33ZS[claveBase];
                for (int i = 1; i < partes.Length; i++)
                    texto = texto.Replace("{" + (i - 1) + "}", partes[i]);

                return texto;
            }

            if (_traducciones_33ZS != null && _traducciones_33ZS.ContainsKey(clave))
                return _traducciones_33ZS[clave];

            return clave;
        }

        public string GetIdiomaActual_33ZS()
        {
            return _idiomaActual_33ZS;
        }

        public void CambiarIdioma_33ZS(string codigoIdioma)
        {
            if (string.IsNullOrWhiteSpace(codigoIdioma))
                throw new Exception("Debe indicar un código de idioma válido.");

            codigoIdioma = codigoIdioma.Trim().ToUpperInvariant();

            string ruta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Idiomas", codigoIdioma + ".json");

            if (!File.Exists(ruta))
                throw new Exception("SessionManager.ArchivoIdiomaNoEncontrado|" + ruta);

            string json = File.ReadAllText(ruta);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            _traducciones_33ZS = serializer.Deserialize<Dictionary<string, string>>(json);
            _idiomaActual_33ZS = codigoIdioma;

            if (UsuarioActual_33ZS != null)
                UsuarioActual_33ZS.Idioma_33ZS = codigoIdioma;

            Notificar_33ZS();
        }
    }
}
