using BLL;
using Servicios;
using Servicios.Composite;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TpIngSoftware.Administracion.Perfiles
{
    public partial class ModificarEliminarPerfiles : Form, IObservador_33ZS
    {
        private readonly PerfilBLL_33ZS bll = new PerfilBLL_33ZS();

        private List<Componente_33ZS> todosComponentes = new List<Componente_33ZS>();

        private List<Componente_33ZS> datosPatentes = new List<Componente_33ZS>();
    
        private List<Componente_33ZS> datosFamilias = new List<Componente_33ZS>();
      

        private List<Familia_33ZS> todasFamilias = new List<Familia_33ZS>();
        private List<Familia_33ZS> todosRoles = new List<Familia_33ZS>();
        private readonly ImageList iconosArbol_33ZS = new ImageList();

        private Familia_33ZS itemSeleccionado = null;
        private bool? filtrandoRoles = null;

        public ModificarEliminarPerfiles()
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

            this.Text = idm.Traducir_33ZS("ModificarPerfiles.Titulo");
            bigLabel5.Text = idm.Traducir_33ZS("ModificarPerfiles.Modificar");
            bigLabel1.Text = idm.Traducir_33ZS("ModificarPerfiles.Nombre");
            bigLabel2.Text = idm.Traducir_33ZS("ModificarPerfiles.FamiliaRolCrear");
            bigLabel3.Text = idm.Traducir_33ZS("ModificarPerfiles.PatentesDisponibles");
            bigLabel4.Text = idm.Traducir_33ZS("ModificarPerfiles.PatentesEnCreacion");
            bigLabel7.Text = idm.Traducir_33ZS("ModificarPerfiles.PresioneFiltrar");
            familiaRDBTN.Text = idm.Traducir_33ZS("ModificarPerfiles.Familia");
            rolRDBTN.Text = idm.Traducir_33ZS("ModificarPerfiles.Rol");
            modificarBTN.Text = idm.Traducir_33ZS("ModificarPerfiles.BtnModificar");
            eliminarBTN.Text = idm.Traducir_33ZS("ModificarPerfiles.Eliminar");
            ActualizarTextoFiltro_33ZS();
        }

        private void ActualizarTextoFiltro_33ZS()
        {
            if (listFamiliasRoles.Items.Count == 0)
            {
                bigLabel7.Text = filtrandoRoles == true
                    ? SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.SinRoles")
                    : SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.SinFamilias");
                return;
            }

            bigLabel7.Text = filtrandoRoles == true
                ? SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.SeleccioneRolEditar")
                : SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.SeleccioneFamiliaEditar");
        }

  
        private void AplicarPermisos_33ZS()
        {
            string rol = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Rol_33ZS
                : null;

            List<string> patentes = bll.ObtenerPatentesDeRol_33ZS(rol);

            modificarBTN.Visible = patentes.Contains("ModificacionPerfil");
            eliminarBTN.Visible = patentes.Contains("BajaPerfil");
        }

        private void InicializarEventos()
        {
            listFamiliasRoles.SelectedIndexChanged += listFamiliasRoles_SelectedIndexChanged;
            listFamiliasRoles.DrawItem += listFamiliasRoles_DrawItem;
            listPatentes.NodeMouseClick += listPatentes_NodeMouseClick;
            listFamilias.NodeMouseClick += listFamilias_NodeMouseClick;
            familiaRDBTN.Click += rdFiltro_CheckedChanged;
            rolRDBTN.Click += rdFiltro_CheckedChanged;
        }

        private void listFamiliasRoles_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            ListBox listBox = (ListBox)sender;
            object item = listBox.Items[e.Index];
            string texto = item is Familia_33ZS componente ? componente.Nombre : item?.ToString() ?? string.Empty;

            e.DrawBackground();

            using (Font font = new Font(e.Font, FontStyle.Bold))
            {
                Color color = ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    ? SystemColors.HighlightText
                    : (filtrandoRoles == true ? Color.SeaGreen : Color.RoyalBlue);

                TextRenderer.DrawText(
                    e.Graphics,
                    texto,
                    font,
                    e.Bounds,
                    color,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }

            e.DrawFocusRectangle();
        }

        private void ConfigurarFiltroInicial_33ZS()
        {
            familiaRDBTN.Checked = true;
            rolRDBTN.Checked = false;
            filtrandoRoles = false;
        }

        private void ConfigurarIconosArbol_33ZS()
        {
            iconosArbol_33ZS.ImageSize = new Size(16, 16);
            iconosArbol_33ZS.ColorDepth = ColorDepth.Depth32Bit;
            iconosArbol_33ZS.Images.Add("familia", CrearIconoFamilia_33ZS());
            iconosArbol_33ZS.Images.Add("patente", CrearIconoPatente_33ZS());

            listFamilias.ImageList = iconosArbol_33ZS;
            listPatentes.ImageList = iconosArbol_33ZS;
            listPatentesSeleccionadas.ImageList = iconosArbol_33ZS;
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
            Font font = esFamilia ? new Font(e.Font, FontStyle.Bold) : e.Font;
            Color color = ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                ? SystemColors.HighlightText
                : esFamilia ? Color.RoyalBlue : e.ForeColor;

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
                todasFamilias.Clear();
                todasFamilias.AddRange(bll.ObtenerFamilias_33ZS());
                todosRoles.Clear();
                todosRoles.AddRange(bll.ObtenerRoles_33ZS());

                todosComponentes.Clear();
                todosComponentes.AddRange(bll.ObtenerPatentes_33ZS());
                todosComponentes.AddRange(todasFamilias);

                LimpiarSeleccion();
                ActualizarListaFamiliasYRoles();
                ActualizarEstadoEdicion_33ZS();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("ModificarPerfiles.ErrorCargarDatos") + " " + idm.Traducir_33ZS(ex.Message));
            }
        }

        private void LimpiarSeleccion()
        {
            itemSeleccionado = null;
            datosFamilias.Clear();
            nombreFamiliaTXT.Text = "";

            datosPatentes.Clear();
            datosPatentes.AddRange(todosComponentes);

            listFamilias.Nodes.Clear();
            listPatentes.Nodes.Clear();
            foreach (Componente_33ZS item in datosPatentes)
                AgregarNodoDisponible_33ZS(listPatentes.Nodes, item);
            listPatentes.CollapseAll();
            listPatentesSeleccionadas.Nodes.Clear();
            ActualizarEstadoEdicion_33ZS();
        }

        private void ActualizarListaFamiliasYRoles()
        {
            listFamiliasRoles.DataSource = null;
            if (filtrandoRoles == true)
                listFamiliasRoles.DataSource = new List<Familia_33ZS>(todosRoles);
            else if (filtrandoRoles == false)
                listFamiliasRoles.DataSource = new List<Familia_33ZS>(todasFamilias);
            listFamiliasRoles.DisplayMember = "Nombre";

            if (listFamiliasRoles.Items.Count > 0)
                listFamiliasRoles.SelectedIndex = 0;

            ActualizarTextoFiltro_33ZS();
            ActualizarEstadoEdicion_33ZS();
        }

        private void ActualizarEstadoEdicion_33ZS()
        {
            bool haySeleccion = itemSeleccionado != null;

            nombreFamiliaTXT.ReadOnly = !haySeleccion;
            modificarBTN.Enabled = haySeleccion;
            eliminarBTN.Enabled = haySeleccion;

            if (!haySeleccion)
            {
                nombreFamiliaTXT.Text = "";
                listPatentesSeleccionadas.Nodes.Clear();

                TreeNode estado = listPatentesSeleccionadas.Nodes.Add(
                    filtrandoRoles == true
                        ? "Seleccione un rol para editarlo."
                        : "Seleccione una familia para editarla.");
                estado.ForeColor = Color.DimGray;
            }
        }

        private void ActualizarListas()
        {
            listFamilias.Nodes.Clear();
            foreach (Componente_33ZS item in datosFamilias)
                AgregarNodo_33ZS(listFamilias.Nodes, item);
            listFamilias.ExpandAll();

            listPatentes.Nodes.Clear();
            foreach (Componente_33ZS item in datosPatentes)
                AgregarNodoDisponible_33ZS(listPatentes.Nodes, item);
            listPatentes.CollapseAll();

            listPatentesSeleccionadas.Nodes.Clear();
            if (itemSeleccionado != null)
            {
                TreeNode raiz = listPatentesSeleccionadas.Nodes.Add(nombreFamiliaTXT.Text);
                raiz.NodeFont = new Font(listPatentesSeleccionadas.Font, FontStyle.Bold);
                raiz.ForeColor = Color.RoyalBlue;
                raiz.ImageKey = "familia";
                raiz.SelectedImageKey = "familia";

                foreach (Componente_33ZS item in datosFamilias)
                    AgregarNodo_33ZS(raiz.Nodes, item);
            }
            else
            {
                foreach (Componente_33ZS item in datosFamilias)
                    AgregarNodo_33ZS(listPatentesSeleccionadas.Nodes, item);
            }
            listPatentesSeleccionadas.ExpandAll();
            ActualizarEstadoEdicion_33ZS();
        }

        private void AgregarNodoDisponible_33ZS(TreeNodeCollection nodos, Componente_33ZS componente)
        {
            TreeNode nodo = nodos.Add(componente.Nombre);
            nodo.Tag = componente;

            if (componente is Familia_33ZS familia)
            {
                nodo.NodeFont = new Font(listPatentes.Font, FontStyle.Bold);
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

        private void AgregarNodo_33ZS(TreeNodeCollection nodos, Componente_33ZS componente)
        {
            TreeNode nodo = nodos.Add(componente.Nombre);
            nodo.Tag = componente;
            if (componente is Familia_33ZS familia)
            {
                nodo.NodeFont = new Font(listPatentesSeleccionadas.Font, FontStyle.Bold);
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

        private void LimpiarResaltadoNodos_33ZS(TreeNodeCollection nodos)
        {
            foreach (TreeNode nodo in nodos)
            {
                nodo.BackColor = Color.White;
                nodo.ForeColor = nodo.NodeFont != null ? Color.RoyalBlue : Color.Black;
                LimpiarResaltadoNodos_33ZS(nodo.Nodes);
            }
        }

        private TreeNode BuscarNodoPorTexto_33ZS(TreeNodeCollection nodos, string texto)
        {
            foreach (TreeNode nodo in nodos)
            {
                if (nodo.Text.Equals(texto, StringComparison.OrdinalIgnoreCase))
                    return nodo;

                TreeNode encontrado = BuscarNodoPorTexto_33ZS(nodo.Nodes, texto);
                if (encontrado != null)
                    return encontrado;
            }

            return null;
        }

        private void ResaltarConflicto_33ZS(string nombrePatente)
        {
            LimpiarResaltadoNodos_33ZS(listPatentesSeleccionadas.Nodes);
            listPatentesSeleccionadas.SelectedNode = null;

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
                        nodoSeleccionado.EnsureVisible();
                    }
                    break;
                }
            }

            TreeNode nodoConflicto = BuscarNodoPorTexto_33ZS(listPatentesSeleccionadas.Nodes, nombrePatente);
            if (nodoConflicto != null)
            {
                nodoConflicto.BackColor = Color.LightSalmon;
                nodoConflicto.ForeColor = Color.Black;
                listPatentesSeleccionadas.SelectedNode = nodoConflicto;
                nodoConflicto.EnsureVisible();
            }
        }

        private void listFamiliasRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(listFamiliasRoles.SelectedItem is Familia_33ZS seleccionado))
                return;

            itemSeleccionado = seleccionado;
            nombreFamiliaTXT.Text = seleccionado.Nombre;

            datosFamilias.Clear();
            foreach (Componente_33ZS sub in seleccionado.ObtenerSubComponentes_33ZS())
                datosFamilias.Add(sub);

            var idsPatentesEnPerfil = new HashSet<int>();
            var idsFamiliasEnPerfil = new HashSet<int>();
            foreach (var comp in datosFamilias)
            {
                if (comp is Patente_33ZS) idsPatentesEnPerfil.Add(comp.Id);
                else if (comp is Familia_33ZS) idsFamiliasEnPerfil.Add(comp.Id);
            }

            datosPatentes.Clear();
            foreach (var comp in todosComponentes)
            {
                if (comp is Patente_33ZS && !idsPatentesEnPerfil.Contains(comp.Id))
                    datosPatentes.Add(comp);
                else if (comp is Familia_33ZS && !idsFamiliasEnPerfil.Contains(comp.Id) && comp.Id != seleccionado.Id)
                    datosPatentes.Add(comp);
            }

            ActualizarListas();
            ActualizarEstadoEdicion_33ZS();
        }

        private void rdFiltro_CheckedChanged(object sender, EventArgs e)
        {
            if (!familiaRDBTN.Checked && !rolRDBTN.Checked)
            {
                familiaRDBTN.Checked = true;
                filtrandoRoles = false;
            }

            filtrandoRoles = (sender == rolRDBTN);
            LimpiarSeleccion();
            ActualizarListaFamiliasYRoles();
        }

        private void listPatentes_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewHitTestInfo hit = listPatentes.HitTest(e.Location);
            if (hit.Location == TreeViewHitTestLocations.PlusMinus)
                return;

            if (!(e.Node?.Tag is Componente_33ZS itemSel))
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.SeleccionePatente"));
                return;
            }

            listPatentes.SelectedNode = e.Node;

            if (bll.PatenteYaIncluida_33ZS(itemSel, datosFamilias))
            {
                string patenteDuplicada = bll.ObtenerPatenteDuplicada_33ZS(itemSel, datosFamilias);
                string nombreElemento = itemSel.Nombre;

                MessageBox.Show(
                    patenteDuplicada == null
                        ? SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.ElementoYaIncluido").Replace("{0}", nombreElemento)
                        : SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.ElementoYaIncluidoPatente").Replace("{0}", nombreElemento).Replace("{1}", patenteDuplicada));
                ResaltarConflicto_33ZS(patenteDuplicada);
                return;
            }

            datosFamilias.Add(itemSel);
            datosPatentes.Remove(itemSel);
            ActualizarListas();
        }

        private Componente_33ZS ObtenerComponenteRaizDesdeNodo_33ZS(TreeNode nodo)
        {
            if (nodo == null)
                return null;

            while (nodo.Parent != null)
                nodo = nodo.Parent;

            return nodo.Tag as Componente_33ZS;
        }

        private void listFamilias_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeViewHitTestInfo hit = listFamilias.HitTest(e.Location);
            if (hit.Location == TreeViewHitTestLocations.PlusMinus)
                return;

            if (!(e.Node?.Tag is Componente_33ZS))

            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.SeleccioneComponenteQuitar"));
                return;
            }

            listFamilias.SelectedNode = e.Node;

            Componente_33ZS itemSel = ObtenerComponenteRaizDesdeNodo_33ZS(e.Node);
            if (itemSel == null)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.NoSePudoIdentificar"));
                return;
            }

            datosFamilias.Remove(itemSel);
            datosPatentes.Add(itemSel);
            ActualizarListas();
        }

        private void modificarBTN_Click(object sender, EventArgs e)
        {
            if (itemSeleccionado == null)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.SeleccioneModificar"));
                return;
            }
            if (string.IsNullOrWhiteSpace(nombreFamiliaTXT.Text))
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.IngreseNombre"));
                return;
            }
            if (datosFamilias.Count == 0)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.AgregueComponente"));
                return;
            }

            string tipo = filtrandoRoles == true ? "rol" : "familia";
            DialogResult confirm = MessageBox.Show(
                SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.ConfirmarModificacion").Replace("{0}", tipo).Replace("{1}", itemSeleccionado.Nombre),
                SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.BtnModificar"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                var subComponentes = new List<Componente_33ZS>(datosFamilias);

                if (filtrandoRoles == true)
                    bll.ModificarRol_33ZS(itemSeleccionado.Id, nombreFamiliaTXT.Text, subComponentes);
                else
                    bll.ModificarFamilia_33ZS(itemSeleccionado.Id, nombreFamiliaTXT.Text, subComponentes);

                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.ModificadoExito"));
                CargarDatosDB();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("ModificarPerfiles.ErrorModificar") + " " + idm.Traducir_33ZS(ex.Message));
            }
        }

        private void ModificarEliminarPerfiles_Load(object sender, EventArgs e)
        {

        }

        private void eliminarBTN_Click(object sender, EventArgs e)
        {
            if (itemSeleccionado == null)
            {
                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.SeleccioneEliminar"));
                return;
            }

            string tipo = filtrandoRoles == true ? "rol" : "familia";

            DialogResult confirm = MessageBox.Show(
                SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.ConfirmarEliminacion").Replace("{0}", tipo).Replace("{1}", itemSeleccionado.Nombre),
                SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.Eliminar"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                if (filtrandoRoles == true)
                    bll.EliminarRol_33ZS(itemSeleccionado.Id);
                else
                    bll.EliminarFamilia_33ZS(itemSeleccionado.Id);

                MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("ModificarPerfiles.EliminadoExito").Replace("{0}", tipo));
                CargarDatosDB();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("ModificarPerfiles.ErrorEliminar") + " " + idm.Traducir_33ZS(ex.Message));
            }
        }
    }
}
