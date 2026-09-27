namespace TpIngSoftware
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TableLayoutPanel lateral;
        private System.Windows.Forms.FlowLayoutPanel navegacion;
        private System.Windows.Forms.Label marca;
        private System.Windows.Forms.Panel pie;
        private System.Windows.Forms.Button idiomaBoton;
        private System.Windows.Forms.Button claveBoton;
        private System.Windows.Forms.Button salirBoton;
        private System.Windows.Forms.TableLayoutPanel cuerpo;
        private System.Windows.Forms.Label titulo;
        private System.Windows.Forms.Label subtitulo;
        private System.Windows.Forms.Label accesosTitulo;
        private System.Windows.Forms.FlowLayoutPanel accesos;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lateral = new System.Windows.Forms.TableLayoutPanel();
            this.navegacion = new System.Windows.Forms.FlowLayoutPanel();
            this.marca = new System.Windows.Forms.Label();
            this.pie = new System.Windows.Forms.Panel();
            this.idiomaBoton = new System.Windows.Forms.Button();
            this.claveBoton = new System.Windows.Forms.Button();
            this.salirBoton = new System.Windows.Forms.Button();
            this.cuerpo = new System.Windows.Forms.TableLayoutPanel();
            this.titulo = new System.Windows.Forms.Label();
            this.subtitulo = new System.Windows.Forms.Label();
            this.accesosTitulo = new System.Windows.Forms.Label();
            this.accesos = new System.Windows.Forms.FlowLayoutPanel();
            this.lateral.SuspendLayout();
            this.pie.SuspendLayout();
            this.cuerpo.SuspendLayout();
            this.SuspendLayout();
            // lateral
            this.lateral.BackColor = System.Drawing.Color.FromArgb(35, 40, 42);
            this.lateral.ColumnCount = 1;
            this.lateral.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lateral.Controls.Add(this.marca, 0, 0);
            this.lateral.Controls.Add(this.navegacion, 0, 1);
            this.lateral.Controls.Add(this.pie, 0, 2);
            this.lateral.Dock = System.Windows.Forms.DockStyle.Left;
            this.lateral.Location = new System.Drawing.Point(0, 0);
            this.lateral.Name = "lateral";
            this.lateral.Padding = new System.Windows.Forms.Padding(18, 24, 18, 18);
            this.lateral.RowCount = 3;
            this.lateral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.lateral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.lateral.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.lateral.Size = new System.Drawing.Size(245, 750);
            this.lateral.TabIndex = 0;
            // marca
            this.marca.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marca.Font = new System.Drawing.Font("Segoe UI Semibold", 23F);
            this.marca.ForeColor = System.Drawing.Color.White;
            this.marca.Location = new System.Drawing.Point(21, 24);
            this.marca.Name = "marca";
            this.marca.Size = new System.Drawing.Size(203, 62);
            this.marca.TabIndex = 0;
            this.marca.Text = "HARLEM";
            // navegacion
            this.navegacion.AutoScroll = true;
            this.navegacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navegacion.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.navegacion.Location = new System.Drawing.Point(21, 89);
            this.navegacion.Name = "navegacion";
            this.navegacion.Size = new System.Drawing.Size(203, 490);
            this.navegacion.TabIndex = 1;
            this.navegacion.WrapContents = false;
            // pie
            this.pie.Controls.Add(this.idiomaBoton);
            this.pie.Controls.Add(this.claveBoton);
            this.pie.Controls.Add(this.salirBoton);
            this.pie.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pie.Location = new System.Drawing.Point(21, 585);
            this.pie.Name = "pie";
            this.pie.Size = new System.Drawing.Size(203, 144);
            this.pie.TabIndex = 2;
            // idiomaBoton
            this.idiomaBoton.BackColor = System.Drawing.Color.FromArgb(35, 40, 42);
            this.idiomaBoton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.idiomaBoton.FlatAppearance.BorderSize = 0;
            this.idiomaBoton.ForeColor = System.Drawing.Color.White;
            this.idiomaBoton.Location = new System.Drawing.Point(0, 49);
            this.idiomaBoton.Name = "idiomaBoton";
            this.idiomaBoton.Size = new System.Drawing.Size(185, 45);
            this.idiomaBoton.TabIndex = 1;
            this.idiomaBoton.Text = "Español / English";
            this.idiomaBoton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.idiomaBoton.UseVisualStyleBackColor = false;
            // claveBoton
            this.claveBoton.BackColor = System.Drawing.Color.FromArgb(35, 40, 42);
            this.claveBoton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.claveBoton.FlatAppearance.BorderSize = 0;
            this.claveBoton.ForeColor = System.Drawing.Color.White;
            this.claveBoton.Location = new System.Drawing.Point(0, 0);
            this.claveBoton.Name = "claveBoton";
            this.claveBoton.Size = new System.Drawing.Size(185, 45);
            this.claveBoton.TabIndex = 0;
            this.claveBoton.Text = "Cambiar contraseña";
            this.claveBoton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.claveBoton.UseVisualStyleBackColor = false;
            // salirBoton
            this.salirBoton.BackColor = System.Drawing.Color.FromArgb(35, 40, 42);
            this.salirBoton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.salirBoton.FlatAppearance.BorderSize = 0;
            this.salirBoton.ForeColor = System.Drawing.Color.White;
            this.salirBoton.Location = new System.Drawing.Point(0, 99);
            this.salirBoton.Name = "salirBoton";
            this.salirBoton.Size = new System.Drawing.Size(185, 45);
            this.salirBoton.TabIndex = 2;
            this.salirBoton.Text = "Cerrar sesión";
            this.salirBoton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.salirBoton.UseVisualStyleBackColor = false;
            // cuerpo
            this.cuerpo.ColumnCount = 1;
            this.cuerpo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cuerpo.Controls.Add(this.titulo, 0, 0);
            this.cuerpo.Controls.Add(this.subtitulo, 0, 1);
            this.cuerpo.Controls.Add(this.accesosTitulo, 0, 2);
            this.cuerpo.Controls.Add(this.accesos, 0, 3);
            this.cuerpo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cuerpo.Location = new System.Drawing.Point(245, 0);
            this.cuerpo.Name = "cuerpo";
            this.cuerpo.Padding = new System.Windows.Forms.Padding(38);
            this.cuerpo.RowCount = 4;
            this.cuerpo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.cuerpo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.cuerpo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.cuerpo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.cuerpo.Size = new System.Drawing.Size(955, 750);
            this.cuerpo.TabIndex = 1;
            // titulo
            this.titulo.AutoSize = true;
            this.titulo.Font = new System.Drawing.Font("Segoe UI Semibold", 27F);
            this.titulo.ForeColor = System.Drawing.Color.FromArgb(35, 40, 42);
            this.titulo.Location = new System.Drawing.Point(41, 38);
            this.titulo.Name = "titulo";
            this.titulo.Size = new System.Drawing.Size(168, 48);
            this.titulo.TabIndex = 0;
            this.titulo.Text = "HARLEM";
            // subtitulo
            this.subtitulo.AutoSize = true;
            this.subtitulo.ForeColor = System.Drawing.Color.FromArgb(95, 99, 98);
            this.subtitulo.Location = new System.Drawing.Point(38, 88);
            this.subtitulo.Margin = new System.Windows.Forms.Padding(0, 2, 0, 30);
            this.subtitulo.Name = "subtitulo";
            this.subtitulo.Size = new System.Drawing.Size(243, 17);
            this.subtitulo.TabIndex = 1;
            this.subtitulo.Text = "Accesos según el puesto del usuario";
            // accesosTitulo
            this.accesosTitulo.AutoSize = true;
            this.accesosTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.accesosTitulo.ForeColor = System.Drawing.Color.FromArgb(127, 79, 50);
            this.accesosTitulo.Location = new System.Drawing.Point(38, 135);
            this.accesosTitulo.Margin = new System.Windows.Forms.Padding(0, 0, 0, 14);
            this.accesosTitulo.Name = "accesosTitulo";
            this.accesosTitulo.Size = new System.Drawing.Size(160, 19);
            this.accesosTitulo.TabIndex = 2;
            this.accesosTitulo.Text = "ACCESOS DISPONIBLES";
            // accesos
            this.accesos.AutoScroll = true;
            this.accesos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accesos.Location = new System.Drawing.Point(41, 171);
            this.accesos.Name = "accesos";
            this.accesos.Size = new System.Drawing.Size(873, 538);
            this.accesos.TabIndex = 3;
            // Form1
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(246, 243, 238);
            this.ClientSize = new System.Drawing.Size(1200, 750);
            this.Controls.Add(this.cuerpo);
            this.Controls.Add(this.lateral);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.MinimumSize = new System.Drawing.Size(970, 620);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Harlem · Gestión de barbería";
            this.lateral.ResumeLayout(false);
            this.pie.ResumeLayout(false);
            this.cuerpo.ResumeLayout(false);
            this.cuerpo.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
