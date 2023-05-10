using Dapper;
using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DapperDB
{
    public partial class DatabaseDapper
    {
        private static SqlConnection getConnection()
        {
            SqlConnectionStringBuilder sb = new();
            sb.DataSource = "192.168.1.70";
            sb.InitialCatalog = "H1PD021123_Gruppe4";
            sb.UserID = "H1PD021123_Gruppe4";
            sb.Password = "H1PD021123_Gruppe4";
            string connectionString = sb.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }


    }
}
