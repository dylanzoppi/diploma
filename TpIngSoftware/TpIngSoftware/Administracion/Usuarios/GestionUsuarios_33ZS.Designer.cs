namespace TpIngSoftware
{
    partial class GestionUsuarios_33ZS
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.Name = "GestionUsuarios_33ZS";
            this.Text = "Gestión de usuarios";
            this.ResumeLayout(false);
        }

        // El diseño se arma fuera de InitializeComponent para que Visual Studio
        // pueda abrir y serializar el formulario sin interpretar lógica de diseño.
        private void AplicarDiseno_33ZS()
        {
            var fondo = System.Drawing.Color.FromArgb(246, 243, 238);
            var tinta = System.Drawing.Color.FromArgb(35, 40, 42);
            var acento = System.Drawing.Color.FromArgb(127, 79, 50);
            var borde = System.Drawing.Color.FromArgb(223, 218, 211);

            this.añadirBTN = new System.Windows.Forms.Button();
            this.desbloquearBTN = new System.Windows.Forms.Button();
            this.modificarBTN = new System.Windows.Forms.Button();
            this.actDesactBTN = new System.Windows.Forms.Button();
            this.aplicarBTN = new System.Windows.Forms.Button();
            this.cancelarBTN = new System.Windows.Forms.Button();
            this.salirBTN = new System.Windows.Forms.Button();
            this.usuariosDGV = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.DNIBOX = new System.Windows.Forms.TextBox();
            this.activoBOX = new System.Windows.Forms.TextBox();
            this.bloqueadoBOX = new System.Windows.Forms.TextBox();
            this.rolBOX = new System.Windows.Forms.ComboBox();
            this.emailBOX = new System.Windows.Forms.TextBox();
            this.nombresBOX = new System.Windows.Forms.TextBox();
            this.ApellidosBOX = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.label12 = new System.Windows.Forms.Label();
            this.labelDetalle = new System.Windows.Forms.Label();

            var raiz = new System.Windows.Forms.TableLayoutPanel();
            var cabecera = new System.Windows.Forms.TableLayoutPanel();
            var contenido = new System.Windows.Forms.TableLayoutPanel();
            var lista = new System.Windows.Forms.TableLayoutPanel();
            var listaCabecera = new System.Windows.Forms.TableLayoutPanel();
            var filtros = new System.Windows.Forms.FlowLayoutPanel();
            var accionesLista = new System.Windows.Forms.FlowLayoutPanel();
            var detalle = new System.Windows.Forms.TableLayoutPanel();
            var campos = new System.Windows.Forms.TableLayoutPanel();
            var estado = new System.Windows.Forms.TableLayoutPanel();
            var pie = new System.Windows.Forms.FlowLayoutPanel();

            ((System.ComponentModel.ISupportInitialize)(this.usuariosDGV)).BeginInit();
            this.SuspendLayout();

            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = fondo;
            this.ClientSize = new System.Drawing.Size(1220, 740);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ForeColor = tinta;
            this.MinimumSize = new System.Drawing.Size(980, 640);
            this.Name = "GestionUsuarios_33ZS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestión de usuarios";
            this.Load += new System.EventHandler(this.GestionUsuarios_Load);

            raiz.Dock = System.Windows.Forms.DockStyle.Fill;
            raiz.Padding = new System.Windows.Forms.Padding(28, 24, 28, 20);
            raiz.ColumnCount = 1;
            raiz.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            raiz.RowCount = 3;
            raiz.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            raiz.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            raiz.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));

            cabecera.Dock = System.Windows.Forms.DockStyle.Fill;
            cabecera.ColumnCount = 2;
            cabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            cabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.label12.Text = "USUARIOS";
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 23F);
            this.label12.ForeColor = tinta;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label12.Name = "label12";
            this.label11.Text = "Número de usuarios: 0";
            this.label11.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(99, 103, 104);
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label11.Name = "label11";
            cabecera.Controls.Add(this.label12, 0, 0);
            cabecera.Controls.Add(this.label11, 1, 0);

            contenido.Dock = System.Windows.Forms.DockStyle.Fill;
            contenido.ColumnCount = 2;
            contenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66F));
            contenido.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            contenido.Margin = new System.Windows.Forms.Padding(0);

            lista.BackColor = System.Drawing.Color.White;
            lista.Dock = System.Windows.Forms.DockStyle.Fill;
            lista.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            lista.Padding = new System.Windows.Forms.Padding(18);
            lista.ColumnCount = 1;
            lista.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            lista.RowCount = 3;
            lista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            lista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            lista.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            listaCabecera.Dock = System.Windows.Forms.DockStyle.Fill;
            listaCabecera.ColumnCount = 2;
            listaCabecera.RowCount = 1;
            listaCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            listaCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            listaCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            var tituloLista = new System.Windows.Forms.Label();
            tituloLista.Text = "Cuentas";
            tituloLista.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            tituloLista.AutoSize = true;
            tituloLista.ForeColor = tinta;
            tituloLista.Anchor = System.Windows.Forms.AnchorStyles.Left;
            tituloLista.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.labelLista = tituloLista;
            filtros.Dock = System.Windows.Forms.DockStyle.Fill;
            filtros.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            filtros.WrapContents = false;
            filtros.Padding = new System.Windows.Forms.Padding(0, 11, 0, 0);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Text = "Inactivos";
            this.radioButton2.AutoSize = true;
            this.radioButton2.Margin = new System.Windows.Forms.Padding(14, 0, 0, 0);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Text = "Activos";
            this.radioButton1.AutoSize = true;
            filtros.Controls.Add(this.radioButton2);
            filtros.Controls.Add(this.radioButton1);
            listaCabecera.Controls.Add(tituloLista, 0, 0);
            listaCabecera.Controls.Add(filtros, 1, 0);

            this.usuariosDGV.Name = "usuariosDGV";
            this.usuariosDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usuariosDGV.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.usuariosDGV.BackgroundColor = System.Drawing.Color.White;
            this.usuariosDGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.usuariosDGV.RowHeadersVisible = false;
            this.usuariosDGV.AllowUserToAddRows = false;
            this.usuariosDGV.AllowUserToDeleteRows = false;
            this.usuariosDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.usuariosDGV.ColumnHeadersHeight = 38;
            this.usuariosDGV.RowTemplate.Height = 34;
            this.usuariosDGV.EnableHeadersVisualStyles = false;
            this.usuariosDGV.ColumnHeadersDefaultCellStyle.BackColor = fondo;
            this.usuariosDGV.ColumnHeadersDefaultCellStyle.ForeColor = tinta;
            this.usuariosDGV.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.usuariosDGV.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(235, 224, 213);
            this.usuariosDGV.DefaultCellStyle.SelectionForeColor = tinta;
            this.usuariosDGV.GridColor = borde;
            this.usuariosDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.usuariosDGV.MultiSelect = false;
            this.usuariosDGV.ReadOnly = true;

            accionesLista.Dock = System.Windows.Forms.DockStyle.Fill;
            accionesLista.Padding = new System.Windows.Forms.Padding(0, 11, 0, 0);
            accionesLista.WrapContents = true;
            ConfigurarBoton(this.añadirBTN, "añadirBTN", "Añadir", acento, System.Drawing.Color.White, 110);
            ConfigurarBoton(this.modificarBTN, "modificarBTN", "Modificar", System.Drawing.Color.White, tinta, 110);
            ConfigurarBoton(this.desbloquearBTN, "desbloquearBTN", "Desbloquear", System.Drawing.Color.White, tinta, 125);
            ConfigurarBoton(this.actDesactBTN, "actDesactBTN", "Act. / Desact.", System.Drawing.Color.White, tinta, 130);
            this.añadirBTN.Click += new System.EventHandler(this.añadirBTN_Click);
            accionesLista.Controls.Add(this.añadirBTN);
            accionesLista.Controls.Add(this.modificarBTN);
            accionesLista.Controls.Add(this.desbloquearBTN);
            accionesLista.Controls.Add(this.actDesactBTN);
            lista.Controls.Add(listaCabecera, 0, 0);
            lista.Controls.Add(this.usuariosDGV, 0, 1);
            lista.Controls.Add(accionesLista, 0, 2);

            detalle.BackColor = System.Drawing.Color.White;
            detalle.Dock = System.Windows.Forms.DockStyle.Fill;
            detalle.Margin = new System.Windows.Forms.Padding(0);
            detalle.Padding = new System.Windows.Forms.Padding(20, 18, 20, 18);
            detalle.ColumnCount = 1;
            detalle.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            detalle.RowCount = 3;
            detalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            detalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            detalle.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.labelDetalle.Name = "labelDetalle";
            this.labelDetalle.Text = "Datos de la cuenta";
            this.labelDetalle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F);
            this.labelDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDetalle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            campos.Dock = System.Windows.Forms.DockStyle.Top;
            campos.AutoSize = true;
            campos.ColumnCount = 2;
            campos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 112F));
            campos.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            campos.RowCount = 7;
            for (int fila = 0; fila < 7; fila++)
                campos.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            ConfigurarCampo(this.label1, this.DNIBOX, "label1", "DNIBOX", "DNI");
            ConfigurarCampo(this.label2, this.ApellidosBOX, "label2", "ApellidosBOX", "Apellidos");
            ConfigurarCampo(this.label3, this.nombresBOX, "label3", "nombresBOX", "Nombres");
            ConfigurarCampo(this.label4, this.emailBOX, "label4", "emailBOX", "Email");
            this.label5.Name = "label5";
            this.label5.Text = "Rol";
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.rolBOX.Name = "rolBOX";
            this.rolBOX.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rolBOX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rolBOX.Margin = new System.Windows.Forms.Padding(0, 9, 0, 7);
            ConfigurarCampo(this.label7, this.bloqueadoBOX, "label7", "bloqueadoBOX", "Bloqueado");
            ConfigurarCampo(this.label8, this.activoBOX, "label8", "activoBOX", "Activo");
            this.bloqueadoBOX.ReadOnly = true;
            this.activoBOX.ReadOnly = true;
            this.bloqueadoBOX.BackColor = fondo;
            this.activoBOX.BackColor = fondo;
            campos.Controls.Add(this.label1, 0, 0);
            campos.Controls.Add(this.DNIBOX, 1, 0);
            campos.Controls.Add(this.label2, 0, 1);
            campos.Controls.Add(this.ApellidosBOX, 1, 1);
            campos.Controls.Add(this.label3, 0, 2);
            campos.Controls.Add(this.nombresBOX, 1, 2);
            campos.Controls.Add(this.label4, 0, 3);
            campos.Controls.Add(this.emailBOX, 1, 3);
            campos.Controls.Add(this.label5, 0, 4);
            campos.Controls.Add(this.rolBOX, 1, 4);
            campos.Controls.Add(this.label7, 0, 5);
            campos.Controls.Add(this.bloqueadoBOX, 1, 5);
            campos.Controls.Add(this.label8, 0, 6);
            campos.Controls.Add(this.activoBOX, 1, 6);

            estado.Dock = System.Windows.Forms.DockStyle.Fill;
            estado.ColumnCount = 1;
            estado.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            estado.RowCount = 2;
            estado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            estado.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.label9.Name = "label9";
            this.label9.Text = "Estado";
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(99, 103, 104);
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label10.Name = "label10";
            this.label10.Text = "Modo consulta";
            this.label10.AutoEllipsis = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            estado.Controls.Add(this.label9, 0, 0);
            estado.Controls.Add(this.label10, 0, 1);
            detalle.Controls.Add(this.labelDetalle, 0, 0);
            detalle.Controls.Add(campos, 0, 1);
            detalle.Controls.Add(estado, 0, 2);

            contenido.Controls.Add(lista, 0, 0);
            contenido.Controls.Add(detalle, 1, 0);
            pie.Dock = System.Windows.Forms.DockStyle.Fill;
            pie.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            pie.WrapContents = false;
            pie.Padding = new System.Windows.Forms.Padding(0, 16, 0, 0);
            ConfigurarBoton(this.salirBTN, "salirBTN", "Salir", System.Drawing.Color.White, tinta, 100);
            ConfigurarBoton(this.cancelarBTN, "cancelarBTN", "Cancelar", System.Drawing.Color.White, tinta, 112);
            ConfigurarBoton(this.aplicarBTN, "aplicarBTN", "Aplicar", acento, System.Drawing.Color.White, 112);
            this.aplicarBTN.Click += new System.EventHandler(this.aplicarBTN_Click);
            this.cancelarBTN.Click += new System.EventHandler(this.cancelarBTN_Click);
            pie.Controls.Add(this.salirBTN);
            pie.Controls.Add(this.cancelarBTN);
            pie.Controls.Add(this.aplicarBTN);

            raiz.Controls.Add(cabecera, 0, 0);
            raiz.Controls.Add(contenido, 0, 1);
            raiz.Controls.Add(pie, 0, 2);
            this.Controls.Add(raiz);
            ((System.ComponentModel.ISupportInitialize)(this.usuariosDGV)).EndInit();
            this.ResumeLayout(false);
        }

        private static void ConfigurarBoton(System.Windows.Forms.Button boton, string nombre,
            string texto, System.Drawing.Color fondo, System.Drawing.Color tinta, int ancho)
        {
            boton.Name = nombre;
            boton.Text = texto;
            boton.Size = new System.Drawing.Size(ancho, 39);
            boton.Margin = new System.Windows.Forms.Padding(0, 0, 9, 0);
            boton.BackColor = fondo;
            boton.ForeColor = tinta;
            boton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            boton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(223, 218, 211);
            boton.Cursor = System.Windows.Forms.Cursors.Hand;
            boton.UseVisualStyleBackColor = false;
        }

        private static void ConfigurarCampo(System.Windows.Forms.Label etiqueta,
            System.Windows.Forms.TextBox entrada, string nombreEtiqueta, string nombreEntrada, string texto)
        {
            etiqueta.Name = nombreEtiqueta;
            etiqueta.Text = texto;
            etiqueta.Dock = System.Windows.Forms.DockStyle.Fill;
            etiqueta.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            entrada.Name = nombreEntrada;
            entrada.Dock = System.Windows.Forms.DockStyle.Fill;
            entrada.Margin = new System.Windows.Forms.Padding(0, 9, 0, 7);
        }

        private System.Windows.Forms.Button añadirBTN;
        private System.Windows.Forms.Button desbloquearBTN;
        private System.Windows.Forms.Button modificarBTN;
        private System.Windows.Forms.Button actDesactBTN;
        private System.Windows.Forms.Button aplicarBTN;
        private System.Windows.Forms.Button cancelarBTN;
        private System.Windows.Forms.Button salirBTN;
        private System.Windows.Forms.DataGridView usuariosDGV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox DNIBOX;
        private System.Windows.Forms.TextBox activoBOX;
        private System.Windows.Forms.TextBox bloqueadoBOX;
        private System.Windows.Forms.ComboBox rolBOX;
        private System.Windows.Forms.TextBox emailBOX;
        private System.Windows.Forms.TextBox nombresBOX;
        private System.Windows.Forms.TextBox ApellidosBOX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelLista;
        private System.Windows.Forms.Label labelDetalle;
    }
}
