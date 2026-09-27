using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BE.PN1;
using BLL.PN1;

namespace TpIngSoftware.PN1
{
    public sealed class HistorialForm_33ZS : Form
    {
        private readonly PN1BLL_33ZS negocio = new PN1BLL_33ZS();
        private readonly bool general;
        private readonly DateTimePicker desde = new DateTimePicker { Width = 140, Format = DateTimePickerFormat.Short };
        private readonly DateTimePicker hasta = new DateTimePicker { Width = 140, Format = DateTimePickerFormat.Short };
        private readonly ComboBox barberos = Filtro();
        private readonly ComboBox servicios = Filtro();
        private readonly ComboBox medios = Filtro();
        private readonly DataGridView tabla = EstiloPN1_33ZS.Tabla();
        private readonly Label resumen = new Label { AutoSize = true };
        private readonly Button consultar = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Consultar", "Search"), true);

        private static ComboBox Filtro() => new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList, Width = 165
        };

        public HistorialForm_33ZS(bool reporteGeneral)
        {
            general = reporteGeneral;
            EstiloPN1_33ZS.Preparar(this, general
                ? EstiloPN1_33ZS.T("Atenciones", "Services")
                : EstiloPN1_33ZS.T("Mi historial", "My history"), 1050, 700);
            desde.Value = DateTime.Today.AddMonths(-1);
            hasta.Value = DateTime.Today;
            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(24),
                ColumnCount = 1, RowCount = 4 };
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(panel);
            panel.Controls.Add(EstiloPN1_33ZS.Titulo(general
                ? EstiloPN1_33ZS.T("Atenciones registradas", "Recorded services")
                : EstiloPN1_33ZS.T("Mis atenciones", "My services")), 0, 0);
            var filtros = EstiloPN1_33ZS.Fila();
            filtros.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Desde", "From")));
            filtros.Controls.Add(desde);
            filtros.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Hasta", "To")));
            filtros.Controls.Add(hasta);
            if (general)
            {
                filtros.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Barbero", "Barber")));
                filtros.Controls.Add(barberos);
                filtros.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Servicio", "Service")));
                filtros.Controls.Add(servicios);
                filtros.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Pago", "Payment")));
                filtros.Controls.Add(medios);
            }
            consultar.Click += async (s, e) => await Consultar();
            filtros.Controls.Add(consultar);
            panel.Controls.Add(filtros, 0, 1);
            panel.Controls.Add(tabla, 0, 2);
            panel.Controls.Add(resumen, 0, 3);
            Shown += async (s, e) =>
            {
                if (general) CargarFiltros();
                await Consultar();
            };
        }

        private void CargarFiltros()
        {
            try
            {
                var listaBarberos = negocio.ListarBarberos();
                listaBarberos.Insert(0, new Barbero_33ZS { Dni = null, Nombre = EstiloPN1_33ZS.T("Todos", "All"), Apellido = "" });
                barberos.DataSource = listaBarberos;
                barberos.DisplayMember = "Descripcion";
                var listaServicios = negocio.ListarServicios();
                listaServicios.Insert(0, new Servicio_33ZS { Id = 0, Nombre = EstiloPN1_33ZS.T("Todos", "All") });
                servicios.DataSource = listaServicios;
                servicios.DisplayMember = "Nombre";
                var listaMedios = negocio.ListarMediosPago();
                listaMedios.Insert(0, new MedioPago_33ZS { Id = 0, Nombre = EstiloPN1_33ZS.T("Todos", "All") });
                medios.DataSource = listaMedios;
                medios.DisplayMember = "Nombre";
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private async Task Consultar()
        {
            if (hasta.Value.Date < desde.Value.Date)
            {
                MessageBox.Show(this, EstiloPN1_33ZS.T("La fecha final debe ser igual o posterior a la inicial.",
                    "The end date must be on or after the start date."), "Harlem");
                return;
            }
            consultar.Enabled = false;
            try
            {
                DateTime fechaDesde = desde.Value.Date, fechaHasta = hasta.Value.Date;
                var barbero = barberos.SelectedItem as Barbero_33ZS;
                var servicio = servicios.SelectedItem as Servicio_33ZS;
                var medio = medios.SelectedItem as MedioPago_33ZS;
                string dni = barbero == null ? null : barbero.Dni;
                int? servicioId = servicio == null || servicio.Id == 0 ? (int?)null : servicio.Id;
                int? medioId = medio == null || medio.Id == 0 ? (int?)null : medio.Id;
                // La lectura se hace en un hilo de trabajo; los controles se actualizan al volver al hilo de UI.
                List<Atencion_33ZS> datos = await Task.Run(() => general
                    ? negocio.ConsultarReporte(fechaDesde, fechaHasta, dni, servicioId, medioId)
                    : negocio.ConsultarPropias(fechaDesde, fechaHasta));
                tabla.DataSource = datos;
                if (!general)
                {
                    tabla.Columns[nameof(Atencion_33ZS.Cliente)].Visible = false;
                    tabla.Columns[nameof(Atencion_33ZS.Barbero)].Visible = false;
                    tabla.Columns[nameof(Atencion_33ZS.MedioPago)].Visible = false;
                    tabla.Columns[nameof(Atencion_33ZS.Importe)].Visible = false;
                }
                resumen.Text = general
                    ? string.Format(EstiloPN1_33ZS.T("{0} atenciones · Cobrado: ${1:N2} · Comisiones: ${2:N2}",
                        "{0} services · Collected: ${1:N2} · Commissions: ${2:N2}"),
                        datos.Count, datos.Sum(a => a.Importe), datos.Sum(a => a.Comision))
                    : string.Format(EstiloPN1_33ZS.T("{0} atenciones · Mi comisión: ${1:N2}",
                        "{0} services · My commission: ${1:N2}"),
                        datos.Count, datos.Sum(a => a.Comision));
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
            finally { consultar.Enabled = true; }
        }
    }
}
