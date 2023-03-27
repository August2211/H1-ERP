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
        private Dictionary<object,List<object>> GetData(string Sql, SqlConnection connection)
        {
            Dictionary <object,List<object>> Rows = new Dictionary<object, List<object>>();
            SqlCommand sqlCommand = new SqlCommand(Sql, connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            int rows = 0;
            
            while (reader.Read())
            {
                rows = 0; 
                List<object> list = new List<object>();
                while (reader.FieldCount > rows)
                {
                    list.Add(reader[rows]);
                    rows++;
                }
                
                Rows.Add(reader.GetValue(0), list); 
            }
            reader.Close();

            if (Rows.Count  <= 0)
            {
                return null; 
            }
            return Rows; 

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

