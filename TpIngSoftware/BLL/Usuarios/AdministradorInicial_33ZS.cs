using System;
using Mappers.Persistence;
using Servicios;

namespace BLL
{
    public static class AdministradorInicial_33ZS
    {
        public static bool RequiereConfiguracion_33ZS()
        {
            return new UsuarioBLL_33ZS().ObtenerUsuarios_33ZS().Count == 0;
        }

        public static bool CrearDesdeEntorno_33ZS()
        {
            if (!RequiereConfiguracion_33ZS())
                return true;

            string dni = Environment.GetEnvironmentVariable("TPINGSOFTWARE_ADMIN_DNI");
            string nombre = Environment.GetEnvironmentVariable("TPINGSOFTWARE_ADMIN_NOMBRE");
            string apellidos = Environment.GetEnvironmentVariable("TPINGSOFTWARE_ADMIN_APELLIDOS");
            string email = Environment.GetEnvironmentVariable("TPINGSOFTWARE_ADMIN_EMAIL");
            string claveAdmin = Environment.GetEnvironmentVariable("TPINGSOFTWARE_ADMIN_PASSWORD");
            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellidos) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(claveAdmin))
                return false;

            try
            {
                CrearCuenta_33ZS(dni, nombre, apellidos, email, claveAdmin);
                return true;
            }
            finally
            {
                Environment.SetEnvironmentVariable("TPINGSOFTWARE_ADMIN_PASSWORD", null,
                    EnvironmentVariableTarget.Process);
            }
        }

        public static void CrearCuenta_33ZS(string dni, string nombre, string apellidos,
            string email, string claveAdmin)
        {
            if (!RequiereConfiguracion_33ZS())
                throw new InvalidOperationException("La base ya tiene usuarios.");

            ValidarClave_33ZS(claveAdmin, "administrador");

            UsuarioBLL_33ZS usuarios = new UsuarioBLL_33ZS();
            MapperTransaction_33ZS.Ejecutar_33ZS(() =>
            {
                new DigitoVerificadorBLL_33ZS().GenerarTodo_33ZS();
                usuarios.AgregarUsuarioInicial_33ZS(new Usuario_33ZS(
                    dni, apellidos, nombre, email, claveAdmin, "Administrador", email));
            });
        }

        private static void ValidarClave_33ZS(string clave, string cuenta)
        {
            if (string.IsNullOrWhiteSpace(clave) || clave.Length < 12 || clave.Length > 50)
                throw new ArgumentException("La contraseña de " + cuenta +
                    " debe tener entre 12 y 50 caracteres.");
        }
    }
}
