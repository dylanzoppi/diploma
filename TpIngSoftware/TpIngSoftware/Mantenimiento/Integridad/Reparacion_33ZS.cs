using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace TpIngSoftware
{
    public partial class Reparacion_33ZS : Form, IObservador_33ZS
    {
        public enum ResultadoReparacion_33ZS { Recalculado, Restaurado, Salir }

        private DigitoVerificadorBLL_33ZS dvBLL;
        private BackupBLL_33ZS backupBLL;
        private BitacoraEventoBLL_33ZS bitacoraBLL;
        private readonly List<string> tablasInconsistentes_33ZS;

        public ResultadoReparacion_33ZS Resultado { get; private set; } = ResultadoReparacion_33ZS.Salir;

        public Reparacion_33ZS(List<string> tablasInconsistentes)
        {
            InitializeComponent();
            AplicarDiseno_33ZS();
            tablasInconsistentes_33ZS = tablasInconsistentes ?? new List<string>();
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                dvBLL = new DigitoVerificadorBLL_33ZS();
                backupBLL = new BackupBLL_33ZS();
                bitacoraBLL = new BitacoraEventoBLL_33ZS();
                SessionManager_33ZS.GetInstance_33ZS().Suscribir_33ZS(this);
                this.FormClosed += (s, e) => SessionManager_33ZS.GetInstance_33ZS().Desuscribir_33ZS(this);
                Actualizar_33ZS();
            }
        }

        public void Actualizar_33ZS()
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();
            string tablas = tablasInconsistentes_33ZS.Count > 0
                ? string.Join(", ", tablasInconsistentes_33ZS)
                : "-";

            this.Text = idm.Traducir_33ZS("Reparacion.Titulo");
            btnRecalcular.Text = idm.Traducir_33ZS("Reparacion.BtnRecalcular");
            btnRestaurar.Text = idm.Traducir_33ZS("Reparacion.BtnRestaurar");
            btnSalir.Text = idm.Traducir_33ZS("Reparacion.BtnSalir");
            lblDetalle.Text = idm.Traducir_33ZS("Reparacion.Detalle") + Environment.NewLine +
                             idm.Traducir_33ZS("Reparacion.TablasAfectadas").Replace("{0}", tablas) + Environment.NewLine + Environment.NewLine +
                             idm.Traducir_33ZS("Reparacion.Accion");
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            try
            {
                dvBLL.GenerarTodo_33ZS();
                RegistrarEvento_33ZS(TipoEvento_33ZS.RecalcularDV);
                Resultado = ResultadoReparacion_33ZS.Recalculado;
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("Reparacion.RecalculoExito"), idm.Traducir_33ZS("Reparacion.Titulo"),
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("Reparacion.ErrorRecalculo") + Environment.NewLine + idm.Traducir_33ZS(ex.Message),
                    idm.Traducir_33ZS("Reparacion.Titulo"));
            }
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialogo = new OpenFileDialog())
            {
                dialogo.Filter = "SQL Backup Files (*.bak)|*.bak";

                try
                {
                    string carpeta = backupBLL.ObtenerCarpetaPorDefecto_33ZS();
                    if (!string.IsNullOrWhiteSpace(carpeta) && Directory.Exists(carpeta))
                        dialogo.InitialDirectory = carpeta;
                }
                catch { }

                if (dialogo.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    backupBLL.RealizarRestore_33ZS(dialogo.FileName);
                    Resultado = ResultadoReparacion_33ZS.Restaurado;
                    MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Reparacion.RestoreExito"));
                    this.Close();
                }
                catch (Exception ex)
                {
                    var idm = SessionManager_33ZS.GetInstance_33ZS();
                    MessageBox.Show(idm.Traducir_33ZS("Reparacion.ErrorRestore") + Environment.NewLine + idm.Traducir_33ZS(ex.Message));
                }
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Resultado = ResultadoReparacion_33ZS.Salir;
            this.Close();
        }

        private void RegistrarEvento_33ZS(TipoEvento_33ZS tipo)
        {
            try
            {
                string login = SessionManager_33ZS.HaySesionActiva_33ZS()
                    ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS
                    : "sistema";

                BitacoraEvento_33ZS evento = new BitacoraEvento_33ZS
                {
                    Login_33ZS = login,
                    Fecha_33ZS = DateTime.Today,
                    Hora_33ZS = DateTime.Now,
                    Modulo_33ZS = ModuloSistema_33ZS.Respaldos.ToString(),
                    NombreEvento_33ZS = tipo.ToString(),
                    Criticidad_33ZS = 3
                };

                bitacoraBLL.RegistrarEvento_33ZS(evento);
            }
            catch
            {
            }
        }

        private void lblDetalle_Click(object sender, EventArgs e)
        {

        }
    }
}
