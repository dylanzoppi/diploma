using System;
using Mappers.PN1;
using Mappers.Persistence;
using Servicios;
using BE.PN1;

namespace BLL.PN1
{
    public static class CargaDemo_33ZS
    {
        // Credenciales publicadas para la instalación de demostración.
        public const string Clave_33ZS = "DemoHarlem2026!";

        public static void Aplicar_33ZS()
        {
            var mapper = new PN1Mapper_33ZS();
            if (!mapper.CargaDemoPendiente()) return;

            MapperTransaction_33ZS.Ejecutar_33ZS(() =>
            {
                if (!mapper.CargaDemoPendiente()) return;
                var usuarios = new UsuarioBLL_33ZS();

                AgregarUsuario(usuarios, "91000001", "López", "Lucía",
                    "recepcion.demo@harlem.local", "Recepcionista");
                AgregarUsuario(usuarios, "91000002", "Fernández", "Mateo",
                    "barbero.mateo@harlem.local", "Barbero");
                AgregarUsuario(usuarios, "91000003", "Gómez", "Tomás",
                    "barbero.tomas@harlem.local", "Barbero");
                AgregarUsuario(usuarios, "91000004", "Pérez", "Valentina",
                    "dueno.demo@harlem.local", "Dueño");

                mapper.ConfigurarBarbero("91000002", 50m, true);
                mapper.ConfigurarBarbero("91000003", 50m, true);

                AgregarCliente(mapper, "Sofía", "Ramírez", "1199000001");
                AgregarCliente(mapper, "Julián", "Torres", "1199000002");
                AgregarCliente(mapper, "Camila", "Morales", "1199000003");
                AgregarCliente(mapper, "Nicolás", "Castro", "1199000004");
                AgregarCliente(mapper, "Martina", "Silva", "1199000005");

                mapper.ConfirmarCargaDemo();
            });
        }

        private static void AgregarUsuario(UsuarioBLL_33ZS usuarios, string dni,
            string apellido, string nombre, string email, string rol)
        {
            usuarios.AgregarUsuarioInicial_33ZS(new Usuario_33ZS(
                dni, apellido, nombre, email, Clave_33ZS, rol, email));
        }

        private static void AgregarCliente(PN1Mapper_33ZS mapper, string nombre,
            string apellido, string telefono)
        {
            mapper.RegistrarCliente(new Cliente_33ZS
            {
                Nombre = nombre,
                Apellido = apellido,
                Telefono = telefono
            });
        }
    }
}
