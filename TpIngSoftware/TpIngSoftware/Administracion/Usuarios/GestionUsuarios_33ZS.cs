using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TpIngSoftware
{
    public enum ModoFormulario_33ZS
    {
        Filtrar,
        Añadir,
        Modificar,
        Desbloquear,
        ActivarDesactivar
    }

    public partial class GestionUsuarios_33ZS : Form, IObservador_33ZS
    {
        UsuarioBLL_33ZS usuarioBLL = new UsuarioBLL_33ZS();
        BitacoraEventoBLL_33ZS bitacoraBLL = new BitacoraEventoBLL_33ZS();
        PerfilBLL_33ZS perfilBLL = new PerfilBLL_33ZS();

        private ModoFormulario_33ZS modoActual_33ZS = ModoFormulario_33ZS.Filtrar;
        private string _claveMensaje_33ZS = "GestionUsuarios.ModoConsulta";

        public GestionUsuarios_33ZS()
        {
            InitializeComponent();
            modificarBTN.Click += modificarBTN_Click;
            actDesactBTN.Click += activarDesactivarBTN_Click;
            desbloquearBTN.Click += desbloquearBTN_Click;
            salirBTN.Click += salirBTN_Click;
            usuariosDGV.SelectionChanged += usuariosDGV_SelectionChanged;
            radioButton1.CheckedChanged += radioButton1_CheckedChanged;
            radioButton2.CheckedChanged += radioButton2_CheckedChanged;
            usuariosDGV.CellFormatting += usuariosDGV_CellFormatting;

            SessionManager_33ZS.GetInstance_33ZS().Suscribir_33ZS(this);
            this.FormClosed += (s, e) =>
                SessionManager_33ZS.GetInstance_33ZS().Desuscribir_33ZS(this);
        }

        public void Actualizar_33ZS()
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();

            this.Text = idm.Traducir_33ZS("GestionUsuarios.Titulo");
            label12.Text = idm.Traducir_33ZS("GestionUsuarios.TituloGrande");

            añadirBTN.Text = idm.Traducir_33ZS("GestionUsuarios.Anadir");
            desbloquearBTN.Text = idm.Traducir_33ZS("GestionUsuarios.Desbloquear");
            modificarBTN.Text = idm.Traducir_33ZS("GestionUsuarios.Modificar");
            actDesactBTN.Text = idm.Traducir_33ZS("GestionUsuarios.ActDesact");
            aplicarBTN.Text = idm.Traducir_33ZS("GestionUsuarios.Aplicar");
            cancelarBTN.Text = idm.Traducir_33ZS("GestionUsuarios.Cancelar");
            salirBTN.Text = idm.Traducir_33ZS("GestionUsuarios.Salir");

            label1.Text = idm.Traducir_33ZS("GestionUsuarios.DNI");
            label2.Text = idm.Traducir_33ZS("GestionUsuarios.Apellidos");
            label3.Text = idm.Traducir_33ZS("GestionUsuarios.Nombres");
            label4.Text = idm.Traducir_33ZS("GestionUsuarios.Email");
            label5.Text = idm.Traducir_33ZS("GestionUsuarios.Rol");
            label7.Text = idm.Traducir_33ZS("GestionUsuarios.Bloqueado");
            label8.Text = idm.Traducir_33ZS("GestionUsuarios.Activo");
            label9.Text = idm.Traducir_33ZS("GestionUsuarios.MensajeTitulo");

            radioButton1.Text = idm.Traducir_33ZS("GestionUsuarios.Activos");
            radioButton2.Text = idm.Traducir_33ZS("GestionUsuarios.Inactivos");

            ActualizarNumeroUsuarios_33ZS();
            label10.Text = idm.Traducir_33ZS(_claveMensaje_33ZS);
            RefrescarCamposBooleanos_33ZS();
        }

        private string TraducirBooleano_33ZS(bool valor)
        {
            return SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS(
                valor ? "Comun.Si" : "Comun.No");
        }

        private void RefrescarCamposBooleanos_33ZS()
        {
            if (bool.TryParse(bloqueadoBOX.Text, out bool bloqueado))
                bloqueadoBOX.Text = TraducirBooleano_33ZS(bloqueado);

            if (bool.TryParse(activoBOX.Text, out bool activo))
                activoBOX.Text = TraducirBooleano_33ZS(activo);
        }

        private void GestionUsuarios_Load(object sender, EventArgs e)
        {
            ConfigurarGrilla_33ZS();
            CargarRoles_33ZS();
            CargarGrillaActivos_33ZS();
            LimpiarCampos_33ZS();
            HabilitarBotones_33ZS();
            AplicarPermisos_33ZS();
            MostrarMensaje_33ZS("GestionUsuarios.ModoConsulta");
            Actualizar_33ZS();
        }

        private void AplicarPermisos_33ZS()
        {
            string rol = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Rol_33ZS
                : null;

            List<string> patentes = perfilBLL.ObtenerPatentesDeRol_33ZS(rol);

            añadirBTN.Visible = patentes.Contains("AltaUsuario");
            actDesactBTN.Visible = patentes.Contains("BajaUsuario");
            modificarBTN.Visible = patentes.Contains("ModificacionUsuario");
            desbloquearBTN.Visible = patentes.Contains("DesbloquearUsuario");
        }

        private void CargarRoles_33ZS()
        {
            try
            {
                rolBOX.Items.Clear();
                foreach (var rol in perfilBLL.ObtenerRoles_33ZS())
                    rolBOX.Items.Add(rol.Nombre);
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("GestionUsuarios.ErrorCargarRoles") + " " + idm.Traducir_33ZS(ex.Message));
            }
        }
        private void usuariosDGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (usuariosDGV.Rows[e.RowIndex].DataBoundItem is Usuario_33ZS usuario)
            {
                if (!usuario.Activo_33ZS)
                {
                    usuariosDGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                }
                else
                {
                    usuariosDGV.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
        private void ConfigurarGrilla_33ZS()
        {
            usuariosDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            usuariosDGV.MultiSelect = false;
            usuariosDGV.ReadOnly = true;
            usuariosDGV.AutoGenerateColumns = true;
        }

        private void CargarGrilla_33ZS()
        {
            try
            {
                usuariosDGV.DataSource = null;
                usuariosDGV.DataSource = usuarioBLL.ObtenerUsuarios_33ZS();
                OcultarColumnasSensiblesGrilla_33ZS();

                ActualizarNumeroUsuarios_33ZS();
                DeseleccionarGrilla_33ZS();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("GestionUsuarios.ErrorCargarUsuariosDetalle") + " " + idm.Traducir_33ZS(ex.Message));
                MostrarMensaje_33ZS("GestionUsuarios.ErrorCargar");
            }
        }

        private void CargarGrillaActivos_33ZS()
        {
            try
            {
                usuariosDGV.DataSource = null;
                usuariosDGV.DataSource = usuarioBLL.ObtenerUsuariosPorEstado_33ZS(true);
                OcultarColumnasSensiblesGrilla_33ZS();

                ActualizarNumeroUsuarios_33ZS();
                DeseleccionarGrilla_33ZS();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("GestionUsuarios.ErrorFiltrarActivosDetalle") + " " + idm.Traducir_33ZS(ex.Message));
                MostrarMensaje_33ZS("GestionUsuarios.ErrorFiltrarActivos");
            }
        }

        private void CargarGrillaInactivos_33ZS()
        {
            try
            {
                usuariosDGV.DataSource = null;
                usuariosDGV.DataSource = usuarioBLL.ObtenerUsuariosPorEstado_33ZS(false);
                OcultarColumnasSensiblesGrilla_33ZS();

                ActualizarNumeroUsuarios_33ZS();
                DeseleccionarGrilla_33ZS();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("GestionUsuarios.ErrorFiltrarInactivosDetalle") + " " + idm.Traducir_33ZS(ex.Message));
                MostrarMensaje_33ZS("GestionUsuarios.ErrorFiltrarInactivos");
            }
        }

        private void ActualizarNumeroUsuarios_33ZS()
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();

            if (usuariosDGV.DataSource == null)
            {
                label11.Text = idm.Traducir_33ZS("GestionUsuarios.NumeroUsuarios") + "0";
                return;
            }

            label11.Text = idm.Traducir_33ZS("GestionUsuarios.NumeroUsuarios") + usuariosDGV.Rows.Count;
        }

        private void añadirBTN_Click(object sender, EventArgs e)
        {
            LimpiarCampos_33ZS();
            DeseleccionarGrilla_33ZS();

            EntrarEnModoOperacion_33ZS(ModoFormulario_33ZS.Añadir);

            DNIBOX.Enabled = true;
            ApellidosBOX.Enabled = true;
            nombresBOX.Enabled = true;
            emailBOX.Enabled = true;
            rolBOX.Enabled = true;
            bloqueadoBOX.Text = TraducirBooleano_33ZS(false);
            activoBOX.Text = TraducirBooleano_33ZS(true);
            bloqueadoBOX.Enabled = false;
            activoBOX.Enabled = false;

            MostrarMensaje_33ZS("GestionUsuarios.ModoAnadir");
        }

        private void modificarBTN_Click(object sender, EventArgs e)
        {
            if (!HayUsuarioSeleccionado_33ZS())
                return;

            EntrarEnModoOperacion_33ZS(ModoFormulario_33ZS.Modificar);

            DNIBOX.Enabled = false;
            ApellidosBOX.Enabled = true;
            nombresBOX.Enabled = true;
            emailBOX.Enabled = true;
            rolBOX.Enabled = true;
            bloqueadoBOX.Enabled = false;
            activoBOX.Enabled = false;

            MostrarMensaje_33ZS("GestionUsuarios.ModoModificar");
        }

        private void desbloquearBTN_Click(object sender, EventArgs e)
        {
            if (!HayUsuarioSeleccionado_33ZS())
                return;

            EntrarEnModoOperacion_33ZS(ModoFormulario_33ZS.Desbloquear);

            DNIBOX.Enabled = false;
            ApellidosBOX.Enabled = false;
            nombresBOX.Enabled = false;
            emailBOX.Enabled = false;
            rolBOX.Enabled = false;
            bloqueadoBOX.Enabled = false;
            activoBOX.Enabled = false;

            MostrarMensaje_33ZS("GestionUsuarios.ModoDesbloquear");
        }

        private void activarDesactivarBTN_Click(object sender, EventArgs e)
        {
            if (!HayUsuarioSeleccionado_33ZS())
                return;

            EntrarEnModoOperacion_33ZS(ModoFormulario_33ZS.ActivarDesactivar);

            DNIBOX.Enabled = false;
            ApellidosBOX.Enabled = false;
            nombresBOX.Enabled = false;
            emailBOX.Enabled = false;
            rolBOX.Enabled = false;
            bloqueadoBOX.Enabled = false;
            activoBOX.Enabled = false;

            MostrarMensaje_33ZS("GestionUsuarios.ModoActDesact");
        }

        private void aplicarBTN_Click(object sender, EventArgs e)
        {
            if (modoActual_33ZS == ModoFormulario_33ZS.Filtrar)
            {
                MostrarMensaje_33ZS("GestionUsuarios.SeleccioneOperacion");
                return;
            }
            try
            {
                switch (modoActual_33ZS)
                {
                    case ModoFormulario_33ZS.Añadir:
                        AgregarUsuarioDesdePantalla_33ZS();
                        MostrarMensaje_33ZS("GestionUsuarios.UsuarioAgregado");
                        break;

                    case ModoFormulario_33ZS.Modificar:
                        ModificarUsuarioDesdePantalla_33ZS();
                        MostrarMensaje_33ZS("GestionUsuarios.UsuarioModificado");
                        break;

                    case ModoFormulario_33ZS.Desbloquear:
                        usuarioBLL.DesbloquearUsuario_33ZS(DNIBOX.Text.Trim());
                        MostrarMensaje_33ZS("GestionUsuarios.UsuarioDesbloqueado");
                        break;

                    case ModoFormulario_33ZS.ActivarDesactivar:
                        usuarioBLL.CambiarEstadoUsuario_33ZS(DNIBOX.Text.Trim());
                        MostrarMensaje_33ZS("GestionUsuarios.EstadoActualizado");
                        break;

                    case ModoFormulario_33ZS.Filtrar:
                    default:
                        MostrarMensaje_33ZS("GestionUsuarios.SeleccioneOperacion");
                        return;
                }

                CargarGrilla_33ZS();
                LimpiarCampos_33ZS();
                HabilitarBotones_33ZS();
                DeseleccionarGrilla_33ZS();
                aplicarBTN.Enabled = false;
                cancelarBTN.Enabled = false;
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                string mensaje = idm.Traducir_33ZS(ex.Message);
                MessageBox.Show(idm.Traducir_33ZS("GestionUsuarios.ErrorGenerico") + " " + mensaje);
                MostrarMensaje_33ZS(idm.Traducir_33ZS("GestionUsuarios.ErrorGenerico") + " " + mensaje);
            }
        }

        private void AgregarUsuarioDesdePantalla_33ZS()
        {
            string dni = DNIBOX.Text.Trim();
            string apellidos = ApellidosBOX.Text.Trim();
            string nombres = nombresBOX.Text.Trim();
            string email = emailBOX.Text.Trim();
            string rol = rolBOX.Text.Trim();
            string login = email;

            string apellidoParaPassword = apellidos.Replace(" ", "");

            if (string.IsNullOrWhiteSpace(dni) || string.IsNullOrWhiteSpace(apellidoParaPassword))
                throw new Exception("GestionUsuarios.ErrorGenerarPassword");

            string passwordGenerado = dni + apellidoParaPassword;

            bool bloqueado = false;
            bool activo = true;

            Usuario_33ZS nuevoUsuario = new Usuario_33ZS(
                dni,
                apellidos,
                nombres,
                login,
                passwordGenerado,
                rol,
                email,
                bloqueado,
                activo
            );

            usuarioBLL.AgregarUsuario_33ZS(nuevoUsuario);
        }

        private void ModificarUsuarioDesdePantalla_33ZS()
        {
            string dni = DNIBOX.Text.Trim();
            string apellidos = ApellidosBOX.Text.Trim();
            string nombres = nombresBOX.Text.Trim();
            string email = emailBOX.Text.Trim();
            string rol = rolBOX.Text.Trim();

            bool bloqueado = ConvertirBooleano_33ZS(bloqueadoBOX.Text);
            bool activo = ConvertirBooleano_33ZS(activoBOX.Text);

            string login = email;

            Usuario_33ZS usuario = new Usuario_33ZS(
                dni,
                apellidos,
                nombres,
                login,
                string.Empty,
                rol,
                email,
                bloqueado,
                activo
            );

            usuarioBLL.ModificarUsuario_33ZS(usuario);
        }

        private bool ConvertirBooleano_33ZS(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return false;

            valor = valor.Trim().ToLower();

            if (valor == "1" || valor == "true" || valor == "verdadero" || valor == "sí" || valor == "si")
                return true;

            if (valor == "0" || valor == "false" || valor == "falso" || valor == "no")
                return false;

            throw new Exception("GestionUsuarios.ErrorBool");
        }
        private void DeseleccionarGrilla_33ZS()
        {
            usuariosDGV.ClearSelection();
            usuariosDGV.CurrentCell = null;
        }
        private bool HayUsuarioSeleccionado_33ZS()
        {
            if (usuariosDGV.Rows.Count == 0)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionUsuarios.NoHayUsuarios"));
                return false;
            }

            if (usuariosDGV.CurrentRow == null)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionUsuarios.SeleccioneUsuarioGrilla"));
                return false;
            }

            if (usuariosDGV.CurrentRow.IsNewRow)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionUsuarios.FilaNoValida"));
                return false;
            }

            if (usuariosDGV.CurrentRow.DataBoundItem == null)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionUsuarios.SeleccioneUsuarioValido"));
                return false;
            }

            Usuario_33ZS usuario = usuariosDGV.CurrentRow.DataBoundItem as Usuario_33ZS;

            if (usuario == null)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionUsuarios.FilaNoCorresponde"));
                return false;
            }

            if (string.IsNullOrWhiteSpace(usuario.DNI_33ZS))
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionUsuarios.UsuarioSinDni"));
                return false;
            }

            return true;
        }

        private void usuariosDGV_SelectionChanged(object sender, EventArgs e)
        {
            if (modoActual_33ZS != ModoFormulario_33ZS.Filtrar)
                return;

            modificarBTN.Enabled = false;
            actDesactBTN.Enabled = false;
            desbloquearBTN.Enabled = false;

            if (usuariosDGV.CurrentRow == null)
                return;

            if (usuariosDGV.CurrentRow.IsNewRow)
                return;

            if (usuariosDGV.CurrentRow.DataBoundItem == null)
                return;

            Usuario_33ZS usuario = usuariosDGV.CurrentRow.DataBoundItem as Usuario_33ZS;

            if (usuario == null)
                return;

            DNIBOX.Text = usuario.DNI_33ZS ?? string.Empty;
            ApellidosBOX.Text = usuario.Apellidos_33ZS ?? string.Empty;
            nombresBOX.Text = usuario.Nombre_33ZS ?? string.Empty;
            emailBOX.Text = usuario.Email_33ZS ?? string.Empty;
            rolBOX.SelectedIndex = rolBOX.FindStringExact(usuario.Rol_33ZS);
            bloqueadoBOX.Text = TraducirBooleano_33ZS(usuario.Bloqueo_33ZS);
            activoBOX.Text = TraducirBooleano_33ZS(usuario.Activo_33ZS);

            modificarBTN.Enabled = true;
            actDesactBTN.Enabled = true;

            desbloquearBTN.Enabled = usuario.Bloqueo_33ZS;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                CargarGrillaActivos_33ZS();
                MostrarMensaje_33ZS("GestionUsuarios.FiltroActivos");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                CargarGrillaInactivos_33ZS();
                MostrarMensaje_33ZS("GestionUsuarios.FiltroInactivos");
            }
        }

        private void LimpiarCampos_33ZS()
        {
            DNIBOX.Clear();
            ApellidosBOX.Clear();
            nombresBOX.Clear();
            emailBOX.Clear();
            rolBOX.SelectedIndex = -1;
            bloqueadoBOX.Clear();
            activoBOX.Clear();
        }

        private void HabilitarBotones_33ZS()
        {
            modoActual_33ZS = ModoFormulario_33ZS.Filtrar;

            añadirBTN.Enabled = true;

            modificarBTN.Enabled = false;
            actDesactBTN.Enabled = false;
            desbloquearBTN.Enabled = false;

            aplicarBTN.Enabled = false;
            cancelarBTN.Enabled = false;

            radioButton1.Enabled = true;
            radioButton2.Enabled = true;

            DNIBOX.Enabled = true;
            ApellidosBOX.Enabled = true;
            nombresBOX.Enabled = true;
            emailBOX.Enabled = true;
            rolBOX.Enabled = true;
            bloqueadoBOX.Enabled = false;
            activoBOX.Enabled = false;
            MostrarMensaje_33ZS("GestionUsuarios.ModoConsulta");

            DeseleccionarGrilla_33ZS();
        }
        private void EntrarEnModoOperacion_33ZS(ModoFormulario_33ZS modo)
        {
            modoActual_33ZS = modo;

            añadirBTN.Enabled = false;
            modificarBTN.Enabled = false;
            actDesactBTN.Enabled = false;
            desbloquearBTN.Enabled = false;


            radioButton1.Enabled = false;
            radioButton2.Enabled = false;

            aplicarBTN.Enabled = true;
            cancelarBTN.Enabled = true;
        }
        private void cancelarBTN_Click(object sender, EventArgs e)
        {
            LimpiarCampos_33ZS();
            HabilitarBotones_33ZS();
            MostrarMensaje_33ZS("GestionUsuarios.OperacionCancelada");
        }

        private void salirBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MostrarMensaje_33ZS(string clave)
        {
            _claveMensaje_33ZS = clave;
            label10.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS(clave);
        }

        private void OcultarColumnasSensiblesGrilla_33ZS()
        {
            if (usuariosDGV.Columns["Password_33ZS"] != null)
                usuariosDGV.Columns["Password_33ZS"].Visible = false;
        }
       
    }
}
