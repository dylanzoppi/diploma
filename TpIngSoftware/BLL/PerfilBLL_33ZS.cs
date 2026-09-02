using DAL;
using Servicios;
using Servicios.Composite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLL
{
    public class PerfilBLL_33ZS
    {
        private readonly PerfilDAL_33ZS _dal = new PerfilDAL_33ZS();
        private readonly BitacoraEventoBLL_33ZS bitacoraBLL = new BitacoraEventoBLL_33ZS();
        private readonly DigitoVerificadorBLL_33ZS dvBLL = new DigitoVerificadorBLL_33ZS();
        private static readonly Regex NombrePerfilRegex_33ZS = new Regex(@"^[\p{L}\p{N}\s_-]+$", RegexOptions.Compiled);

        public List<Patente_33ZS> ObtenerPatentes_33ZS()
        {
            return _dal.ObtenerPatentes_33ZS();
        }

        public List<Familia_33ZS> ObtenerFamilias_33ZS()
        {
            return _dal.ObtenerFamilias_33ZS();
        }

        public List<Componente_33ZS> ObtenerTodosLosComponentes_33ZS()
        {
            List<Componente_33ZS> componentes = new List<Componente_33ZS>();
            componentes.AddRange(_dal.ObtenerPatentes_33ZS());
            componentes.AddRange(_dal.ObtenerFamilias_33ZS());
            return componentes;
        }

        public bool PatenteYaIncluida_33ZS(Componente_33ZS itemAgregar, List<Componente_33ZS> componentesActuales)
        {
            List<int> idsNuevos = ObtenerIdsPatentes_33ZS(itemAgregar);
            foreach (Componente_33ZS componenteExistente in componentesActuales)
                foreach (int id in ObtenerIdsPatentes_33ZS(componenteExistente))
                    if (idsNuevos.Contains(id))
                        return true;
            return false;
        }

        public string ObtenerPatenteDuplicada_33ZS(Componente_33ZS itemAgregar, List<Componente_33ZS> componentesActuales)
        {
            List<Patente_33ZS> patentesNuevas = ObtenerPatentes_33ZS(itemAgregar);

            foreach (Componente_33ZS componenteExistente in componentesActuales)
            {
                foreach (Patente_33ZS patenteExistente in ObtenerPatentes_33ZS(componenteExistente))
                {
                    foreach (Patente_33ZS patenteNueva in patentesNuevas)
                    {
                        if (patenteNueva.Id == patenteExistente.Id)
                            return patenteNueva.Nombre;
                    }
                }
            }

            return null;
        }

        private List<int> ObtenerIdsPatentes_33ZS(Componente_33ZS componente)
        {
            List<int> ids = new List<int>();
            if (componente is Patente_33ZS p)
                ids.Add(p.Id);
            else if (componente is Familia_33ZS f)
                foreach (Patente_33ZS pat in f.ObtenerPatentes_33ZS())
                    ids.Add(pat.Id);
            return ids;
        }

        private List<Patente_33ZS> ObtenerPatentes_33ZS(Componente_33ZS componente)
        {
            List<Patente_33ZS> patentes = new List<Patente_33ZS>();

            if (componente is Patente_33ZS p)
            {
                patentes.Add(p);
            }
            else if (componente is Familia_33ZS f)
            {
                patentes.AddRange(f.ObtenerPatentes_33ZS());
            }

            return patentes;
        }

        private string NormalizarNombre_33ZS(string nombre, string tipo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException($"Perfil.NombreVacio|{tipo}");

            nombre = nombre.Trim();
            nombre = Regex.Replace(nombre, @"\s+", " ");

            if (nombre.Length < 3)
                throw new ArgumentException($"Perfil.NombreMinimo|{tipo}");
            if (nombre.Length > 100)
                throw new ArgumentException($"Perfil.NombreMaximo|{tipo}");
            if (!NombrePerfilRegex_33ZS.IsMatch(nombre))
                throw new ArgumentException($"Perfil.NombreCaracteresInvalidos|{tipo}");

            return nombre;
        }

        private void ValidarSubComponentes_33ZS(string tipo, List<Componente_33ZS> subComponentes, int? familiaObjetivoId = null)
        {
            if (subComponentes == null || subComponentes.Count == 0)
                throw new ArgumentException($"Perfil.SinComponentes|{tipo}");

            List<Componente_33ZS> verificados = new List<Componente_33ZS>();
            HashSet<string> componentesDirectos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Componente_33ZS componente in subComponentes)
            {
                if (componente == null)
                    throw new ArgumentException($"Perfil.ComponenteInvalido|{tipo}");

                string claveDirecta = $"{componente.GetType().Name}:{componente.Id}";
                if (!componentesDirectos.Add(claveDirecta))
                    throw new ArgumentException($"Perfil.ComponenteDuplicado|{tipo}|{componente.Nombre}");

                if (familiaObjetivoId.HasValue && componente is Familia_33ZS familia)
                {
                    if (familia.Id == familiaObjetivoId.Value)
                        throw new ArgumentException("Perfil.FamiliaSeContieneASiMisma");

                    if (ContieneFamilia_33ZS(familia, familiaObjetivoId.Value))
                        throw new ArgumentException($"Perfil.ReferenciaCircular|{familia.Nombre}");
                }

                if (PatenteYaIncluida_33ZS(componente, verificados))
                {
                    string patenteDuplicada = ObtenerPatenteDuplicada_33ZS(componente, verificados);
                    if (string.IsNullOrWhiteSpace(patenteDuplicada))
                        throw new ArgumentException($"Perfil.PatentesDuplicadas|{tipo}");

                    throw new ArgumentException($"Perfil.PatenteDuplicada|{tipo}|{patenteDuplicada}");
                }

                verificados.Add(componente);
            }
        }

        private bool ContieneFamilia_33ZS(Familia_33ZS familia, int familiaBuscadaId)
        {
            foreach (Componente_33ZS subComponente in familia.ObtenerSubComponentes_33ZS())
            {
                if (subComponente is Familia_33ZS subFamilia)
                {
                    if (subFamilia.Id == familiaBuscadaId)
                        return true;

                    if (ContieneFamilia_33ZS(subFamilia, familiaBuscadaId))
                        return true;
                }
            }

            return false;
        }

        public Familia_33ZS GuardarFamilia_33ZS(string nombre, List<Componente_33ZS> subComponentes)
        {
            nombre = NormalizarNombre_33ZS(nombre, "familia");
            ValidarSubComponentes_33ZS("familia", subComponentes);

            bool nombreDuplicado = _dal.ObtenerFamilias_33ZS()
                .Exists(f => f.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (nombreDuplicado)
                throw new ArgumentException($"Perfil.FamiliaNombreDuplicado|{nombre}");

            int nuevoId = _dal.GuardarFamilia_33ZS(nombre, subComponentes);
            dvBLL.GenerarTodo_33ZS();

            string login = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS
                : "sistema";
            RegistrarEventoSeguro_33ZS(login, TipoEvento_33ZS.CrearFamilia, 2);

            Familia_33ZS nueva = new Familia_33ZS(nuevoId, nombre);
            foreach (Componente_33ZS componente in subComponentes)
                nueva.Agregar_33ZS(componente);

            return nueva;
        }

        public List<Familia_33ZS> ObtenerRoles_33ZS()
        {
            return _dal.ObtenerRoles_33ZS();
        }

        public List<string> ObtenerPatentesDeRol_33ZS(string nombreRol)
        {
            List<string> nombres = new List<string>();
            if (string.IsNullOrWhiteSpace(nombreRol))
                return nombres;

            Familia_33ZS rol = _dal.ObtenerRoles_33ZS()
                .Find(r => r.Nombre.Equals(nombreRol, StringComparison.OrdinalIgnoreCase));
            if (rol == null)
                return nombres;

            foreach (Patente_33ZS patente in rol.ObtenerPatentes_33ZS())
                nombres.Add(patente.Nombre);

            return nombres;
        }


        public bool RolTienePatente_33ZS(string nombreRol, string nombrePatente)
        {
            return ObtenerPatentesDeRol_33ZS(nombreRol)
                .Exists(p => p.Equals(nombrePatente, StringComparison.OrdinalIgnoreCase));
        }

        public Familia_33ZS GuardarRol_33ZS(string nombre, List<Componente_33ZS> subComponentes)
        {
            nombre = NormalizarNombre_33ZS(nombre, "rol");
            ValidarSubComponentes_33ZS("rol", subComponentes);

            bool nombreDuplicado = _dal.ObtenerRoles_33ZS()
                .Exists(r => r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (nombreDuplicado)
                throw new ArgumentException($"Perfil.RolNombreDuplicado|{nombre}");

            int nuevoId = _dal.GuardarRol_33ZS(nombre, subComponentes);
            dvBLL.GenerarTodo_33ZS();

            string login = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS
                : "sistema";
            RegistrarEventoSeguro_33ZS(login, TipoEvento_33ZS.CrearRol, 2);

            Familia_33ZS nuevo = new Familia_33ZS(nuevoId, nombre);
            foreach (Componente_33ZS componente in subComponentes)
                nuevo.Agregar_33ZS(componente);

            return nuevo;
        }

        public void EliminarFamilia_33ZS(int id)
        {
            if (_dal.FamiliaEnUso_33ZS(id))
                throw new ArgumentException("Perfil.FamiliaEnUso");

            _dal.EliminarFamilia_33ZS(id);
            dvBLL.GenerarTodo_33ZS();

            string login = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS
                : "sistema";
            RegistrarEventoSeguro_33ZS(login, TipoEvento_33ZS.EliminarFamilia, 3);
        }

        public bool UsuarioTienePatente_33ZS(Componente_33ZS componente, string nombrePatente)
        {
            if (componente is Patente_33ZS p)
                return p.Nombre.Equals(nombrePatente, StringComparison.OrdinalIgnoreCase);

            if (componente is Familia_33ZS f)
            {
                foreach (Patente_33ZS patente in f.ObtenerPatentes_33ZS())
                {
                    if (patente.Nombre.Equals(nombrePatente, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            return false;
        }

        public bool FamiliaEnUso_33ZS(int id) => _dal.FamiliaEnUso_33ZS(id);

        public bool RolEnUso_33ZS(int id) => _dal.RolEnUso_33ZS(id);

        public void ModificarFamilia_33ZS(int id, string nombre, List<Componente_33ZS> subComponentes)
        {
            nombre = NormalizarNombre_33ZS(nombre, "familia");
            ValidarSubComponentes_33ZS("familia", subComponentes, id);

            bool nombreDuplicado = _dal.ObtenerFamilias_33ZS()
                .Exists(f => f.Id != id && f.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (nombreDuplicado)
                throw new ArgumentException($"Perfil.FamiliaNombreDuplicado|{nombre}");

            _dal.ModificarFamilia_33ZS(id, nombre, subComponentes);
            dvBLL.GenerarTodo_33ZS();

            string login = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS
                : "sistema";
            RegistrarEventoSeguro_33ZS(login, TipoEvento_33ZS.ModificarFamilia, 2);
        }

        public void ModificarRol_33ZS(int id, string nombre, List<Componente_33ZS> subComponentes)
        {
            nombre = NormalizarNombre_33ZS(nombre, "rol");
            ValidarSubComponentes_33ZS("rol", subComponentes);

            Familia_33ZS rolActual = _dal.ObtenerRoles_33ZS().FirstOrDefault(r => r.Id == id);
            if (rolActual == null)
                throw new ArgumentException("Perfil.RolNoExiste");

            bool nombreDuplicado = _dal.ObtenerRoles_33ZS()
                .Exists(r => r.Id != id && r.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
            if (nombreDuplicado)
                throw new ArgumentException($"Perfil.RolNombreDuplicado|{nombre}");

            _dal.ModificarRol_33ZS(id, nombre, subComponentes);
            dvBLL.GenerarTodo_33ZS();

            if (SessionManager_33ZS.HaySesionActiva_33ZS() &&
                SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Rol_33ZS.Equals(rolActual.Nombre, StringComparison.OrdinalIgnoreCase))
            {
                SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Rol_33ZS = nombre;
            }

            string login = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS
                : "sistema";
            RegistrarEventoSeguro_33ZS(login, TipoEvento_33ZS.ModificarRol, 2);
        }

        public void EliminarRol_33ZS(int id)
        {
            if (_dal.RolEnUso_33ZS(id))
                throw new ArgumentException("Perfil.RolEnUso");

            _dal.EliminarRol_33ZS(id);
            dvBLL.GenerarTodo_33ZS();

            string login = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS.Login_33ZS
                : "sistema";
            RegistrarEventoSeguro_33ZS(login, TipoEvento_33ZS.EliminarRol, 3);
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
                    Modulo_33ZS = ModuloSistema_33ZS.Perfiles.ToString(),
                    NombreEvento_33ZS = tipoEvento.ToString(),
                    Criticidad_33ZS = criticidad
                };
                bitacoraBLL.RegistrarEvento_33ZS(evento);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"No se pudo registrar el evento de perfiles: {ex.Message}");
                Trace.WriteLine($"No se pudo registrar el evento de perfiles: {ex}");
            }
        }
    }
}
