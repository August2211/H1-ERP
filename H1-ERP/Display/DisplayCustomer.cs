using H1_ERP.DomainModel;
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

            public CustomerDisplay(int referenceNumber, string fullname, string phoneNumber, string email)
            {
                Title1 = referenceNumber.ToString();      
                Title2= fullname.Replace("\"","");
                Title3 = phoneNumber;   
                Title4 = email;

                
            }
        } 

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
            listPage.AddColumn("ReferenceNumber", "Title1", 15);
            listPage.AddColumn("Fullname", "Title2", customers.Select(x => x.FullName().Length).Max());
            listPage.AddColumn("PhoneNumber", "Title3", customers.Select(x => x.PhoneNumber.Length).Max());
            listPage.AddColumn("Email", "Title4",customers.Select(x => x.Email.Length).Max());

            listPage.Draw();
        }
    }
}
