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
        private static SqlConnection getConnection()
        {
            SqlConnectionStringBuilder sb = new();
            sb.DataSource = "docker.data.techcollege.dk";
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
        /// <summary>
        /// Gets the data from a given SQL statement 
        /// </summary>
        /// <param name="Sql"> Wrtie your SQL statement in this</param>
        /// <param name="connection">Your connection to a given DB</param>
        /// <returns>a dictionary with the rownumber as a key and the value as a list of obejcts(row content in database) :) if the statement does not contain anything it returns null</returns>
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
                foreach(var s in list)
                {
                    switch(s) 
                    {




                    }


                }
            }
            reader.Close();

            if (Rows.Count  <= 0)
            {
                return null; 
            }
            return Rows; 

        }


        public Dictionary<object, List<object>> GetData(string Sql)
        {
            var connection = getConnection();
            Dictionary<object, List<object>> Rows = new Dictionary<object, List<object>>();
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

            if (Rows.Count <= 0)
            {
                return null;
            }
            connection.Close();
            return Rows;
        }

        public Dictionary<object, object[]> GetDatafast(string Sql)
        {
            var connection = getConnection();
            Dictionary<object, object[]> Rows = new Dictionary<object, object[]>();
            using (var sqlCommand = new SqlCommand(Sql, connection))
            {
                using (var reader = sqlCommand.ExecuteReader())
                {
                    int rows = 0;

                    while (reader.Read())
                    {
                        rows = 0;
                        object[] list = new object[reader.FieldCount];

                        reader.GetValues(list);
                        

                        Rows.Add(reader.GetValue(0), list);
                    }
                    reader.Close();

                    if (Rows.Count <= 0)
                    {
                        return null;
                    }
                    connection.Close();
                    return Rows;
                }
            }
        }


        public Dictionary<object, object[]> GetdataFastFromJoinsWithouttheKeyvalueparoftheId(string sql)
        {
            var connection = getConnection();
            Dictionary<object, object[]> Rows = new Dictionary<object, object[]>();
            int i = 0; 
            using (var sqlCommand = new SqlCommand(sql, connection))
            {
                using (var reader = sqlCommand.ExecuteReader())
                {
                    int rows = 0;

                    while (reader.Read())
                    {
                        rows = 0;
                        object[] list = new object[reader.FieldCount];

                        reader.GetValues(list);


                        Rows.Add(i, list);
                        ++i; 
                    }
                    reader.Close();

                    if (Rows.Count <= 0)
                    {
                        return null;
                    }
                    connection.Close();
                    return Rows;
                }
            }

        }

        /// <summary>
        /// Executes a SQL command with a connection
        /// </summary>
        /// <param name="SQL"></param>
        /// <param name="connection"></param>
        public void Exec_SQL_Command(string SQL, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(SQL, connection);
            command.ExecuteNonQuery();  
        }

        public void Exec_SQL_Command(string SQL)
        {
            var connection = getConnection();
            SqlCommand command = new SqlCommand(SQL, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

}

