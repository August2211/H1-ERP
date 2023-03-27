using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {


        private SqlConnection getConnection()
        {
            SqlConnectionStringBuilder sb = new();
            sb.DataSource = "docker.data.techcollege.dk";
            sb.InitialCatalog = "H1PD021123_Gruppe4";
            sb.UserID = "H1PD021123_Gruppe4";
            sb.Password = "H1PD021123_Gruppe4";
            string connectionString = sb.ToString();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open(); 
            return connection;

        }
        /// <summary>
        /// Gets the data from a given SQL statement 
        /// </summary>
        /// <param name="Sql"> Wrtie your SQL statement in this</param>
        /// <param name="connection">Your connection to a given DB</param>
        /// <returns></returns>
        private List<object> GetData(string Sql, SqlConnection connection)
        {
            List<object> list = new List<object>();
            SqlCommand sqlCommand = new SqlCommand(Sql, connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            int rows = 0;
            int cols = 0;
            while (reader.Read())
            {
                while(reader.FieldCount > rows)
                {
                    rows++;
                }
            }
            reader.Close(); 
            return list; 

        }
        /// <summary>
        /// Executes a SQL command with a connection
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="connection"></param>
        private void Exec_SQL_Command(string SQL, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(SQL, connection);
            command.ExecuteNonQuery();  
        }
        
          
    }

}

