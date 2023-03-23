using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static H1_ERP.Display.SalesList;

namespace H1_ERP.Display
{
    public class SalesList
    {
        //Title property to hold the sales order data.
        public string Title { get; set; } = "";
        //IntBoxOutput property to select the OrderLineID.
        public int IntBoxOutput { get; set; } = -1;

        //Constructor to set the title.
        public SalesList(string title)
        {
            Title = title + " ";
        }
    }
    public class DisplaySalesScreen : Screen
    {
        //Header that says Sales Orders over the screen.
        public override string Title { get; set; } = "Sales Orders ";
        protected override void Draw()
        {
            //Clear the current screen.
            Clear(this);

            //Create a list with SalesOrders to show on the screen.
            ListPage<SalesList> list = new ListPage<SalesList>();

            SalesList SalesList = new SalesList("SalesList");

            //Editor to select the OrderLineID to get the order details.
            Form<SalesList> editor = new Form<SalesList>();

            //Int box to select the OrderLineID to get the order details.
            editor.IntBox("OrderlineID: ", "IntBoxOutput");

            DataBase.DataBase db = new DataBase.DataBase();

            //Get all the sales orders from the database.
            List<SalesOrderHeader> SalesOrderHeaders = db.GetAll();

            SalesOrderHeaders.ForEach(OrderHeader =>
            {
                List<SalesOrderLine> OrderLines = OrderHeader.OrderLines;

                //Get the customer from the current orderheader.
                Customer OrderCustomer = db.GetCustomerFromID((int)OrderHeader.CustomerID);

                //Get the customers full name.
                string CustomerFullName = OrderCustomer.FullName();

                //Add all the SalesOrder data to the list.
                list.Add(new SalesList($"Order Line ID: {OrderHeader.OrderID} | " +
                    $"Date Of Order: {OrderHeader.Creationtime} | " +
                    $"Expected Delivery Date: {OrderHeader.CompletionTime} | " +
                    $"Customer ID: {OrderHeader.CustomerID} " +
                    $"Full Name: {CustomerFullName} " +
                    $"Total Price: {OrderHeader.TotalOrderPrice()} |"));
            });

            //Create a coumn called Sales Orders and add the Title property to it.
            list.AddColumn("Sales Orders ", "Title");

            //Draw the list.
            list.Draw();

            //Try on editor to prevent crash when leaving it.
            try
            {
                editor.Edit(SalesList);
            }
            catch (Exception)
            { }

            //Clear everything currently on screen to make space for the orderline details.
            Clear(this);

            list = new ListPage<SalesList>();

            SalesOrderHeader OrderHeader = db.GetSalesOrderHeaderFromID(SalesList.IntBoxOutput);

            Customer OrderCustomer = db.GetCustomerFromID((int)OrderHeader.CustomerID);
            if (OrderCustomer.CustomerId != 0)
            {
                string CustomerFullName = OrderCustomer.FullName();

                //Add details to the list.
                list.Add(new SalesList($"Order ID: {OrderHeader.OrderID} | Date Of Order: {OrderHeader.Creationtime} | Expected Delivery Date {OrderHeader.CompletionTime} | Customer ID: {OrderHeader.CustomerID} | Full Name: {CustomerFullName}"));

                list.AddColumn("Sales Order ", "Title");

                //Show the list.
                list.Draw();
            }
            else
            {
                Console.WriteLine("Order not found!");
            }

            //Hide the cursor.
            Console.CursorVisible = false;
        }
    }
    public class DisplaySalesOrderDetails : Screen
    {
        public override string Title { get; set; }
        protected override void Draw()
        {
            Clear(this);
        }
    }
}
