using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    // Ejecuta procedimientos y comparte una conexión cuando una operación de BLL
    // necesita guardar los datos y sus dígitos verificadores atómicamente.
    public sealed class Acceso_33ZS
    {
        [ThreadStatic] private static SqlConnection currentConnection;
        [ThreadStatic] private static SqlTransaction currentTransaction;

        public static void EjecutarEnTransaccion_33ZS(Action operation)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (currentTransaction != null)
            {
                operation();
                return;
            }

            using (SqlConnection connection = new SqlConnection(DatabaseConnection_33ZS.ConnectionString_33ZS()))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    currentConnection = connection;
                    currentTransaction = transaction;
                    try
                    {
                        operation();
                        transaction.Commit();
                    }
                    catch
                    {
                        try { transaction.Rollback(); }
                        catch (InvalidOperationException) { /* El SP ya pudo haber revertido la transacción. */ }
                        throw;
                    }
                    finally
                    {
                        currentTransaction = null;
                        currentConnection = null;
                    }
                }
            }
        }

        public DataSet ExecuteDataSet_33ZS(string procedure, SqlParameter[] parameters = null)
        {
            if (currentConnection != null)
            {
                using (SqlCommand command = CreateCommand(procedure, currentConnection, parameters))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataSet result = new DataSet();
                    adapter.Fill(result);
                    return result;
                }
            }
            using (SqlConnection connection = OpenConnection())
            using (SqlCommand command = CreateCommand(procedure, connection, parameters))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataSet result = new DataSet();
                adapter.Fill(result);
                return result;
            }
        }

        public int ExecuteNonQuery_33ZS(string procedure, SqlParameter[] parameters = null)
        {
            if (currentConnection != null)
            {
                using (SqlCommand command = CreateCommand(procedure, currentConnection, parameters))
                    return command.ExecuteNonQuery();
            }
            using (SqlConnection connection = OpenConnection())
            using (SqlCommand command = CreateCommand(procedure, connection, parameters))
                return command.ExecuteNonQuery();
        }

        public object ExecuteScalar_33ZS(string procedure, SqlParameter[] parameters = null)
        {
            if (currentConnection != null)
            {
                using (SqlCommand command = CreateCommand(procedure, currentConnection, parameters))
                    return command.ExecuteScalar();
            }
            using (SqlConnection connection = OpenConnection())
            using (SqlCommand command = CreateCommand(procedure, connection, parameters))
                return command.ExecuteScalar();
        }

        private static SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(DatabaseConnection_33ZS.ConnectionString_33ZS());
            connection.Open();
            return connection;
        }

        private static SqlCommand CreateCommand(string procedure, SqlConnection connection, SqlParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(procedure))
                throw new ArgumentException("El nombre del procedimiento es obligatorio.", nameof(procedure));
            SqlCommand command = new SqlCommand(procedure, connection, currentTransaction);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = 60;
            if (parameters != null) command.Parameters.AddRange(parameters);
            return command;
        }
    }
}
