using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DAL;

namespace Servicios
{
    public static class DatabaseMigrator_33ZS
    {
        private static readonly Migracion_33ZS[] Migraciones_33ZS =
        {
            new Migracion_33ZS("001_AlinearSeguridad", "001_AlinearSeguridad.sql"),
            new Migracion_33ZS("002_ProcedimientosSeguridad", "002_ProcedimientosSeguridad.sql"),
            new Migracion_33ZS("003_IntegridadFamilias", "003_IntegridadFamilias.sql"),
            new Migracion_33ZS("004_IntentosLogin", "004_IntentosLogin.sql"),
            new Migracion_33ZS("005_RolesPN1", "005_RolesPN1.sql"),
            new Migracion_33ZS("006_DatosPN1", "006_DatosPN1.sql"),
            new Migracion_33ZS("007_AtencionesPN1", "007_AtencionesPN1.sql"),
            new Migracion_33ZS("008_CatalogoServiciosPN1", "008_CatalogoServiciosPN1.sql"),
            new Migracion_33ZS("009_PermisosAtencionesPN1", "009_PermisosAtencionesPN1.sql")
        };

        public static void AplicarMigraciones_33ZS()
        {
            using (SqlConnection conexion = new SqlConnection(DatabaseConnection_33ZS.ConnectionString_33ZS()))
            {
                conexion.Open();
                AsegurarRegistroMigraciones_33ZS(conexion);

                foreach (Migracion_33ZS migracion in Migraciones_33ZS)
                {
                    if (EstaAplicada_33ZS(conexion, migracion.Id_33ZS))
                        continue;

                    using (SqlTransaction transaccion = conexion.BeginTransaction())
                    {
                        try
                        {
                            EjecutarScript_33ZS(conexion, transaccion, LeerRecurso_33ZS(migracion.Recurso_33ZS));

                            using (SqlCommand comando = new SqlCommand(
                                "INSERT INTO dbo.SchemaMigration (MigrationId) VALUES (@MigrationId)",
                                conexion,
                                transaccion))
                            {
                                comando.Parameters.AddWithValue("@MigrationId", migracion.Id_33ZS);
                                comando.ExecuteNonQuery();
                            }

                            transaccion.Commit();
                        }
                        catch
                        {
                            try { transaccion.Rollback(); }
                            catch (InvalidOperationException) { /* SQL Server ya revirtió la transacción. */ }
                            catch (SqlException) { /* Se conserva el error original de la migración. */ }
                            throw;
                        }
                    }
                }
            }
        }

        private static void AsegurarRegistroMigraciones_33ZS(SqlConnection conexion)
        {
            const string script = @"
                IF OBJECT_ID(N'dbo.SchemaMigration', N'U') IS NULL
                BEGIN
                    CREATE TABLE dbo.SchemaMigration
                    (
                        MigrationId varchar(100) NOT NULL,
                        AppliedAtUtc datetime2 NOT NULL CONSTRAINT DF_SchemaMigration_AppliedAtUtc DEFAULT (SYSUTCDATETIME()),
                        CONSTRAINT PK_SchemaMigration PRIMARY KEY CLUSTERED (MigrationId)
                    );
                END";

            using (SqlCommand comando = new SqlCommand(script, conexion))
                comando.ExecuteNonQuery();
        }

        private static bool EstaAplicada_33ZS(SqlConnection conexion, string migrationId)
        {
            using (SqlCommand comando = new SqlCommand(
                "SELECT COUNT(*) FROM dbo.SchemaMigration WHERE MigrationId = @MigrationId",
                conexion))
            {
                comando.Parameters.AddWithValue("@MigrationId", migrationId);
                return Convert.ToInt32(comando.ExecuteScalar()) > 0;
            }
        }

        private static void EjecutarScript_33ZS(SqlConnection conexion, SqlTransaction transaccion, string script)
        {
            string[] lotes = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (string lote in lotes)
            {
                if (string.IsNullOrWhiteSpace(lote))
                    continue;

                using (SqlCommand comando = new SqlCommand(lote, conexion, transaccion))
                    comando.ExecuteNonQuery();
            }
        }

        private static string LeerRecurso_33ZS(string nombreArchivo)
        {
            Assembly ensamblado = Assembly.GetExecutingAssembly();
            string recurso = Array.Find(
                ensamblado.GetManifestResourceNames(),
                nombre => nombre.EndsWith(nombreArchivo, StringComparison.OrdinalIgnoreCase));

            if (recurso == null)
                throw new FileNotFoundException("No se encontró la migración embebida: " + nombreArchivo);

            using (Stream stream = ensamblado.GetManifestResourceStream(recurso))
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        private sealed class Migracion_33ZS
        {
            public string Id_33ZS { get; }
            public string Recurso_33ZS { get; }

            public Migracion_33ZS(string id, string recurso)
            {
                Id_33ZS = id;
                Recurso_33ZS = recurso;
            }
        }
    }
}
