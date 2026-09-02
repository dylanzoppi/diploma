using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class Acceso_33ZS
    {
        private static readonly string[] InstanciasSql_33ZS = { @"(localdb)\MSSQLLocalDB", @".\SQLEXPRESS" };
        private static string cadenaConexionBd_33ZS;
        private SqlConnection cn = new SqlConnection();
        private SqlTransaction TR = null;

        public void Conectar_33ZS()
        {
            cn.ConnectionString = ObtenerCadenaConexionBd_33ZS();
            cn.Open();
        }

        private static string ObtenerCadenaConexionBd_33ZS()
        {
            if (!string.IsNullOrWhiteSpace(cadenaConexionBd_33ZS))
                return cadenaConexionBd_33ZS;

            foreach (string instancia in InstanciasSql_33ZS)
            {
                string candidata = $@"Data Source={instancia};Initial Catalog=TpIngSoftware;Integrated Security=True";

                try
                {
                    using (SqlConnection conexion = new SqlConnection(candidata))
                    {
                        conexion.Open();
                        cadenaConexionBd_33ZS = candidata;
                        return cadenaConexionBd_33ZS;
                    }
                }
                catch
                {
                }
            }

            throw new InvalidOperationException("No se encontró una instancia SQL disponible en ., .\\SQLEXPRESS o (localdb)\\MSSQLLocalDB.");
        }

        public void Desconectar_33ZS()
        {
            if (cn.State == ConnectionState.Open)
                cn.Close();
        }

        public int ExecuteNonQuery_33ZS(string commandText, SqlParameter[] parametros = null)
        {
            int fa = 0;

            try
            {
                Conectar_33ZS();

                SqlCommand cmd = new SqlCommand(commandText, cn);

                if (parametros != null)
                    cmd.Parameters.AddRange(parametros);

                TR = cn.BeginTransaction();
                cmd.Transaction = TR;

                fa = cmd.ExecuteNonQuery();

                TR.Commit();

                return fa;
            }
            catch
            {
                TR?.Rollback();
                throw;
            }
            finally
            {
                Desconectar_33ZS();
            }
        }

        public DataSet ExecuteDataSet_33ZS(string commandText, SqlParameter[] parametros = null)
        {
            try
            {
                Conectar_33ZS();

                SqlCommand cmd = new SqlCommand(commandText, cn);

                if (parametros != null)
                    cmd.Parameters.AddRange(parametros);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();

                adapter.Fill(ds);

                return ds;
            }
            catch
            {
                throw;
            }
            finally
            {
                Desconectar_33ZS();
            }
        }
        
        public int ObtenerUltimoId_33ZS(string query)
        {
            int id = 0;
            try
            {
                Conectar_33ZS();
                SqlCommand comand = new SqlCommand(query, cn);
                id = Convert.ToInt32(comand.ExecuteScalar().ToString());
                return id;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Desconectar_33ZS();
            }
        }

        public void ExecuteNonQueryBatch_33ZS(List<Tuple<string, SqlParameter[]>> comandos)
        {
            try
            {
                Conectar_33ZS();
                TR = cn.BeginTransaction();

                foreach (var comando in comandos)
                {
                    SqlCommand cmd = new SqlCommand(comando.Item1, cn);
                    cmd.Transaction = TR;
                    if (comando.Item2 != null)
                        cmd.Parameters.AddRange(comando.Item2);
                    cmd.ExecuteNonQuery();
                }

                TR.Commit();
            }
            catch
            {
                TR?.Rollback();
                throw;
            }
            finally
            {
                Desconectar_33ZS();
            }
        }
    }
}
