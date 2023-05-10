using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TECHCOOL.UI;
using static H1_ERP.Display.DisplayCompany;
using static H1_ERP.Display.SalesList;
using static H1_ERP.DomainModel.Company;

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

            SalesList SalesList = new SalesList("Order ID", "Date Of Order", "Expected Delivery Date", "Customer ID", "Customer Full ProductName", "Total Order Price");

            //Editor to select the OrderLineID to get the order details.
            Form<SalesList> editor = new Form<SalesList>();

            //Int box to select the OrderLineID to get the order details.
            editor.IntBox("Order Line ID: ", "IntBoxOutput");

            DataBase.DataBase db = new DataBase.DataBase();

            //Get all the sales orders from the database.
            List<SalesOrderHeader> SalesOrderHeaders = db.GetAll();

            //Get all customers
            List<Customer> Customers = db.GetAllCustomers();

            if (SalesOrderHeaders == null) { Console.WriteLine("No sales orders found."); return; };
            if (Customers == null) { Console.WriteLine("No customers found."); return; };

            SalesOrderHeaders.ForEach(OrderHeader =>
            {
                List<SalesOrderLine> OrderLines = OrderHeader.OrderLines;

                //Get the customer that made the order.
                Customer OrderCustomer = Customers.Find(Customer => Customer.CustomerId == OrderHeader.CustomerID);

                //Get the customers full name.
                string CustomerFullName = OrderCustomer.FullName();

                //Add all the SalesOrder data to the list.
                list.Add(new SalesList($"{OrderHeader.OrderID}",
                    $"{OrderHeader.DateOfOrder}",
                    $"{OrderHeader.ExpectedDeliveryDate}",
                    $"{OrderHeader.CustomerID}",
                    $"{CustomerFullName}",
                    $"{OrderHeader.TotalOrderPrice()}"));
            });

            Action<SalesListDetails> action = delegate (SalesListDetails salesOrderDetails)
            {
                //we take the path of the invocie template
                string path = "../../.././InvoiceTemplate/template.html";
                StreamReader reader = new StreamReader(path);
                List<string> lines = new List<string>();
                string html = "";
                while (!reader.EndOfStream)
                {


                    html += reader.ReadLine();
                }
                //switch the certaint text int the html file with our address and the invoice information 
                html = html.Replace("${Your company}", "Working Architects National Knowledge");
                html = html.Replace("${CompanyAddress}", "Tyrchit 30");
                html = html.Replace("${CompanyZipCode}", "736000");
                html = html.Replace("${CompanyCity}", "Khorog");
                html = html.Replace("${Receiver}", salesOrderDetails.CustomerFullName);
                //get alle information regarding the invoice
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
                //foreach orderline we have we add a new table row and 5 column's accordingly   
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
                //opens a browser window with the invoice 
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
            list.AddColumn("Customer Full ProductName ", "CustomerFullName", 25);
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
            }
            catch (Exception e) { }

            if (OrderHeader == null)
            {
                Console.WriteLine("Not Found!");
                MenuDisplay menuDisplay = new MenuDisplay();
                Console.ReadKey();
                Screen.Display(menuDisplay);
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
                    $"{OrderHeader.DateOfOrder}", $"{OrderHeader.ExpectedDeliveryDate}",
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


    public partial class DisplaySales : Screen
    {
        public override string Title { get; set; } = "Sales";
        public class SalesDisplay
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string RoadName { get; set; }
            public string StreetNumber { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
            public string ID { get; set; }

            public SalesDisplay(string firstName, string lastName, string roadName, string streetNumber, string zipCode, string city, string phoneNumber, string email)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                FirstName = firstName;
                LastName = lastName;
                RoadName = roadName;
                StreetNumber = streetNumber;
                ZipCode = zipCode;
                City = city;
                PhoneNumber = phoneNumber;
                Email = email;

            }
            public SalesDisplay(string firstName, string lastName, string roadName, string streetNumber, string zipCode, string city, string phoneNumber, string email, string id)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                FirstName = firstName;
                LastName = lastName;
                RoadName = roadName;
                StreetNumber = streetNumber;
                ZipCode = zipCode;
                City = city;
                PhoneNumber = phoneNumber;
                Email = email;
                ID = id;
            }

        }
        protected override void Draw()
        {
            Clear(this);
            Console.ForegroundColor = ConsoleColor.Blue;

            DataBase.DataBase db = new DataBase.DataBase();

            ListPage<SalesDisplay> listPage = new ListPage<SalesDisplay>();
            SalesDisplay salesDisplay = new SalesDisplay("FirstName", "LastName", "RoadName", "StreetNumber", "ZipCode", "city", "PhoneNumber", "Email");

            List<Customer> customers = db.GetAllCustomers();
            foreach (var Customer in customers)
            {
                listPage.Add(new SalesDisplay(Customer.FirstName, Customer.LastName, Customer.Address.RoadName, Customer.Address.StreetNumber, Customer.Address.ZipCode, Customer.Address.City, Customer.PhoneNumber, Customer.Email, Customer.CustomerId.ToString()));
            }
            listPage.AddColumn("FirstName", "FirstName", 10);
            listPage.AddColumn("LastName", "LastName", 10);
            listPage.AddColumn("RoadName", "RoadName", 10);
            listPage.AddColumn("StreetNumber", "StreetNumber", 5);
            listPage.AddColumn("ZipCode", "ZipCode", 5);
            listPage.AddColumn("City", "City", 10);
            listPage.AddColumn("PhoneNumber", "PhoneNumber", 15);
            listPage.AddColumn("Email", "Email", 20);


            Form<SalesDisplay> form = new Form<SalesDisplay>();
            try
            {
                Action<SalesDisplay> GoBackFunction = delegate (SalesDisplay display)
                {
                    MenuDisplay menuDisplay = new MenuDisplay();
                    Screen.Display(menuDisplay);
                };
                listPage.AddKey(ConsoleKey.Q, GoBackFunction);

                var SelectedRow = listPage.Select();
                if (SelectedRow != null)
                {


                    if (SelectedRow.FirstName != null)
                    {
                        Clear();
                        Console.Clear();
                        ListPage<SalesDisplay> SelectedSalesDisplay = new ListPage<SalesDisplay>();

                        SelectedSalesDisplay.AddColumn("FirstName", "FirstName", 10);
                        SelectedSalesDisplay.AddColumn("LastName", "LastName", 10);
                        SelectedSalesDisplay.AddColumn("RoadName", "RoadName", 10);
                        SelectedSalesDisplay.AddColumn("StreetNumber", "StreetNumber", 5);
                        SelectedSalesDisplay.AddColumn("ZipCode", "ZipCode", 5);
                        SelectedSalesDisplay.AddColumn("City", "City", 10);
                        SelectedSalesDisplay.AddColumn("PhoneNumber", "PhoneNumber", 15);
                        SelectedSalesDisplay.AddColumn("Email", "Email", 20);
                        Customer SelectedSales = customers.Select(x => x).Where(x => x.FirstName == SelectedRow.FirstName).FirstOrDefault();
                        SelectedSalesDisplay.Add(new SalesDisplay(SelectedSales.FirstName, SelectedSales.LastName, SelectedSales.Address.RoadName, SelectedSales.Address.StreetNumber, SelectedSales.Address.ZipCode, SelectedSales.Address.City, SelectedSales.PhoneNumber, SelectedSales.Email, SelectedSales.Address.AdressID.ToString()));

                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Click 'F1' to edit Sales.");
                        Console.WriteLine("Click 'F2' to add a new Sales.");
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine("Click 'Enter' to save.");
                        Console.WriteLine("Click 'Escape' to cancel.");
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        SelectedSalesDisplay.Draw();
                    }
                    ConsoleKeyInfo keyInfo = Console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.F2)
                    {

                        Form<SalesDisplay> editor = new Form<SalesDisplay>();
                        editor.TextBox("FirstName", "FirstName");
                        editor.TextBox("LastName", "LastName");
                        editor.TextBox("RoadName", "RoadName");
                        editor.TextBox("StreetNumber", "StreetNumber");
                        editor.TextBox("ZipCode", "ZipCode");
                        editor.TextBox("City", "City");
                        editor.TextBox("PhoneNumber", "PhoneNumber");
                        editor.TextBox("Email", "Email");

                        try
                        {
                            Console.SetCursorPosition(50, 10);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("-----Edit Sales details-----");
                            Console.SetCursorPosition(46, 12);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            editor.Edit(salesDisplay);


                        }
                        catch (Exception ex) { }


                        while (true)
                        {

                            ConsoleKeyInfo saveKeyInfo = Console.ReadKey();
                            if (saveKeyInfo.Key == ConsoleKey.Enter)
                            {
                                Customer newCustomer = new Customer();
                                Address newAddress = new Address();

                                newAddress.StreetNumber = salesDisplay.StreetNumber;
                                newAddress.City = salesDisplay.City;
                                newAddress.ZipCode = salesDisplay.ZipCode;
                                newAddress.RoadName = salesDisplay.RoadName;

                                var getdataFast = db.GetDatafast($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customer.Customers] Inner join [dbo].[Customers.Person] " +
                                    $"on [Customers.Person].PersonID = [Customer.Customers].PersonID inner join  [dbo].[Customer.Adress] " +
                                    $"on [Customers.Person].AdressID = [Customer.Adress].AdressID WHERE CustomerID = {SelectedRow.ID}");
                                newAddress.Country = getdataFast.ElementAt(0).Value[0].ToString();
                                newAddress.AdressID = Convert.ToUInt32(getdataFast.ElementAt(0).Value[8]);

                                newCustomer.FirstName = salesDisplay.FirstName;
                                newCustomer.LastName = salesDisplay.LastName;
                                newCustomer.PhoneNumber = salesDisplay.PhoneNumber;
                                newCustomer.Email = salesDisplay.Email;
                                newCustomer.PersonID = Convert.ToUInt32(getdataFast.ElementAt(0).Value[2]);


                                newCustomer.Address = newAddress;
                                newCustomer.CustomerId = newCustomer.CustomerId;

                                db.UpdateCustomer(newCustomer);

                                Console.SetCursorPosition(45, 21);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Sales edit saved.");
                                Console.ResetColor();
                                break;

                            }
                            else if (saveKeyInfo.Key == ConsoleKey.Escape)
                            {
                                Console.SetCursorPosition(45, 21);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("sSales edit cancelled.");
                                Console.ResetColor();
                                break;

                            }

                        }
                    }
                }
            }
            catch { }
        }
    }
}
