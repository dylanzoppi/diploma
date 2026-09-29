using Servicios;
using Mappers.Security;
using System;
using System.IO;

namespace BLL
{
    public class BackupBLL_33ZS
    {
        private readonly BackupMapper_33ZS _dal = new BackupMapper_33ZS();
        private readonly BitacoraEventoBLL_33ZS _bitacora = new BitacoraEventoBLL_33ZS();
        private readonly DigitoVerificadorBLL_33ZS _dv = new DigitoVerificadorBLL_33ZS();

        private static void ExigirPermiso_33ZS()
        {
            Usuario_33ZS actual = SessionManager_33ZS.HaySesionActiva_33ZS()
                ? SessionManager_33ZS.GetInstance_33ZS().UsuarioActual_33ZS : null;
            if (actual == null || !actual.Activo_33ZS || actual.Bloqueo_33ZS ||
                !new PerfilBLL_33ZS().RolTienePatente_33ZS(actual.Rol_33ZS, "GestionRespaldos"))
                throw new UnauthorizedAccessException("Su rol no tiene permiso para esta operación.");
        }

        public string ObtenerCarpetaPorDefecto_33ZS()
        {
            return _dal.ObtenerCarpetaPorDefecto_33ZS();
        }

        public string RealizarBackup_33ZS(string carpetaDestino)
        {
            ExigirPermiso_33ZS();
            if (string.IsNullOrWhiteSpace(carpetaDestino))
                throw new Exception("Backup.DebeSeleccionarCarpetaDestino");

            if (!Directory.Exists(carpetaDestino))
                throw new Exception("Backup.CarpetaDestinoNoExiste");

            string nombreArchivo = $"TpIngSoftware_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);

            _dal.RealizarBackup_33ZS(rutaCompleta);

            RegistrarEvento_33ZS(TipoEvento_33ZS.RealizarBackup);

            return rutaCompleta;
        }

        public void RealizarRestore_33ZS(string archivoBak)
        {
            ExigirPermiso_33ZS();
            if (string.IsNullOrWhiteSpace(archivoBak))
                throw new Exception("Backup.DebeSeleccionarArchivo");

            if (!File.Exists(archivoBak))
                throw new Exception("Backup.ArchivoNoExiste");

            _dal.RestaurarBackup_33ZS(archivoBak);

            // Un respaldo anterior puede no incluir los procedimientos actuales.
            DatabaseMigrator_33ZS.AplicarMigraciones_33ZS();

            _dv.GenerarTodo_33ZS();

            RegistrarEvento_33ZS(TipoEvento_33ZS.RealizarRestore);
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

                _bitacora.RegistrarEvento_33ZS(evento);
            }
            catch
            {
            }
        }
    }
}
