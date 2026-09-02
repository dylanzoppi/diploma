using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DigitoVerificadorDAL_33ZS
    {
        public DataTable ObtenerTabla_33ZS(string tabla, string ordenarPor)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            string command = $"SELECT * FROM {tabla} ORDER BY {ordenarPor}";
            return acc.ExecuteDataSet_33ZS(command).Tables[0];
        }
        public void GuardarDV_33ZS(string tabla, string dvh, string dvv)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            string command = @"
                IF EXISTS (SELECT 1 FROM DV WHERE NombreTabla = @T)
                    UPDATE DV SET DVH = @DVH, DVV = @DVV WHERE NombreTabla = @T;
                ELSE
                    INSERT INTO DV (NombreTabla, DVH, DVV) VALUES (@T, @DVH, @DVV);";

            SqlParameter[] parametros =
            {
                new SqlParameter("@T", tabla),
                new SqlParameter("@DVH", dvh),
                new SqlParameter("@DVV", dvv)
            };

            acc.ExecuteNonQuery_33ZS(command, parametros);
        }
        public DataTable ObtenerDVGuardado_33ZS(string tabla)
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            string command = "SELECT DVH, DVV FROM DV WHERE NombreTabla = @T";
            SqlParameter[] parametros = { new SqlParameter("@T", tabla) };
            return acc.ExecuteDataSet_33ZS(command, parametros).Tables[0];
        }
    }
}
