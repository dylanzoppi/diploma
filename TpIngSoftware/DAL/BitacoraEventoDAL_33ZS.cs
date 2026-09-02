using System;
using System.Collections.Generic;
using System.Data; 
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicios;

namespace DAL
{
    public class BitacoraEventoDAL_33ZS
    {
        private int ProximoId_33ZS()
        {
            string query = "SELECT ISNULL(MAX(Id_Evento), 0) + 1 FROM Evento";            
            Acceso_33ZS acceso = new Acceso_33ZS();
            return acceso.ObtenerUltimoId_33ZS(query); 
        }
        
        public void RegistrarEvento_33ZS(BitacoraEvento_33ZS evento)
        {
            int nuevoId = ProximoId_33ZS();

            string command = "INSERT INTO Evento (Id_Evento, Login, Fecha, Hora, Modulo, Evento, Criticidad) " +
                             "VALUES (@IdEvento, @Login, @Fecha, @Hora, @Modulo, @EventoNombre, @Criticidad)";
            SqlParameter[] parametros = new SqlParameter[]
            {
                new SqlParameter("@IdEvento", nuevoId),
                new SqlParameter("@Login", evento.Login_33ZS),
                new SqlParameter("@Fecha", evento.Fecha_33ZS), 
                new SqlParameter("@Hora", evento.Hora_33ZS),   
                new SqlParameter("@Modulo", evento.Modulo_33ZS),
                new SqlParameter("@EventoNombre", evento.NombreEvento_33ZS),
                new SqlParameter("@Criticidad", evento.Criticidad_33ZS)
            };
            Acceso_33ZS acceso = new Acceso_33ZS();
            acceso.ExecuteNonQuery_33ZS(command, parametros);
        }

        public List<BitacoraEvento_33ZS> ObtenerEventos_33ZS()
        {
            Acceso_33ZS acc = new Acceso_33ZS();
            string command = "SELECT * FROM Evento ORDER BY Fecha DESC, Hora DESC";
            DataSet ds = acc.ExecuteDataSet_33ZS(command);
            
            List<BitacoraEvento_33ZS> listaEventos = new List<BitacoraEvento_33ZS>();

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    listaEventos.Add(new BitacoraEvento_33ZS(dr));
                }
            }            
            return listaEventos;
        }
    }
}
