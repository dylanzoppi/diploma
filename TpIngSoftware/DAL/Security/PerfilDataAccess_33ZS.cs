using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Security
{
    public sealed class PerfilDataAccess_33ZS
    {
        private readonly Acceso_33ZS acceso = new Acceso_33ZS();

        public DataSet Patentes_33ZS() => acceso.ExecuteDataSet_33ZS("dbo.Perfil_ObtenerPatentes_33ZS");
        public DataSet Familias_33ZS() => acceso.ExecuteDataSet_33ZS("dbo.Perfil_ObtenerFamilias_33ZS");
        public DataSet Roles_33ZS() => acceso.ExecuteDataSet_33ZS("dbo.Perfil_ObtenerRoles_33ZS");

        private static SqlParameter Componentes(DataTable components) =>
            new SqlParameter("@Componentes", SqlDbType.Structured)
            { TypeName = "dbo.ComponentePerfil_33ZS", Value = components };

        public int GuardarFamilia_33ZS(string nombre, DataTable components) =>
            Convert.ToInt32(acceso.ExecuteScalar_33ZS("dbo.Perfil_GuardarFamilia_33ZS",
                new[] { new SqlParameter("@Nombre", nombre), Componentes(components) }));

        public int GuardarRol_33ZS(string nombre, DataTable components) =>
            Convert.ToInt32(acceso.ExecuteScalar_33ZS("dbo.Perfil_GuardarRol_33ZS",
                new[] { new SqlParameter("@Nombre", nombre), Componentes(components) }));

        public void ModificarFamilia_33ZS(int id, string nombre, DataTable components) =>
            acceso.ExecuteNonQuery_33ZS("dbo.Perfil_ModificarFamilia_33ZS",
                new[] { new SqlParameter("@Id", id), new SqlParameter("@Nombre", nombre), Componentes(components) });

        public void ModificarRol_33ZS(int id, string nombre, DataTable components) =>
            acceso.ExecuteNonQuery_33ZS("dbo.Perfil_ModificarRol_33ZS",
                new[] { new SqlParameter("@Id", id), new SqlParameter("@Nombre", nombre), Componentes(components) });

        public void EliminarFamilia_33ZS(int id) => acceso.ExecuteNonQuery_33ZS(
            "dbo.Perfil_EliminarFamilia_33ZS", new[] { new SqlParameter("@Id", id) });
        public void EliminarRol_33ZS(int id) => acceso.ExecuteNonQuery_33ZS(
            "dbo.Perfil_EliminarRol_33ZS", new[] { new SqlParameter("@Id", id) });
        public bool FamiliaEnUso_33ZS(int id) => Convert.ToInt32(acceso.ExecuteScalar_33ZS(
            "dbo.Perfil_FamiliaEnUso_33ZS", new[] { new SqlParameter("@Id", id) })) > 0;
        public bool RolEnUso_33ZS(int id) => Convert.ToInt32(acceso.ExecuteScalar_33ZS(
            "dbo.Perfil_RolEnUso_33ZS", new[] { new SqlParameter("@Id", id) })) > 0;
    }
}
