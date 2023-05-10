using H1_ERP.DomainModel;
using Org.BouncyCastle.Utilities.Zlib;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {
        /// <summary>
        /// Method to check if the SQL statement contains any illegal comments
        /// </summary>
        /// <param name="sql">The sql statement to check</param>
        /// <exception cref="Exception">Exception that is thrown if it contains illegal comments</exception>
        void IllegalComments(string sql)
        {
            int total = 0;
            int i = 1;
            foreach (char c in sql.ToCharArray())
            {
                if (c == '\'')
                {
                    total++;
                }

                if (c == '-' && sql.ToCharArray()[i] == '-')
                {
                    if (total % 2 == 0)
                    {
                        throw new Exception("SQL Inject Detected");
                    }
                    break;
                }
                i++;
            }

            // If there is a -- after the last ' then throw an exception
            if (sql.LastIndexOf('\'') < sql.LastIndexOf("--"))
            {
                throw new Exception("SQL Inject Detected");
            }
        }
        /// <summary>
        /// Creates a connection to the database
        /// </summary>
        /// <returns>A database connection</returns>
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
        /// <summary>
        /// Gets the data from a given SQL statement 
        /// </summary>
        /// <param name="Sql"> Wrtie your SQL statement in this</param>
        /// <param name="connection">Your connection to a given DB</param>
        /// <returns>a dictionary with the rownumber as a key and the value as a list of obejcts(row content in database) :) if the statement does not contain anything it returns null</returns>
        private Dictionary<object, List<object>> GetData(string Sql, SqlConnection connection)
        {
            // While there is data to read from the database then read it and add it to the dictionary
            IllegalComments(Sql);
            Dictionary<object, List<object>> Rows = new Dictionary<object, List<object>>();
            SqlCommand sqlCommand = new SqlCommand(Sql, connection);
            SqlDataReader reader = sqlCommand.ExecuteReader();
            int rows = 0;

            while (reader.Read())
            {
                // Creates a new list, adds the rows to the lists and puts it in the dictionary with the primary key as dictionary key
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
            return Rows;
        }

        /// <summary>
        /// Gets data from a database given an SQL statement
        /// </summary>
        /// <param name="Sql">The SQL statement to get the data</param>
        /// <returns>A dictionary of data</returns>
        public Dictionary<object, List<object>> GetData(string Sql)
        {
            // Does the same as the other GetData method but creates a new connection automatically
            IllegalComments(Sql);
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

        /// <summary>
        /// A faster version of the GetData method
        /// </summary>
        /// <param name="Sql">The SQL statement to get data with</param>
        /// <returns>A dictionary of the data</returns>
        public Dictionary<object, object[]> GetDatafast(string Sql)
        {
            // While there is data to read from the database then read it and add it to the dictionary
            IllegalComments(Sql);
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
        /// <summary>
        /// Gets data from a database given an SQL statement in a join safe way
        /// </summary>
        /// <param name="sql">The SQL statement to get data with</param>
        /// <returns>A dictionary of the data</returns>
        public Dictionary<object, object[]> GetDataFastJoinSafe(string sql)
        {
            // While there is data to read from the database then read it and add it to the dictionary without risk of duplicate key values
            IllegalComments(sql);
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
        /// <param name="SQL">The SQL statement to run</param>
        /// <param name="connection">The connection to run the SQL command on</param>
        public void Exec_SQL_Command(string SQL, SqlConnection connection)
        {
            IllegalComments(SQL);
            SqlCommand command = new SqlCommand(SQL, connection);
            command.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes an SQL command automatically opening a connection
        /// </summary>
        /// <param name="SQL">The SQL statement to run</param>
        public void Exec_SQL_Command(string SQL)
        {
            IllegalComments(SQL);
            var connection = getConnection();
            SqlCommand command = new SqlCommand(SQL, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}

