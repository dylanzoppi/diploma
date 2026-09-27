using System.Data;
using System.Data.SqlClient;

namespace DAL.Security
{
    public sealed class UsuarioDataAccess_33ZS
    {
        private readonly Acceso_33ZS acceso = new Acceso_33ZS();
        private static SqlParameter P(string name, object value) => new SqlParameter(name, value);

        public DataTable Todos_33ZS() => acceso.ExecuteDataSet_33ZS("dbo.Usuario_ObtenerTodos_33ZS").Tables[0];
        public DataTable PorLogin_33ZS(string login) => acceso.ExecuteDataSet_33ZS(
            "dbo.Usuario_ObtenerPorLogin_33ZS", new[] { P("@Login", login) }).Tables[0];
        public DataTable PorEstado_33ZS(bool activo) => acceso.ExecuteDataSet_33ZS(
            "dbo.Usuario_ObtenerPorEstado_33ZS", new[] { P("@Activo", activo) }).Tables[0];
        public DataTable PorNombre_33ZS(string login) => acceso.ExecuteDataSet_33ZS(
            "dbo.Usuario_ObtenerPorNombre_33ZS", new[] { P("@Login", login) }).Tables[0];
        public DataTable PorDNI_33ZS(string dni) => acceso.ExecuteDataSet_33ZS(
            "dbo.Usuario_ObtenerPorDNI_33ZS", new[] { P("@DNI", dni) }).Tables[0];

        public void Agregar_33ZS(string dni, string apellidos, string nombre, string login,
            string password, string rol, string email, bool bloqueo, bool activo)
        {
            acceso.ExecuteNonQuery_33ZS("dbo.Usuario_Agregar_33ZS", new[]
            {
                P("@DNI", dni), P("@Apellidos", apellidos), P("@Nombre", nombre),
                P("@Login", login), P("@Password", password), P("@Rol", rol),
                P("@Email", email), P("@Bloqueo", bloqueo), P("@Activo", activo)
            });
        }

        public void Modificar_33ZS(string dni, string apellidos, string nombre, string login,
            string rol, string email, bool bloqueo, bool activo)
        {
            acceso.ExecuteNonQuery_33ZS("dbo.Usuario_Modificar_33ZS", new[]
            {
                P("@DNI", dni), P("@Apellidos", apellidos), P("@Nombre", nombre),
                P("@Login", login), P("@Rol", rol), P("@Email", email),
                P("@Bloqueo", bloqueo), P("@Activo", activo)
            });
        }

        public void Bloquear_33ZS(string dni) => acceso.ExecuteNonQuery_33ZS(
            "dbo.Usuario_Bloquear_33ZS", new[] { P("@DNI", dni) });
        public void Desbloquear_33ZS(string dni) => acceso.ExecuteNonQuery_33ZS(
            "dbo.Usuario_Desbloquear_33ZS", new[] { P("@DNI", dni) });
        public void CambiarEstado_33ZS(string dni, bool activo) => acceso.ExecuteNonQuery_33ZS(
            "dbo.Usuario_CambiarEstado_33ZS", new[] { P("@DNI", dni), P("@Activo", activo) });
        public void ActualizarIdioma_33ZS(string dni, string idioma) => acceso.ExecuteNonQuery_33ZS(
            "dbo.Usuario_ActualizarIdioma_33ZS", new[] { P("@DNI", dni), P("@Idioma", idioma) });
        public void CambiarClave_33ZS(string dni, string passwordHash) => acceso.ExecuteNonQuery_33ZS(
            "dbo.Usuario_CambiarClave_33ZS", new[] { P("@DNI", dni), P("@Password", passwordHash) });

        public int RegistrarIntentoFallido_33ZS(string dni, int maxIntentos) =>
            (int)acceso.ExecuteScalar_33ZS("dbo.Usuario_RegistrarIntentoFallido_33ZS",
                new[] { P("@DNI", dni), P("@MaxIntentos", maxIntentos) });

        public void ReiniciarIntentos_33ZS(string dni) => acceso.ExecuteNonQuery_33ZS(
            "dbo.Usuario_ReiniciarIntentos_33ZS", new[] { P("@DNI", dni) });
    }
}
