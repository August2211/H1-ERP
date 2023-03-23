using H1_ERP.DomainModel;
using System.Data.SqlClient;
using System.Runtime.Loader;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {
        public Customer GetCustomerFromID(int id)
        {
            Customer result = new Customer();
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
                address.Country = reader.GetString(5);
            }

            //Only set the address if the id is not 0
            if (address.AdressID != 0)
            {
                result.Address = address;
            }

            connection.Close();

            return result;
        }

        public List<Customer> GetAllCustomers()
        {
            SqlConnection connection = getConnection();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person]";
            List<int> ids = new List<int>();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader sqlreader = command.ExecuteReader();
            List<Customer> result = new List<Customer>();
            while (sqlreader.Read())
            {
                ids.Add(sqlreader.GetInt32(0));
            }
            foreach (int id in ids)
            {
                Customer tempcustomer = GetCustomerFromID(id);
                result.Add(tempcustomer);

            }
            return result;
        }


        public void InsertCustomer(Customer input)
        {

            SqlConnection Connection = getConnection();
            Connection.Open();
            string sql = $"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customer.Adress] (RoadName,StreetNumber,ZipCode,City,Country) VALUES({input.Address.RoadName},{input.Address.StreetNumber},{input.Address.ZipCode},{input.Address.City},{input.Address.Country})";
            SqlCommand sqlCommand = new SqlCommand(sql, Connection);
        }

        public void UpdateCustomer(int id, Customer input)
        {
           SqlConnection connection = getConnection();
           SqlCommand command = new SqlCommand($"UPDATE [H1PD021123_Gruppe4].[dbo].[Customer.Adress] SET RoadName = '{input.Address.RoadName}',StreetNumber = '{input.Address.StreetNumber}',ZipCode = '{input.Address.ZipCode}',City = '{input.Address.City}', Country = '{input.Address.Country}' WHERE AdressID = {input.Address.AdressID}");
            command.ExecuteNonQuery();
            command = new SqlCommand($"UPDATE [H1PD021123_Gruppe4].[dbo].[Customers.Person] SET FirstName = '{input.FirstName}', LastName = '{input.LastName}', Email = '{input.Email}', PhoneNumber = '{input.PhoneNumber}'"); 
            command.ExecuteNonQuery();
            command = new SqlCommand($"UPDATE [H1PD021123_Gruppe4].[dbo].[Customer.Customer] SET LastPurchaseDate = {input.LastPurchaseDate}"); 
            command.ExecuteNonQuery();
            connection.Close(); 
        }


        public void DeleteCustomer(Customer input)
        {
            SqlConnection connection = getConnection();
            SqlCommand command = new SqlCommand($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] WHERE AdressID = {input.Address.AdressID}");
            command.ExecuteNonQuery();
            command = new SqlCommand($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person] WHERE PersonID = {input.PersonID}");
            command.ExecuteNonQuery();
            command = new SqlCommand($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customers.Customer] WHERE CustomerID ={input.CustomerId}"); 
            command.ExecuteNonQuery();  
            connection.Close();
        }





    }
}
