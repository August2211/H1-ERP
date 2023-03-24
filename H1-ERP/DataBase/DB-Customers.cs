using H1_ERP.DomainModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection.Metadata;
using System.Runtime.Loader;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {
        public Customer GetCustomerFromID(int id)
        {
            //for a customer obejct we're gonna need an adress aswell for the customer 
            Customer result = new Customer();
            result.CustomerId = id; 
            Address address = new Address();
            SqlConnection connection = getConnection(); 
            connection.Open();
            //Get the details of the Customer from the person table
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person] WHERE PersonID = {id}";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader reader = command.ExecuteReader();
            result.PersonID = Convert.ToUInt32(id);
            while (reader.Read())
            {
                result.CustomerId = reader.GetInt32(0);
                result.FirstName = reader.GetString(1);
                result.LastName = reader.GetString(2);
                result.Email = reader.GetString(3);
                result.PhoneNumber = reader.GetString(4);
            }
            reader.Close();
            //Get's the details of the adress which the customer is connected to
            command = new SqlCommand($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] WHERE AdressID = {id}", connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                address.AdressID =Convert.ToUInt32(reader.GetInt32(0));
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
            connection.Open();
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
            connection.Close();
            return result;
        }


        /// <summary>
        /// Inserts a Customer to our DB table
        /// </summary>
        /// <param name="input"></param>
        public void InsertCustomer(Customer input)
        {
            SqlConnection Connection = getConnection();
            //findes a the adress which has just been created
            uint adressid = InsertAddress(input.Address); 
            Connection.Open();
            
            //finds the current person that has been created for the customer
            uint personID = InsertPerson(input, adressid);
            SqlCommand sqlCommand = new SqlCommand($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customer.Customers] (LastPurchaseDate,PersonID) VALUES(@Date,{personID})", Connection);
            sqlCommand.Parameters.AddWithValue("@Date", input.LastPurchaseDate);
            sqlCommand.ExecuteNonQuery();
            Connection.Close();
        }
        /// <summary>
        /// Updates a Customer 
        /// </summary>
        /// <param name="input"></param>
        public void UpdateCustomer(Customer input)
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

        /// <summary>
        /// Delete's a customer by an ID from the DB
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteCustomer(int ID)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            Customer customercustomer = GetCustomerFromID(ID);
            string sqlgetrfkid = $"SELECT OrderID FROM [dbo].[Sales.Orders] WHERE CustomerID = {customercustomer.CustomerId}";
            SqlCommand command = new SqlCommand(sqlgetrfkid, connection);
            int OrderID = 0; 
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                OrderID = Convert.ToInt32(reader[0]);
            }
            reader.Close(); 

            command = new SqlCommand($"DELETE FROM [dbo].[Sales.Orderlines] WHERE OrderID = {OrderID}", connection);
            command.ExecuteNonQuery();
            command = new SqlCommand($"DELETE FROM [dbo].[Sales.Orders] WHERE OrderID = {OrderID}", connection);
            command.ExecuteNonQuery();
            command = new SqlCommand($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Customers] WHERE CustomerID ={customercustomer.CustomerId}", connection);
            command.ExecuteNonQuery();
            command = new SqlCommand($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person] WHERE PersonID = {customercustomer.PersonID}", connection);
            command.ExecuteNonQuery();
            command = new SqlCommand($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] WHERE AdressID = {customercustomer.Address.AdressID}",connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        /// <summary>
        /// Finds the ID of a newly inserted address 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        private uint InsertAddress(Address address)
        {
            uint AddressID = 0;
            SqlConnection connection = getConnection();
            connection.Open(); 
            SqlCommand command = new SqlCommand($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customer.Adress] (RoadName, StreetNumber, ZipCode, City, Country) VALUES  ('{address.RoadName}','{address.StreetNumber}','{address.ZipCode}','{address.City}','{address.Country}')",connection);
            command.ExecuteNonQuery();
            command = new SqlCommand("SELECT TOP (1) [AdressID] FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] ORDER BY [AdressID] desc;",connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                AddressID = Convert.ToUInt32(reader.GetValue(0));
            }
            return AddressID;
        }
        /// <summary>
        /// Finds the ID of a newly Inserted Person 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="adressid"></param>
        /// <returns></returns>
        private uint InsertPerson(Customer input,uint adressid)
        {

            uint PersonID = 0;
            SqlConnection connection = getConnection();
            connection.Open();
            SqlCommand command = new SqlCommand($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customers.Person] (FirstName,LastName,Email,PhoneNumber,AdressID) VALUES('{input.FirstName}','{input.LastName}','{input.Email}','{input.PhoneNumber}',{adressid})", connection);
            command.ExecuteNonQuery();
            command = new SqlCommand("SELECT TOP (1) [PersonID] FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person] ORDER BY [PersonID] desc;", connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                PersonID = Convert.ToUInt32(reader.GetValue(0));
            }
            return PersonID;
        }      
    }
}
