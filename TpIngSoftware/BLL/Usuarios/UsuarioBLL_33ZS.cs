using Servicios;
using Mappers.Security;
using Mappers.Persistence;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLL
{
    public class UsuarioBLL_33ZS
    {
        UsuarioMapper_33ZS usuarioDAL = new UsuarioMapper_33ZS();
        private readonly BitacoraEventoBLL_33ZS bitacoraBLL = new BitacoraEventoBLL_33ZS();
        private readonly PerfilBLL_33ZS perfilBLL = new PerfilBLL_33ZS();
        private readonly DigitoVerificadorBLL_33ZS dvBLL = new DigitoVerificadorBLL_33ZS();

        private void GuardarConDV_33ZS(Action cambio, string login = null,
            TipoEvento_33ZS? tipoEvento = null, int criticidad = 0)
        {
            MapperTransaction_33ZS.Ejecutar_33ZS(() =>
            {
                cambio();
                dvBLL.GuardarDigitos_33ZS("Usuario");
                if (tipoEvento.HasValue)
                    RegistrarEvento_33ZS(login, tipoEvento.Value, criticidad);
            });
        }

        private static readonly int MaxIntentosLogin_33ZS = LeerMaxIntentosLogin_33ZS();

        private void ExigirPatente_33ZS(string patente)
        {
            Usuario_33ZS actual = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS : null;
            if (actual == null || !actual.Activo_33ZS || actual.Bloqueo_33ZS ||
                !perfilBLL.RolTienePatente_33ZS(actual.Rol_33ZS, patente))
                throw new UnauthorizedAccessException("Su rol no tiene permiso para esta operación.");
        }

        private static void RestringirCuentaAdministrador_33ZS(string rol)
        {
            Usuario_33ZS actual = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS : null;
            if (actual != null && actual.Rol_33ZS == "Dueño" &&
                string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase))
                throw new UnauthorizedAccessException("El dueño no puede administrar cuentas del sistema.");
        }

        private static int LeerMaxIntentosLogin_33ZS()
        {
            int valor;
            if (!int.TryParse(ConfigurationManager.AppSettings["MaxIntentosLogin"], out valor) ||
                valor < 1 || valor > 20)
                throw new ConfigurationErrorsException("Configure MaxIntentosLogin con un entero entre 1 y 20.");
            return valor;
        }

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
            ExigirPatente_33ZS("AltaUsuario");
            AgregarUsuarioCore_33ZS(nuevoUsuario);
        }

        internal void AgregarUsuarioInicial_33ZS(Usuario_33ZS nuevoUsuario)
        {
            AgregarUsuarioCore_33ZS(nuevoUsuario);
        }

        private void AgregarUsuarioCore_33ZS(Usuario_33ZS nuevoUsuario)
        {
            ValidarDatosBasicos_33ZS(nuevoUsuario);
            RestringirCuentaAdministrador_33ZS(nuevoUsuario.Rol_33ZS);

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

            nuevoUsuario.Password_33ZS = PasswordHasher_33ZS.Crear_33ZS(nuevoUsuario.Password_33ZS.Trim());

            string loginObjetivo = SessionManager_33ZS.HaySesionActiva_33ZS() ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS : nuevoUsuario.Email_33ZS;
            GuardarConDV_33ZS(() => usuarioDAL.AgregarUsuario_33ZS(nuevoUsuario),
                loginObjetivo, TipoEvento_33ZS.CrearUsuario, 2);
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

            bool actualizarHash;
            if (!PasswordHasher_33ZS.Verificar_33ZS(password, usuario.Password_33ZS, out actualizarHash))
            {
                int intentosRealizados = 0;
                try
                {
                    MapperTransaction_33ZS.Ejecutar_33ZS(() =>
                    {
                        intentosRealizados = usuarioDAL.RegistrarIntentoFallido_33ZS(
                            usuario.DNI_33ZS, MaxIntentosLogin_33ZS);
                        if (intentosRealizados >= MaxIntentosLogin_33ZS)
                        {
                            dvBLL.GuardarDigitos_33ZS("Usuario");
                            RegistrarEvento_33ZS(usuario.Login_33ZS,
                                TipoEvento_33ZS.BloquearUsuario, 3);
                        }
                    });
                }
                catch (SqlException ex) when (ex.Number == 52012)
                {
                    throw new Exception("Usuario.Bloqueado", ex);
                }

                int intentosRestantes = MaxIntentosLogin_33ZS - intentosRealizados;

                if (intentosRealizados >= MaxIntentosLogin_33ZS)
                    throw new Exception("Usuario.PasswordIncorrectaBloqueado");

                throw new Exception($"Login.IntentosRestantes|{intentosRestantes}");
            }

            string hashActualizado = actualizarHash ? PasswordHasher_33ZS.Crear_33ZS(password) : null;
            try
            {
                MapperTransaction_33ZS.Ejecutar_33ZS(() =>
                {
                    usuarioDAL.ReiniciarIntentos_33ZS(usuario.DNI_33ZS);
                    if (actualizarHash)
                    {
                        usuarioDAL.CambiarClave_33ZS(usuario.DNI_33ZS, hashActualizado);
                        dvBLL.GuardarDigitos_33ZS("Usuario");
                    }
                });
            }
            catch (SqlException ex) when (ex.Number == 52012)
            {
                throw new Exception("Usuario.Bloqueado", ex);
            }
            if (actualizarHash)
                usuario.Password_33ZS = hashActualizado;

            SessionManager_33ZS.Login_33ZS(usuario);

            return usuario;
        }

        public void ModificarUsuario_33ZS(Usuario_33ZS usuario)
        {
            ExigirPatente_33ZS("ModificacionUsuario");
            ValidarDatosBasicos_33ZS(usuario);

            Usuario_33ZS usuarioExistente = usuarioDAL.ObtenerUsuarioPorDNI_33ZS(usuario.DNI_33ZS);

            if (usuarioExistente == null)
                throw new Exception("Usuario.DniNoExiste");

            RestringirCuentaAdministrador_33ZS(usuarioExistente.Rol_33ZS);
            RestringirCuentaAdministrador_33ZS(usuario.Rol_33ZS);

            List<Usuario_33ZS> usuariosActuales = ObtenerUsuarios_33ZS();

            bool emailUsadoPorOtro = usuariosActuales.Any(u =>
                !u.DNI_33ZS.Equals(usuario.DNI_33ZS, StringComparison.OrdinalIgnoreCase) &&
                u.Email_33ZS.Equals(usuario.Email_33ZS, StringComparison.OrdinalIgnoreCase));

            if (emailUsadoPorOtro)
                throw new Exception("Usuario.EmailAsignadoAOtro");

            string loginObjetivo = SessionManager_33ZS.HaySesionActiva_33ZS() ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS : usuario.Email_33ZS;
            GuardarConDV_33ZS(() => usuarioDAL.ModificarUsuario_33ZS(usuario),
                loginObjetivo, TipoEvento_33ZS.ModificarUsuario, 2);
        }

        public void DesbloquearUsuario_33ZS(string dni)
        {
            ExigirPatente_33ZS("DesbloquearUsuario");
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception("Usuario.SeleccioneUsuario");

            Usuario_33ZS usuario = usuarioDAL.ObtenerUsuarioPorDNI_33ZS(dni);

            if (usuario == null)
                throw new Exception("Usuario.SeleccionNoExiste");

            RestringirCuentaAdministrador_33ZS(usuario.Rol_33ZS);

            if (!usuario.Bloqueo_33ZS)
                throw new Exception("Usuario.SeleccionNoBloqueado");

            string loginObjetivo = SessionManager_33ZS.HaySesionActiva_33ZS() ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS : usuario.Email_33ZS;
            GuardarConDV_33ZS(() => usuarioDAL.DesbloquearUsuario_33ZS(dni),
                loginObjetivo, TipoEvento_33ZS.DesbloquearUsuario, 2);
        }

        public void CambiarEstadoUsuario_33ZS(string dni)
        {
            ExigirPatente_33ZS("BajaUsuario");
            if (string.IsNullOrWhiteSpace(dni))
                throw new Exception("Usuario.SeleccioneUsuario");

            Usuario_33ZS usuario = usuarioDAL.ObtenerUsuarioPorDNI_33ZS(dni);

            if (usuario == null)
                throw new Exception("Usuario.SeleccionNoExiste");

            RestringirCuentaAdministrador_33ZS(usuario.Rol_33ZS);

            bool nuevoEstado = !usuario.Activo_33ZS;

            TipoEvento_33ZS tipoEvento = usuario.Activo_33ZS ? TipoEvento_33ZS.DesactivarUsuario : TipoEvento_33ZS.ActivarUsuario;
            string loginObjetivo = SessionManager_33ZS.HaySesionActiva_33ZS() ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS : usuario.Email_33ZS;
            GuardarConDV_33ZS(() => usuarioDAL.CambiarEstadoUsuario_33ZS(dni, nuevoEstado),
                loginObjetivo, tipoEvento, 2);
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
            ExigirPatente_33ZS("CambiarClave");

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

            bool actualizarHash;
            if (!PasswordHasher_33ZS.Verificar_33ZS(claveActual, usuarioBD.Password_33ZS, out actualizarHash))
                throw new Exception("Usuario.ClaveActualIncorrecta");

            string claveNuevaHash = PasswordHasher_33ZS.Crear_33ZS(claveNueva);

            GuardarConDV_33ZS(() => usuarioDAL.CambiarClave_33ZS(usuarioBD.DNI_33ZS, claveNuevaHash),
                usuarioBD.Login_33ZS, TipoEvento_33ZS.CambiarClave, 2);
        }

        private void RegistrarEvento_33ZS(string login, TipoEvento_33ZS tipoEvento, int criticidad)
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
        
        public void CambiarIdioma_33ZS(string codigoIdioma)
        {
            SessionManager_33ZS.GetInstance_33ZS().CambiarIdioma_33ZS(codigoIdioma);

            if (SessionManager_33ZS.HaySesionActiva_33ZS())
            {
                Usuario_33ZS usuarioSesion = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;
                GuardarConDV_33ZS(() => usuarioDAL.ActualizarIdioma_33ZS(usuarioSesion.DNI_33ZS, codigoIdioma),
                    usuarioSesion.Login_33ZS, TipoEvento_33ZS.CambiarIdioma, 5);
            }
        }

        public void Logout_33ZS()
        {
            if (SessionManager_33ZS.HaySesionActiva_33ZS())
            {
                Usuario_33ZS usuarioSesion = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;

                try
                {
                    GuardarConDV_33ZS(() => usuarioDAL.ActualizarIdioma_33ZS(usuarioSesion.DNI_33ZS, usuarioSesion.Idioma_33ZS));
                }
                catch
                {
                }

                try { RegistrarEvento_33ZS(usuarioSesion.Login_33ZS, TipoEvento_33ZS.Logout, 1); }
                catch (Exception) { /* El cierre de sesión no depende de la bitácora. */ }
                SessionManager_33ZS.Logout_33ZS();
            }
        }
    }
}
