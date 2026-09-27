using System.Drawing;
using System.Windows.Forms;

namespace TpIngSoftware
{
    internal static class EstiloPantallas_33ZS
    {
        internal static readonly Color Fondo = Color.FromArgb(246, 243, 238);
        internal static readonly Color Tinta = Color.FromArgb(35, 40, 42);
        internal static readonly Color Acento = Color.FromArgb(127, 79, 50);
        internal static readonly Color Secundario = Color.FromArgb(99, 103, 104);
        internal static readonly Color Borde = Color.FromArgb(223, 218, 211);
        internal static readonly Color Superficie = Color.White;

        internal static void Formulario(Form formulario, int ancho, int alto, int minimoAncho, int minimoAlto)
        {
            formulario.SuspendLayout();
            formulario.Size = new Size(ancho, alto);
            formulario.MinimumSize = new Size(minimoAncho, minimoAlto);
            formulario.StartPosition = FormStartPosition.CenterParent;
            formulario.AutoScaleMode = AutoScaleMode.Dpi;
            formulario.BackColor = Fondo;
            formulario.ForeColor = Tinta;
            formulario.Font = new Font("Segoe UI", 10F);
        }

        internal static TableLayoutPanel Raiz(int margen = 24)
        {
            return new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(margen),
                BackColor = Fondo,
                ColumnCount = 1,
                RowCount = 1
            };
        }

        internal static TableLayoutPanel Tarjeta(int margen = 20)
        {
            return new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(margen),
                BackColor = Superficie,
                ColumnCount = 1,
                RowCount = 1,
                Margin = new Padding(0)
            };
        }

        internal static void Titulo(Control control, int puntos = 20)
        {
            control.Font = new Font("Segoe UI Semibold", puntos);
            control.ForeColor = Tinta;
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0);
        }

        internal static void Etiqueta(Control control)
        {
            control.Font = new Font("Segoe UI", 10F);
            control.ForeColor = Tinta;
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0, 0, 8, 0);
        }

        internal static void Entrada(Control control)
        {
            control.Font = new Font("Segoe UI", 10F);
            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0, 4, 0, 8);
            if (control is TextBox texto)
            {
                texto.BorderStyle = BorderStyle.FixedSingle;
                texto.BackColor = Color.FromArgb(250, 248, 245);
            }
        }

        internal static void Boton(Button boton, bool principal = false)
        {
            boton.Font = new Font("Segoe UI Semibold", 10F);
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderColor = Borde;
            boton.BackColor = principal ? Acento : Superficie;
            boton.ForeColor = principal ? Color.White : Tinta;
            boton.UseVisualStyleBackColor = false;
            boton.Cursor = Cursors.Hand;
            boton.MinimumSize = new Size(104, 40);
            boton.Margin = new Padding(0, 0, 10, 0);
        }

        internal static void Boton(ReaLTaiizor.Controls.Button boton, bool principal = false)
        {
            boton.Font = new Font("Segoe UI Semibold", 10F);
            boton.ForeColor = principal ? Color.White : Tinta;
            boton.BackColor = Color.Transparent;
            boton.InactiveColor = principal ? Acento : Superficie;
            boton.EnteredColor = principal ? Color.FromArgb(103, 62, 39) : Fondo;
            boton.PressedColor = principal ? Color.FromArgb(82, 48, 29) : Borde;
            boton.BorderColor = principal ? Acento : Borde;
            boton.EnteredBorderColor = principal ? Acento : Borde;
            boton.PressedBorderColor = principal ? Acento : Borde;
            boton.Cursor = Cursors.Hand;
            boton.MinimumSize = new Size(104, 40);
            boton.Margin = new Padding(0, 0, 10, 0);
        }

        internal static void Grilla(DataGridView tabla)
        {
            tabla.Dock = DockStyle.Fill;
            tabla.ReadOnly = true;
            tabla.AllowUserToAddRows = false;
            tabla.AllowUserToDeleteRows = false;
            tabla.RowHeadersVisible = false;
            tabla.BackgroundColor = Superficie;
            tabla.BorderStyle = BorderStyle.None;
            tabla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabla.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tabla.MultiSelect = false;
            tabla.EnableHeadersVisualStyles = false;
            tabla.ColumnHeadersDefaultCellStyle.BackColor = Fondo;
            tabla.ColumnHeadersDefaultCellStyle.ForeColor = Tinta;
            tabla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F);
            tabla.DefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 224, 213);
            tabla.DefaultCellStyle.SelectionForeColor = Tinta;
            tabla.ColumnHeadersHeight = 36;
            tabla.RowTemplate.Height = 32;
            tabla.GridColor = Borde;
        }

        internal static FlowLayoutPanel Acciones(bool derecha = false)
        {
            return new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = derecha ? FlowDirection.RightToLeft : FlowDirection.LeftToRight,
                WrapContents = true,
                Padding = new Padding(0, 8, 0, 0),
                BackColor = Superficie
            };
        }
    }
}
