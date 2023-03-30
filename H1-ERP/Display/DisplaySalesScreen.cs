using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Crypto.Engines;
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
        //IntBoxOutput property to select the OrderLineID.
        public int IntBoxOutput { get; set; } = -1;
        public string OrderID { get; set; } = "";
        public string DateOfOrder { get; set; } = "";
        public string ExpectedDeliveryDate { get; set; } = "";
        public string CustomerID { get; set; } = "";
        public string CustomerFullName { get; set; } = "";
        public string TotalOrderPrice { get; set; } = "";
        public SalesList(string orderID, string dateOfOrder, string expectedDeliveryDate, string customerID, string customerFullName, string totalOrderPrice)
        {
            OrderID = orderID;
            DateOfOrder = dateOfOrder;
            ExpectedDeliveryDate = expectedDeliveryDate;
            CustomerID = customerID;
            CustomerFullName = customerFullName;
            TotalOrderPrice = totalOrderPrice;
        }
    }
    public class DisplaySalesScreen : Screen
    {
        //Header that says Sales Orders over the screen.
        public override string Title { get; set; } = "Orders";


        private class SalesListDetails
        {
            public string OrderID { get; set; }
            public string DateOfOrder { get; set; }
            public string ExpectedDeliveryDate { get; set; }
            public string CustomerID { get; set; }
            public string CustomerFullName { get; set; }

            public SalesListDetails(string orderID, string dateOfOrder, string expectedDeliveryDate, string customerID, string customerFullName)
            {
                OrderID = orderID;
                DateOfOrder = dateOfOrder;
                ExpectedDeliveryDate = expectedDeliveryDate;
                CustomerID = customerID;
                CustomerFullName = customerFullName;
            }
        }
        protected override void Draw()
        {
            //Clear the current screen.
            Clear(this);

            //Create a list with SalesOrders to show on the screen.
            ListPage<SalesList> list = new ListPage<SalesList>();

            SalesList SalesList = new SalesList("Order ID", "Date Of Order", "Expected Delivery Date", "Customer ID", "Customer Full Name", "Total Order Price");

            //Editor to select the OrderLineID to get the order details.
            Form<SalesList> editor = new Form<SalesList>();

            //Int box to select the OrderLineID to get the order details.
            editor.IntBox("Order Line ID: ", "IntBoxOutput");

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
                list.Add(new SalesList($"{OrderHeader.OrderID}",
                    $"{OrderHeader.Creationtime}",
                    $"{OrderHeader.CompletionTime}",
                    $"{OrderHeader.CustomerID}",
                    $"{CustomerFullName}",
                    $"{OrderHeader.TotalOrderPrice()}"));
            });



            //Create a coumn called Sales Orders and add the Title property to it.
            list.AddColumn("Order ID ", "OrderID");
            list.AddColumn("Date Of Order ", "DateOfOrder", 25);
            list.AddColumn("Expected Delivery Date ", "ExpectedDeliveryDate", 25);
            list.AddColumn("Customer ID ", "CustomerID");
            list.AddColumn("Customer Full Name ", "CustomerFullName", 25);
            list.AddColumn("Total Order Price", "TotalOrderPrice", 17);

            //Draw the list.
            Clear(this);
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

            ListPage<SalesListDetails> SalesDetailsListPage = new ListPage<SalesListDetails>();

            SalesOrderHeader OrderHeader = db.GetSalesOrderHeaderFromID(SalesList.IntBoxOutput);

            Customer OrderCustomer = db.GetCustomerFromID((int)OrderHeader.CustomerID);

            //Only pull the customer data if his id is not 0 (He doesn't exist)
            if (OrderCustomer.CustomerId != 0)
            {
                string CustomerFullName = OrderCustomer.FullName();

                //Add details to the list.
                SalesDetailsListPage.Add(new SalesListDetails($"{OrderHeader.OrderID}", 
                    $"{OrderHeader.Creationtime}", $"{OrderHeader.CompletionTime}", 
                    $"{OrderHeader.CustomerID}", 
                    $"{CustomerFullName}"));

                SalesDetailsListPage.AddColumn("Sales Order ", "OrderID");
                SalesDetailsListPage.AddColumn("Sales Order ", "DateOfOrder", 25);
                SalesDetailsListPage.AddColumn("Sales Order ", "ExpectedDeliveryDate", 25);
                SalesDetailsListPage.AddColumn("Sales Order ", "CustomerFullName");

                //Show the list.
                SalesDetailsListPage.Draw();
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
