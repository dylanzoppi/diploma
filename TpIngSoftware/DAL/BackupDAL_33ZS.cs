using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BackupDAL_33ZS
    {
        private static readonly string[] InstanciasSql_33ZS = { ".", @".\SQLEXPRESS", @"(localdb)\MSSQLLocalDB" };
        private static string cadenaMaster_33ZS;

        private static string ObtenerCadenaMaster_33ZS()
        {
            if (!string.IsNullOrWhiteSpace(cadenaMaster_33ZS))
                return cadenaMaster_33ZS;

            foreach (string instancia in InstanciasSql_33ZS)
            {
                string candidata = $@"Data Source={instancia};Initial Catalog=master;Integrated Security=True";

                try
                {
                    using (SqlConnection conexion = new SqlConnection(candidata))
                    {
                        conexion.Open();
                        cadenaMaster_33ZS = candidata;
                        return cadenaMaster_33ZS;
                    }
                }
                catch
                {
                }
            }

            throw new InvalidOperationException("No se encontró una instancia SQL disponible en ., .\\SQLEXPRESS o (localdb)\\MSSQLLocalDB.");
        }

        private void EjecutarSinTransaccion_33ZS(string commandText, SqlParameter[] parametros = null)
        {
            using (SqlConnection cn = new SqlConnection(ObtenerCadenaMaster_33ZS()))
            {
                SqlCommand cmd = new SqlCommand(commandText, cn);
                if (parametros != null)
                    cmd.Parameters.AddRange(parametros);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public string ObtenerCarpetaPorDefecto_33ZS()
        {
            using (SqlConnection cn = new SqlConnection(ObtenerCadenaMaster_33ZS()))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS nvarchar(4000));", cn);
                cn.Open();
                object resultado = cmd.ExecuteScalar();
                return resultado == null || resultado == DBNull.Value
                    ? null
                    : resultado.ToString();
            }
        }

        public void RealizarBackup_33ZS(string rutaCompleta)
        {
            string command = "BACKUP DATABASE [TpIngSoftware] TO DISK = @ruta " +
                             "WITH FORMAT, INIT, NAME = N'Backup TpIngSoftware';";

            SqlParameter[] parametros = { new SqlParameter("@ruta", rutaCompleta) };
            EjecutarSinTransaccion_33ZS(command, parametros);
        }

        public void RestaurarBackup_33ZS(string rutaCompleta)
        {
            string command =
                "ALTER DATABASE [TpIngSoftware] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                "RESTORE DATABASE [TpIngSoftware] FROM DISK = @ruta WITH REPLACE; " +
                "ALTER DATABASE [TpIngSoftware] SET MULTI_USER;";

            SqlParameter[] parametros = { new SqlParameter("@ruta", rutaCompleta) };
            EjecutarSinTransaccion_33ZS(command, parametros);
        }
    }
}
