using System;
using System.Linq;
using System.Windows.Forms;
using BE.PN1;
using BLL.PN1;

namespace TpIngSoftware.PN1
{
    public sealed class NuevaAtencionForm_33ZS : Form
    {
        private readonly PN1BLL_33ZS negocio = new PN1BLL_33ZS();
        private readonly Label clienteTexto = new Label { AutoSize = true, Text = EstiloPN1_33ZS.T("Ningún cliente seleccionado", "No customer selected") };
        private readonly Label precioTexto = new Label { AutoSize = true };
        private readonly Label consumoTexto = new Label { AutoSize = true };
        private readonly ComboBox barberos = Lista();
        private readonly ComboBox servicios = Lista();
        private readonly ComboBox medios = Lista();
        private readonly NumericUpDown importe = new NumericUpDown
        {
            Width = 150, DecimalPlaces = 2, Maximum = 1000000, ThousandsSeparator = true
        };
        private readonly Button guardar = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Registrar atención y cobro", "Record service and payment"), true);
        private Cliente_33ZS cliente;
        private Guid operacionId = Guid.NewGuid();

        private static ComboBox Lista() => new ComboBox
        {
            Width = 290, DropDownStyle = ComboBoxStyle.DropDownList
        };

        public NuevaAtencionForm_33ZS()
        {
            EstiloPN1_33ZS.Preparar(this, EstiloPN1_33ZS.T("Nueva atención", "New service"), 800, 540);
            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(30),
                ColumnCount = 1, RowCount = 7 };
            for (int i = 0; i < 7; i++) panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(panel);
            panel.Controls.Add(EstiloPN1_33ZS.Titulo(EstiloPN1_33ZS.T("Registrar atención", "Record a service")), 0, 0);
            panel.Controls.Add(new Label { AutoSize = true,
                Text = EstiloPN1_33ZS.T("Elegí el cliente, el barbero y el servicio realizado. El cobro se registra en la misma operación.",
                    "Choose the customer, barber and service. Payment is recorded in the same operation.") }, 0, 1);

            var filaCliente = EstiloPN1_33ZS.Fila();
            var buscar = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Buscar o registrar cliente", "Find or register customer"));
            buscar.Click += (s, e) => BuscarCliente();
            filaCliente.Controls.Add(buscar);
            filaCliente.Controls.Add(clienteTexto);
            panel.Controls.Add(filaCliente, 0, 2);

            var filaTrabajo = EstiloPN1_33ZS.Fila();
            filaTrabajo.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Barbero", "Barber")));
            filaTrabajo.Controls.Add(barberos);
            filaTrabajo.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Servicio", "Service")));
            filaTrabajo.Controls.Add(servicios);
            panel.Controls.Add(filaTrabajo, 0, 3);
            panel.Controls.Add(precioTexto, 0, 4);
            panel.Controls.Add(consumoTexto, 0, 5);

            var filaCobro = EstiloPN1_33ZS.Fila();
            filaCobro.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Medio de pago", "Payment method")));
            filaCobro.Controls.Add(medios);
            filaCobro.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Importe cobrado", "Amount paid")));
            filaCobro.Controls.Add(importe);
            guardar.Click += (s, e) => Registrar();
            filaCobro.Controls.Add(guardar);
            panel.Controls.Add(filaCobro, 0, 6);
            servicios.SelectedIndexChanged += (s, e) => ServicioSeleccionado();
            Shown += (s, e) => Cargar();
        }

        private void Cargar()
        {
            try
            {
                barberos.DataSource = negocio.ListarBarberos()
                    .Where(b => b.UsuarioActivo && b.PerfilActivo && b.PorcentajeComision.HasValue).ToList();
                barberos.DisplayMember = "Descripcion";
                servicios.DataSource = negocio.ListarServicios()
                    .Where(s => s.Activo && s.Precio.HasValue).ToList();
                servicios.DisplayMember = "Descripcion";
                medios.DataSource = negocio.ListarMediosPago();
                medios.DisplayMember = "Nombre";
                ServicioSeleccionado();
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void BuscarCliente()
        {
            using (var ventana = new ClientesForm_33ZS(true))
                if (ventana.ShowDialog(this) == DialogResult.OK)
                {
                    cliente = ventana.ClienteSeleccionado;
                    clienteTexto.Text = cliente.Descripcion;
                }
        }

        private void ServicioSeleccionado()
        {
            var servicio = servicios.SelectedItem as Servicio_33ZS;
            if (servicio == null) return;
            importe.Value = servicio.Precio ?? 0;
            precioTexto.Text = EstiloPN1_33ZS.T("Precio vigente: $", "Current price: $") + servicio.Precio.Value.ToString("N2");
            try
            {
                var consumos = negocio.ListarConsumos(servicio.Id);
                consumoTexto.Text = consumos.Count == 0
                    ? EstiloPN1_33ZS.T("Sin insumos asociados", "No supplies configured")
                    : EstiloPN1_33ZS.T("Consumo previsto: ", "Expected usage: ") + string.Join(" · ", consumos.Select(c =>
                        c.Insumo + " " + c.Cantidad.ToString("N2") + " (stock " + c.Stock.ToString("N2") + ")"));
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void Registrar()
        {
            try
            {
                var barbero = barberos.SelectedItem as Barbero_33ZS;
                var servicio = servicios.SelectedItem as Servicio_33ZS;
                var medio = medios.SelectedItem as MedioPago_33ZS;
                if (cliente == null || barbero == null || servicio == null || medio == null)
                    throw new InvalidOperationException("Seleccioná cliente, barbero, servicio y medio de pago.");
                guardar.Enabled = false;
                int id = negocio.RegistrarAtencion(operacionId, cliente.Id, barbero.Dni,
                    servicio.Id, medio.Id, importe.Value);
                MessageBox.Show(this, EstiloPN1_33ZS.T("Atención y cobro registrados. N.º ", "Service and payment recorded. No. ") + id,
                    "Harlem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                operacionId = Guid.NewGuid();
                cliente = null;
                clienteTexto.Text = EstiloPN1_33ZS.T("Ningún cliente seleccionado", "No customer selected");
                Cargar();
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
            finally { guardar.Enabled = true; }
        }
    }
}
