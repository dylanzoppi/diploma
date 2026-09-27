using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;
using Servicios;

namespace TpIngSoftware
{
    public partial class CambiarClave33ZS : Form, IObservador_33ZS
    {
        private UsuarioBLL_33ZS usuarioBLL = new UsuarioBLL_33ZS();
        private readonly BitacoraEventoBLL_33ZS bitacoraBLL = new BitacoraEventoBLL_33ZS();

        public CambiarClave33ZS()
        {
            InitializeComponent();

            aceptarBTN.Click += aceptarBTN_Click;
            cancelarBTN.Click += cancelarBTN_Click;
            mostrarClavesCHK.CheckedChanged += mostrarClavesCHK_CheckedChanged;
            this.Load += CambiarClave33ZS_Load;

            claveActualTXT.TextChanged += claveTXT_TextChanged;
            claveNuevaTXT.TextChanged += claveTXT_TextChanged;
            claveRepetirTXT.TextChanged += claveTXT_TextChanged;

            this.AcceptButton = aceptarBTN;
            this.CancelButton = cancelarBTN;

            SessionManager_33ZS.GetInstance_33ZS().Suscribir_33ZS(this);
            this.FormClosed += (s, e) =>
                SessionManager_33ZS.GetInstance_33ZS().Desuscribir_33ZS(this);

            Actualizar_33ZS();
        }

        public void Actualizar_33ZS()
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();
            this.Text = idm.Traducir_33ZS("CambiarClave.Titulo");
            label3.Text = idm.Traducir_33ZS("CambiarClave.ContrasenaActual");
            label2.Text = idm.Traducir_33ZS("CambiarClave.NuevaContrasena");
            label1.Text = idm.Traducir_33ZS("CambiarClave.RepetirContrasena");
            label4.Text = idm.Traducir_33ZS("CambiarClave.MensajeTitulo");
            mostrarClavesCHK.Text = idm.Traducir_33ZS("CambiarClave.MostrarContrasenas");
            aceptarBTN.Text = idm.Traducir_33ZS("CambiarClave.Aceptar");
            cancelarBTN.Text = idm.Traducir_33ZS("CambiarClave.Cancelar");
            chkMinimoCaracteres.Text = idm.Traducir_33ZS("CambiarClave.MinCaracteres");
            chkMaximoCaracteres.Text = idm.Traducir_33ZS("CambiarClave.MaxCaracteres");
            chkSinEspacios.Text = idm.Traducir_33ZS("CambiarClave.SinEspacios");
            chkDistintaActual.Text = idm.Traducir_33ZS("CambiarClave.DistintaActual");
            chkCoincideRepeticion.Text = idm.Traducir_33ZS("CambiarClave.CoincideRepeticion");
        }

        private void CambiarClave33ZS_Load(object sender, EventArgs e)
        {
            ConfigurarFormulario_33ZS();
        }
        private void claveTXT_TextChanged(object sender, EventArgs e)
        {
            ActualizarReglasPassword_33ZS();
        }
        private void ActualizarReglasPassword_33ZS()
        {
            string claveActual = claveActualTXT.Text;
            string claveNueva = claveNuevaTXT.Text;
            string claveRepetida = claveRepetirTXT.Text;

            chkMinimoCaracteres.Checked = claveNueva.Length >= 6;

            chkMaximoCaracteres.Checked = claveNueva.Length <= 50 && claveNueva.Length > 0;

            chkSinEspacios.Checked = !claveNueva.Contains(" ") && claveNueva.Length > 0;

            chkDistintaActual.Checked =
                !string.IsNullOrWhiteSpace(claveNueva) &&
                !string.IsNullOrWhiteSpace(claveActual) &&
                claveNueva != claveActual;

            chkCoincideRepeticion.Checked =
                !string.IsNullOrWhiteSpace(claveNueva) &&
                !string.IsNullOrWhiteSpace(claveRepetida) &&
                claveNueva == claveRepetida;

            aceptarBTN.Enabled =
            chkMinimoCaracteres.Checked &&
            chkMaximoCaracteres.Checked &&
            chkSinEspacios.Checked &&
            chkDistintaActual.Checked &&
            chkCoincideRepeticion.Checked;
        }
        private void ConfigurarFormulario_33ZS()
        {
            claveActualTXT.Clear();
            claveNuevaTXT.Clear();
            claveRepetirTXT.Clear();

            claveActualTXT.UseSystemPasswordChar = true;
            claveNuevaTXT.UseSystemPasswordChar = true;
            claveRepetirTXT.UseSystemPasswordChar = true;

            mostrarClavesCHK.Checked = false;

            mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("CambiarClave.MensajeInicial");

            claveActualTXT.Focus();
            ResetearReglasPassword_33ZS();
            aceptarBTN.Enabled = false;
        }
        private void ResetearReglasPassword_33ZS()
        {
            chkMinimoCaracteres.Checked = false;
            chkMaximoCaracteres.Checked = false;
            chkSinEspacios.Checked = false;
            chkDistintaActual.Checked = false;
            chkCoincideRepeticion.Checked = false;
        }
        private void aceptarBTN_Click(object sender, EventArgs e)
        {
            try
            {
                string claveActual = claveActualTXT.Text.Trim();
                string claveNueva = claveNuevaTXT.Text.Trim();
                string claveRepetida = claveRepetirTXT.Text.Trim();

                usuarioBLL.CambiarClave_33ZS(claveActual, claveNueva, claveRepetida);
                string loginSesion = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS;
                RegistrarEventoSeguro_33ZS(loginSesion, TipoEvento_33ZS.Logout, 1);

                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("CambiarClave.MsgExito"));

                SessionManager_33ZS.Logout_33ZS();

                Form formMenu = this.Owner;
                this.Close();

                if (formMenu != null)
                {
                    formMenu.Close();
                }
            }
            catch (Exception ex)
            {
                mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS(ex.Message);
                claveActualTXT.Clear();
                claveNuevaTXT.Clear();
                claveRepetirTXT.Clear();
                claveActualTXT.Focus();
            }
        }

        private void cancelarBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mostrarClavesCHK_CheckedChanged(object sender, EventArgs e)
        {
            bool ocultar = !mostrarClavesCHK.Checked;

            claveActualTXT.UseSystemPasswordChar = ocultar;
            claveNuevaTXT.UseSystemPasswordChar = ocultar;
            claveRepetirTXT.UseSystemPasswordChar = ocultar;
        }

        private void LimpiarCampos_33ZS()
        {
            claveActualTXT.Clear();
            claveNuevaTXT.Clear();
            claveRepetirTXT.Clear();
            ResetearReglasPassword_33ZS();

        }

        private void RegistrarEventoSeguro_33ZS(string login, TipoEvento_33ZS tipoEvento, int criticidad)
        {
            try
            {
                BitacoraEvento_33ZS evento = new BitacoraEvento_33ZS
                {
                    Login_33ZS = login,
                    Fecha_33ZS = DateTime.Today,
                    Hora_33ZS = DateTime.Now,
                    Modulo_33ZS = ModuloSistema_33ZS.Usuario.ToString(),
                    NombreEvento_33ZS = tipoEvento.ToString(),
                    Criticidad_33ZS = criticidad
                };

                bitacoraBLL.RegistrarEvento_33ZS(evento);
            }
            catch
            {
               
            }
        }
    }
}
