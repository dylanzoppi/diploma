using System.Drawing;
using System.Windows.Forms;
using Servicios;

namespace TpIngSoftware
{
    public partial class BitacoraEventos33ZS
    {
        private void AplicarDiseno_33ZS()
        {
            EstiloPantallas_33ZS.Formulario(this, 1190, 820, 980, 710);
            Controls.Clear();
            var raiz = EstiloPantallas_33ZS.Raiz();
            raiz.RowCount = 4;
            raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 246F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            var cabecera = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            cabecera.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135F));
            EstiloPantallas_33ZS.Titulo(bigLabel1, 21);
            EstiloPantallas_33ZS.Boton(salirBTN);
            salirBTN.Dock = DockStyle.Fill;
            salirBTN.Margin = new Padding(0, 6, 0, 12);
            cabecera.Controls.Add(bigLabel1, 0, 0);
            cabecera.Controls.Add(salirBTN, 1, 0);

            var listado = EstiloPantallas_33ZS.Tarjeta(14);
            EstiloPantallas_33ZS.Grilla(eventosDGV);
            listado.Controls.Add(eventosDGV, 0, 0);

            var filtros = EstiloPantallas_33ZS.Tarjeta(16);
            filtros.Margin = new Padding(0, 12, 0, 0);
            filtros.ColumnCount = 4;
            filtros.RowCount = 4;
            for (int i = 0; i < 4; i++)
                filtros.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            filtros.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            filtros.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            filtros.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            filtros.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            Control[] etiquetas = { dungeonLabel1, dungeonLabel8, dungeonLabel3, dungeonLabel4,
                dungeonLabel5, dungeonLabel7, dungeonLabel6, dungeonLabel2 };
            Control[] entradas = { nombreBOX, apellidoBOX, loginCOMBOBOX, moduloCOMBOBOX,
                fechaInicioCOMBOBOX, fechaFinCOMBOBOX, eventoCOMBOBOX, criticidadCOMBOBOX };
            fechaInicioCOMBOBOX.Format = DateTimePickerFormat.Short;
            fechaFinCOMBOBOX.Format = DateTimePickerFormat.Short;
            for (int i = 0; i < etiquetas.Length; i++)
            {
                EstiloPantallas_33ZS.Etiqueta(etiquetas[i]);
                EstiloPantallas_33ZS.Entrada(entradas[i]);
                etiquetas[i].Margin = new Padding(0, 0, 10, 0);
                entradas[i].Margin = new Padding(0, 2, 14, 12);
                filtros.Controls.Add(etiquetas[i], i % 4, (i / 4) * 2);
                filtros.Controls.Add(entradas[i], i % 4, (i / 4) * 2 + 1);
            }
            EstiloPantallas_33ZS.Boton(limpiarBTN);
            EstiloPantallas_33ZS.Boton(aplicarBTN, true);
            EstiloPantallas_33ZS.Boton(imprimirBTN);
            var acciones = EstiloPantallas_33ZS.Acciones(true);
            acciones.BackColor = EstiloPantallas_33ZS.Fondo;
            acciones.Controls.Add(imprimirBTN);
            acciones.Controls.Add(aplicarBTN);
            acciones.Controls.Add(limpiarBTN);
            raiz.Controls.Add(cabecera, 0, 0);
            raiz.Controls.Add(listado, 0, 1);
            raiz.Controls.Add(filtros, 0, 2);
            raiz.Controls.Add(acciones, 0, 3);
            Controls.Add(raiz);
            ResumeLayout(true);
        }
    }

    public partial class GestionPerfiles_33ZS
    {
        private Label tituloVista_33ZS;

        private void AplicarDiseno_33ZS()
        {
            EstiloPantallas_33ZS.Formulario(this, 1330, 810, 1100, 700);
            Controls.Clear();
            var raiz = EstiloPantallas_33ZS.Raiz();
            raiz.RowCount = 2;
            raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tituloVista_33ZS = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 21F),
                ForeColor = EstiloPantallas_33ZS.Tinta,
                TextAlign = ContentAlignment.MiddleLeft,
                Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.Titulo")
            };
            raiz.Controls.Add(tituloVista_33ZS, 0, 0);
            var columnas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3 };
            columnas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            columnas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            columnas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            columnas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var crear = EstiloPantallas_33ZS.Tarjeta(18);
            crear.Margin = new Padding(0, 0, 14, 0);
            crear.RowCount = 9;
            crear.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            crear.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            crear.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            crear.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            crear.RowStyles.Add(new RowStyle(SizeType.Absolute, 39F));
            crear.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            crear.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            crear.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            crear.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            EstiloPantallas_33ZS.Titulo(bigLabel5, 18);
            EstiloPantallas_33ZS.Etiqueta(bigLabel1);
            EstiloPantallas_33ZS.Etiqueta(bigLabel6);
            EstiloPantallas_33ZS.Etiqueta(bigLabel10);
            EstiloPantallas_33ZS.Entrada(nombreFamiliaTXT);
            listFamilias.Dock = DockStyle.Fill;
            listFamilias.BorderStyle = BorderStyle.None;
            listFamilias.BackColor = EstiloPantallas_33ZS.Fondo;
            EstiloPantallas_33ZS.Boton(confirmarBTN, true);
            EstiloPantallas_33ZS.Boton(button1);
            confirmarBTN.Dock = DockStyle.Fill;
            button1.Dock = DockStyle.Fill;
            confirmarBTN.Margin = new Padding(0, 9, 0, 5);
            button1.Margin = new Padding(0, 7, 0, 0);
            var tipo = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
            familiaRDBTN.Width = 140;
            rolRDBTN.Width = 100;
            familiaRDBTN.ForeColor = EstiloPantallas_33ZS.Tinta;
            rolRDBTN.ForeColor = EstiloPantallas_33ZS.Tinta;
            tipo.Controls.Add(familiaRDBTN);
            tipo.Controls.Add(rolRDBTN);
            crear.Controls.Add(bigLabel5, 0, 0);
            crear.Controls.Add(tipo, 0, 1);
            crear.Controls.Add(bigLabel1, 0, 2);
            crear.Controls.Add(nombreFamiliaTXT, 0, 3);
            crear.Controls.Add(bigLabel6, 0, 4);
            crear.Controls.Add(listFamilias, 0, 5);
            crear.Controls.Add(confirmarBTN, 0, 6);
            crear.Controls.Add(bigLabel10, 0, 7);
            crear.Controls.Add(button1, 0, 8);

            var disponibles = EstiloPantallas_33ZS.Tarjeta(18);
            disponibles.Margin = new Padding(0, 0, 14, 0);
            disponibles.RowCount = 5;
            disponibles.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            disponibles.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            disponibles.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            disponibles.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            disponibles.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            EstiloPantallas_33ZS.Etiqueta(biglabel55);
            biglabel55.Font = new Font("Segoe UI Semibold", 12F);
            EstiloPantallas_33ZS.Etiqueta(bigLabel7);
            bigLabel7.Font = new Font("Segoe UI Semibold", 12F);
            var busqueda = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            busqueda.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72F));
            busqueda.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            EstiloPantallas_33ZS.Etiqueta(bigLabel11);
            EstiloPantallas_33ZS.Entrada(txtBuscar);
            busqueda.Controls.Add(bigLabel11, 0, 0);
            busqueda.Controls.Add(txtBuscar, 1, 0);
            listPatentes.Dock = DockStyle.Fill;
            listPatentes.BorderStyle = BorderStyle.None;
            listPatentes.BackColor = EstiloPantallas_33ZS.Fondo;
            listRoles.Dock = DockStyle.Fill;
            listRoles.BorderStyle = BorderStyle.None;
            listRoles.BackColor = EstiloPantallas_33ZS.Fondo;
            disponibles.Controls.Add(biglabel55, 0, 0);
            disponibles.Controls.Add(busqueda, 0, 1);
            disponibles.Controls.Add(listPatentes, 0, 2);
            disponibles.Controls.Add(bigLabel7, 0, 3);
            disponibles.Controls.Add(listRoles, 0, 4);

            var permisos = EstiloPantallas_33ZS.Tarjeta(18);
            permisos.RowCount = 2;
            permisos.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            permisos.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            EstiloPantallas_33ZS.Etiqueta(bigLabel8);
            bigLabel8.Font = new Font("Segoe UI Semibold", 12F);
            listPatentesRoles.Dock = DockStyle.Fill;
            listPatentesRoles.BorderStyle = BorderStyle.None;
            listPatentesRoles.BackColor = EstiloPantallas_33ZS.Fondo;
            permisos.Controls.Add(bigLabel8, 0, 0);
            permisos.Controls.Add(listPatentesRoles, 0, 1);

            columnas.Controls.Add(crear, 0, 0);
            columnas.Controls.Add(disponibles, 1, 0);
            columnas.Controls.Add(permisos, 2, 0);
            raiz.Controls.Add(columnas, 0, 1);
            Controls.Add(raiz);
            ResumeLayout(true);
        }
    }
}

namespace TpIngSoftware.Administracion.Perfiles
{
    public partial class ModificarEliminarPerfiles
    {
        private void AplicarDiseno_33ZS()
        {
            EstiloPantallas_33ZS.Formulario(this, 1320, 800, 1080, 690);
            Controls.Clear();
            var raiz = EstiloPantallas_33ZS.Raiz();
            raiz.RowCount = 2;
            raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            EstiloPantallas_33ZS.Titulo(bigLabel5, 21);
            raiz.Controls.Add(bigLabel5, 0, 0);
            var columnas = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3 };
            columnas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31F));
            columnas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            columnas.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            columnas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var selector = EstiloPantallas_33ZS.Tarjeta(18);
            selector.Margin = new Padding(0, 0, 14, 0);
            selector.RowCount = 6;
            selector.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            selector.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            selector.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            selector.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            selector.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            selector.RowStyles.Add(new RowStyle(SizeType.Absolute, 59F));
            EstiloPantallas_33ZS.Etiqueta(bigLabel7);
            bigLabel7.Font = new Font("Segoe UI Semibold", 12F);
            var tipo = new FlowLayoutPanel { Dock = DockStyle.Fill };
            familiaRDBTN.Width = 130;
            rolRDBTN.Width = 100;
            familiaRDBTN.ForeColor = EstiloPantallas_33ZS.Tinta;
            rolRDBTN.ForeColor = EstiloPantallas_33ZS.Tinta;
            tipo.Controls.Add(familiaRDBTN);
            tipo.Controls.Add(rolRDBTN);
            listFamiliasRoles.Dock = DockStyle.Fill;
            listFamiliasRoles.BorderStyle = BorderStyle.None;
            listFamiliasRoles.BackColor = EstiloPantallas_33ZS.Fondo;
            EstiloPantallas_33ZS.Etiqueta(bigLabel1);
            EstiloPantallas_33ZS.Entrada(nombreFamiliaTXT);
            EstiloPantallas_33ZS.Boton(modificarBTN, true);
            EstiloPantallas_33ZS.Boton(eliminarBTN);
            var acciones = EstiloPantallas_33ZS.Acciones();
            acciones.Controls.Add(modificarBTN);
            acciones.Controls.Add(eliminarBTN);
            selector.Controls.Add(bigLabel7, 0, 0);
            selector.Controls.Add(tipo, 0, 1);
            selector.Controls.Add(listFamiliasRoles, 0, 2);
            selector.Controls.Add(bigLabel1, 0, 3);
            selector.Controls.Add(nombreFamiliaTXT, 0, 4);
            selector.Controls.Add(acciones, 0, 5);

            var composicion = EstiloPantallas_33ZS.Tarjeta(18);
            composicion.Margin = new Padding(0, 0, 14, 0);
            composicion.RowCount = 4;
            composicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            composicion.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            composicion.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            composicion.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            EstiloPantallas_33ZS.Etiqueta(bigLabel2);
            EstiloPantallas_33ZS.Etiqueta(bigLabel4);
            bigLabel2.Font = new Font("Segoe UI Semibold", 12F);
            bigLabel4.Font = new Font("Segoe UI Semibold", 12F);
            listFamilias.Dock = DockStyle.Fill;
            listPatentesSeleccionadas.Dock = DockStyle.Fill;
            listFamilias.BorderStyle = BorderStyle.None;
            listPatentesSeleccionadas.BorderStyle = BorderStyle.None;
            listFamilias.BackColor = EstiloPantallas_33ZS.Fondo;
            listPatentesSeleccionadas.BackColor = EstiloPantallas_33ZS.Fondo;
            composicion.Controls.Add(bigLabel2, 0, 0);
            composicion.Controls.Add(listFamilias, 0, 1);
            composicion.Controls.Add(bigLabel4, 0, 2);
            composicion.Controls.Add(listPatentesSeleccionadas, 0, 3);

            var disponibles = EstiloPantallas_33ZS.Tarjeta(18);
            disponibles.RowCount = 2;
            disponibles.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            disponibles.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            EstiloPantallas_33ZS.Etiqueta(bigLabel3);
            bigLabel3.Font = new Font("Segoe UI Semibold", 12F);
            listPatentes.Dock = DockStyle.Fill;
            listPatentes.BorderStyle = BorderStyle.None;
            listPatentes.BackColor = EstiloPantallas_33ZS.Fondo;
            disponibles.Controls.Add(bigLabel3, 0, 0);
            disponibles.Controls.Add(listPatentes, 0, 1);

            columnas.Controls.Add(selector, 0, 0);
            columnas.Controls.Add(composicion, 1, 0);
            columnas.Controls.Add(disponibles, 2, 0);
            raiz.Controls.Add(columnas, 0, 1);
            Controls.Add(raiz);
            ResumeLayout(true);
        }
    }
}
