using DAL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLL
{
    public class UsuarioBLL_33ZS
    {
        UsuarioDAL_33ZS usuarioDAL = new UsuarioDAL_33ZS();
        private readonly BitacoraEventoBLL_33ZS bitacoraBLL = new BitacoraEventoBLL_33ZS();
        private readonly PerfilBLL_33ZS perfilBLL = new PerfilBLL_33ZS();
        private readonly DigitoVerificadorBLL_33ZS dvBLL = new DigitoVerificadorBLL_33ZS();

        private const int MAX_INTENTOS_LOGIN_33ZS = 3;

        private static Dictionary<string, int> intentosFallidosMemoria_33ZS = new Dictionary<string, int>();
        
        public List<Usuario_33ZS> ObtenerUsuarios_33ZS()
        {
            return usuarioDAL.ObtenerUsuarios_33ZS();
        }

        public List<Usuario_33ZS> ObtenerUsuariosPorEstado_33ZS(bool activo)
        {
            return usuarioDAL.ObtenerUsuariosPorEstado_33ZS(activo);
        }

        public void AgregarUsuario_33ZS(Usuario_33ZS nuevoUsuario)
        {
            ValidarDatosBasicos_33ZS(nuevoUsuario);

            List<Usuario_33ZS> usuariosActuales = ObtenerUsuarios_33ZS();

            bool existeDNI = usuariosActuales.Any(u =>
                u.DNI_33ZS.Equals(nuevoUsuario.DNI_33ZS, StringComparison.OrdinalIgnoreCase));

            if (existeDNI)
                throw new Exception("Usuario.DniDuplicado");

            bool existeEmail = usuariosActuales.Any(u =>
                u.Email_33ZS.Equals(nuevoUsuario.Email_33ZS, StringComparison.OrdinalIgnoreCase));

            if (existeEmail)
                throw new Exception("Usuario.EmailDuplicado");

            if (string.IsNullOrWhiteSpace(nuevoUsuario.Password_33ZS))
                throw new Exception("Usuario.PasswordInicialNoGenerada");

            if (nuevoUsuario.Password_33ZS.Length > 255)
                throw new Exception("Usuario.PasswordInicialMuyLarga");

            nuevoUsuario.Password_33ZS = Encriptador_33ZS.Hash(nuevoUsuario.Password_33ZS);

            usuarioDAL.AgregarUsuario_33ZS(nuevoUsuario);
            dvBLL.GuardarDigitos_33ZS("Usuario");

            string loginObjetivo = SessionManager_33ZS.HaySesionActiva_33ZS() ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS : nuevoUsuario.Email_33ZS;
            RegistrarEventoSeguro_33ZS(loginObjetivo, TipoEvento_33ZS.CrearUsuario, 2);
        }

        public Usuario_33ZS ValidarLogin_33ZS(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new Exception("Usuario.LoginRequerido");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("Usuario.PasswordRequerido");

            login = login.Trim();
            password = password.Trim();

            Usuario_33ZS usuario = usuarioDAL.ObtenerUsuarioPorLogin_33ZS(login);
            if (usuario == null)
                throw new Exception("Usuario.LoginNoExiste");

            if (!usuario.Activo_33ZS)
                throw new Exception("Usuario.Inactivo");

            if (usuario.Bloqueo_33ZS)
                throw new Exception("Usuario.Bloqueado");

            string claveIntentos = usuario.DNI_33ZS;

            if (!intentosFallidosMemoria_33ZS.ContainsKey(claveIntentos))
                intentosFallidosMemoria_33ZS[claveIntentos] = 0;

            string passwordHash = Encriptador_33ZS.Hash(password);

            if (!usuario.Password_33ZS.Equals(passwordHash, StringComparison.OrdinalIgnoreCase))
            {
                intentosFallidosMemoria_33ZS[claveIntentos]++;

                int intentosRealizados = intentosFallidosMemoria_33ZS[claveIntentos];
                int intentosRestantes = MAX_INTENTOS_LOGIN_33ZS - intentosRealizados;

                if (intentosRealizados >= MAX_INTENTOS_LOGIN_33ZS)
                {
                    usuarioDAL.BloquearUsuario_33ZS(usuario.DNI_33ZS);
                    dvBLL.GuardarDigitos_33ZS("Usuario");
                    intentosFallidosMemoria_33ZS[claveIntentos] = 0;
                    RegistrarEventoSeguro_33ZS(usuario.Login_33ZS, TipoEvento_33ZS.BloquearUsuario, 3);

                    throw new Exception("Usuario.PasswordIncorrectaBloqueado");
                }

                throw new Exception($"Login.IntentosRestantes|{intentosRestantes}");
            }

            intentosFallidosMemoria_33ZS[claveIntentos] = 0;

            SessionManager_33ZS.Login_33ZS(usuario);

            return usuario;
        }

        public void ModificarUsuario_33ZS(Usuario_33ZS usuario)
        {
            ValidarDatosBasicos_33ZS(usuario);

            Usuario_33ZS usuarioExistente = usuarioDAL.ObtenerUsuarioPorDNI_33ZS(usuario.DNI_33ZS);

            if (usuarioExistente == null)
                throw new Exception("Usuario.DniNoExiste");

            List<Usuario_33ZS> usuariosActuales = ObtenerUsuarios_33ZS();

            bool emailUsadoPorOtro = usuariosActuales.Any(u =>
                !u.DNI_33ZS.Equals(usuario.DNI_33ZS, StringComparison.OrdinalIgnoreCase) &&
                u.Email_33ZS.Equals(usuario.Email_33ZS, StringComparison.OrdinalIgnoreCase));

            if (emailUsadoPorOtro)
                throw new Exception("Usuario.EmailAsignadoAOtro");

            usuarioDAL.ModificarUsuario_33ZS(usuario);
            dvBLL.GuardarDigitos_33ZS("Usuario");

            string loginObjetivo = SessionManager_33ZS.HaySesionActiva_33ZS() ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS : usuario.Email_33ZS;
            RegistrarEventoSeguro_33ZS(loginObjetivo, TipoEvento_33ZS.ModificarUsuario, 2);
        }

        public void DesbloquearUsuario_33ZS(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception("Usuario.SeleccioneUsuario");

            Usuario_33ZS usuario = usuarioDAL.ObtenerUsuarioPorDNI_33ZS(dni);

            if (usuario == null)
                throw new Exception("Usuario.SeleccionNoExiste");

            if (!usuario.Bloqueo_33ZS)
                throw new Exception("Usuario.SeleccionNoBloqueado");

            usuarioDAL.DesbloquearUsuario_33ZS(dni);
            dvBLL.GuardarDigitos_33ZS("Usuario");

            string loginObjetivo = SessionManager_33ZS.HaySesionActiva_33ZS() ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS : usuario.Email_33ZS;
            RegistrarEventoSeguro_33ZS(loginObjetivo, TipoEvento_33ZS.DesbloquearUsuario, 2);
        }

        public void CambiarEstadoUsuario_33ZS(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception("Usuario.SeleccioneUsuario");

            Usuario_33ZS usuario = usuarioDAL.ObtenerUsuarioPorDNI_33ZS(dni);

            if (usuario == null)
                throw new Exception("Usuario.SeleccionNoExiste");

            bool nuevoEstado = !usuario.Activo_33ZS;

            usuarioDAL.CambiarEstadoUsuario_33ZS(dni, nuevoEstado);
            dvBLL.GuardarDigitos_33ZS("Usuario");

            TipoEvento_33ZS tipoEvento = usuario.Activo_33ZS ? TipoEvento_33ZS.DesactivarUsuario : TipoEvento_33ZS.ActivarUsuario;
            string loginObjetivo = SessionManager_33ZS.HaySesionActiva_33ZS() ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS : usuario.Email_33ZS;
            RegistrarEventoSeguro_33ZS(loginObjetivo, tipoEvento, 2);
        }

        private void ValidarDatosBasicos_33ZS(Usuario_33ZS usuario)
        {
            if (usuario == null)
                throw new Exception("Usuario.Nulo");

            usuario.DNI_33ZS = usuario.DNI_33ZS?.Trim();
            usuario.Apellidos_33ZS = usuario.Apellidos_33ZS?.Trim();
            usuario.Nombre_33ZS = usuario.Nombre_33ZS?.Trim();
            usuario.Email_33ZS = usuario.Email_33ZS?.Trim();
            usuario.Login_33ZS = usuario.Login_33ZS?.Trim();

            if (string.IsNullOrWhiteSpace(usuario.DNI_33ZS))
                throw new Exception("Usuario.DniRequerido");

            if (!Regex.IsMatch(usuario.DNI_33ZS, @"^\d{7,20}$"))
                throw new Exception("Usuario.DniInvalido");

            if (string.IsNullOrWhiteSpace(usuario.Apellidos_33ZS))
                throw new Exception("Usuario.ApellidoRequerido");

            if (usuario.Apellidos_33ZS.Length > 100)
                throw new Exception("Usuario.ApellidoMaximo");

            if (!Regex.IsMatch(usuario.Apellidos_33ZS, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
                throw new Exception("Usuario.ApellidoFormato");

            if (string.IsNullOrWhiteSpace(usuario.Nombre_33ZS))
                throw new Exception("Usuario.NombreRequerido");

            if (usuario.Nombre_33ZS.Length > 100)
                throw new Exception("Usuario.NombreMaximo");

            if (!Regex.IsMatch(usuario.Nombre_33ZS, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s'-]+$"))
                throw new Exception("Usuario.NombreFormato");

            if (string.IsNullOrWhiteSpace(usuario.Email_33ZS))
                throw new Exception("Usuario.EmailRequerido");

            if (usuario.Email_33ZS.Length > 150)
                throw new Exception("Usuario.EmailMaximo");

            if (!Regex.IsMatch(usuario.Email_33ZS, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new Exception("Usuario.EmailFormato");

            if (string.IsNullOrWhiteSpace(usuario.Rol_33ZS))
                throw new Exception("Usuario.RolRequerido");

            if (usuario.Rol_33ZS.Length > 50)
                throw new Exception("Usuario.RolMaximo");

            bool rolExiste = perfilBLL.ObtenerRoles_33ZS()
                .Any(r => r.Nombre.Equals(usuario.Rol_33ZS, StringComparison.OrdinalIgnoreCase));

            if (!rolExiste)
                throw new Exception("Usuario.RolNoExiste");

            if (string.IsNullOrWhiteSpace(usuario.Login_33ZS))
                usuario.Login_33ZS = usuario.Email_33ZS;

            if (usuario.Login_33ZS.Length > 150)
                throw new Exception("Usuario.LoginMaximo");
        }

        public void CambiarClave_33ZS(string claveActual, string claveNueva, string claveRepetida)
        {
            if (!SessionManager_33ZS.HaySesionActiva_33ZS())
                throw new Exception("Usuario.SinSesionActiva");

            if (string.IsNullOrWhiteSpace(claveActual))
                throw new Exception("Usuario.ClaveActualRequerida");

            if (string.IsNullOrWhiteSpace(claveNueva))
                throw new Exception("Usuario.ClaveNuevaRequerida");

            if (string.IsNullOrWhiteSpace(claveRepetida))
                throw new Exception("Usuario.ClaveRepetidaRequerida");

            claveActual = claveActual.Trim();
            claveNueva = claveNueva.Trim();
            claveRepetida = claveRepetida.Trim();

            if (claveNueva != claveRepetida)
                throw new Exception("Usuario.ClavesNoCoinciden");

            if (claveNueva.Length < 6)
                throw new Exception("Usuario.ClaveMinima");

            if (claveNueva.Length > 50)
                throw new Exception("Usuario.ClaveMaxima");

            if (claveNueva.Contains(" "))
                throw new Exception("Usuario.ClaveSinEspacios");

            if (claveActual == claveNueva)
                throw new Exception("Usuario.ClaveIgualActual");

            Usuario_33ZS usuarioSesion = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;

            if (usuarioSesion == null)
                throw new Exception("Usuario.SesionNoObtenida");

            Usuario_33ZS usuarioBD = usuarioDAL.ObtenerUsuarioPorDNI_33ZS(usuarioSesion.DNI_33ZS);

            if (usuarioBD == null)
                throw new Exception("Usuario.SesionUsuarioNoEncontrado");

            if (!usuarioBD.Activo_33ZS)
                throw new Exception("Usuario.Inactivo");

            if (usuarioBD.Bloqueo_33ZS)
                throw new Exception("Usuario.Bloqueado");

            if (claveNueva == usuarioBD.DNI_33ZS)
                throw new Exception("Usuario.ClaveIgualDni");

            string claveActualHash = Encriptador_33ZS.Hash(claveActual);

            if (!usuarioBD.Password_33ZS.Equals(claveActualHash, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Usuario.ClaveActualIncorrecta");

            string claveNuevaHash = Encriptador_33ZS.Hash(claveNueva);

            usuarioDAL.CambiarClave_33ZS(usuarioBD.DNI_33ZS, claveNuevaHash);
            dvBLL.GuardarDigitos_33ZS("Usuario");
        }

        private void RegistrarEventoSeguro_33ZS(string login, TipoEvento_33ZS tipoEvento, int criticidad)
        {
            try
            {
                BitacoraEvento_33ZS evento = new BitacoraEvento_33ZS
                {
                    Login_33ZS = login,
                    Fecha_33ZS = DateTime.Today,
                    Hora_33ZS = DateTime.Now,
                    Modulo_33ZS = ModuloSistema_33ZS.Usuario.ToString(),
                    NombreEvento_33ZS = tipoEvento.ToString(),
                    Criticidad_33ZS = criticidad
                };

                bitacoraBLL.RegistrarEvento_33ZS(evento);
            }
            catch
            {
            }
        }
        
        public void CambiarIdioma_33ZS(string codigoIdioma)
        {
            SessionManager_33ZS.GetInstance_33ZS().CambiarIdioma_33ZS(codigoIdioma);

            if (SessionManager_33ZS.HaySesionActiva_33ZS())
            {
                Usuario_33ZS usuarioSesion = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;
                usuarioDAL.ActualizarIdioma_33ZS(usuarioSesion.DNI_33ZS, codigoIdioma);
                dvBLL.GuardarDigitos_33ZS("Usuario");
                RegistrarEventoSeguro_33ZS(usuarioSesion.Login_33ZS, TipoEvento_33ZS.CambiarIdioma, 5);
            }
        }

        public void Logout_33ZS()
        {
            if (SessionManager_33ZS.HaySesionActiva_33ZS())
            {
                Usuario_33ZS usuarioSesion = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;

                try
                {
                    usuarioDAL.ActualizarIdioma_33ZS(usuarioSesion.DNI_33ZS, usuarioSesion.Idioma_33ZS);
                }
                catch
                {
                }

                RegistrarEventoSeguro_33ZS(usuarioSesion.Login_33ZS, TipoEvento_33ZS.Logout, 1);
                SessionManager_33ZS.Logout_33ZS();
            }
        }
    }
}
