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
    internal class DatabaseDapper<T>
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

        public T GetSingleEntity(string sql)
        {
            using (var connenction = getConnection())
            {
                T Entity = connenction.QuerySingle<T>(sql);
                connenction.Close();
                return Entity;
            }
        }

        public (T1, T2) GetSingleEntityWithMultipleObectRef<T1, T2>(string sql, Func<T1, T2, bool> predicate, string splitOn)
        {
            using (var connection = getConnection())
            {
                var results = connection.Query<T1, T2, (T1, T2)>(
                    sql,
                    (t1, t2) =>
                    {
                        return (t1, t2);
                    },
                    splitOn: splitOn
                );

                var filteredResults = results.FirstOrDefault(t => predicate(t.Item1, t.Item2));

                connection.Close();

                return (filteredResults.Item1, filteredResults.Item2);
            }
        }

        public List<T> GetAllSingleEntities<T>(string sql)
        {
            using (var conn = getConnection())
            {
                var res = conn.Query<T>(sql, (Func<IDataReader, T>)(t1 =>
                {
                    return (T)t1;
                }));

                return res.ToList();
            }
        }
        public T GetSingleEntityWithMultipleObectRef2<T, U>(string sql, Func<T, U, T> map, string splitOn)
        {
            using (var connection = getConnection())
            {
                return connection.Query<T, U, T>(sql, map, splitOn: splitOn).FirstOrDefault();
            }
        }
    }
}
