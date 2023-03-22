using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {
        public Product GetFromID(int id)
        {

            SqlConnection conn = getConnection();
            conn.Open();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Product] WHERE ProductID = {id}";
            SqlCommand commando = new SqlCommand(sql, conn);
            SqlDataReader sqlDataReader = commando.ExecuteReader();
            int rows = 0;
          Product product = new Product();
            while (sqlDataReader.Read())
            {
                if (rows < sqlDataReader.FieldCount)
                {
                    
                    sqlDataReader.GetValue(rows);
                    var prod = sqlDataReader;

                    product.ProductId = (int)prod[0];
                    product.Name = (string)prod[1];
                    product.Description = (string)prod[2];
                   //var SalePrice = prod.GetValue(3);
                    product.SalePrice = (decimal)prod[3];
                    product.PurchasePrice = (decimal)prod[4];
                    product.Location = (string)prod[5];
                    product.ProductQuantity = (int)prod[6];
                    product.Unit = (int)prod[7];

                    rows++;
                }
            }
            return product;


        }
    }
}
