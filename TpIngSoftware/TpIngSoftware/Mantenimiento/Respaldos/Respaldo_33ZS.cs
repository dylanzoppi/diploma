using BLL;
using Servicios;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TpIngSoftware
{
    public partial class Respaldo_33ZS : Form, IObservador_33ZS
    {
        private readonly BackupBLL_33ZS backupBLL = new BackupBLL_33ZS();
        private string _carpetaPorDefecto;

        public Respaldo_33ZS()
        {
            InitializeComponent();
            AplicarDiseno_33ZS();

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                SessionManager_33ZS.GetInstance_33ZS().Suscribir_33ZS(this);
                this.FormClosed += (s, e) =>
                    SessionManager_33ZS.GetInstance_33ZS().Desuscribir_33ZS(this);
                Actualizar_33ZS();
            }
        }

        private void PrecargarCarpetaPorDefecto_33ZS()
        {
            try
            {
                string carpeta = backupBLL.ObtenerCarpetaPorDefecto_33ZS();
                if (!string.IsNullOrWhiteSpace(carpeta))
                {
                    _carpetaPorDefecto = carpeta.TrimEnd('\\');
                    txtBackup.Text = _carpetaPorDefecto;
                }
            }
            catch
            {
            }
        }

        public void Actualizar_33ZS()
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();

            this.Text = idm.Traducir_33ZS("Respaldo.Titulo");
            labelTitulo.Text = idm.Traducir_33ZS("Respaldo.Titulo");
            groupBackup.Text = idm.Traducir_33ZS("Respaldo.Backup");
            groupRestore.Text = idm.Traducir_33ZS("Respaldo.Restore");
            btnExaminarCarpeta.Text = idm.Traducir_33ZS("Respaldo.Examinar");
            btnExaminarArchivo.Text = idm.Traducir_33ZS("Respaldo.Examinar");
            btnRealizarBackup.Text = idm.Traducir_33ZS("Respaldo.RealizarBackup");
            btnRestaurar.Text = idm.Traducir_33ZS("Respaldo.Restaurar");
            btnSalir.Text = idm.Traducir_33ZS("Respaldo.Salir");
        }

        private void btnExaminarCarpeta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialogo = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(txtBackup.Text) && System.IO.Directory.Exists(txtBackup.Text))
                    dialogo.SelectedPath = txtBackup.Text;

                if (dialogo.ShowDialog() == DialogResult.OK)
                    txtBackup.Text = dialogo.SelectedPath;
            }
        }

        private void btnRealizarBackup_Click(object sender, EventArgs e)
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();

            if (string.IsNullOrWhiteSpace(txtBackup.Text))
            {
                MessageBox.Show(idm.Traducir_33ZS("Respaldo.SeleccioneCarpeta"));
                return;
            }

            try
            {
                string ruta = backupBLL.RealizarBackup_33ZS(txtBackup.Text);
                MessageBox.Show(idm.Traducir_33ZS("Respaldo.BackupExito") + "\n" + ruta);
                txtBackup.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(idm.Traducir_33ZS("Respaldo.BackupError") + "\n" + idm.Traducir_33ZS(ex.Message));
            }
        }

        private void btnExaminarArchivo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Filter = "SQL Backup Files (*.bak)|*.bak";

                if (!string.IsNullOrWhiteSpace(_carpetaPorDefecto) && System.IO.Directory.Exists(_carpetaPorDefecto))
                    dialogo.InitialDirectory = _carpetaPorDefecto;

                if (dialogo.ShowDialog() == DialogResult.OK)
                    txtRestore.Text = dialogo.FileName;
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();

            if (string.IsNullOrWhiteSpace(txtRestore.Text))
            {
                MessageBox.Show(idm.Traducir_33ZS("Respaldo.SeleccioneArchivo"));
                return;
            }

            if (MessageBox.Show(idm.Traducir_33ZS("Respaldo.ConfirmarRestore"),
                    idm.Traducir_33ZS("Respaldo.Titulo"),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                backupBLL.RealizarRestore_33ZS(txtRestore.Text);
                MessageBox.Show(idm.Traducir_33ZS("Respaldo.RestoreExito"));
                txtRestore.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(idm.Traducir_33ZS("Respaldo.RestoreError") + "\n" + idm.Traducir_33ZS(ex.Message));
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupRestore_Enter(object sender, EventArgs e)
        {

        }

        private void Respaldo_33ZS_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
                PrecargarCarpetaPorDefecto_33ZS();
        }
    }
}
