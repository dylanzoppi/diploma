using DAL;

namespace Mappers.Security
{
    public class BackupMapper_33ZS
    {
        private readonly DatabaseMaintenance_33ZS maintenance = new DatabaseMaintenance_33ZS();

        public string ObtenerCarpetaPorDefecto_33ZS() => maintenance.DefaultBackupPath_33ZS();
        public void RealizarBackup_33ZS(string rutaCompleta) => maintenance.Backup_33ZS(rutaCompleta);
        public void RestaurarBackup_33ZS(string rutaCompleta) => maintenance.Restore_33ZS(rutaCompleta);
    }
}
