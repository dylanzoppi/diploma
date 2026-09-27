using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL.Security
{
    public sealed class EventoDataAccess_33ZS
    {
        private readonly Acceso_33ZS acceso = new Acceso_33ZS();

        public DataTable ObtenerTodos_33ZS() =>
            acceso.ExecuteDataSet_33ZS("dbo.Evento_ObtenerTodos_33ZS").Tables[0];

        public void Registrar_33ZS(string login, DateTime fecha, DateTime hora,
            string modulo, string evento, int criticidad)
        {
            acceso.ExecuteNonQuery_33ZS("dbo.Evento_Registrar_33ZS", new[]
            {
                new SqlParameter("@Login", login), new SqlParameter("@Fecha", fecha),
                new SqlParameter("@Hora", hora), new SqlParameter("@Modulo", modulo),
                new SqlParameter("@EventoNombre", evento), new SqlParameter("@Criticidad", criticidad)
            });
        }
    }
}
