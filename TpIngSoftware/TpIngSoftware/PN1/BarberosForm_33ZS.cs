using System;
using System.Windows.Forms;
using BE.PN1;
using BLL.PN1;

namespace TpIngSoftware.PN1
{
    public sealed class BarberosForm_33ZS : Form
    {
        private readonly PN1BLL_33ZS negocio = new PN1BLL_33ZS();
        private readonly ComboBox usuarios = new ComboBox { Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly NumericUpDown porcentaje = new NumericUpDown
        {
            Minimum = 0, Maximum = 100, DecimalPlaces = 2, Increment = 0.5M, Width = 90
        };
        private readonly CheckBox activo = new CheckBox { Text = EstiloPN1_33ZS.T("Activo", "Active"), Checked = true, AutoSize = true };
        private readonly DataGridView tabla = EstiloPN1_33ZS.Tabla();

        public BarberosForm_33ZS()
        {
            EstiloPN1_33ZS.Preparar(this, EstiloPN1_33ZS.T("Barberos", "Barbers"));
            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(24),
                ColumnCount = 1, RowCount = 4 };
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(panel);
            panel.Controls.Add(EstiloPN1_33ZS.Titulo(EstiloPN1_33ZS.T("Barberos y comisiones", "Barbers and commissions")), 0, 0);
            panel.Controls.Add(new Label
            {
                Text = EstiloPN1_33ZS.T(
                    "Primero creá el usuario con rol Barbero en Usuarios. Luego configurá su comisión aquí.",
                    "First create a user with the Barber role in Users. Then set their commission here."),
                Dock = DockStyle.Fill, AutoSize = true
            }, 0, 1);
            panel.Controls.Add(tabla, 0, 2);

            var fila = EstiloPN1_33ZS.Fila();
            fila.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Barbero", "Barber")));
            fila.Controls.Add(usuarios);
            fila.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Comisión %", "Commission %")));
            fila.Controls.Add(porcentaje);
            fila.Controls.Add(activo);
            var guardar = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Guardar comisión", "Save commission"), true);
            guardar.Click += (s, e) => Guardar();
            fila.Controls.Add(guardar);
            panel.Controls.Add(fila, 0, 3);
            usuarios.SelectedIndexChanged += (s, e) => SeleccionCambio();
            Shown += (s, e) => Cargar();
        }

        private void Cargar()
        {
            try
            {
                var barberos = negocio.ListarBarberos();
                usuarios.DataSource = barberos;
                usuarios.DisplayMember = "Descripcion";
                tabla.DataSource = barberos;
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void SeleccionCambio()
        {
            var barbero = usuarios.SelectedItem as Barbero_33ZS;
            if (barbero == null) return;
            porcentaje.Value = barbero.PorcentajeComision ?? 0;
            activo.Checked = barbero.PerfilActivo || !barbero.PorcentajeComision.HasValue;
        }

        private void Guardar()
        {
            try
            {
                var barbero = usuarios.SelectedItem as Barbero_33ZS;
                if (barbero == null) throw new InvalidOperationException("Seleccioná un usuario Barbero.");
                negocio.ConfigurarBarbero(barbero.Dni, porcentaje.Value, activo.Checked);
                Cargar();
                MessageBox.Show(this, EstiloPN1_33ZS.T("Comisión guardada.", "Commission saved."), "Harlem");
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }
    }
}
