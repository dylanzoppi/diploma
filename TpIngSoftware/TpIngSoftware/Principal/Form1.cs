using BLL;
using Servicios;
using System;
using System.Windows.Forms;
using ReaLTaiizor.Forms; 

namespace TpIngSoftware
{

    public partial class Form1 : CrownForm, IObservador_33ZS
    {
        private readonly BitacoraEventoBLL_33ZS bitacoraBLL = new BitacoraEventoBLL_33ZS();

        private readonly PerfilBLL_33ZS perfilBLL = new PerfilBLL_33ZS();
        private readonly UsuarioBLL_33ZS usuarioBLL = new UsuarioBLL_33ZS();
 
        public Form1()
        {
            InitializeComponent();        

            SessionManager_33ZS.GetInstance_33ZS().Suscribir_33ZS(this);
            this.FormClosed += (s, e) =>
                SessionManager_33ZS.GetInstance_33ZS().Desuscribir_33ZS(this);

            Actualizar_33ZS();
        }

        public void Actualizar_33ZS()
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();

            this.Text = idm.Traducir_33ZS("Form1.Titulo");

            usuario33ZSToolStripMenuItem3.Text = idm.Traducir_33ZS("Menu.Usuario");
            admin33ZSToolStripMenuItem2.Text = idm.Traducir_33ZS("Menu.Admin");
            maestros33ZSToolStripMenuItem1.Text = idm.Traducir_33ZS("Menu.Maestros");
            ventas33ZSToolStripMenuItem1.Text = idm.Traducir_33ZS("Menu.Ventas");
            compras33ZSToolStripMenuItem1.Text = idm.Traducir_33ZS("Menu.Compras");
            reportes33ZSToolStripMenuItem1.Text = idm.Traducir_33ZS("Menu.Reportes");
            ayuda33ZSToolStripMenuItem1.Text = idm.Traducir_33ZS("Menu.Ayuda");

            iniciarSesionToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.IniciarSesion");
            cambiarClaveToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.CambiarClave");
            cambiarIdiomaToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.CambiarIdioma");
            cerrarSesionToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.CerrarSesion");
            españolToolStripMenuItem.Text = idm.Traducir_33ZS("Idioma.Espanol");
            inglesToolStripMenuItem.Text = idm.Traducir_33ZS("Idioma.Ingles");
           
            usuariosToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Usuarios");
            perfilesToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Perfiles");
            backupToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Backup");
            restoreToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Restore");
            bitacoraEToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.BitacoraEventos");

            productosToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Productos");
            clienteToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Clientes");
            proveedoresToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Proveedores");
            bitacoraToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Bitacora");

            carritoToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Carrito");
            facturarToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Facturar");

            generarOrdenDeCompraToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.GenerarOrden");
            recepcionProductosToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Recepcion");

            rep1ToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Rep1");
            rep2ToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Rep2");
            rep3ToolStripMenuItem.Text = idm.Traducir_33ZS("Menu.Rep3");
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigurarAccesosPorRol_33ZS();
        }

        private void ConfigurarAccesosPorRol_33ZS()
        {
            string rol = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Rol_33ZS
                : null;

            var patentes = perfilBLL.ObtenerPatentesDeRol_33ZS(rol);

            bool puedeUsuarios = patentes.Contains("AltaUsuario")
                || patentes.Contains("BajaUsuario")
                || patentes.Contains("ModificacionUsuario")
                || patentes.Contains("DesbloquearUsuario");

            usuariosToolStripMenuItem.Enabled = puedeUsuarios;
            usuariosToolStripMenuItem.Visible = puedeUsuarios;


            bool puedePerfiles = patentes.Contains("AltaPerfil")
                || patentes.Contains("BajaPerfil")
                || patentes.Contains("ModificacionPerfil");

            perfilesToolStripMenuItem.Enabled = puedePerfiles;
            perfilesToolStripMenuItem.Visible = puedePerfiles;

            bool puedeBitacora = patentes.Contains("ConsultarBitacora");
            bitacoraEToolStripMenuItem.Enabled = puedeBitacora;
            bitacoraEToolStripMenuItem.Visible = puedeBitacora;

            bool puedeRespaldos = patentes.Contains("GestionRespaldos");
            backupToolStripMenuItem.Enabled = puedeRespaldos;
            backupToolStripMenuItem.Visible = puedeRespaldos;
            restoreToolStripMenuItem.Enabled = puedeRespaldos;
            restoreToolStripMenuItem.Visible = puedeRespaldos;
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Respaldo_33ZS respaldo = new Respaldo_33ZS();
            this.Hide();
            respaldo.ShowDialog();
            this.Show();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Respaldo_33ZS respaldo = new Respaldo_33ZS();
            this.Hide();
            respaldo.ShowDialog();
            this.Show();
        }

        private void uToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login33ZS login = new Login33ZS(false, false);
            login.ShowDialog();
        }

        private void maestrosToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void ventaToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionUsuarios_33ZS gestionUsuarios = new GestionUsuarios_33ZS();
            this.Hide();
            gestionUsuarios.ShowDialog();
            this.Show();
        }

        private void cerrarSesionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBLL_33ZS usuarioBLL = new UsuarioBLL_33ZS();
                usuarioBLL.Logout_33ZS();

                this.Close();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("Form1.ErrorCerrarSesion") + " " + idm.Traducir_33ZS(ex.Message));
            }
        }

        private void cambiarClaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CambiarClave33ZS cambiarClave = new CambiarClave33ZS();
            cambiarClave.ShowDialog(this);
        }

        private void bitacoraEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BitacoraEventos33ZS bitacoraEventos33ZS = new BitacoraEventos33ZS();
            this.Hide();
            bitacoraEventos33ZS.ShowDialog();
            this.Show();
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void asdToolStripMenuItem5_Click(object sender, EventArgs e)
        {

        }

        private void perfilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GestionPerfiles_33ZS gestionPerfiles = new GestionPerfiles_33ZS();
            this.Hide();
            gestionPerfiles.ShowDialog();
            this.Show();
        }

        private void españolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            usuarioBLL.CambiarIdioma_33ZS("ESP");
        }

        private void inglesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            usuarioBLL.CambiarIdioma_33ZS("ENG");
        }
    }
}
