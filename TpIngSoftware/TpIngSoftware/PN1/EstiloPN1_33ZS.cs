using System.Drawing;
using System.Windows.Forms;
using Servicios;

namespace TpIngSoftware.PN1
{
    internal static class EstiloPN1_33ZS
    {
        internal static string T(string espanol, string ingles) =>
            SessionManager_33ZS.GetInstance_33ZS().GetIdiomaActual_33ZS() == "ENG"
                ? ingles : espanol;

        internal static readonly Color Fondo = Color.FromArgb(246, 243, 238);
        internal static readonly Color Tinta = Color.FromArgb(35, 40, 42);
        internal static readonly Color Acento = Color.FromArgb(127, 79, 50);
        internal static readonly Color Superficie = Color.White;

        internal static void Preparar(Form form, string titulo, int ancho = 950, int alto = 680)
        {
            form.Text = titulo + " · Harlem";
            form.StartPosition = FormStartPosition.CenterParent;
            form.Size = new Size(ancho, alto);
            form.MinimumSize = new Size(760, 560);
            form.BackColor = Fondo;
            form.ForeColor = Tinta;
            form.Font = new Font("Segoe UI", 10);
        }

        internal static Label Titulo(string texto) => new Label
        {
            Text = texto, AutoSize = true, Font = new Font("Segoe UI Semibold", 20),
            ForeColor = Tinta, Margin = new Padding(0, 0, 0, 8)
        };

        internal static Label Etiqueta(string texto) => new Label
        {
            Text = texto, AutoSize = true, ForeColor = Tinta,
            Margin = new Padding(0, 7, 8, 4)
        };

        internal static TextBox Entrada(int ancho = 180) => new TextBox
        {
            Width = ancho, Margin = new Padding(0, 4, 14, 4),
            BackColor = Color.FromArgb(250, 248, 245),
            BorderStyle = BorderStyle.FixedSingle
        };

        internal static Button Boton(string texto, bool principal = false)
        {
            var boton = new Button
            {
                Text = texto, AutoSize = true, MinimumSize = new Size(118, 40),
                FlatStyle = FlatStyle.Flat,
                BackColor = principal ? Acento : Superficie,
                ForeColor = principal ? Color.White : Tinta,
                Font = new Font("Segoe UI Semibold", 10F),
                Margin = new Padding(0, 4, 12, 4),
                Cursor = Cursors.Hand
            };
            boton.FlatAppearance.BorderColor = principal ? Acento : EstiloPantallas_33ZS.Borde;
            return boton;
        }

        internal static DataGridView Tabla()
        {
            var tabla = new DataGridView();
            EstiloPantallas_33ZS.Grilla(tabla);
            return tabla;
        }

        internal static FlowLayoutPanel Fila() => new FlowLayoutPanel
        {
            Dock = DockStyle.Top, AutoSize = true, WrapContents = true,
            Padding = new Padding(0, 5, 0, 5)
        };

        internal static void Error(Form owner, System.Exception error) =>
            MessageBox.Show(owner, error.Message, T("No se pudo completar la operación", "Operation failed"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
