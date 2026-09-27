using System;
using System.Drawing;
using System.Windows.Forms;
using BLL;

namespace TpIngSoftware
{
    internal sealed class ConfiguracionInicial33ZS : Form
    {
        private readonly TextBox dni = new TextBox();
        private readonly TextBox nombre = new TextBox();
        private readonly TextBox apellidos = new TextBox();
        private readonly TextBox email = new TextBox();
        private readonly TextBox claveAdmin = new TextBox();
        private readonly TextBox claveDemo = new TextBox();
        private readonly Label error = new Label();

        public ConfiguracionInicial33ZS()
        {
            Text = "Configuración inicial de Harlem";
            EstiloPantallas_33ZS.Formulario(this, 790, 670, 790, 670);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Padding = new Padding(24);

            var tabla = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(28),
                BackColor = EstiloPantallas_33ZS.Superficie,
                ColumnCount = 2,
                RowCount = 11
            };
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 225));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < tabla.RowCount; i++)
                tabla.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(tabla);

            var titulo = new Label
            {
                Text = "Preparar la instalación",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 20F),
                ForeColor = EstiloPantallas_33ZS.Tinta,
                Margin = new Padding(0, 0, 0, 14)
            };
            tabla.Controls.Add(titulo, 0, 0);
            tabla.SetColumnSpan(titulo, 2);

            var descripcion = new Label
            {
                Text = "Creá el administrador para esta instalación. También se crearán dos " +
                       "cuentas de ejemplo: una Recepcionista y un Barbero. Elegí sus contraseñas antes de continuar.",
                AutoSize = true,
                MaximumSize = new Size(670, 0),
                ForeColor = EstiloPantallas_33ZS.Secundario,
                Margin = new Padding(0, 0, 0, 14)
            };
            tabla.Controls.Add(descripcion, 0, 1);
            tabla.SetColumnSpan(descripcion, 2);

            AgregarCampo_33ZS(tabla, "DNI del administrador", dni, 2);
            AgregarCampo_33ZS(tabla, "Nombre", nombre, 3);
            AgregarCampo_33ZS(tabla, "Apellido", apellidos, 4);
            AgregarCampo_33ZS(tabla, "Email / usuario", email, 5);
            AgregarCampo_33ZS(tabla, "Clave del administrador", claveAdmin, 6);
            AgregarCampo_33ZS(tabla, "Clave de las cuentas demo", claveDemo, 7);
            claveAdmin.UseSystemPasswordChar = true;
            claveDemo.UseSystemPasswordChar = true;

            var cuentasDemo = new Label
            {
                Text = "Cuentas de ejemplo: " + AdministradorInicial_33ZS.EmailDemoUno_33ZS +
                       " (Recepcionista) y " + AdministradorInicial_33ZS.EmailDemoDos_33ZS +
                       " (Barbero). Ambas usarán la clave demo que ingreses. Cada clave debe tener de 12 a 50 caracteres.",
                AutoSize = true,
                MaximumSize = new Size(670, 0),
                ForeColor = EstiloPantallas_33ZS.Secundario,
                Margin = new Padding(0, 10, 0, 10)
            };
            tabla.Controls.Add(cuentasDemo, 0, 8);
            tabla.SetColumnSpan(cuentasDemo, 2);

            error.ForeColor = Color.FromArgb(155, 57, 46);
            error.AutoSize = true;
            error.MaximumSize = new Size(670, 0);
            tabla.Controls.Add(error, 0, 9);
            tabla.SetColumnSpan(error, 2);

            var botones = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.RightToLeft,
                Dock = DockStyle.Fill
            };
            var crear = new Button { Text = "Crear cuentas", Width = 150, Height = 42 };
            EstiloPantallas_33ZS.Boton(crear, true);
            crear.Click += Crear_Click_33ZS;
            var cancelar = new Button { Text = "Cancelar", Width = 110, Height = 42 };
            EstiloPantallas_33ZS.Boton(cancelar);
            cancelar.Click += (sender, args) => { DialogResult = DialogResult.Cancel; Close(); };
            botones.Controls.Add(crear);
            botones.Controls.Add(cancelar);
            tabla.Controls.Add(botones, 0, 10);
            tabla.SetColumnSpan(botones, 2);
            AcceptButton = crear;
            CancelButton = cancelar;
            ResumeLayout(true);
        }

        private static void AgregarCampo_33ZS(TableLayoutPanel tabla, string texto,
            TextBox campo, int fila)
        {
            tabla.Controls.Add(new Label
            {
                Text = texto,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                ForeColor = EstiloPantallas_33ZS.Tinta,
                Margin = new Padding(0, 7, 10, 7)
            }, 0, fila);
            EstiloPantallas_33ZS.Entrada(campo);
            campo.Margin = new Padding(0, 4, 0, 4);
            tabla.Controls.Add(campo, 1, fila);
        }

        private void Crear_Click_33ZS(object sender, EventArgs args)
        {
            error.Text = string.Empty;
            try
            {
                AdministradorInicial_33ZS.CrearCuentas_33ZS(
                    dni.Text, nombre.Text, apellidos.Text, email.Text,
                    claveAdmin.Text, claveDemo.Text);
                MessageBox.Show(this,
                    "Se crearon el administrador y las dos cuentas de ejemplo. " +
                    "Usá el email y las contraseñas que acabás de elegir para iniciar sesión.",
                    "Instalación completada", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                error.Text = ex.Message;
            }
        }
    }
}
