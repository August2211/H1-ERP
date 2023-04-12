using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace H1_ERP.Display
{
    public class DisplayCustomer : Screen
    {
        public override string Title { get; set; } = "Customers "; 

        private class CustomerDisplay
        {
            public string Title1 { get; set; }
            public string Title2 { get; set; }
            public string Title3 { get; set; }
            public string Title4 { get; set; }
            public string Title5 { get; set; }
            public string Title6 { get; set; }
            public int IntboxOutput { get; set; } = -1; 

            public CustomerDisplay(int referenceNumber, string fullname, string phoneNumber, string email)
            {
                Title1 = referenceNumber.ToString();      
                Title2= fullname.Replace("\"","");
                Title3 = phoneNumber;   
                Title4 = email;                
            }
            public CustomerDisplay(string fullname,string Roadname, string Country,string City,string Postalcode,DateTime? LastPruchaseTime)
            {
                Title1 = fullname;
                Title2 = Roadname;
                Title3 = Country;
                Title4= City;
                Title5 = Postalcode;
                Title6 = LastPruchaseTime.ToString();
            }
        } 
        /// <summary>
        /// main way of displaying customers
        /// </summary>
        protected override void Draw()
        {
            Clear(this); 
            ListPage<CustomerDisplay> listPage= new ListPage<CustomerDisplay>();
            
            
            DataBase.DataBase data = new DataBase.DataBase();
            List<Customer> customers = data.GetAllCustomers(); 
            foreach(var customer in customers)
            {
                listPage.Add(new CustomerDisplay(customer.CustomerId, customer.FullName(), customer.PhoneNumber, customer.Email)); 
            }
            //we add the length of the columns by taking the longest possible string and then we size the column by that number 
            listPage.AddColumn("ReferenceNumber", "Title1", 15);
            listPage.AddColumn("Fullname", "Title2", customers.Select(x => x.FullName().Length).Max());
            listPage.AddColumn("PhoneNumber", "Title3", customers.Select(x => x.PhoneNumber.Length).Max());
            listPage.AddColumn("Email", "Title4",customers.Select(x => x.Email.Length).Max());
            
            Form<CustomerDisplay> form = new Form<CustomerDisplay>();
              //User get's to press enter on 1 of our lines and afterwards they can se the details with a new customerdisplay of one customer.
             var SelectedRow= listPage.Select();             
             if(SelectedRow.Title1 != null)
             {
                Clear(this); 
                Console.Clear();
                ListPage<CustomerDisplay> SelectedCustomerDisplay = new ListPage<CustomerDisplay>();
                SelectedCustomerDisplay.AddColumn("Fullname", "Title1");
                SelectedCustomerDisplay.AddColumn("RoadName", "Title2");
                SelectedCustomerDisplay.AddColumn("Country", "Title3");
                SelectedCustomerDisplay.AddColumn("City", "Title4");
                SelectedCustomerDisplay.AddColumn("PostalCode", "Title5");
                Customer SelectedCustomer = customers.Select(x => x).Where(x => x.CustomerId == Convert.ToInt32(SelectedRow.Title1)).FirstOrDefault(); 
                SelectedCustomerDisplay.Add(new CustomerDisplay(SelectedCustomer.FullName(), SelectedCustomer.Address.RoadName, SelectedCustomer.Address.Country, SelectedCustomer.Address.City, SelectedCustomer.Address.ZipCode, SelectedCustomer.LastPurchaseDate));
                SelectedCustomerDisplay.Draw(); 
             }
              var alleheaders = data.GetAll(); 
             if(SelectedRow.Title1 == null) {
                listPage.Draw();
            }
        } 
    }
}
