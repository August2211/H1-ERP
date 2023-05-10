using H1_ERP.DomainModel;
using H1_ERP.ErrorHandling;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
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
        /// <summary>
        /// Gets a product with a specific id
        /// </summary>
        /// <param name="id">The id of the product</param>
        /// <returns>A product with the given id</returns>
        public Product GetProductFromID(int id)
        {
            var data =  GetData($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Product] WHERE ProductID = {id}"); 
            
            // Creates a new product and adds the respective values to it
            Product product = new Product(); 
          
            foreach (var s in data.Values)
            {
                    product.ProductID = (int)s[0];
                    product.ProductName = (string)s[1];
                    product.ProductDescription = (string)s[2];
                    product.ProductSalePrice = (decimal)s[3];
                    product.ProductPurchasePrice = (decimal)s[4];
                    product.ProductLocation = (string)s[5];
                    product.ProductQuantity = (int)s[6];
                    product.ProductUnit = (int)s[7];                
            }
            
            return product;
        }

        /// <summary>
        /// Gets all products from the databaseq
        /// </summary>
        /// <returns>A list of Product</returns>
        public List<Product> GetAllProduct() 
        {
            // Creates a list of products and adds the respective values to it
            List<Product> list = new List<Product>();
            var listOfProducts = GetData($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Product]");
            
            if(listOfProducts == null || listOfProducts.Count == 0)
            {
                return null;
            }

            foreach(var s in listOfProducts.Values)
            {
                Product product = new Product();
                product.ProductID = (int)s[0];
                product.ProductName = (string)s[1];
                product.ProductDescription = (string)s[2];
                product.ProductPurchasePrice = (decimal)s[3];
                product.ProductSalePrice = (decimal)s[4];
                product.ProductLocation = (string)s[5];
                product.ProductQuantity= (int)s[6];
                product.ProductUnit = (int)s[7];
                list.Add(product);
            }
            return list;
        }
        
        /// <summary>
        /// Inserts a product into the database
        /// </summary>
        /// <param name="Input">The product to be inserted into the database</param>
        public void InputProduct(Product Input)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Product](ProductName,ProductDescription,ProductSalePrice,ProductPurchasePrice,ProductLocation,ProductQuantity,ProductUnit) VALUES ('{Input.ProductName}','{Input.ProductDescription}','{Input.ProductSalePrice}','{Input.ProductPurchasePrice}','{Input.ProductLocation}','{Input.ProductQuantity}','{Input.ProductUnit}')";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        /// <summary>
        /// Updates a product in the database
        /// </summary>
        /// <param name="Input">The product to be updated</param>
        public void UpdataProduct(Product Input)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"UPDATE [H1PD021123_Gruppe4].[dbo].[Product] set ProductName = '{Input.ProductName}', ProductDescription = '{Input.ProductDescription}',ProductSalePrice = '{Input.ProductSalePrice}',ProductPurchasePrice = '{Input.ProductPurchasePrice}', ProductLocation ='{Input.ProductLocation}', ProductQuantity'{Input.ProductQuantity}', ProductUnit='{Input.ProductUnit}')";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// Deletes a product from the databasee
        /// </summary>
        /// <param name="ID">The id of the product to delete</param>
        public void DeleteProduct(int ID)
        {
            Exec_SQL_Command($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Product] WHERE ProductID = {ID}"); 
        }
    }
}
