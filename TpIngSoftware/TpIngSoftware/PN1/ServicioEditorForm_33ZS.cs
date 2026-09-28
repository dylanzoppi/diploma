using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BE.PN1;
using BLL.PN1;

namespace TpIngSoftware.PN1
{
    public sealed class ServicioEditorForm_33ZS : Form
    {
        private readonly PN1BLL_33ZS negocio = new PN1BLL_33ZS();
        private readonly Servicio_33ZS servicio;
        private readonly TextBox nombre = EstiloPN1_33ZS.Entrada(300);
        private readonly NumericUpDown precio = new NumericUpDown
        {
            Width = 150, DecimalPlaces = 2, Maximum = 9999999999.99m,
            ThousandsSeparator = true
        };
        private readonly CheckBox activo = new CheckBox
        {
            Checked = true, AutoSize = true
        };
        private readonly TableLayoutPanel lista = new TableLayoutPanel
        {
            Dock = DockStyle.Top, AutoSize = true, ColumnCount = 3,
            Padding = new Padding(8), BackColor = Color.White
        };
        private readonly Button guardar = EstiloPN1_33ZS.Boton(
            EstiloPN1_33ZS.T("Guardar servicio", "Save service"), true);
        private readonly Dictionary<int, NumericUpDown> cantidades =
            new Dictionary<int, NumericUpDown>();

        public int ServicioId { get; private set; }

        public ServicioEditorForm_33ZS(Servicio_33ZS servicioExistente = null)
        {
            servicio = servicioExistente;
            EstiloPN1_33ZS.Preparar(this, EstiloPN1_33ZS.T(
                servicio == null ? "Nuevo servicio" : "Editar servicio",
                servicio == null ? "New service" : "Edit service"), 800, 630);

            var raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill, Padding = new Padding(24),
                ColumnCount = 1, RowCount = 5
            };
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            raiz.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(raiz);

            raiz.Controls.Add(EstiloPN1_33ZS.Titulo(EstiloPN1_33ZS.T(
                servicio == null ? "Crear servicio" : "Editar servicio",
                servicio == null ? "Create service" : "Edit service")), 0, 0);
            raiz.Controls.Add(new Label
            {
                AutoSize = true,
                Text = EstiloPN1_33ZS.T(
                    "Definí el precio y cuántas unidades de cada insumo se descuentan por atención. Cantidad 0 significa que no se usa.",
                    "Set the price and supply quantity used per service. A quantity of 0 means it is not used."),
                ForeColor = EstiloPantallas_33ZS.Secundario,
                Margin = new Padding(0, 0, 0, 14)
            }, 0, 1);

            var datos = EstiloPN1_33ZS.Fila();
            datos.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Nombre", "Name")));
            datos.Controls.Add(nombre);
            datos.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Precio", "Price")));
            datos.Controls.Add(precio);
            activo.Text = EstiloPN1_33ZS.T("Disponible", "Available");
            datos.Controls.Add(activo);
            raiz.Controls.Add(datos, 0, 2);

            var contenedor = new Panel { Dock = DockStyle.Fill, AutoScroll = true,
                BackColor = Color.White, Padding = new Padding(8) };
            contenedor.Controls.Add(lista);
            raiz.Controls.Add(contenedor, 0, 3);

            var acciones = EstiloPN1_33ZS.Fila();
            var cancelar = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Cancelar", "Cancel"));
            cancelar.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            guardar.Enabled = false;
            guardar.Click += (s, e) => Guardar();
            acciones.Controls.Add(guardar);
            acciones.Controls.Add(cancelar);
            raiz.Controls.Add(acciones, 0, 4);
            AcceptButton = guardar;
            CancelButton = cancelar;

            if (servicio != null)
            {
                nombre.Text = servicio.Nombre;
                precio.Value = servicio.Precio ?? 0;
                activo.Checked = servicio.Activo;
            }
            Shown += (s, e) => CargarInsumos();
        }

        private void CargarInsumos()
        {
            try
            {
                var disponibles = negocio.ListarInsumos();
                var configurados = servicio == null
                    ? new Dictionary<int, decimal>()
                    : negocio.ListarConsumos(servicio.Id).ToDictionary(c => c.InsumoId, c => c.Cantidad);

                lista.Controls.Clear();
                lista.RowStyles.Clear();
                cantidades.Clear();
                lista.ColumnStyles.Clear();
                lista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48));
                lista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27));
                lista.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                lista.RowCount = disponibles.Count + 1;
                lista.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
                lista.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Insumo", "Supply")), 0, 0);
                lista.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Stock actual", "Current stock")), 1, 0);
                lista.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Por atención", "Per service")), 2, 0);

                for (int indice = 0; indice < disponibles.Count; indice++)
                {
                    Insumo_33ZS insumo = disponibles[indice];
                    int fila = indice + 1;
                    lista.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
                    var etiqueta = EstiloPN1_33ZS.Etiqueta(insumo.Nombre +
                        (insumo.Activo ? "" : EstiloPN1_33ZS.T(" (inactivo)", " (inactive)")));
                    if (!insumo.Activo) etiqueta.ForeColor = EstiloPantallas_33ZS.Advertencia;
                    lista.Controls.Add(etiqueta, 0, fila);
                    lista.Controls.Add(EstiloPN1_33ZS.Etiqueta(insumo.Stock.ToString("N2")), 1, fila);
                    var cantidad = new NumericUpDown
                    {
                        DecimalPlaces = 2, Maximum = 9999999999.99m,
                        ThousandsSeparator = true, Dock = DockStyle.Fill,
                        Margin = new Padding(0, 4, 16, 4),
                        Value = configurados.TryGetValue(insumo.Id, out decimal valor) ? valor : 0
                    };
                    lista.Controls.Add(cantidad, 2, fila);
                    cantidades.Add(insumo.Id, cantidad);
                }
                guardar.Enabled = disponibles.Count > 0;
                if (disponibles.Count == 0)
                    MessageBox.Show(this, EstiloPN1_33ZS.T(
                        "No hay insumos cargados para configurar un servicio.",
                        "No supplies are available to configure a service."), "Harlem");
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void Guardar()
        {
            guardar.Enabled = false;
            try
            {
                var consumos = cantidades.Where(par => par.Value.Value > 0)
                    .Select(par => new ConsumoServicio_33ZS
                    {
                        InsumoId = par.Key,
                        Cantidad = par.Value.Value
                    }).ToList();
                ServicioId = negocio.GuardarServicio(servicio?.Id, nombre.Text,
                    precio.Value, activo.Checked, consumos);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                EstiloPN1_33ZS.Error(this, ex);
                guardar.Enabled = true;
            }
        }
    }
}
