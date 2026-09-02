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
                bool baseCreada = DatabaseInitializer_33ZS.AsegurarBaseDeDatos_33ZS();

                if (baseCreada)
                {
                    // Solo se generan los DV en la inicialización de una BD nueva.
                    new DigitoVerificadorBLL_33ZS().GenerarTodo_33ZS();
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
