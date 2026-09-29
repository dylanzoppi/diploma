using System;
using System.Linq;
using System.Windows.Forms;
using BE.PN1;
using BLL.PN1;

namespace TpIngSoftware.PN1
{
    public sealed class ClientesForm_33ZS : Form
    {
        private readonly PN1BLL_33ZS negocio = new PN1BLL_33ZS();
        private readonly TextBox buscar = EstiloPN1_33ZS.Entrada(270);
        private readonly TextBox nombre = EstiloPN1_33ZS.Entrada();
        private readonly TextBox apellido = EstiloPN1_33ZS.Entrada();
        private readonly TextBox telefono = EstiloPN1_33ZS.Entrada();
        private readonly TextBox correo = EstiloPN1_33ZS.Entrada(220);
        private readonly DataGridView tabla = EstiloPN1_33ZS.Tabla();
        private readonly Button seleccionar = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Usar cliente", "Use customer"), true);
        private readonly bool modoSeleccion;

        public Cliente_33ZS ClienteSeleccionado { get; private set; }

        public ClientesForm_33ZS(bool seleccionarCliente = false)
        {
            modoSeleccion = seleccionarCliente;
            EstiloPN1_33ZS.Preparar(this, EstiloPN1_33ZS.T("Clientes", "Customers"));

            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(24),
                ColumnCount = 1, RowCount = 5 };
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            Controls.Add(panel);
            panel.Controls.Add(EstiloPN1_33ZS.Titulo(EstiloPN1_33ZS.T("Identificar cliente", "Find a customer")), 0, 0);

            var busqueda = EstiloPN1_33ZS.Fila();
            busqueda.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Nombre, apellido o teléfono", "Name or phone")));
            busqueda.Controls.Add(buscar);
            var botonBuscar = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Buscar", "Search"));
            botonBuscar.Click += (s, e) => Cargar();
            busqueda.Controls.Add(botonBuscar);
            panel.Controls.Add(busqueda, 0, 1);
            panel.Controls.Add(tabla, 0, 2);

            var alta = EstiloPN1_33ZS.Fila();
            alta.Visible = negocio.TienePermiso("RegistrarCliente");
            alta.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Nombre", "First name")));
            alta.Controls.Add(nombre);
            alta.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Apellido", "Last name")));
            alta.Controls.Add(apellido);
            alta.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Teléfono", "Phone")));
            alta.Controls.Add(telefono);
            alta.Controls.Add(EstiloPN1_33ZS.Etiqueta(EstiloPN1_33ZS.T("Correo opcional", "Email (optional)")));
            alta.Controls.Add(correo);
            panel.Controls.Add(alta, 0, 3);

            var acciones = EstiloPN1_33ZS.Fila();
            var guardar = EstiloPN1_33ZS.Boton(EstiloPN1_33ZS.T("Registrar cliente", "Register customer"), true);
            guardar.Visible = alta.Visible;
            guardar.Click += (s, e) => Registrar();
            acciones.Controls.Add(guardar);
            seleccionar.Visible = modoSeleccion;
            seleccionar.Click += (s, e) => Elegir();
            acciones.Controls.Add(seleccionar);
            panel.Controls.Add(acciones, 0, 4);
            tabla.CellDoubleClick += (s, e) => { if (modoSeleccion) Elegir(); };
            Shown += (s, e) => Cargar();
        }

        private void Cargar()
        {
            try { tabla.DataSource = negocio.BuscarClientes(buscar.Text); }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void Registrar()
        {
            try
            {
                int id = negocio.RegistrarCliente(new Cliente_33ZS
                {
                    Nombre = nombre.Text, Apellido = apellido.Text,
                    Telefono = telefono.Text, Correo = correo.Text
                });
                buscar.Text = telefono.Text.Trim();
                Cargar();
                if (modoSeleccion)
                {
                    ClienteSeleccionado = negocio.BuscarClientes(buscar.Text).First(c => c.Id == id);
                    DialogResult = DialogResult.OK;
                }
                else MessageBox.Show(this, EstiloPN1_33ZS.T("Cliente registrado.", "Customer registered."), "Harlem");
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void Elegir()
        {
            ClienteSeleccionado = tabla.CurrentRow == null ? null :
                tabla.CurrentRow.DataBoundItem as Cliente_33ZS;
            if (ClienteSeleccionado == null)
            {
                MessageBox.Show(this, EstiloPN1_33ZS.T("Seleccioná un cliente.", "Select a customer."), "Harlem");
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}
