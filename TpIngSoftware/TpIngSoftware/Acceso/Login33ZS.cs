using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TpIngSoftware
{
    public partial class Login33ZS : Form, IObservador_33ZS
    {
        private UsuarioBLL_33ZS usuarioBLL;
        private DigitoVerificadorBLL_33ZS dvBLL;
        private PerfilBLL_33ZS perfilBLL;
        private bool cerrarAplicacionAlCancelar_33ZS = true;
        private bool abrirMenuAlIngresar_33ZS = true;

        public Login33ZS(bool cerrarAplicacionAlCancelar, bool abrirMenuAlIngresar)
        {
            InitializeComponent();
            AplicarDiseno_33ZS();

            cerrarAplicacionAlCancelar_33ZS = cerrarAplicacionAlCancelar;
            abrirMenuAlIngresar_33ZS = abrirMenuAlIngresar;

            ingresarBTN.Click += ingresarBTN_Click;
            cancelarBTN.Click += cancelarBTN_Click;
            mostrarPasswordCHK.CheckedChanged += mostrarPasswordCHK_CheckedChanged;
            this.AcceptButton = ingresarBTN;
            this.CancelButton = cancelarBTN;

            InicializarObservadorIdioma_33ZS();
        }
        public Login33ZS()
        {
            InitializeComponent();
            AplicarDiseno_33ZS();

            ingresarBTN.Click += ingresarBTN_Click;
            cancelarBTN.Click += cancelarBTN_Click;
            mostrarPasswordCHK.CheckedChanged += mostrarPasswordCHK_CheckedChanged;

            this.AcceptButton = ingresarBTN;
            this.CancelButton = cancelarBTN;

            InicializarObservadorIdioma_33ZS();
        }

        private void InicializarObservadorIdioma_33ZS()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            SessionManager_33ZS.GetInstance_33ZS().Suscribir_33ZS(this);
            this.FormClosed += (s, e) =>
                SessionManager_33ZS.GetInstance_33ZS().Desuscribir_33ZS(this);
            Actualizar_33ZS();
        }

        public void Actualizar_33ZS()
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();
            this.Text = idm.Traducir_33ZS("Login.Titulo");
            tituloLBL.Text = idm.Traducir_33ZS("Login.Titulo");
            usuarioLBL.Text = idm.Traducir_33ZS("Login.UsuarioEmail");
            passwordLBL.Text = idm.Traducir_33ZS("Login.Password");
            mostrarPasswordCHK.Text = idm.Traducir_33ZS("Login.MostrarPassword");
            mensajeTituloLBL.Text = idm.Traducir_33ZS("Login.MensajeTitulo");
            ingresarBTN.Text = idm.Traducir_33ZS("Login.Ingresar");
            cancelarBTN.Text = idm.Traducir_33ZS("Login.Cancelar");
        }

        private void Login33ZS_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            usuarioBLL = new UsuarioBLL_33ZS();
            dvBLL = new DigitoVerificadorBLL_33ZS();
            perfilBLL = new PerfilBLL_33ZS();
            ConfigurarLogin_33ZS();
        }

        private void ConfigurarLogin_33ZS()
        {
            usuarioTXT.Clear();
            passworTXT.Clear();

            passworTXT.UseSystemPasswordChar = true;
            mostrarPasswordCHK.Checked = false;

            mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Login.MensajeCredenciales");
            usuarioTXT.Focus();

        }

        private async void ingresarBTN_Click(object sender, EventArgs e)
        {
            try
            {
                ingresarBTN.Enabled = false;
                cancelarBTN.Enabled = false;
                UseWaitCursor = true;

                string login = usuarioTXT.Text.Trim();
                string password = passworTXT.Text.Trim();

                Usuario_33ZS usuario = usuarioBLL.ValidarLogin_33ZS(login, password);

                List<string> inconsistentes = await Task.Run(() => dvBLL.Verificar_33ZS());
                if (inconsistentes.Count > 0)
                {
                    RegistrarEventoInconsistencia_33ZS(usuario, inconsistentes);

                    bool puedeReparar = perfilBLL.ObtenerPatentesDeRol_33ZS(usuario.Rol_33ZS)
                        .Contains("GestionRespaldos");

                    if (puedeReparar)
                    {
                        using (Reparacion_33ZS frmReparacion = new Reparacion_33ZS(inconsistentes))
                        {
                            frmReparacion.ShowDialog();

                            switch (frmReparacion.Resultado)
                            {
                                case Reparacion_33ZS.ResultadoReparacion_33ZS.Recalculado:
                                    mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Login.MsgDvRecalculado");
                                    break;
                                case Reparacion_33ZS.ResultadoReparacion_33ZS.Restaurado:
                                    mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Login.MsgBackupRestaurado");
                                    break;
                                default:
                                    mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Login.MsgAccesoCancelado");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Login.MsgInconsistenciaAdmin");
                    }
                    SessionManager_33ZS.Logout_33ZS();
                    passworTXT.Clear();
                    return;
                }

                RegistrarEventoLoginSeguro_33ZS(usuario);
                SessionManager_33ZS.GetInstance_33ZS().CambiarIdioma_33ZS(usuario.Idioma_33ZS);

                mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Login.MsgIngresoCorrecto");

                if (abrirMenuAlIngresar_33ZS)
                {
                    Form1 menuPrincipal = new Form1();

                    this.Hide();
                    menuPrincipal.ShowDialog();

                    ConfigurarLogin_33ZS();
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                mensajeLBL.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS(error);
                passworTXT.Clear();

                if (error.StartsWith("Usuario.LoginNoExiste") || error.Contains("no existe"))
                {
                    usuarioTXT.Focus();
                    usuarioTXT.SelectAll();
                }
                else
                {
                    passworTXT.Focus();
                }
            }
            finally
            {
                UseWaitCursor = false;
                ingresarBTN.Enabled = true;
                cancelarBTN.Enabled = true;
            }
        }

        private void RegistrarEventoLoginSeguro_33ZS(Usuario_33ZS usuario)
        {
            try
            {
                BitacoraEventoBLL_33ZS bitacoraBLL = new BitacoraEventoBLL_33ZS();
                BitacoraEvento_33ZS evento = new BitacoraEvento_33ZS
                {
                    Login_33ZS = usuario.Login_33ZS,
                    Fecha_33ZS = DateTime.Today,
                    Hora_33ZS = DateTime.Now,
                    Modulo_33ZS = ModuloSistema_33ZS.Usuario.ToString(),
                    NombreEvento_33ZS = TipoEvento_33ZS.Login.ToString(),
                    Criticidad_33ZS = 1
                };

                bitacoraBLL.RegistrarEvento_33ZS(evento);
            }
            catch
            {

            }
        }

        private void RegistrarEventoInconsistencia_33ZS(Usuario_33ZS usuario, List<string> tablas)
        {
            try
            {
                BitacoraEventoBLL_33ZS bitacoraBLL = new BitacoraEventoBLL_33ZS();
                BitacoraEvento_33ZS evento = new BitacoraEvento_33ZS
                {
                    Login_33ZS = usuario.Login_33ZS,
                    Fecha_33ZS = DateTime.Today,
                    Hora_33ZS = DateTime.Now,
                    Modulo_33ZS = ModuloSistema_33ZS.Usuario.ToString(),
                    NombreEvento_33ZS = "InconsistenciaDV (" + tablas.Count + " tablas)",
                    Criticidad_33ZS = 3
                };

                bitacoraBLL.RegistrarEvento_33ZS(evento);
            }
            catch
            {

            }
        }

        private void cancelarBTN_Click(object sender, EventArgs e)
        {
            if (cerrarAplicacionAlCancelar_33ZS)
            {
                Application.Exit();
            }
            else
            {
                this.Close();
            }
        }

        private void mostrarPasswordCHK_CheckedChanged(object sender, EventArgs e)
        {
            passworTXT.UseSystemPasswordChar = !mostrarPasswordCHK.Checked;
        }
    }
}
