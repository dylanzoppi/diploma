namespace TpIngSoftware
{
    partial class Respaldo_33ZS
    {
        /// <summary>
        /// Variable del disenador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se esten usando.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codigo generado por el Disenador de Windows Forms

        private void InitializeComponent()
        {
            this.labelTitulo = new System.Windows.Forms.Label();
            this.groupBackup = new System.Windows.Forms.GroupBox();
            this.txtBackup = new System.Windows.Forms.TextBox();
            this.btnExaminarCarpeta = new System.Windows.Forms.Button();
            this.btnRealizarBackup = new System.Windows.Forms.Button();
            this.groupRestore = new System.Windows.Forms.GroupBox();
            this.txtRestore = new System.Windows.Forms.TextBox();
            this.btnExaminarArchivo = new System.Windows.Forms.Button();
            this.btnRestaurar = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.groupBackup.SuspendLayout();
            this.groupRestore.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitulo
            // 
            this.labelTitulo.AutoSize = true;
            this.labelTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitulo.Location = new System.Drawing.Point(17, 13);
            this.labelTitulo.Name = "labelTitulo";
            this.labelTitulo.Size = new System.Drawing.Size(201, 25);
            this.labelTitulo.TabIndex = 0;
            this.labelTitulo.Text = "Gestion de Respaldos";
            // 
            // groupBackup
            // 
            this.groupBackup.Controls.Add(this.txtBackup);
            this.groupBackup.Controls.Add(this.btnExaminarCarpeta);
            this.groupBackup.Controls.Add(this.btnRealizarBackup);
            this.groupBackup.Location = new System.Drawing.Point(21, 48);
            this.groupBackup.Name = "groupBackup";
            this.groupBackup.Size = new System.Drawing.Size(411, 95);
            this.groupBackup.TabIndex = 1;
            this.groupBackup.TabStop = false;
            this.groupBackup.Text = "Backup";
            // 
            // txtBackup
            // 
            this.txtBackup.Location = new System.Drawing.Point(13, 26);
            this.txtBackup.Name = "txtBackup";
            this.txtBackup.ReadOnly = true;
            this.txtBackup.Size = new System.Drawing.Size(283, 20);
            this.txtBackup.TabIndex = 0;
            // 
            // btnExaminarCarpeta
            // 
            this.btnExaminarCarpeta.Location = new System.Drawing.Point(304, 25);
            this.btnExaminarCarpeta.Name = "btnExaminarCarpeta";
            this.btnExaminarCarpeta.Size = new System.Drawing.Size(90, 22);
            this.btnExaminarCarpeta.TabIndex = 1;
            this.btnExaminarCarpeta.Text = "Examinar...";
            this.btnExaminarCarpeta.UseVisualStyleBackColor = true;
            this.btnExaminarCarpeta.Click += new System.EventHandler(this.btnExaminarCarpeta_Click);
            // 
            // btnRealizarBackup
            // 
            this.btnRealizarBackup.Location = new System.Drawing.Point(304, 53);
            this.btnRealizarBackup.Name = "btnRealizarBackup";
            this.btnRealizarBackup.Size = new System.Drawing.Size(90, 26);
            this.btnRealizarBackup.TabIndex = 2;
            this.btnRealizarBackup.Text = "Realizar Backup";
            this.btnRealizarBackup.UseVisualStyleBackColor = true;
            this.btnRealizarBackup.Click += new System.EventHandler(this.btnRealizarBackup_Click);
            // 
            // groupRestore
            // 
            this.groupRestore.Controls.Add(this.txtRestore);
            this.groupRestore.Controls.Add(this.btnExaminarArchivo);
            this.groupRestore.Controls.Add(this.btnRestaurar);
            this.groupRestore.Location = new System.Drawing.Point(21, 156);
            this.groupRestore.Name = "groupRestore";
            this.groupRestore.Size = new System.Drawing.Size(411, 95);
            this.groupRestore.TabIndex = 2;
            this.groupRestore.TabStop = false;
            this.groupRestore.Text = "Restore";
            this.groupRestore.Enter += new System.EventHandler(this.groupRestore_Enter);
            // 
            // txtRestore
            // 
            this.txtRestore.Location = new System.Drawing.Point(13, 26);
            this.txtRestore.Name = "txtRestore";
            this.txtRestore.ReadOnly = true;
            this.txtRestore.Size = new System.Drawing.Size(283, 20);
            this.txtRestore.TabIndex = 0;
            // 
            // btnExaminarArchivo
            // 
            this.btnExaminarArchivo.Location = new System.Drawing.Point(304, 25);
            this.btnExaminarArchivo.Name = "btnExaminarArchivo";
            this.btnExaminarArchivo.Size = new System.Drawing.Size(90, 22);
            this.btnExaminarArchivo.TabIndex = 1;
            this.btnExaminarArchivo.Text = "Examinar...";
            this.btnExaminarArchivo.UseVisualStyleBackColor = true;
            this.btnExaminarArchivo.Click += new System.EventHandler(this.btnExaminarArchivo_Click);
            // 
            // btnRestaurar
            // 
            this.btnRestaurar.Location = new System.Drawing.Point(304, 56);
            this.btnRestaurar.Name = "btnRestaurar";
            this.btnRestaurar.Size = new System.Drawing.Size(90, 26);
            this.btnRestaurar.TabIndex = 2;
            this.btnRestaurar.Text = "Restaurar";
            this.btnRestaurar.UseVisualStyleBackColor = true;
            this.btnRestaurar.Click += new System.EventHandler(this.btnRestaurar_Click);
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(343, 264);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(90, 26);
            this.btnSalir.TabIndex = 3;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // Respaldo_33ZS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 303);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.groupRestore);
            this.Controls.Add(this.groupBackup);
            this.Controls.Add(this.labelTitulo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Respaldo_33ZS";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestion de Respaldos";
            this.Load += new System.EventHandler(this.Respaldo_33ZS_Load);
            this.groupBackup.ResumeLayout(false);
            this.groupBackup.PerformLayout();
            this.groupRestore.ResumeLayout(false);
            this.groupRestore.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitulo;
        private System.Windows.Forms.GroupBox groupBackup;
        private System.Windows.Forms.TextBox txtBackup;
        private System.Windows.Forms.Button btnExaminarCarpeta;
        private System.Windows.Forms.Button btnRealizarBackup;
        private System.Windows.Forms.GroupBox groupRestore;
        private System.Windows.Forms.TextBox txtRestore;
        private System.Windows.Forms.Button btnExaminarArchivo;
        private System.Windows.Forms.Button btnRestaurar;
        private System.Windows.Forms.Button btnSalir;
    }
}
