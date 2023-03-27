using H1_ERP.DomainModel;
using H1_ERP.ErrorHandling;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Ubiety.Dns.Core.Records.NotUsed;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {
       
     
        public Product GetProductFromID(int id)
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
                    product.SellingPrice = (decimal)prod[3];
                    product.PurchasePrice = (decimal)prod[4];
                    product.Location = (string)prod[5];
                    product.ProductQuantity = (int)prod[6];
                    product.Unit = (int)prod[7];

                    rows++;
                }
            }
            return product;
        }
        //Get all product and return it to the list 
        public List<Product> GetAllProduct() 
        { SqlConnection connection = getConnection();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Product]";
            List<int> Id = new List<int>();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader sqlDataReader = command.ExecuteReader();
            List<Product> result = new List<Product>();
            
            while (sqlDataReader.Read())
            {
                Id.Add(sqlDataReader.GetInt32(0));
            }
            
            return result;
        }
        
        //InsertSalesOrderHeader the product and put it in dataBase
        public void InputProduct(Product Input)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Product](ProductName,ProductDescription,ProductSalePrice,ProductPurchasePrice,ProductLocation,ProductQuantity,ProductUnit) VALUES ('{Input.Name}','{Input.Description}','{Input.SellingPrice}','{Input.PurchasePrice}','{Input.Location}','{Input.ProductQuantity}','{Input.Unit}')";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        //Updata the product in dataBase
        public void UpdataProduct(Product Input)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"UPDATA [H1PD021123_Gruppe4].[dbo].[Product] set ProductName = '{Input.Name}', ProductDescription = '{Input.Description}',ProductSalePrice = '{Input.SellingPrice}',ProductPurchasePrice = '{Input.PurchasePrice}', ProductLocation ='{Input.Location}', ProductQuantity'{Input.ProductQuantity}', ProductUnit='{Input.Unit}')";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        //DeleteSalesOrderHeaderFromID the product from dataBase

        public void DeleteProduct(Product Input)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Product] WHERE ProductID = {Input.ProductId}";

            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
