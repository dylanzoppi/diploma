using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Servicios
{
    public static class DatabaseInitializer_33ZS
    {
        private static readonly string[] InstanciasSql_33ZS = { ".", @".\SQLEXPRESS", @"(localdb)\MSSQLLocalDB" };
        private static string cadenaMaster_33ZS;

        private const string NombreBase_33ZS = "TpIngSoftware";

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

        public static bool AsegurarBaseDeDatos_33ZS()
        {
            if (BaseExiste_33ZS())
                return false;

            EjecutarScript_33ZS(LeerRecurso_33ZS("BD.sql"));
            EjecutarScript_33ZS(LeerRecurso_33ZS("QueryCargarDatosIniciales.sql"));

            return true;
        }

        private static bool BaseExiste_33ZS()
        {
            using (var cn = new SqlConnection(ObtenerCadenaMaster_33ZS()))
            {
                cn.Open();
                using (var cmd = new SqlCommand("SELECT DB_ID(@n)", cn))
                {
                    cmd.Parameters.AddWithValue("@n", NombreBase_33ZS);
                    var res = cmd.ExecuteScalar();
                    return res != null && res != DBNull.Value;
                }
            }
        }

        private static void EjecutarScript_33ZS(string script)
        {
            string[] lotes = Regex.Split(script, @"^\s*GO\s*$",
                                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            using (var cn = new SqlConnection(ObtenerCadenaMaster_33ZS()))
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
