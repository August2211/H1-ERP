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

            var data =  GetData($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Product] WHERE ProductID = {id}"); 
            
            Product product = new Product(); //obj 
          
            foreach (var s in data.Values)
            {
                    product.ProductId = (int)s[0];
                    product.Name = (string)s[1];
                    product.Description = (string)s[2];
                    product.SellingPrice = (decimal)s[3];
                    product.PurchasePrice = (decimal)s[4];
                    product.Location = (string)s[5];
                    product.ProductQuantity = (int)s[6];
                    product.Unit = (int)s[7];                
            }
            
            return product;
        }
        //Get all product and return it to the list 
        public List<Product> GetAllProduct() 
        {
            List<Product> list = new List<Product>();
            var listofproducst = GetData($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Product]");
            foreach(var s in listofproducst.Values)
            {
                Product product = new Product();
                product.ProductId = (int)s[0];
                product.Name = (string)s[1];
                product.Description = (string)s[2];
                product.PurchasePrice = (decimal)s[3];
                product.SellingPrice = (decimal)s[4];
                product.Location = (string)s[5];
                product.ProductQuantity= (int)s[6];
                product.Unit = (int)s[7];
                list.Add(product);
            }


            return list;
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
            string sql = $"UPDATE [H1PD021123_Gruppe4].[dbo].[Product] set ProductName = '{Input.Name}', ProductDescription = '{Input.Description}',ProductSalePrice = '{Input.SellingPrice}',ProductPurchasePrice = '{Input.PurchasePrice}', ProductLocation ='{Input.Location}', ProductQuantity'{Input.ProductQuantity}', ProductUnit='{Input.Unit}')";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        //DeleteSalesOrderHeaderFromID the product from dataBase

        public void DeleteProduct(int Input)
        {
            Exec_SQL_Command($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Product] WHERE ProductID = {Input.ProductId}"); 
        }
    }
}
