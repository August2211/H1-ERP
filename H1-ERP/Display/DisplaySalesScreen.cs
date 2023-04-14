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

            Action<SalesListDetails> action = delegate (SalesListDetails salesOrderDetails)
            {
                string path = "../../.././InvoiceTemplate/template.html";
                StreamReader reader = new StreamReader(path);
                List<string> lines = new List<string>();
                string html = "";
                while (!reader.EndOfStream)
                {


                    html += reader.ReadLine();
                }
                html = html.Replace("${Your company}", "Working Architects National Knowledge");
                html = html.Replace("${CompanyAddress}", "Tyrchit 30");
                html = html.Replace("${CompanyZipCode}", "736000");
                html = html.Replace("${CompanyCity}", "Khorog");
                html = html.Replace("${Receiver}", salesOrderDetails.CustomerFullName);
                var data = db.GetDatafast($"SELECT * FROM [dbo].[Customer.Customers] INNER JOIN " +
                    $"[dbo].[Customers.Person] ON " +
                    $"[Customer.Customers].PersonID = [Customers.Person].PersonID INNER JOIN " +
                    $"[dbo].[Customer.Adress] ON " +
                    $"[Customer.Adress].AdressID = [Customers.Person].AdressID INNER JOIN " +
                    $"[dbo].[Sales.Orders] ON" +
                    $"[Sales.Orders].CustomerID = [Customer.Customers].CustomerID WHERE [dbo].[Customer.Customers].CustomerID = {salesOrderDetails.CustomerID}");
                var data2 = db.GetDatafast($"SELECT * FROM [dbo].[Sales.OrderLines]" +
                    $"INNER JOIN [dbo].[Product] ON " +
                    $"[Product].ProductID = [Sales.OrderLines].ProductID " +
                    $"WHERE [Sales.OrderLines].OrderID = {salesOrderDetails.OrderID}");
                var data3 = db.GetDatafast($"SELECT CONCAT(FirstName,' ',LastName) AS FullName FROM [Company.Employees] INNER JOIN" +
                    $" [Customers.Person] ON" +
                    $" [Customers.Person].PersonID = [Company.Employees].PersonID INNER JOIN" +
                    $" [Sales.Orders] ON" +
                    $" [Sales.Orders].SalesPerson = [Company.Employees].Id WHERE [dbo].[Sales.Orders].CustomerID = {salesOrderDetails.CustomerID}");
                string address = data.ElementAt(0).Value[10] + " " + data.ElementAt(0).Value[11];
                string zipcode = data.ElementAt(0).Value[12].ToString();
                string city = data.ElementAt(0).Value[13].ToString();
                string country = data.ElementAt(0).Value[14].ToString();
                html = html.Replace("${Address}", address);
                html = html.Replace("${ZipCode}", zipcode);
                html = html.Replace("${City}", city);
                html = html.Replace("${Country}", country);
                html = html.Replace("${InvoiceNumber}", salesOrderDetails.OrderID);
                html = html.Replace("${InvoiceDate}", salesOrderDetails.DateOfOrder);
                html = html.Replace("${DueDate}", salesOrderDetails.ExpectedDeliveryDate);
                html = html.Replace("${Price}", data.ElementAt(0).Value[17].ToString());
                string ordertable = "";
                foreach (var s in data2.Values)
                {
                    ordertable += "<tr style=\"1px solid black;\"><td> ";
                    ordertable += s[8].ToString();
                    ordertable += "</td><td>";
                    ordertable += s[3].ToString();
                    ordertable += "</td><td>";
                    ordertable += s[13].ToString();
                    ordertable += "</td><td>";
                    ordertable += s[9].ToString();
                    ordertable += "</td><td>";
                    ordertable += s[12].ToString();
                    ordertable += "</td></tr>";
                }
                decimal withoutVAT = (decimal)data.ElementAt(0).Value[17] * 0.8m;
                html = html.Replace("${PriceWithoutVat}", withoutVAT.ToString("N2"));
                html = html.Replace("${OrderTable}", ordertable);
                html = html.Replace("${EmployeeName}", data3.ElementAt(0).Value[0].ToString());
                string fileName = $"Invoice {salesOrderDetails.OrderID}.html";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                File.WriteAllText(filePath, html);

                Process.Start(new ProcessStartInfo()
                {
                    UseShellExecute = true,
                    FileName = filePath
                });

            };

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

            SalesOrderHeader OrderHeader = null;
            try
            {
                OrderHeader = db.GetSalesOrderHeaderFromID(SalesList.IntBoxOutput);
            } catch (Exception e) { }

            if (OrderHeader == null)
            {
                Console.WriteLine("Not Found!");
                return;
            };


            Customer OrderCustomer = db.GetCustomerFromID((int)OrderHeader.CustomerID);

            //Only pull the customer data if his id is not 0 (He doesn't exist)
            if (OrderCustomer.CustomerId != 0)
            {

                SalesDetailsListPage.AddKey(ConsoleKey.F1, action);

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
                SalesDetailsListPage.Select(); 
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
