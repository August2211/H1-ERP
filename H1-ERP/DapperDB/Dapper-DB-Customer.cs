using Dapper;
using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DapperDB
{
    public partial class DatabaseDapper
    {
        public Customer GetCustomerFromID(int id)
        {
            using (var conn = getConnection())
            {
                var query = conn.Query<Customer, Address, Customer>($"SELECT " +
                $"[Customer.Customers].CustomerID, " +
                $"[Customers.Person].FirstName, " +
                $"[Customers.Person].LastName, " +
                $"[Customers.Person].Email, " +
                $"[Customers.Person].PhoneNumber, " +
                $"[Customer.Adress].AdressID, " +
                $"[Customer.Adress].RoadName, " +
                $"[Customer.Adress].StreetNumber, " +
                $"[Customer.Adress].ZipCode, " +
                $"[Customer.Adress].City, " +
                $"[Customer.Adress].Country, " +
                $"[Customers.Person].PersonID " +
                $"FROM [Customer.Customers] " +
                $"INNER JOIN [Customers.Person] on " +
                $"[Customers.Person].PersonID = [Customer.Customers].PersonID " +
                $"INNER JOIN [Customer.Adress] on " +
                $"[Customer.Adress].AdressID = [Customers.Person].AdressID " +
                $"WHERE CustomerID = {id}", (customer, address) =>
                {

                    customer.Address = address;
                    return customer;

                },splitOn: "AdressID");

                return query.FirstOrDefault(); 

            }



        }
        public List<Customer> GetAllCustomers()
        {

            using (var conn = getConnection())
            {
                var query = conn.Query<Customer, Address, Customer>($"SELECT " +
                $"[Customer.Customers].CustomerID, " +
                $"[Customers.Person].FirstName, " +
                $"[Customers.Person].LastName, " +
                $"[Customers.Person].Email, " +
                $"[Customers.Person].PhoneNumber, " +
                $"[Customer.Adress].AdressID, " +
                $"[Customer.Adress].RoadName, " +
                $"[Customer.Adress].StreetNumber, " +
                $"[Customer.Adress].ZipCode, " +
                $"[Customer.Adress].City, " +
                $"[Customer.Adress].Country, " +
                $"[Customers.Person].PersonID " +
                $"FROM [Customer.Customers] " +
                $"INNER JOIN [Customers.Person] on " +
                $"[Customers.Person].PersonID = [Customer.Customers].PersonID " +
                $"INNER JOIN [Customer.Adress] on [Customer.Adress].AdressID = [Customers.Person].AdressID", (customer, address) =>
                {
                    customer.Address = address;
                    return customer;
                }, splitOn: "AdressID"); 
                return query.ToList();
            }
        }

        public void DeleteCustomerFromID(int id)
        {

            using (var conn = getConnection())
            {
                Customer customer = GetCustomerFromID(id);
               var listofsalesorderheaders  =  conn.Query<SalesOrderHeader>
                    ($"SELECT OrderID FROM [dbo].[Sales.Orders] WHERE CustomerID = {customer.CustomerId}").ToList();
                string orderheadersting = ""; 
                if (listofsalesorderheaders != null)
                {
                    foreach (var orderheader in listofsalesorderheaders)
                    {
                        orderheadersting += orderheader.OrderID.ToString() + ",";  
                    }
                    orderheadersting =  orderheadersting.Remove(orderheadersting.Length - 1);
                    conn.Execute($"DELETE FROM [dbo].[Sales.OrderLines] WHERE OrderID IN ({orderheadersting}) " +
                $"DELETE FROM [dbo].[Sales.Orders] WHERE CustomerID = {id}");
                

                }
                conn.Execute
                (
                $"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Customers] WHERE CustomerID ={customer.CustomerId} " +
                $"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person] WHERE PersonID = {customer.PersonID} " +
                $"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] WHERE AdressID = {customer.Address.AdressID}"
                );

            }
        }
        public void UpdateCustomer(Customer Customer)
        {
            using (var conn = getConnection())
            {
                string nullhandelstring = Customer.LastPurchaseDate.ToString();
                if (nullhandelstring == null || nullhandelstring == "")
                {
                    nullhandelstring = "NULL";
                }
                conn.Execute($"UPDATE [H1PD021123_Gruppe4].[dbo].[Customer.Adress] SET RoadName = '{Customer.Address.RoadName}',StreetNumber = '{Customer.Address.StreetNumber}',ZipCode = '{Customer.Address.ZipCode}',City = '{Customer.Address.City}', Country = '{Customer.Address.Country}' WHERE AdressID = {Customer.Address.AdressID} " +
                $"UPDATE [H1PD021123_Gruppe4].[dbo].[Customers.Person] SET FirstName = '{Customer.FirstName}', LastName = '{Customer.LastName}', Email = '{Customer.Email}', PhoneNumber = '{Customer.PhoneNumber}' WHERE PersonID = {Customer.PersonID} " +
                $"UPDATE [H1PD021123_Gruppe4].[dbo].[Customer.Customers] SET LastPurchaseDate = {nullhandelstring} WHERE CustomerID = {Customer.CustomerId}");
            }
        }
    }
}
