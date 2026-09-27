using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public static class DatabaseConnection_33ZS
    {
        private const string SettingName = "TpIngSoftware";
        private const string EnvironmentName = "TPINGSOFTWARE_CONNECTION_STRING";

        public static string ConnectionString_33ZS()
        {
            string value = Environment.GetEnvironmentVariable(EnvironmentName);
            if (string.IsNullOrWhiteSpace(value))
            {
                ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[SettingName];
                value = setting == null ? null : setting.ConnectionString;
            }

            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException("Configure la conexión TpIngSoftware en App.config o TPINGSOFTWARE_CONNECTION_STRING.");

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(value);
            if (string.IsNullOrWhiteSpace(builder.InitialCatalog))
                throw new InvalidOperationException("La conexión TpIngSoftware debe indicar Initial Catalog.");
            return builder.ConnectionString;
        }

        public static string MasterConnectionString_33ZS()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(ConnectionString_33ZS());
            builder.InitialCatalog = "master";
            return builder.ConnectionString;
        }

        public static string DatabaseName_33ZS()
        {
            return new SqlConnectionStringBuilder(ConnectionString_33ZS()).InitialCatalog;
        }
    }
}
