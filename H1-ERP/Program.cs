using H1_ERP.DataBase;
using H1_ERP.Display;
using H1_ERP.DomainModel;
using System.Data.Entity;
using System.Runtime.Loader;
using TECHCOOL.UI;

namespace H1_ERP
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Address address= new Address();
            address.StreetNumber = "123";
            address.RoadName = "hej";
            address.Country = "denimarka"; 
            address.City = "123";
            address.ZipCode = "123";
           

            Customer customer = new Customer();
            customer.Address = address;
            customer.PhoneNumber = "123";
            customer.FirstName = "jens"; 
            customer.LastName = "jensen";
            customer.Email = "Jensjens@gmail.com"; 
            customer.LastPurchaseDate = DateTime.Now;

            DataBase.DataBase data = new DataBase.DataBase(); 
            data.InsertCustomer(customer);
            


        }
    }
}