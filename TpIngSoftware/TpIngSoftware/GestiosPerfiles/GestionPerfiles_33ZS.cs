using BLL;
using Servicios;
using Servicios.Composite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TpIngSoftware.GestiosPerfiles;

namespace TpIngSoftware
{
    public partial class GestionPerfiles_33ZS : Form, IObservador_33ZS
    {
        private readonly PerfilBLL_33ZS bll = new PerfilBLL_33ZS();
        private List<Componente_33ZS> datosPatentes = new List<Componente_33ZS>();
        private List<Componente_33ZS> datosFamilias = new List<Componente_33ZS>();
        private List<Componente_33ZS> datosRoles = new List<Componente_33ZS>();
        private readonly ImageList iconosArbol_33ZS = new ImageList();

        public GestionPerfiles_33ZS()
        {
            InitializeComponent();
            ConfigurarIconosArbol_33ZS();
            ConfigurarFiltroInicial_33ZS();
            InicializarEventos();
            CargarDatosDB();
            AplicarPermisos_33ZS();

            SessionManager_33ZS.GetInstance_33ZS().Suscribir_33ZS(this);
            this.FormClosed += (s, e) =>
                SessionManager_33ZS.GetInstance_33ZS().Desuscribir_33ZS(this);

            Actualizar_33ZS();
        }

        public void Actualizar_33ZS()
        {
            var idm = SessionManager_33ZS.GetInstance_33ZS();

            this.Text = idm.Traducir_33ZS("GestionPerfiles.Titulo");
            bigLabel5.Text = idm.Traducir_33ZS("GestionPerfiles.Crear");
            bigLabel1.Text = idm.Traducir_33ZS("GestionPerfiles.Nombre");
            bigLabel2.Text = idm.Traducir_33ZS("GestionPerfiles.FamiliaRolCrear");
            bigLabel6.Text = idm.Traducir_33ZS("GestionPerfiles.FamiliaRolCrear");
            biglabel55.Text = idm.Traducir_33ZS("GestionPerfiles.PatentesDisponibles");
            bigLabel7.Text = idm.Traducir_33ZS("GestionPerfiles.RolesCreados");
            bigLabel8.Text = idm.Traducir_33ZS("GestionPerfiles.PatentesDeRol");
            bigLabel10.Text = idm.Traducir_33ZS("GestionPerfiles.ModificarEliminar");
            familiaRDBTN.Text = idm.Traducir_33ZS("GestionPerfiles.Familia");
            rolRDBTN.Text = idm.Traducir_33ZS("GestionPerfiles.Rol");
            confirmarBTN.Text = idm.Traducir_33ZS("GestionPerfiles.Confirmar");
            button1.Text = idm.Traducir_33ZS("GestionPerfiles.BtnModificarEliminar");
        }

        private void AplicarPermisos_33ZS()
        {
            string rol = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Rol_33ZS
                : null;

            List<string> patentes = bll.ObtenerPatentesDeRol_33ZS(rol);

            confirmarBTN.Visible = patentes.Contains("AltaPerfil");
            button1.Visible = patentes.Contains("ModificacionPerfil") || patentes.Contains("BajaPerfil");
        }

        private void InicializarEventos()
        {
            listRoles.SelectedIndexChanged += listRoles_SelectedIndexChanged;
            listFamilias.NodeMouseClick += listFamilias_NodeMouseClick;
            listPatentes.NodeMouseClick += listPatentes_NodeMouseClick;
        }

        private void ConfigurarFiltroInicial_33ZS()
        {
            familiaRDBTN.Checked = true;
            rolRDBTN.Checked = false;
        }

        private void ConfigurarIconosArbol_33ZS()
        {
            iconosArbol_33ZS.ImageSize = new Size(16, 16);
            iconosArbol_33ZS.ColorDepth = ColorDepth.Depth32Bit;
            iconosArbol_33ZS.Images.Add("familia", CrearIconoFamilia_33ZS());
            iconosArbol_33ZS.Images.Add("patente", CrearIconoPatente_33ZS());

            listPatentes.ImageList = iconosArbol_33ZS;
            listPatentesRoles.ImageList = iconosArbol_33ZS;
            listPatentes.ShowNodeToolTips = true;
            listFamilias.ShowNodeToolTips = true;
            listPatentesRoles.ShowNodeToolTips = true;
        }

        private Bitmap CrearIconoFamilia_33ZS()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush fondo = new SolidBrush(Color.FromArgb(65, 105, 225)))
            using (SolidBrush solapa = new SolidBrush(Color.FromArgb(120, 149, 255)))
            using (Pen borde = new Pen(Color.FromArgb(40, 72, 160)))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.FillRectangle(solapa, 2, 3, 6, 3);
                g.FillRectangle(fondo, 2, 5, 12, 8);
                g.DrawRectangle(borde, 2, 5, 11, 7);
            }
            return bmp;
        }

        private Bitmap CrearIconoPatente_33ZS()
        {
            Bitmap bmp = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bmp))
            using (SolidBrush fondo = new SolidBrush(Color.White))
            using (Pen borde = new Pen(Color.Gray))
            using (Pen linea = new Pen(Color.LightGray))
            {
                g.FillRectangle(fondo, 3, 2, 10, 12);
                g.DrawRectangle(borde, 3, 2, 9, 11);
                g.DrawLine(linea, 5, 5, 11, 5);
                g.DrawLine(linea, 5, 8, 11, 8);
                g.DrawLine(linea, 5, 11, 11, 11);
            }
            return bmp;
        }

        private void listComponentes_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ListBox listBox = (ListBox)sender;
            Componente_33ZS componente = listBox.Items[e.Index] as Componente_33ZS;
            if (componente == null)
                return;

            e.DrawBackground();

            bool esFamilia = componente is Familia_33ZS;
            Font font = esFamilia
                ? new Font(e.Font, FontStyle.Bold)
                : e.Font;

            Color color;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                color = SystemColors.HighlightText;
            }
            else
            {
                color = esFamilia ? Color.RoyalBlue : e.ForeColor;
            }

            TextRenderer.DrawText(
                e.Graphics,
                componente.Nombre,
                font,
                e.Bounds,
                color,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

            if (!ReferenceEquals(font, e.Font))
                font.Dispose();

            e.DrawFocusRectangle();
        }

        private void CargarDatosDB()
        {
            try
            {
                datosFamilias.Clear();
                datosPatentes.Clear();
                datosPatentes.AddRange(bll.ObtenerPatentes_33ZS());
                datosPatentes.AddRange(bll.ObtenerFamilias_33ZS());
                datosRoles.Clear();
                datosRoles.AddRange(bll.ObtenerRoles_33ZS());
                ActualizarComponentes();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("GestionPerfiles.ErrorCargarDatos") + " " + idm.Traducir_33ZS(ex.Message));
            }
        }

        private void ActualizarComponentesDiferido_33ZS()
        {
            if (IsDisposed || !IsHandleCreated)
                return;

            BeginInvoke((Action)ActualizarComponentes);
        }

        private void ActualizarComponentes()
        {
            listPatentes.Nodes.Clear();
            foreach (Componente_33ZS item in datosPatentes)
                AgregarNodoDisponible_33ZS(listPatentes.Nodes, item);
            listPatentes.CollapseAll();

            listFamilias.Nodes.Clear();
            foreach (Componente_33ZS item in datosFamilias)
                AgregarNodo_33ZS(listFamilias.Nodes, item);
            listFamilias.ExpandAll();

            listRoles.DataSource = null;
            listRoles.DataSource = datosRoles;
            listRoles.DisplayMember = "Nombre";
        }

        private void AgregarNodoDisponible_33ZS(TreeNodeCollection nodos, Componente_33ZS componente)
        {
            TreeNode nodo = nodos.Add(componente.Nombre);
            nodo.Tag = componente;
            nodo.ToolTipText = componente.Nombre;

            if (componente is Familia_33ZS familia)
            {
                nodo.ForeColor = Color.RoyalBlue;
                nodo.ImageKey = "familia";
                nodo.SelectedImageKey = "familia";

                foreach (Componente_33ZS hijo in familia.ObtenerSubComponentes_33ZS())
                    AgregarNodoDisponible_33ZS(nodo.Nodes, hijo);
            }
            else
            {
                nodo.ImageKey = "patente";
                nodo.SelectedImageKey = "patente";
            }
        }

        private void ResaltarConflicto_33ZS(string nombrePatente)
        {
            LimpiarResaltadoNodos_33ZS(listFamilias.Nodes);
            listFamilias.SelectedNode = null;

            if (string.IsNullOrWhiteSpace(nombrePatente))
                return;

            foreach (Componente_33ZS componente in datosFamilias)
            {
                if (bll.UsuarioTienePatente_33ZS(componente, nombrePatente))
                {
                    TreeNode nodoSeleccionado = BuscarNodoPorTexto_33ZS(listFamilias.Nodes, componente.Nombre);
                    if (nodoSeleccionado != null)
                    {
                        listFamilias.SelectedNode = nodoSeleccionado;
                        nodoSeleccionado.BackColor = System.Drawing.Color.LightSalmon;
                        nodoSeleccionado.ForeColor = System.Drawing.Color.Black;
                        nodoSeleccionado.EnsureVisible();
                    }
                    break;
                }
            }

            listFamilias.Focus();
        }

        private void LimpiarResaltadoNodos_33ZS(TreeNodeCollection nodos)
        {
            foreach (TreeNode nodo in nodos)
            {
                nodo.BackColor = System.Drawing.Color.White;
                nodo.ForeColor = System.Drawing.Color.Black;
                LimpiarResaltadoNodos_33ZS(nodo.Nodes);
            }
        }

        private TreeNode BuscarNodoPorTexto_33ZS(TreeNodeCollection nodos, string texto)
        {
            foreach (TreeNode nodo in nodos)
            {
                if (nodo.Text.Equals(texto, StringComparison.OrdinalIgnoreCase))
                    return nodo;

                TreeNode nodoHijo = BuscarNodoPorTexto_33ZS(nodo.Nodes, texto);
                if (nodoHijo != null)
                    return nodoHijo;
            }

            return null;
        }

        private Componente_33ZS ObtenerComponenteRaizDesdeNodo_33ZS(TreeNode nodo)
        {
            if (nodo == null)
                return null;

            while (nodo.Parent != null)
                nodo = nodo.Parent;

            return nodo.Tag as Componente_33ZS;
        }

        private void AgregarNodo_33ZS(TreeNodeCollection nodos, Componente_33ZS componente)
        {
            TreeNode nodo = nodos.Add(componente.Nombre);
            nodo.Tag = componente;
            nodo.ToolTipText = componente.Nombre;
            if (componente is Familia_33ZS familia)
            {
                nodo.ForeColor = Color.RoyalBlue;
                nodo.ImageKey = "familia";
                nodo.SelectedImageKey = "familia";

                foreach (Componente_33ZS hijo in familia.ObtenerSubComponentes_33ZS())
                    AgregarNodo_33ZS(nodo.Nodes, hijo);
            }
            else
            {
                nodo.ImageKey = "patente";
                nodo.SelectedImageKey = "patente";
            }
        }


        private void confirmarBTN_Click(object sender, EventArgs e)
        {
            if (!familiaRDBTN.Checked && !rolRDBTN.Checked)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.SeleccioneTipo"));
                return;
            }
            string tipo = familiaRDBTN.Checked ? "familia" : "rol";
            if (string.IsNullOrWhiteSpace(nombreFamiliaTXT.Text))
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.IngreseNombre") + " " + tipo + ".");
                return;
            }
            if (datosFamilias.Count == 0)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.AgregueComponente") + " " + tipo + ".");
                return;
            }

            try
            {
                List<Componente_33ZS> subComponentes = new List<Componente_33ZS>();
                subComponentes.AddRange(datosFamilias);

                Familia_33ZS nuevo = familiaRDBTN.Checked
                    ? bll.GuardarFamilia_33ZS(nombreFamiliaTXT.Text, subComponentes)
                    : bll.GuardarRol_33ZS(nombreFamiliaTXT.Text, subComponentes);

                if (familiaRDBTN.Checked)
                    datosPatentes.Add(nuevo);
                else
                    datosRoles.Add(nuevo);

                datosPatentes.AddRange(datosFamilias);
                datosFamilias.Clear();
                nombreFamiliaTXT.Text = "";
                ActualizarComponentes();
                MessageBox.Show(tipo + " " + SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.GuardadoExito"));
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("GestionPerfiles.ErrorGuardar") + " " + tipo + ": " + idm.Traducir_33ZS(ex.Message));
            }
        }

        private void listPatentes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewHitTestInfo hit = listPatentes.HitTest(e.Location);
            if (hit.Location == TreeViewHitTestLocations.PlusMinus)
                return;

            if (e.Node?.Tag is Componente_33ZS itemSeleccionado)
            {
                listPatentes.SelectedNode = e.Node;

                if (bll.PatenteYaIncluida_33ZS(itemSeleccionado, datosFamilias))
                {
                    string patenteDuplicada = bll.ObtenerPatenteDuplicada_33ZS(itemSeleccionado, datosFamilias);
                    string nombreElemento = itemSeleccionado.Nombre;

                    MessageBox.Show(
                        patenteDuplicada == null
                            ? SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.ElementoYaIncluido").Replace("{0}", nombreElemento)
                            : SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.ElementoYaIncluidoPatente").Replace("{0}", nombreElemento).Replace("{1}", patenteDuplicada));

                    ResaltarConflicto_33ZS(patenteDuplicada);
                    return;
                }

                datosFamilias.Add(itemSeleccionado);
                datosPatentes.Remove(itemSeleccionado);

                ActualizarComponentesDiferido_33ZS();
            }
            else
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.SeleccionePatente"));
            }
        }

        private void listFamilias_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewHitTestInfo hit = listFamilias.HitTest(e.Location);
            if (hit.Location == TreeViewHitTestLocations.PlusMinus)
                return;

            if (e.Node?.Tag is Componente_33ZS itemSeleccionado)
            {
                listFamilias.SelectedNode = e.Node;

                Componente_33ZS componenteRaiz = ObtenerComponenteRaizDesdeNodo_33ZS(e.Node);
                if (componenteRaiz == null)
                {
                    MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.NoSePudoIdentificar"));
                    return;
                }

                datosPatentes.Add(componenteRaiz);
                datosFamilias.Remove(componenteRaiz);

                ActualizarComponentesDiferido_33ZS();
            }
            else
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("GestionPerfiles.SeleccioneComponenteQuitar"));
            }
        }

        private void listRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listRoles.SelectedItem is Familia_33ZS rolSeleccionado)
            {
                listPatentesRoles.Nodes.Clear();
                AgregarNodo_33ZS(listPatentesRoles.Nodes, rolSeleccionado);
                listPatentesRoles.ExpandAll();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text?.Trim() ?? string.Empty;
            listPatentes.Nodes.Clear();

            foreach (Componente_33ZS item in datosPatentes)
                AgregarNodoFiltrado_33ZS(listPatentes.Nodes, item, filtro);

            if (string.IsNullOrEmpty(filtro))
                listPatentes.CollapseAll();
            else
                listPatentes.ExpandAll();
        }

        private bool AgregarNodoFiltrado_33ZS(TreeNodeCollection nodos, Componente_33ZS componente, string filtro)
        {
            bool coincide = string.IsNullOrEmpty(filtro)
                || componente.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0;

            bool algunHijoCoincide = false;

            if (componente is Familia_33ZS familia)
            {
                List<TreeNode> hijosVisibles = new List<TreeNode>();
                foreach (Componente_33ZS hijo in familia.ObtenerSubComponentes_33ZS())
                {
                    TreeNode nodoHijo = CrearNodoFiltrado_33ZS(hijo, filtro);
                    if (nodoHijo != null)
                    {
                        hijosVisibles.Add(nodoHijo);
                        algunHijoCoincide = true;
                    }
                }

                if (coincide || algunHijoCoincide)
                {
                    TreeNode nodo = nodos.Add(componente.Nombre);
                    nodo.Tag = componente;
                    nodo.ToolTipText = componente.Nombre;
                    nodo.ForeColor = Color.RoyalBlue;
                    nodo.ImageKey = "familia";
                    nodo.SelectedImageKey = "familia";

                    if (algunHijoCoincide)
                        nodo.Nodes.AddRange(hijosVisibles.ToArray());

                    return true;
                }

                return false;
            }

            if (coincide)
            {
                TreeNode nodo = nodos.Add(componente.Nombre);
                nodo.Tag = componente;
                nodo.ToolTipText = componente.Nombre;
                nodo.ImageKey = "patente";
                nodo.SelectedImageKey = "patente";
                return true;
            }

            return false;
        }

        private TreeNode CrearNodoFiltrado_33ZS(Componente_33ZS componente, string filtro)
        {
            bool coincidePadre = componente.Nombre.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0;

            if (componente is Familia_33ZS familia)
            {
                List<TreeNode> hijosVisibles = new List<TreeNode>();
                bool algunHijoCoincide = false;

                foreach (Componente_33ZS hijo in familia.ObtenerSubComponentes_33ZS())
                {
                    TreeNode nodoHijo = CrearNodoFiltrado_33ZS(hijo, filtro);
                    if (nodoHijo != null)
                    {
                        hijosVisibles.Add(nodoHijo);
                        algunHijoCoincide = true;
                    }
                }

                if (coincidePadre || algunHijoCoincide)
                {
                    TreeNode nodo = new TreeNode(componente.Nombre);
                    nodo.Tag = componente;
                    nodo.ToolTipText = componente.Nombre;
                    nodo.ForeColor = Color.RoyalBlue;
                    nodo.ImageKey = "familia";
                    nodo.SelectedImageKey = "familia";

                    if (algunHijoCoincide)
                        nodo.Nodes.AddRange(hijosVisibles.ToArray());

                    return nodo;
                }

                return null;
            }

            if (coincidePadre)
            {
                TreeNode nodo = new TreeNode(componente.Nombre);
                nodo.Tag = componente;
                nodo.ToolTipText = componente.Nombre;
                nodo.ImageKey = "patente";
                nodo.SelectedImageKey = "patente";
                return nodo;
            }

            return null;
        }

        private void GestionPerfiles_33ZS_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ModificarEliminarPerfiles modificarEliminarPerfiles = new ModificarEliminarPerfiles();
            this.Hide();
            modificarEliminarPerfiles.ShowDialog();
            CargarDatosDB();
            this.Show();
        }
    }
}
