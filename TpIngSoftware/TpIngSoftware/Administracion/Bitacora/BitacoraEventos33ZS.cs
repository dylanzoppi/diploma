using BLL;
using Servicios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace TpIngSoftware
{
    public partial class BitacoraEventos33ZS : Form, IObservador_33ZS
    {
        private BitacoraEventoBLL_33ZS bitacoraBLL;
        private UsuarioBLL_33ZS usuarioBLL;
        private int filaActualImpresion_33ZS;
        private List<DataGridViewColumn> columnasImpresion_33ZS = new List<DataGridViewColumn>();

        public BitacoraEventos33ZS()
        {
            InitializeComponent();
            AplicarDiseno_33ZS();

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

            this.Text = idm.Traducir_33ZS("Bitacora.Titulo");
            bigLabel1.Text = idm.Traducir_33ZS("Bitacora.TituloGrande");

            dungeonLabel1.Text = idm.Traducir_33ZS("Bitacora.Nombre");
            dungeonLabel8.Text = idm.Traducir_33ZS("Bitacora.Apellido");
            dungeonLabel3.Text = idm.Traducir_33ZS("Bitacora.Login");
            dungeonLabel4.Text = idm.Traducir_33ZS("Bitacora.Modulo");
            dungeonLabel6.Text = idm.Traducir_33ZS("Bitacora.Evento");
            dungeonLabel2.Text = idm.Traducir_33ZS("Bitacora.Criticidad");
            dungeonLabel5.Text = idm.Traducir_33ZS("Bitacora.FechaIni");
            dungeonLabel7.Text = idm.Traducir_33ZS("Bitacora.FechaFin");

            limpiarBTN.Text = idm.Traducir_33ZS("Bitacora.Limpiar");
            aplicarBTN.Text = idm.Traducir_33ZS("Bitacora.Aplicar");
            imprimirBTN.Text = idm.Traducir_33ZS("Bitacora.Imprimir");
            salirBTN.Text = idm.Traducir_33ZS("Bitacora.Salir");
            ConfigurarColumnas_33ZS();
        }

        private void BitacoraEventos33ZS_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            bitacoraBLL = new BitacoraEventoBLL_33ZS();
            usuarioBLL = new UsuarioBLL_33ZS();
            CargarCombos_33ZS();

            CargarGrillaEventos_33ZS();
            RegistrarConsultaBitacoraSegura_33ZS();
        }

        private void CargarGrillaEventos_33ZS()
        {
            try
            {
                List<BitacoraEvento_33ZS> listaEventos = bitacoraBLL.ObtenerEventos_33ZS();
                DateTime haceTresDias = DateTime.Today.AddDays(-3);
                listaEventos = listaEventos.Where(ev => ev.Fecha_33ZS >= haceTresDias).ToList();
                eventosDGV.DataSource = null;
                eventosDGV.DataSource = listaEventos;
                OcultarColumnasInnecesarias();
            }
            catch (Exception ex)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("Bitacora.ErrorCargar") + " " + idm.Traducir_33ZS(ex.Message), idm.Traducir_33ZS("Bitacora.Error"));
            }
        }

        private void CargarCombos_33ZS()
        {
            moduloCOMBOBOX.Items.Clear();
            eventoCOMBOBOX.Items.Clear();
            criticidadCOMBOBOX.Items.Clear();
            loginCOMBOBOX.Items.Clear();

            foreach (ModuloSistema_33ZS modulo in Enum.GetValues(typeof(ModuloSistema_33ZS)))
            {
                moduloCOMBOBOX.Items.Add(modulo.ToString());
            }

            foreach (TipoEvento_33ZS evento in Enum.GetValues(typeof(TipoEvento_33ZS)))
            {
                eventoCOMBOBOX.Items.Add(evento.ToString());
            }

            for (int i = 1; i <= 5; i++)
            {
                criticidadCOMBOBOX.Items.Add(i.ToString());
            }

            List<Usuario_33ZS> listaUsuarios = usuarioBLL.ObtenerUsuarios_33ZS();
            foreach (var user in listaUsuarios)
            {
                loginCOMBOBOX.Items.Add(user.Login_33ZS);
            }

            LimpiarFiltros_33ZS(); 
        }

        private void LimpiarFiltros_33ZS()
        {
            nombreBOX.Text = "";
            apellidoBOX.Text = "";
            loginCOMBOBOX.SelectedIndex = -1;
            moduloCOMBOBOX.SelectedIndex = -1;
            eventoCOMBOBOX.SelectedIndex = -1;
            criticidadCOMBOBOX.SelectedIndex = -1;

            fechaInicioCOMBOBOX.Value = DateTime.Today.AddDays(-3);
            fechaFinCOMBOBOX.Value = DateTime.Today.AddDays(1).AddTicks(-1); 
        }

        private void limpiarBTN_Click(object sender, EventArgs e)
        {
            LimpiarFiltros_33ZS();
            CargarGrillaEventos_33ZS(); 
        }

        private void aplicarBTN_Click(object sender, EventArgs e)
        {
            try
            {
                if (fechaInicioCOMBOBOX.Value.Date > DateTime.Today || fechaFinCOMBOBOX.Value.Date > DateTime.Today)
                {
                    MessageBox.Show(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Bitacora.NoFechasFuturas"));
                    return; 
                }
                List<BitacoraEvento_33ZS> eventosFiltrados = bitacoraBLL.ObtenerEventos_33ZS();

                DateTime fInicio = fechaInicioCOMBOBOX.Value.Date;
                DateTime fFin = fechaFinCOMBOBOX.Value.Date;
                eventosFiltrados = eventosFiltrados.Where(ev => ev.Fecha_33ZS >= fInicio && ev.Fecha_33ZS <= fFin).ToList();

                if (loginCOMBOBOX.SelectedIndex != -1)
                {
                    string loginSeleccionado = loginCOMBOBOX.SelectedItem.ToString();
                    eventosFiltrados = eventosFiltrados.Where(ev => ev.Login_33ZS == loginSeleccionado).ToList();
                }

                if (moduloCOMBOBOX.SelectedIndex != -1)
                {
                    string moduloSeleccionado = moduloCOMBOBOX.SelectedItem.ToString();
                    eventosFiltrados = eventosFiltrados.Where(ev => ev.Modulo_33ZS == moduloSeleccionado).ToList();
                }

                if (eventoCOMBOBOX.SelectedIndex != -1)
                {
                    string eventoSeleccionado = eventoCOMBOBOX.SelectedItem.ToString();
                    eventosFiltrados = eventosFiltrados.Where(ev => ev.NombreEvento_33ZS == eventoSeleccionado).ToList();
                }

                if (criticidadCOMBOBOX.SelectedIndex != -1)
                {
                    int criticidadSeleccionada = Convert.ToInt32(criticidadCOMBOBOX.SelectedItem.ToString());
                    eventosFiltrados = eventosFiltrados.Where(ev => ev.Criticidad_33ZS == criticidadSeleccionada).ToList();
                }

                eventosDGV.DataSource = null;
                eventosDGV.DataSource = eventosFiltrados;        
                OcultarColumnasInnecesarias();
            }
            catch (Exception ex)
            {
                 var idm = SessionManager_33ZS.GetInstance_33ZS();
                 MessageBox.Show(idm.Traducir_33ZS("Bitacora.ErrorAplicar") + " " + idm.Traducir_33ZS(ex.Message), idm.Traducir_33ZS("Bitacora.Error"));
            }
        }

        private void OcultarColumnasInnecesarias()
        {
            if (eventosDGV.Columns["Id_Evento_33ZS"] != null)
                eventosDGV.Columns["Id_Evento_33ZS"].Visible = false;
            ConfigurarColumnas_33ZS();
        }

        private void ConfigurarColumnas_33ZS()
        {
            var idioma = SessionManager_33ZS.GetInstance_33ZS();
            var encabezados = new Dictionary<string, string>
            {
                { "Login_33ZS", "Bitacora.Login" },
                { "Fecha_33ZS", "Bitacora.Fecha" },
                { "Hora_33ZS", "Bitacora.Hora" },
                { "Modulo_33ZS", "Bitacora.Modulo" },
                { "NombreEvento_33ZS", "Bitacora.Evento" },
                { "Criticidad_33ZS", "Bitacora.Criticidad" }
            };
            foreach (var par in encabezados)
                if (eventosDGV.Columns[par.Key] != null)
                    eventosDGV.Columns[par.Key].HeaderText = idioma.Traducir_33ZS(par.Value);
            if (eventosDGV.Columns["Fecha_33ZS"] != null)
                eventosDGV.Columns["Fecha_33ZS"].DefaultCellStyle.Format = "dd/MM/yyyy";
            if (eventosDGV.Columns["Hora_33ZS"] != null)
                eventosDGV.Columns["Hora_33ZS"].DefaultCellStyle.Format = "HH:mm";
        }

        private void eventosDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
            if (e.RowIndex >= 0)
            {
                string loginGrilla = eventosDGV.Rows[e.RowIndex].Cells["Login_33ZS"].Value.ToString();
                
                Usuario_33ZS usuarioSeleccionado = usuarioBLL.ObtenerUsuarios_33ZS().FirstOrDefault(u => u.Login_33ZS == loginGrilla);
                
                if (usuarioSeleccionado != null)
                {
                    nombreBOX.Text = usuarioSeleccionado.Nombre_33ZS;
                    apellidoBOX.Text = usuarioSeleccionado.Apellidos_33ZS;
                }
                else
                {
                    nombreBOX.Text = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Bitacora.NoEncontrado");
                    apellidoBOX.Text = "";
                }
            }
        }

        private void salirBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imprimirBTN_Click(object sender, EventArgs e)
        {
            if (eventosDGV.Rows.Count == 0)
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("Bitacora.SinDatosImprimir"), idm.Traducir_33ZS("Bitacora.Titulo"));
                return;
            }

            if (!EstaDisponibleImpresoraPdf_33ZS())
            {
                var idm = SessionManager_33ZS.GetInstance_33ZS();
                MessageBox.Show(idm.Traducir_33ZS("Bitacora.PdfPrinterNoEncontrada"), idm.Traducir_33ZS("Bitacora.Titulo"));
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Archivo PDF|*.pdf";
                saveDialog.Title = SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Bitacora.GuardarPdfTitulo");
                saveDialog.FileName = $"Bitacora_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                if (saveDialog.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    filaActualImpresion_33ZS = 0;
                    columnasImpresion_33ZS = eventosDGV.Columns
                        .Cast<DataGridViewColumn>()
                        .Where(c => c.Visible)
                        .OrderBy(c => c.DisplayIndex)
                        .ToList();

                    using (PrintDocument documento = new PrintDocument())
                    {
                        documento.DocumentName = "BitacoraEventos";
                        documento.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                        documento.PrinterSettings.PrintToFile = true;
                        documento.PrinterSettings.PrintFileName = saveDialog.FileName;
                        documento.PrintPage += Documento_PrintPage_33ZS;
                        documento.Print();
                        documento.PrintPage -= Documento_PrintPage_33ZS;
                    }

                    var idm = SessionManager_33ZS.GetInstance_33ZS();
                    MessageBox.Show(idm.Traducir_33ZS("Bitacora.PdfGenerado"), idm.Traducir_33ZS("Bitacora.Titulo"));
                }
                catch (Exception ex)
                {
                    var idm = SessionManager_33ZS.GetInstance_33ZS();
                    MessageBox.Show(idm.Traducir_33ZS("Bitacora.PdfError") + " " + idm.Traducir_33ZS(ex.Message), idm.Traducir_33ZS("Bitacora.Titulo"));
                }
            }
        }

        private void Documento_PrintPage_33ZS(object sender, PrintPageEventArgs e)
        {
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            int anchoPagina = e.MarginBounds.Width;

            using (Font titulo = new Font("Segoe UI", 12, FontStyle.Bold))
            using (Font texto = new Font("Segoe UI", 8))
            {
                e.Graphics.DrawString(SessionManager_33ZS.GetInstance_33ZS().Traducir_33ZS("Bitacora.DocumentoTitulo"), titulo, Brushes.Black, x, y);
                y += 26;
                e.Graphics.DrawString($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}", texto, Brushes.Black, x, y);
                y += 22;

                if (columnasImpresion_33ZS.Count == 0)
                {
                    e.HasMorePages = false;
                    return;
                }

                int anchoColumna = Math.Max(60, anchoPagina / columnasImpresion_33ZS.Count);
                int altoFila = 22;

                DibujarCabecera_33ZS(e, texto, x, y, anchoColumna, altoFila);
                y += altoFila;

                while (filaActualImpresion_33ZS < eventosDGV.Rows.Count)
                {
                    DataGridViewRow fila = eventosDGV.Rows[filaActualImpresion_33ZS];

                    if (fila.IsNewRow)
                    {
                        filaActualImpresion_33ZS++;
                        continue;
                    }

                    if (y + altoFila > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    int xCol = x;
                    foreach (DataGridViewColumn columna in columnasImpresion_33ZS)
                    {
                        string valor = Convert.ToString(fila.Cells[columna.Index].Value) ?? string.Empty;
                        valor = RecortarTexto_33ZS(valor, 24);

                        Rectangle rect = new Rectangle(xCol, y, anchoColumna, altoFila);
                        e.Graphics.DrawRectangle(Pens.Black, rect);
                        e.Graphics.DrawString(valor, texto, Brushes.Black, rect);
                        xCol += anchoColumna;
                    }

                    y += altoFila;
                    filaActualImpresion_33ZS++;
                }
            }

            e.HasMorePages = false;
            filaActualImpresion_33ZS = 0;
        }

        private void DibujarCabecera_33ZS(PrintPageEventArgs e, Font fuente, int x, int y, int anchoColumna, int altoFila)
        {
            int xCol = x;
            foreach (DataGridViewColumn columna in columnasImpresion_33ZS)
            {
                string titulo = RecortarTexto_33ZS(columna.HeaderText, 24);
                Rectangle rect = new Rectangle(xCol, y, anchoColumna, altoFila);
                e.Graphics.FillRectangle(Brushes.LightGray, rect);
                e.Graphics.DrawRectangle(Pens.Black, rect);
                e.Graphics.DrawString(titulo, fuente, Brushes.Black, rect);
                xCol += anchoColumna;
            }
        }

        private bool EstaDisponibleImpresoraPdf_33ZS()
        {
            foreach (string impresora in PrinterSettings.InstalledPrinters)
            {
                if (impresora.Equals("Microsoft Print to PDF", StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private string RecortarTexto_33ZS(string valor, int maximo)
        {
            if (string.IsNullOrEmpty(valor) || valor.Length <= maximo)
                return valor;

            return valor.Substring(0, maximo - 3) + "...";
        }

        private void bigLabel1_Click(object sender, EventArgs e)
        {
        }

        private void RegistrarConsultaBitacoraSegura_33ZS()
        {
            try
            {
                string login = "sistema";

                if (SessionManager_33ZS.HaySesionActiva_33ZS())
                {
                    login = SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS;
                }

                BitacoraEvento_33ZS evento = new BitacoraEvento_33ZS
                {
                    Login_33ZS = login,
                    Fecha_33ZS = DateTime.Today,
                    Hora_33ZS = DateTime.Now,
                    Modulo_33ZS = ModuloSistema_33ZS.Bitacora.ToString(),
                    NombreEvento_33ZS = TipoEvento_33ZS.ConsultarBitacora.ToString(),
                    Criticidad_33ZS = 1
                };

                bitacoraBLL.RegistrarEvento_33ZS(evento);
            }
            catch
            {
                
            }
        }
    }
}
