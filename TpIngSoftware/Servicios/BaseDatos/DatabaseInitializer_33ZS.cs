using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using DAL;

namespace Servicios
{
    public static class DatabaseInitializer_33ZS
    {
        public static bool AsegurarBaseDeDatos_33ZS()
        {
            using (var master = new SqlConnection(DatabaseConnection_33ZS.MasterConnectionString_33ZS()))
            {
                master.Open();
                // Evita que dos procesos de la aplicación creen/inicialicen la misma BD a la vez.
                using (var lockCommand = new SqlCommand("sys.sp_getapplock", master))
                {
                    lockCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    lockCommand.Parameters.AddWithValue("@Resource", "TpIngSoftware.Initialize." + DatabaseConnection_33ZS.DatabaseName_33ZS());
                    lockCommand.Parameters.AddWithValue("@LockMode", "Exclusive");
                    lockCommand.Parameters.AddWithValue("@LockOwner", "Session");
                    var result = lockCommand.Parameters.Add("@Result", System.Data.SqlDbType.Int);
                    result.Direction = System.Data.ParameterDirection.ReturnValue;
                    lockCommand.ExecuteNonQuery();
                    if (Convert.ToInt32(result.Value) < 0)
                        throw new InvalidOperationException("No se pudo bloquear la inicialización de la base de datos.");
                }

                if (BaseExiste_33ZS(master))
                    return false;

                EjecutarScript_33ZS(PrepararNombreBase_33ZS(LeerRecurso_33ZS("BD.sql")));
                EjecutarScript_33ZS(PrepararNombreBase_33ZS(LeerRecurso_33ZS("QueryCargarDatosIniciales.sql")));
                return true;
            }
        }

        private static bool BaseExiste_33ZS(SqlConnection cn)
        {
            using (var cmd = new SqlCommand("SELECT DB_ID(@n)", cn))
            {
                cmd.Parameters.AddWithValue("@n", DatabaseConnection_33ZS.DatabaseName_33ZS());
                var res = cmd.ExecuteScalar();
                return res != null && res != DBNull.Value;
            }
        }

        private static string PrepararNombreBase_33ZS(string script)
        {
            string name = DatabaseConnection_33ZS.DatabaseName_33ZS();
            if (name.Length > 128)
                throw new InvalidOperationException("El nombre de la base de datos supera 128 caracteres.");
            return script.Replace("N'TpIngSoftware'", "N'" + name.Replace("'", "''") + "'")
                .Replace("[TpIngSoftware]", "[" + name.Replace("]", "]]") + "]");
        }

        private static void EjecutarScript_33ZS(string script)
        {
            string[] lotes = Regex.Split(script, @"^\s*GO\s*$",
                                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (var cn = new SqlConnection(DatabaseConnection_33ZS.MasterConnectionString_33ZS()))
            {
                cn.Open();
                foreach (var lote in lotes)
                {
                    if (string.IsNullOrWhiteSpace(lote))
                        continue;

                    using (var cmd = new SqlCommand(lote, cn))
                        cmd.ExecuteNonQuery();
                }
            }
        }

        private static string LeerRecurso_33ZS(string nombreArchivo)
        {
            var asm = Assembly.GetExecutingAssembly();

            string recurso = Array.Find(
                asm.GetManifestResourceNames(),
                n => n.EndsWith(nombreArchivo, StringComparison.OrdinalIgnoreCase));

            if (recurso == null)
                throw new FileNotFoundException(
                    "No se encontro el script embebido: " + nombreArchivo);

            using (var stream = asm.GetManifestResourceStream(recurso))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
