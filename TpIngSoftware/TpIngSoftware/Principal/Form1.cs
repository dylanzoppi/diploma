using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BLL;
using BLL.PN1;
using Servicios;
using TpIngSoftware.PN1;

namespace TpIngSoftware
{
    public partial class Form1 : Form
    {
        private PN1BLL_33ZS negocio;

        private static string T(string espanol, string ingles) => EstiloPN1_33ZS.T(espanol, ingles);

        public Form1()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            negocio = new PN1BLL_33ZS();
            idiomaBoton.Click += (s, e) => CambiarIdioma();
            claveBoton.Click += (s, e) => Abrir(new CambiarClave33ZS());
            salirBoton.Click += (s, e) => CerrarSesion();
            Load += (s, e) => Configurar();
            FormClosed += (s, e) =>
            {
                if (SessionManager_33ZS.HaySesionActiva_33ZS())
                    try { new UsuarioBLL_33ZS().Logout_33ZS(); }
                    catch { SessionManager_33ZS.Logout_33ZS(); }
            };
        }

        private static Button BotonLateral(string texto, Action accion)
        {
            var boton = new Button { Text = texto, Width = 175, Height = 45,
                FlatStyle = FlatStyle.Flat, BackColor = EstiloPN1_33ZS.Tinta,
                ForeColor = Color.White, TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.Hand };
            boton.FlatAppearance.BorderSize = 0;
            boton.Click += (s, e) => accion();
            return boton;
        }

        private void Agregar(string texto, string descripcion, Action accion)
        {
            navegacion.Controls.Add(BotonLateral(texto, accion));
            var tarjeta = new Button { Text = texto + Environment.NewLine + descripcion,
                Width = 255, Height = 115, Margin = new Padding(0, 0, 18, 18),
                FlatStyle = FlatStyle.Flat, BackColor = Color.White,
                ForeColor = EstiloPN1_33ZS.Tinta, TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16), Cursor = Cursors.Hand };
            tarjeta.FlatAppearance.BorderColor = Color.FromArgb(222, 216, 209);
            tarjeta.Click += (s, e) => accion();
            accesos.Controls.Add(tarjeta);
        }

        private bool TienePatente(string patente)
        {
            var usuario = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;
            return usuario != null && usuario.Activo_33ZS && !usuario.Bloqueo_33ZS &&
                new PerfilBLL_33ZS().RolTienePatente_33ZS(usuario.Rol_33ZS, patente);
        }

        private void AgregarConPatente(string texto, string descripcion, string patente, Action accion)
        {
            if (TienePatente(patente)) Agregar(texto, descripcion, accion);
        }

        private void Configurar()
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            var usuario = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS;
            if (usuario == null) { Close(); return; }
            bool ingles = SessionManager_33ZS.GetInstance_33ZS().GetIdiomaActual_33ZS() == "ENG";
            Text = ingles ? "Harlem · Barbershop management" : "Harlem · Gestión de barbería";
            titulo.Text = (ingles ? "Hello, " : "Hola, ") + usuario.Nombre_33ZS;
            subtitulo.Text = RolTexto(usuario.Rol_33ZS, ingles) + " · " + DateTime.Today.ToString("D",
                CultureInfo.GetCultureInfo(ingles ? "en-US" : "es-AR"));
            accesosTitulo.Text = T("ACCESOS DISPONIBLES", "AVAILABLE ACTIONS");
            claveBoton.Text = T("Cambiar contraseña", "Change password");
            salirBoton.Text = T("Cerrar sesión", "Sign out");
            navegacion.Controls.Clear();
            accesos.Controls.Clear();
            AgregarConPatente(T("Nueva atención", "New service"), T("Registrar servicio y cobro", "Record service and payment"), "RegistrarAtencion",
                () => Abrir(new NuevaAtencionForm_33ZS()));
            AgregarConPatente(T("Clientes", "Customers"), T("Buscar y registrar clientes", "Find and register customers"), "BuscarCliente",
                () => Abrir(new ClientesForm_33ZS()));
            if (usuario.Rol_33ZS == "Barbero")
                AgregarConPatente(T("Mi historial", "My history"), T("Ver mis servicios y comisiones", "View my services and commissions"), "ConsultarAtencionesPropias",
                    () => Abrir(new HistorialForm_33ZS(false)));
            AgregarConPatente(T("Atenciones", "Services"), T("Consultar actividad del negocio", "View shop activity"), "ConsultarAtencionesGenerales",
                () => Abrir(new HistorialForm_33ZS(true)));
            AgregarConPatente(T("Barberos", "Barbers"), T("Configurar comisión y disponibilidad", "Set commission and availability"), "GestionarBarberos",
                () => Abrir(new BarberosForm_33ZS()));
            AgregarConPatente(T("Catálogo y stock", "Services and stock"), T("Precios, insumos y existencias", "Prices, supplies and stock"), "GestionarCatalogo",
                () => Abrir(new CatalogoForm_33ZS()));
            if (TienePatente("AltaUsuario") || TienePatente("ModificacionUsuario"))
                Agregar(T("Usuarios", "Users"), T("Cuentas y roles", "Accounts and roles"), () => Abrir(new GestionUsuarios_33ZS()));
            if (TienePatente("AltaPerfil") || TienePatente("ModificacionPerfil"))
                Agregar(T("Perfiles", "Profiles"), T("Patentes y permisos", "Permissions"), () => Abrir(new GestionPerfiles_33ZS()));
            AgregarConPatente(T("Bitácora", "Event log"), T("Eventos de seguridad", "Security events"), "ConsultarBitacora",
                () => Abrir(new BitacoraEventos33ZS()));
            AgregarConPatente(T("Respaldos", "Backups"), T("Copia y restauración", "Backup and restore"), "GestionRespaldos",
                () => Abrir(new Respaldo_33ZS()));
            if (accesos.Controls.Count == 0)
                accesos.Controls.Add(new Label { Text = T("Este rol no tiene acciones disponibles.", "This role has no available actions."), AutoSize = true });
        }

        private void Abrir(Form ventana)
        {
            using (ventana) ventana.ShowDialog(this);
            Configurar();
        }

        private void CerrarSesion()
        {
            try { new UsuarioBLL_33ZS().Logout_33ZS(); Close(); }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private void CambiarIdioma()
        {
            try
            {
                var sesion = SessionManager_33ZS.GetInstance_33ZS();
                new UsuarioBLL_33ZS().CambiarIdioma_33ZS(
                    sesion.GetIdiomaActual_33ZS() == "ESP" ? "ENG" : "ESP");
                Configurar();
            }
            catch (Exception ex) { EstiloPN1_33ZS.Error(this, ex); }
        }

        private static string RolTexto(string rol, bool ingles)
        {
            if (!ingles) return rol;
            switch (rol)
            {
                case "Barbero": return "Barber";
                case "Recepcionista": return "Receptionist";
                case "Dueño": return "Owner";
                case "Administrador": return "Administrator";
                default: return rol;
            }
        }
    }
}
