using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace H1_ERP.Display
{
    public class MenuDisplay : Screen
    {
        public override string Title { get; set; } = "Working Architects National Knowledge"; 

        protected override void Draw()
        {
            Menu menu = new Menu(); 
            DisplayCustomer displayCustomer= new DisplayCustomer(); 
            DisplayCompany displayCompany = new DisplayCompany();
            DisplayProduct displayProduct = new DisplayProduct();
            DisplaySalesScreen displaySalesOrderDetails = new DisplaySalesScreen();
            menu.Add(displayCustomer);
            menu.Add(displayCompany);
            menu.Add(displayProduct);
            menu.Add(displaySalesOrderDetails);
            menu.Start(this); 


        }
    }
}
