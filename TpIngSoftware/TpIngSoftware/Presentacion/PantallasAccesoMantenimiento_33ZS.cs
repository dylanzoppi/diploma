using System.Drawing;
using System.Windows.Forms;
using Servicios;

namespace TpIngSoftware
{
    public partial class Login33ZS
    {
        private void AplicarDiseno_33ZS()
        {
            EstiloPantallas_33ZS.Formulario(this, 760, 540, 650, 470);
            Controls.Clear();
            var raiz = EstiloPantallas_33ZS.Raiz(32);
            raiz.ColumnCount = 3;
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 66F));
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            var tarjeta = EstiloPantallas_33ZS.Tarjeta(32);
            tarjeta.RowCount = 8;
            foreach (int alto in new[] { 64, 31, 43, 31, 43, 43, 60, 52 })
                tarjeta.RowStyles.Add(new RowStyle(SizeType.Absolute, alto));
            EstiloPantallas_33ZS.Titulo(tituloLBL, 22);
            tituloLBL.TextAlign = ContentAlignment.MiddleLeft;
            EstiloPantallas_33ZS.Etiqueta(usuarioLBL);
            EstiloPantallas_33ZS.Etiqueta(passwordLBL);
            EstiloPantallas_33ZS.Entrada(usuarioTXT);
            EstiloPantallas_33ZS.Entrada(passworTXT);
            usuarioTXT.Width = 200;
            mostrarPasswordCHK.Dock = DockStyle.Fill;
            mostrarPasswordCHK.Margin = new Padding(0, 0, 0, 0);
            mostrarPasswordCHK.ForeColor = EstiloPantallas_33ZS.Secundario;
            var mensaje = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1 };
            mensaje.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            mensaje.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            EstiloPantallas_33ZS.Etiqueta(mensajeTituloLBL);
            mensajeTituloLBL.Font = new Font("Segoe UI Semibold", 9F);
            mensajeTituloLBL.ForeColor = EstiloPantallas_33ZS.Secundario;
            mensajeLBL.Dock = DockStyle.Fill;
            mensajeLBL.AutoEllipsis = true;
            mensajeLBL.ForeColor = EstiloPantallas_33ZS.Tinta;
            mensaje.Controls.Add(mensajeTituloLBL, 0, 0);
            mensaje.Controls.Add(mensajeLBL, 0, 1);
            EstiloPantallas_33ZS.Boton(ingresarBTN, true);
            EstiloPantallas_33ZS.Boton(cancelarBTN);
            var acciones = EstiloPantallas_33ZS.Acciones(true);
            acciones.Controls.Add(cancelarBTN);
            acciones.Controls.Add(ingresarBTN);
            tarjeta.Controls.Add(tituloLBL, 0, 0);
            tarjeta.Controls.Add(usuarioLBL, 0, 1);
            tarjeta.Controls.Add(usuarioTXT, 0, 2);
            tarjeta.Controls.Add(passwordLBL, 0, 3);
            tarjeta.Controls.Add(passworTXT, 0, 4);
            tarjeta.Controls.Add(mostrarPasswordCHK, 0, 5);
            tarjeta.Controls.Add(mensaje, 0, 6);
            tarjeta.Controls.Add(acciones, 0, 7);
            raiz.Controls.Add(tarjeta, 1, 0);
            Controls.Add(raiz);
            ResumeLayout(true);
        }
    }

    public partial class CambiarClave33ZS
    {
        private void AplicarDiseno_33ZS()
        {
            EstiloPantallas_33ZS.Formulario(this, 890, 560, 760, 490);
            Controls.Clear();
            var raiz = EstiloPantallas_33ZS.Raiz();
            raiz.ColumnCount = 2;
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            var formulario = EstiloPantallas_33ZS.Tarjeta();
            formulario.Margin = new Padding(0, 0, 16, 0);
            formulario.RowCount = 10;
            foreach (int alto in new[] { 32, 40, 32, 40, 32, 40, 42, 64, 58, 10 })
                formulario.RowStyles.Add(new RowStyle(SizeType.Absolute, alto));
            Control[] etiquetas = { label3, label2, label1 };
            Control[] entradas = { claveActualTXT, claveNuevaTXT, claveRepetirTXT };
            for (int i = 0; i < 3; i++)
            {
                EstiloPantallas_33ZS.Etiqueta(etiquetas[i]);
                EstiloPantallas_33ZS.Entrada(entradas[i]);
                formulario.Controls.Add(etiquetas[i], 0, i * 2);
                formulario.Controls.Add(entradas[i], 0, i * 2 + 1);
            }
            mostrarClavesCHK.Dock = DockStyle.Fill;
            mostrarClavesCHK.ForeColor = EstiloPantallas_33ZS.Secundario;
            formulario.Controls.Add(mostrarClavesCHK, 0, 6);
            var mensaje = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1 };
            mensaje.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            mensaje.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            EstiloPantallas_33ZS.Etiqueta(label4);
            label4.Font = new Font("Segoe UI Semibold", 9F);
            label4.ForeColor = EstiloPantallas_33ZS.Secundario;
            mensajeLBL.Dock = DockStyle.Fill;
            mensajeLBL.AutoEllipsis = true;
            mensaje.Controls.Add(label4, 0, 0);
            mensaje.Controls.Add(mensajeLBL, 0, 1);
            formulario.Controls.Add(mensaje, 0, 7);
            EstiloPantallas_33ZS.Boton(aceptarBTN, true);
            EstiloPantallas_33ZS.Boton(cancelarBTN);
            var acciones = EstiloPantallas_33ZS.Acciones();
            acciones.Controls.Add(aceptarBTN);
            acciones.Controls.Add(cancelarBTN);
            formulario.Controls.Add(acciones, 0, 8);

            var reglas = EstiloPantallas_33ZS.Tarjeta();
            reglas.RowCount = 7;
            reglas.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            for (int i = 0; i < 5; i++)
                reglas.RowStyles.Add(new RowStyle(SizeType.Absolute, 49F));
            reglas.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            var titulo = new Label
            {
                Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("CambiarClave.Titulo"),
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI Semibold", 16F),
                ForeColor = EstiloPantallas_33ZS.Tinta,
                TextAlign = ContentAlignment.MiddleLeft
            };
            reglas.Controls.Add(titulo, 0, 0);
            CheckBox[] validaciones = { chkMinimoCaracteres, chkMaximoCaracteres,
                chkSinEspacios, chkDistintaActual, chkCoincideRepeticion };
            for (int i = 0; i < validaciones.Length; i++)
            {
                validaciones[i].Dock = DockStyle.Fill;
                validaciones[i].Margin = new Padding(0);
                validaciones[i].ForeColor = EstiloPantallas_33ZS.Tinta;
                validaciones[i].AutoCheck = false;
                validaciones[i].TabStop = false;
                reglas.Controls.Add(validaciones[i], 0, i + 1);
            }
            raiz.Controls.Add(formulario, 0, 0);
            raiz.Controls.Add(reglas, 1, 0);
            Controls.Add(raiz);
            ResumeLayout(true);
        }
    }

    public partial class Respaldo_33ZS
    {
        private void AplicarDiseno_33ZS()
        {
            EstiloPantallas_33ZS.Formulario(this, 830, 570, 700, 520);
            Controls.Clear();
            var raiz = EstiloPantallas_33ZS.Raiz();
            raiz.RowCount = 4;
            raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            EstiloPantallas_33ZS.Titulo(labelTitulo, 21);
            labelTitulo.TextAlign = ContentAlignment.MiddleLeft;
            raiz.Controls.Add(labelTitulo, 0, 0);
            PrepararGrupo_33ZS(groupBackup, txtBackup, btnExaminarCarpeta, btnRealizarBackup);
            PrepararGrupo_33ZS(groupRestore, txtRestore, btnExaminarArchivo, btnRestaurar);
            groupBackup.Margin = new Padding(0, 0, 0, 12);
            groupRestore.Margin = new Padding(0);
            raiz.Controls.Add(groupBackup, 0, 1);
            raiz.Controls.Add(groupRestore, 0, 2);
            EstiloPantallas_33ZS.Boton(btnSalir);
            var pie = EstiloPantallas_33ZS.Acciones(true);
            pie.BackColor = EstiloPantallas_33ZS.Fondo;
            pie.Controls.Add(btnSalir);
            raiz.Controls.Add(pie, 0, 3);
            Controls.Add(raiz);
            ResumeLayout(true);
        }

        private static void PrepararGrupo_33ZS(GroupBox grupo, TextBox ruta, Button examinar, Button ejecutar)
        {
            grupo.Controls.Clear();
            grupo.Dock = DockStyle.Fill;
            grupo.BackColor = EstiloPantallas_33ZS.Superficie;
            grupo.ForeColor = EstiloPantallas_33ZS.Tinta;
            grupo.Font = new Font("Segoe UI Semibold", 11F);
            grupo.Padding = new Padding(20, 22, 20, 16);
            var contenido = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
            contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            contenido.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155F));
            contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            contenido.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            EstiloPantallas_33ZS.Entrada(ruta);
            ruta.Font = new Font("Segoe UI", 10F);
            ruta.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            EstiloPantallas_33ZS.Boton(examinar);
            EstiloPantallas_33ZS.Boton(ejecutar, true);
            examinar.Dock = DockStyle.Fill;
            ejecutar.Dock = DockStyle.Fill;
            examinar.Margin = new Padding(10, 2, 0, 4);
            ejecutar.Margin = new Padding(10, 2, 0, 4);
            contenido.Controls.Add(ruta, 0, 0);
            contenido.Controls.Add(examinar, 1, 0);
            contenido.Controls.Add(ejecutar, 1, 1);
            grupo.Controls.Add(contenido);
        }
    }

    public partial class Reparacion_33ZS
    {
        private void AplicarDiseno_33ZS()
        {
            EstiloPantallas_33ZS.Formulario(this, 690, 430, 600, 390);
            Controls.Clear();
            var raiz = EstiloPantallas_33ZS.Raiz();
            var tarjeta = EstiloPantallas_33ZS.Tarjeta();
            tarjeta.RowCount = 4;
            tarjeta.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tarjeta.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tarjeta.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tarjeta.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            lblDetalle.Dock = DockStyle.Fill;
            lblDetalle.Font = new Font("Segoe UI", 11F);
            lblDetalle.ForeColor = EstiloPantallas_33ZS.Tinta;
            lblDetalle.Margin = new Padding(0, 0, 0, 12);
            EstiloPantallas_33ZS.Boton(btnRecalcular, true);
            EstiloPantallas_33ZS.Boton(btnRestaurar);
            EstiloPantallas_33ZS.Boton(btnSalir);
            btnRecalcular.Dock = DockStyle.Fill;
            btnRestaurar.Dock = DockStyle.Fill;
            btnSalir.Dock = DockStyle.Fill;
            btnRecalcular.Margin = new Padding(0, 0, 0, 8);
            btnRestaurar.Margin = new Padding(0, 0, 0, 8);
            btnSalir.Margin = new Padding(0);
            tarjeta.Controls.Add(lblDetalle, 0, 0);
            tarjeta.Controls.Add(btnRecalcular, 0, 1);
            tarjeta.Controls.Add(btnRestaurar, 0, 2);
            tarjeta.Controls.Add(btnSalir, 0, 3);
            raiz.Controls.Add(tarjeta, 0, 0);
            Controls.Add(raiz);
            ResumeLayout(true);
        }
    }
}
