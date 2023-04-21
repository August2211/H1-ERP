using Dapper;
using H1_ERP.DomainModel;
using Org.BouncyCastle.Math.EC.Multiplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DapperDB
{
    public partial class DatabaseDapper
    {

        public Product GetProductFromID(int ID)
        {
            using (var conn = getConnection())
            {

                var query = conn.QuerySingle<Product>($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Product] WHERE ProductID = {ID}"); 

                return query;

            }



        }

        public List<Product> GetallProducts()
        {

            using (var conn = getConnection())
            {
                var query = conn.Query<Product>($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Product]").ToList();
                return query; 
            }

        }

        public void DeleteProducFromID(int id)
        {
            using(var conn = getConnection())
            {
                conn.Execute($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Product] WHERE ProductID = {id}"); 
            }

        }


        public void UpdateProduct(Product Input)
        {
            using (var conn = getConnection())
            {
                conn.Execute($"UPDATE [H1PD021123_Gruppe4].[dbo].[Product] set ProductName = '{Input.ProductName}', ProductDescription = '{Input.ProductDescription}',ProductSalePrice = '{Input.ProductSalePrice}',ProductPurchasePrice = '{Input.ProductPurchasePrice}', ProductLocation ='{Input.ProductLocation}', ProductQuantity'{Input.ProductQuantity}', ProductUnit='{Input.ProductUnit}') WHERE ProductID = {Input.ProductID}"); 
            }
        }

    }
}
