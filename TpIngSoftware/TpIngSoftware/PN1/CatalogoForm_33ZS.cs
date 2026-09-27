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
        private readonly NumericUpDown precio = Numero();
        private readonly NumericUpDown stock = Numero();
        private readonly NumericUpDown minimo = Numero();
        private readonly CheckBox servicioActivo = new CheckBox { Text = EstiloPN1_33ZS.T("Disponible", "Available"), Checked = true, AutoSize = true };
        private readonly TextBox motivo = EstiloPN1_33ZS.Entrada(260);
        private readonly Label consumo = new Label { AutoSize = true };

        private static NumericUpDown Numero() => new NumericUpDown
        {
            Width = 110, DecimalPlaces = 2, Maximum = 1000000, ThousandsSeparator = true
        };

        public CatalogoForm_33ZS()
        {
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
            filaServicio.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Precio", "Price")));
            filaServicio.Controls.Add(precio);
            filaServicio.Controls.Add(servicioActivo);
            var guardarPrecio = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Guardar servicio", "Save service"), true);
            guardarPrecio.Click += (s, e) => GuardarServicio();
            filaServicio.Controls.Add(guardarPrecio);
            ps.Controls.Add(filaServicio, 0, 2);
            pestanas.TabPages.Add(paginaServicios);

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
            pestanas.TabPages.Add(paginaInsumos);

            servicios.SelectionChanged += (s, e) => ServicioSeleccionado();
            insumos.SelectionChanged += (s, e) => InsumoSeleccionado();
            Shown += (s, e) => Cargar();
        }

        private void Cargar()
        {
            try
            {
                servicios.DataSource = negocio.ListarServicios();
                insumos.DataSource = negocio.ListarInsumos();
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void ServicioSeleccionado()
        {
            var s = servicios.CurrentRow?.DataBoundItem as Servicio_33ZS;
            if (s == null) return;
            precio.Value = s.Precio ?? 0;
            servicioActivo.Checked = s.Activo;
            try
            {
                consumo.Text = EstiloPN1_33ZS.T("Consumo: ", "Usage: ") + string.Join(" · ",
                    negocio.ListarConsumos(s.Id).Select(c => c.Insumo + " " + c.Cantidad.ToString("N2")));
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

        private void GuardarServicio()
        {
            try
            {
                var s = servicios.CurrentRow?.DataBoundItem as Servicio_33ZS;
                if (s == null) throw new InvalidOperationException("Seleccioná un servicio.");
                negocio.ActualizarPrecio(s.Id, precio.Value, servicioActivo.Checked);
                Cargar();
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
