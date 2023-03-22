using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1;
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
        public SalesOrderHeader GetfromID(int id)
        {
            SqlConnection conn = getConnection(); 
            conn.Open();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines] WHERE OrderID = {id}"; 
            SqlCommand commando = new SqlCommand(sql, conn);
            SqlDataReader sqlDataReader = commando.ExecuteReader();
            int rows = 0; 

           List<SalesOrderLine> list = new List<SalesOrderLine>();
            while(sqlDataReader.Read())
            {
                if(rows < sqlDataReader.FieldCount)
                {

                    sqlDataReader.GetValue(rows);

                    rows++; 
                }

            }

            return null; 
        }  

    }
}
