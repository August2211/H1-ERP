using H1_ERP.DomainModel;
using System.Data.SqlClient;
using System.Runtime.Loader;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {
        public Customer GetCustomerFromID(int id)
        {
            Customer result = new Customer(id);
            Address address = new Address(); 
            SqlConnection connection = getConnection(); 
            connection.Open();



            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person] WHERE PersonID = {id}";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader(); 
            while (reader.Read()) 
            { 
               result.CustomerId = reader.GetInt32(0);
               result.FirstName = reader.GetString(1);
                result.LastName = reader.GetString(2);
                result.Email = reader.GetString(3);
                result.PhoneNumber = reader.GetString(4);

            }
            reader.Close();
            command = new SqlCommand($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] WHERE AdressID = {id}", connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                address.RoadName = reader.GetString(1);
                address.StreetNumber = reader.GetString(2);
                address.ZipCode = reader.GetString(3);
                address.City = reader.GetString(4);
                address.Country= reader.GetString(5);
            }
            result.Address = address; 
            


            connection.Close(); 
            return result;
        }

        public List<Customer> GetAllCustomers()
        {
            SqlConnection connection= getConnection();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person]";
            List<int> ids = new List<int>();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader  sqlreader = command.ExecuteReader();
            List<Customer> result = new List<Customer>();
            while (sqlreader.Read())
            {
                ids.Add(sqlreader.GetInt32(0));
            }
            foreach(int id in ids)
            {
                Customer tempcustomer = GetCustomerFromID(id);
                result.Add(tempcustomer); 
               
            }
            return result;  
        }




    }
}
