using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    // BACKUP/RESTORE son operaciones de instancia y no admiten una transacción de usuario.
    public sealed class DatabaseMaintenance_33ZS
    {
        private static SqlConnection OpenMaster()
        {
            SqlConnection connection = new SqlConnection(DatabaseConnection_33ZS.MasterConnectionString_33ZS());
            connection.Open();
            return connection;
        }

        private static string QuotedDatabaseName()
        {
            return "[" + DatabaseConnection_33ZS.DatabaseName_33ZS().Replace("]", "]]") + "]";
        }

        private static void Execute(SqlConnection connection, string sql, string path = null)
        {
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.CommandTimeout = 0;
                if (path != null)
                    command.Parameters.Add("@ruta", SqlDbType.NVarChar, 4000).Value = path;
                command.ExecuteNonQuery();
            }
        }

        public string DefaultBackupPath_33ZS()
        {
            using (SqlConnection connection = OpenMaster())
            using (SqlCommand command = new SqlCommand(
                "SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS nvarchar(4000))", connection))
            {
                object value = command.ExecuteScalar();
                return value == DBNull.Value || value == null ? null : value.ToString();
            }
        }

        public void Backup_33ZS(string path)
        {
            using (SqlConnection connection = OpenMaster())
            {
                Execute(connection, "BACKUP DATABASE " + QuotedDatabaseName() +
                    " TO DISK=@ruta WITH INIT,CHECKSUM", path);
                Execute(connection, "RESTORE VERIFYONLY FROM DISK=@ruta WITH CHECKSUM", path);
            }
        }

        public void Restore_33ZS(string path)
        {
            using (SqlConnection connection = OpenMaster())
            {
                // Evita aplicar accidentalmente un respaldo de otra base.
                using (SqlCommand header = new SqlCommand("RESTORE HEADERONLY FROM DISK=@ruta", connection))
                {
                    header.Parameters.Add("@ruta", SqlDbType.NVarChar, 4000).Value = path;
                    using (SqlDataReader reader = header.ExecuteReader())
                    {
                        if (!reader.Read() || !string.Equals(
                            reader["DatabaseName"].ToString(), DatabaseConnection_33ZS.DatabaseName_33ZS(),
                            StringComparison.OrdinalIgnoreCase))
                            throw new InvalidOperationException("El respaldo no corresponde a la base configurada.");
                    }
                }
                Execute(connection, "RESTORE VERIFYONLY FROM DISK=@ruta WITH CHECKSUM", path);
                string database = QuotedDatabaseName();
                Execute(connection, "ALTER DATABASE " + database + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                Exception restoreFailure = null;
                try
                {
                    Execute(connection, "RESTORE DATABASE " + database +
                        " FROM DISK=@ruta WITH REPLACE,CHECKSUM", path);
                }
                catch (Exception ex)
                {
                    restoreFailure = ex;
                    throw;
                }
                finally
                {
                    try
                    {
                        Execute(connection, "ALTER DATABASE " + database + " SET MULTI_USER");
                    }
                    catch (Exception resetFailure)
                    {
                        if (restoreFailure == null) throw;
                        throw new AggregateException("Fallaron la restauración y la vuelta a MULTI_USER.",
                            restoreFailure, resetFailure);
                    }
                }
            }
        }
    }
}
