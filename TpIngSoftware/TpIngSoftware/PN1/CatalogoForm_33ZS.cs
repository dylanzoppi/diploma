using System;
using System.Linq;
using System.Windows.Forms;
using BE.PN1;
using BLL.PN1;

namespace TpIngSoftware.PN1
{
    public sealed class CatalogoForm_33ZS : Form
    {
        private readonly PN1BLL_33ZS negocio = new PN1BLL_33ZS();
        private readonly DataGridView servicios = EstiloPN1_33ZS.Tabla();
        private readonly DataGridView insumos = EstiloPN1_33ZS.Tabla();
        private readonly NumericUpDown stock = Numero();
        private readonly NumericUpDown minimo = Numero();
        private readonly TextBox motivo = EstiloPN1_33ZS.Entrada(260);
        private readonly Label consumo = new Label { AutoSize = true };
        private readonly bool puedeVerCatalogo;
        private readonly bool puedeGestionarCatalogo;
        private readonly bool puedeGestionarStock;

        private static NumericUpDown Numero() => new NumericUpDown
        {
            Width = 110, DecimalPlaces = 2, Maximum = 1000000, ThousandsSeparator = true
        };

        public CatalogoForm_33ZS()
        {
            puedeGestionarCatalogo = negocio.TienePermiso("GestionarCatalogo");
            puedeVerCatalogo = puedeGestionarCatalogo || negocio.TienePermiso("ConsultarCatalogo");
            puedeGestionarStock = negocio.TienePermiso("GestionarStock");
            EstiloPN1_33ZS.Preparar(this, EstiloPN1_33ZS.T("Catálogo y stock", "Services and stock"));
            var raiz = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(24),
                ColumnCount = 1, RowCount = 2 };
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(raiz);
            raiz.Controls.Add(EstiloPN1_33ZS.Titulo(EstiloPN1_33ZS.T("Servicios e insumos", "Services and supplies")), 0, 0);
            var pestanas = new TabControl { Dock = DockStyle.Fill };
            raiz.Controls.Add(pestanas, 0, 1);

            var paginaServicios = new TabPage(EstiloPN1_33ZS.T("Servicios", "Services"));
            var ps = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(14),
                ColumnCount = 1, RowCount = 3 };
            ps.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            ps.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            ps.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            paginaServicios.Controls.Add(ps);
            ps.Controls.Add(servicios, 0, 0);
            ps.Controls.Add(consumo, 0, 1);
            var filaServicio = EstiloPN1_33ZS.Fila();
            var nuevoServicio = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Nuevo servicio", "New service"), true);
            nuevoServicio.Visible = puedeGestionarCatalogo;
            nuevoServicio.Click += (s, e) => EditarServicio(true);
            filaServicio.Controls.Add(nuevoServicio);
            var editarServicio = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Editar precio e insumos", "Edit price and supplies"));
            editarServicio.Visible = puedeGestionarCatalogo;
            editarServicio.Click += (s, e) => EditarServicio(false);
            filaServicio.Controls.Add(editarServicio);
            ps.Controls.Add(filaServicio, 0, 2);
            if (puedeVerCatalogo) pestanas.TabPages.Add(paginaServicios);

            var paginaInsumos = new TabPage(EstiloPN1_33ZS.T("Stock", "Stock"));
            var pi = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(14),
                ColumnCount = 1, RowCount = 2 };
            pi.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            pi.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            paginaInsumos.Controls.Add(pi);
            pi.Controls.Add(insumos, 0, 0);
            var filaInsumo = EstiloPN1_33ZS.Fila();
            filaInsumo.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Stock actual", "Current stock")));
            filaInsumo.Controls.Add(stock);
            filaInsumo.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Mínimo", "Minimum")));
            filaInsumo.Controls.Add(minimo);
            filaInsumo.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Motivo", "Reason")));
            filaInsumo.Controls.Add(motivo);
            var guardarStock = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Registrar ajuste", "Record adjustment"), true);
            guardarStock.Click += (s, e) => GuardarStock();
            filaInsumo.Controls.Add(guardarStock);
            pi.Controls.Add(filaInsumo, 0, 1);
            if (puedeGestionarStock) pestanas.TabPages.Add(paginaInsumos);

            servicios.SelectionChanged += (s, e) => ServicioSeleccionado();
            insumos.SelectionChanged += (s, e) => InsumoSeleccionado();
            Shown += (s, e) => Cargar();
        }

        private void Cargar(int? seleccionarServicioId = null)
        {
            try
            {
                if (puedeVerCatalogo) servicios.DataSource = negocio.ListarServicios();
                if (puedeGestionarStock) insumos.DataSource = negocio.ListarInsumos();
                if (puedeVerCatalogo && servicios.Columns[nameof(Servicio_33ZS.Id)] != null)
                    servicios.Columns[nameof(Servicio_33ZS.Id)].Visible = false;
                if (puedeVerCatalogo && servicios.Columns[nameof(Servicio_33ZS.Descripcion)] != null)
                    servicios.Columns[nameof(Servicio_33ZS.Descripcion)].Visible = false;
                if (puedeVerCatalogo && servicios.Columns[nameof(Servicio_33ZS.Activo)] != null)
                    servicios.Columns[nameof(Servicio_33ZS.Activo)].HeaderText =
                        EstiloPN1_33ZS.T("Disponible", "Available");
                if (puedeVerCatalogo && seleccionarServicioId.HasValue)
                    foreach (DataGridViewRow fila in servicios.Rows)
                    {
                        var item = fila.DataBoundItem as Servicio_33ZS;
                        if (item != null && item.Id == seleccionarServicioId.Value)
                        {
                            servicios.CurrentCell = fila.Cells[nameof(Servicio_33ZS.Nombre)];
                            break;
                        }
                    }
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void ServicioSeleccionado()
        {
            var s = servicios.CurrentRow?.DataBoundItem as Servicio_33ZS;
            if (s == null) return;
            try
            {
                var consumos = negocio.ListarConsumos(s.Id);
                consumo.Text = consumos.Count == 0
                    ? EstiloPN1_33ZS.T("Sin insumos configurados", "No supplies configured")
                    : EstiloPN1_33ZS.T("Consumo por atención: ", "Usage per service: ") +
                        string.Join(" · ", consumos.Select(c => c.Insumo + " " + c.Cantidad.ToString("N2")));
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void InsumoSeleccionado()
        {
            var i = insumos.CurrentRow?.DataBoundItem as Insumo_33ZS;
            if (i == null) return;
            stock.Value = i.Stock;
            minimo.Value = i.StockMinimo;
        }

        private void EditarServicio(bool nuevo)
        {
            try
            {
                var seleccionado = nuevo ? null : servicios.CurrentRow?.DataBoundItem as Servicio_33ZS;
                if (!nuevo && seleccionado == null)
                    throw new InvalidOperationException("Seleccioná un servicio para editar.");
                using (var editor = new ServicioEditorForm_33ZS(seleccionado))
                    if (editor.ShowDialog(this) == DialogResult.OK)
                        Cargar(editor.ServicioId);
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void GuardarStock()
        {
            try
            {
                var i = insumos.CurrentRow?.DataBoundItem as Insumo_33ZS;
                if (i == null) throw new InvalidOperationException("Seleccioná un insumo.");
                negocio.AjustarStock(i.Id, stock.Value, minimo.Value, motivo.Text);
                motivo.Clear();
                Cargar();
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }
    }
}
