using System.Data;
using System.Data.SqlClient;

namespace DAL.Integrity
{
    public sealed class DigitoVerificadorDataAccess_33ZS
    {
        private readonly Acceso_33ZS acceso = new Acceso_33ZS();

        public DataTable ObtenerTabla_33ZS(string tabla) => acceso.ExecuteDataSet_33ZS(
            "dbo.DV_ObtenerTabla_33ZS", new[] { new SqlParameter("@Tabla", tabla) }).Tables[0];
        public DataTable ObtenerGuardado_33ZS(string tabla) => acceso.ExecuteDataSet_33ZS(
            "dbo.DV_ObtenerGuardado_33ZS", new[] { new SqlParameter("@Tabla", tabla) }).Tables[0];
        public void Guardar_33ZS(string tabla, string dvh, string dvv) =>
            acceso.ExecuteNonQuery_33ZS("dbo.DV_Guardar_33ZS", new[]
            {
                new SqlParameter("@Tabla", tabla), new SqlParameter("@DVH", dvh),
                new SqlParameter("@DVV", dvv)
            });
    }
}
