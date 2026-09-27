using System;
using Mappers.Persistence;
using Servicios;

namespace BLL
{
    public static class AdministradorInicial_33ZS
    {
        public const string EmailDemoUno_33ZS = "demo1@harlem.local";
        public const string EmailDemoDos_33ZS = "demo2@harlem.local";

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
            string claveDemo = Environment.GetEnvironmentVariable("TPINGSOFTWARE_DEMO_PASSWORD");

            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellidos) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(claveAdmin) || string.IsNullOrWhiteSpace(claveDemo))
                return false;

            try
            {
                CrearCuentas_33ZS(dni, nombre, apellidos, email, claveAdmin, claveDemo);
                return true;
            }
            finally
            {
                Environment.SetEnvironmentVariable("TPINGSOFTWARE_ADMIN_PASSWORD", null,
                    EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TPINGSOFTWARE_DEMO_PASSWORD", null,
                    EnvironmentVariableTarget.Process);
            }
        }

        public static void CrearCuentas_33ZS(string dni, string nombre, string apellidos,
            string email, string claveAdmin, string claveDemo)
        {
            if (!RequiereConfiguracion_33ZS())
                throw new InvalidOperationException("La base ya tiene usuarios.");

            ValidarClave_33ZS(claveAdmin, "administrador");
            ValidarClave_33ZS(claveDemo, "demostración");

            UsuarioBLL_33ZS usuarios = new UsuarioBLL_33ZS();
            MapperTransaction_33ZS.Ejecutar_33ZS(() =>
            {
                new DigitoVerificadorBLL_33ZS().GenerarTodo_33ZS();
                usuarios.AgregarUsuario_33ZS(new Usuario_33ZS(
                    dni, apellidos, nombre, email, claveAdmin, "Administrador", email));
                usuarios.AgregarUsuario_33ZS(new Usuario_33ZS(
                    "90000001", "Ejemplo", "Demo Uno", EmailDemoUno_33ZS,
                    claveDemo, "Recepcionista", EmailDemoUno_33ZS));
                usuarios.AgregarUsuario_33ZS(new Usuario_33ZS(
                    "90000002", "Ejemplo", "Demo Dos", EmailDemoDos_33ZS,
                    claveDemo, "Barbero", EmailDemoDos_33ZS));
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
