using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Servicios;

namespace TpIngSoftware
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DatabaseInitializer_33ZS.AsegurarBaseDeDatos_33ZS();

                DatabaseMigrator_33ZS.AplicarMigraciones_33ZS();

                if (AdministradorInicial_33ZS.RequiereConfiguracion_33ZS() &&
                    !AdministradorInicial_33ZS.CrearDesdeEntorno_33ZS())
                {
                    using (ConfiguracionInicial33ZS configuracion = new ConfiguracionInicial33ZS())
                    {
                        if (configuracion.ShowDialog() != DialogResult.OK)
                            return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo inicializar la base de datos:\n" + ex.Message,
                    "Error de inicialización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Idioma por defecto antes del Login
            SessionManager_33ZS.GetInstance_33ZS().CambiarIdioma_33ZS("ESP");

            Application.Run(new Login33ZS());
        }
    }
}
