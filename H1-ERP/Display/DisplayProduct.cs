using Google.Protobuf.WellKnownTypes;
using H1_ERP.DomainModel;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace H1_ERP.Display
{
    internal class DisplayProduct : Screen
    {

        public override string Title { get; set; } = "Products";

        protected override void Draw()
        {
            //instance of the "DataBase" 
            DataBase.DataBase dataBase = new DataBase.DataBase();
            Clear(this);
            // instance of the "ListPage" class, specialized to hold elements of type "ProductDisplay"
            ListPage<ProductDisplay> listPage = new ListPage<ProductDisplay>();
            // It gets all products from database and puts them in a list.
            List<Product> products = dataBase.GetAllProduct();



            foreach (Product product in products)
            {
                //For each product in the list, it creates a new ProductDisplay object and adds it to the list page object.
                listPage.Add(new ProductDisplay(product.ProductName, product.ProductID.ToString(), product.ProductQuantity.ToString(), product.ProductPurchasePrice, product.ProductSalePrice, (double)product.ProductPurchasePrice / (double)product.ProductSalePrice * 100));

            }

            //It adds a column to the list page object with a specific title and width.
            listPage.AddColumn("ProductName", "Title1", 10);
            listPage.AddColumn("Itemnumber", "Title2", products.Select(x => x.ProductName.Length).Max());
            listPage.AddColumn("Quantity", "Title3", 10);
            listPage.AddColumn("BuyPrice", "Title4", 10);
            listPage.AddColumn("SalePrice", "Title5", 10);
            listPage.AddColumn("Margin %", "Title6", 10);

            // Declare a delegate called "editFunction" that takes a ProductDisplay object as input
            Action<ProductDisplay> editFunction = delegate (ProductDisplay product)
            {
                // Get data from a database based on the "Title2" property of the input object
                var data = dataBase.GetDatafast($"SELECT * FROM [dbo].[Product] WHERE ProductID = {product.Title2}");
                listPage = new ListPage<ProductDisplay>();

                // Add a column to the ListPage object for displaying the "Title1" property of ProductDisplay objects
                listPage.AddColumn("Edit", "Title1", 30);

                // Create a new ProductDisplay object from the retrieved data and add it to the ListPage object
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[0].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[1].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[2].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[3].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[4].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[5].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[6].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[7].ToString()));


                Clear(this);
                //Gets a collection of items from listPage and stores it in selectedProduct.
                var selectedProduct = listPage.Select();
                //Sets the console cursor position to a specific location.
                Console.SetCursorPosition(1, Console.GetCursorPosition().Top + 4);
                Console.Write("                              ");
                Console.SetCursorPosition(1, Console.GetCursorPosition().Top);
                //Waits for user input and stores it in newValue.
                string newValue = Console.ReadLine();
                //Creates an array of strings with a length of 8 and stores it in values.
                string[] values = new string[8];
               // Gets a specific value from data and stores it as the first element of values.
                values[0] = data.ElementAt(0).Value[0].ToString();
                values[1] = data.ElementAt(0).Value[1].ToString();
                values[2] = data.ElementAt(0).Value[2].ToString();
                values[3] = data.ElementAt(0).Value[3].ToString();
                values[4] = data.ElementAt(0).Value[4].ToString();
                values[5] = data.ElementAt(0).Value[5].ToString();
                values[6] = data.ElementAt(0).Value[6].ToString();
                values[7] = data.ElementAt(0).Value[7].ToString();
             
                //Checks if the cursor position is at the top of the console window.
                if (Console.GetCursorPosition().Top - 5 == 0)
                {
                    Clear(this);
                    Console.WriteLine("You cannot edit the ID");
                    Console.ReadKey();
                }
                else
                {
                   // Sets the appropriate element of values to the new value entered by the user.
                    values[Console.GetCursorPosition().Top - 5] = newValue;
                    //Executes an SQL command to update the relevant row in the database with the new values.
                    dataBase.Exec_SQL_Command($"UPDATE [dbo].[Product] SET ProductName = '{values[1]}', ProductDescription = '{values[2]}', ProductSalePrice = '{values[3]}', ProductPurchasePrice = '{values[4]}', ProductLocation = '{values[5]}', ProductQuantity = '{values[6]}', ProductUnit = '{values[7]}' WHERE ProductID = '{values[0]}'");
                    dataBase.Exec_SQL_Command($"UPDATE [dbo].[Sales.OrderLines] SET TotalQuantityPrice = (SinglePrice * OrderQuantity) WHERE ProductID = '{values[0]}'");
                    dataBase.Exec_SQL_Command($"UPDATE [dbo].[Sales.Orders] SET TotalPriceOfOrder = (SELECT SUM(Total) FROM [dbo].[Sales.OrderLines] WHERE OrderID = {values[0]}) WHERE OrderID = {values[0]}");
                    Clear(this);
                    Console.WriteLine("Product edited");
                    Console.ReadKey();
                }
            };
            //Defines a new delegate that takes a ProductDisplay object as a parameter.
            Action<ProductDisplay> GoBackFunction = delegate (ProductDisplay display)
            {
                // creates a new MenuDisplay object and displays it using the Screen.Display method.
                MenuDisplay menuDisplay = new MenuDisplay();
                Screen.Display(menuDisplay);
            };
            // Associate the "Q" key with the delegate
            listPage.AddKey(ConsoleKey.Q, GoBackFunction);

            // Define a delegate to remove a product from the database
            Action<ProductDisplay> removeFunction = delegate (ProductDisplay product)
            {  // Execute an SQL command to delete the product from the database
                var data = dataBase.GetDatafast($"DELETE FROM [dbo].[Product] WHERE ProductID = {product.Title2}");
            };
            // Define a delegate to add a new product to the database
            Action<ProductDisplay> addFunction = delegate (ProductDisplay product)
            {
                Clear(this);
                Console.WriteLine("Enter the name of the product");
                string name = Console.ReadLine();
                Console.WriteLine("Enter the description of the product");
                string description = Console.ReadLine();
                Console.WriteLine("Enter the sales price of the product");
                string salesPrice = Console.ReadLine();
                Console.WriteLine("Enter the purchase price of the product");
                string purchasePrice = Console.ReadLine();
                Console.WriteLine("Enter the location of the product");
                string location = Console.ReadLine();
                Console.WriteLine("Enter the quantity of the product");
                string quantity = Console.ReadLine();
                Console.WriteLine("Enter the unit of the product");
                string unit = Console.ReadLine();

                dataBase.Exec_SQL_Command($"INSERT INTO [dbo].[Product] (ProductName, ProductDescription, ProductSalePrice, ProductPurchasePrice, ProductLocation, ProductQuantity, ProductUnit) VALUES ('{name}', '{description}', '{salesPrice}', '{purchasePrice}', '{location}', '{quantity}', '{unit}')");

                Clear(this);
                Console.WriteLine("Product added");
                Console.ReadKey();
            };

            listPage.AddKey(ConsoleKey.F2, editFunction);
            listPage.AddKey(ConsoleKey.F5, removeFunction);
            listPage.AddKey(ConsoleKey.F3, addFunction);

            //Waits for the user to select a row in the listPage object, and stores the selected row in the SelectedRow variable.
            var SelectedRow = listPage.Select();

            //Checks if no row was selected and displays the main menu
            if (SelectedRow == null)
            {
                Clear(this);
                MenuDisplay menu = new MenuDisplay();
                Screen.Display(menu);
            }

            //Checks if a row was selected, and sets the Title property to "Product Details".
            if (SelectedRow.Title1 != null)
            {
                Title = "Product Details";
                Clear(this);
                Console.Clear();
                ListPage<ProductDisplay> SelectedCustomerDisplay = new ListPage<ProductDisplay>();
                SelectedCustomerDisplay.AddColumn("Itemnumber", "Title1");
                SelectedCustomerDisplay.AddColumn("ProductName", "Title2");
                SelectedCustomerDisplay.AddColumn("ProductDescription", "Title3");
                SelectedCustomerDisplay.AddColumn("SalesPrice", "Title4");
                SelectedCustomerDisplay.AddColumn("BuyPrice", "Title5");
                SelectedCustomerDisplay.AddColumn("Quantity", "Title6");
                SelectedCustomerDisplay.AddColumn("ProductLocation", "Title7");
                SelectedCustomerDisplay.AddColumn("Avance %", "Title8");
                SelectedCustomerDisplay.AddColumn("ProductUnit", "Title9");
                SelectedCustomerDisplay.AddColumn("Avance KR", "Title10");
                Product SelectedProduct = products.Select(x => x).Where(x => x.ProductID == Convert.ToInt32(SelectedRow.Title2)).FirstOrDefault();
                SelectedCustomerDisplay.Add(new ProductDisplay(
                    SelectedProduct.ProductID.ToString(),
                    SelectedProduct.ProductName,
                    SelectedProduct.ProductDescription,
                    SelectedProduct.ProductSalePrice,
                    SelectedProduct.ProductPurchasePrice, SelectedProduct.ProductLocation, (double)SelectedProduct.ProductPurchasePrice / (double)SelectedProduct.ProductSalePrice * 100,
                    SelectedProduct.ProductUnit,
                    ((double)SelectedProduct.ProductPurchasePrice / (double)SelectedProduct.ProductSalePrice * 100) * (double)SelectedProduct.ProductPurchasePrice,
                    SelectedProduct.ProductQuantity));
                SelectedCustomerDisplay.Draw();
            }
        }

        private class ProductDisplay
        {
            public string Title1 { get; set; }
            public string Title2 { get; set; }
            public string Title3 { get; set; }
            public string Title4 { get; set; }
            public string Title5 { get; set; }
            public string Title6 { get; set; }
            public string Title7 { get; set; }
            public string Title8 { get; set; }

            public string Title9 { get; set; }

            public string Title10 { get; set; }

            public ProductDisplay(string value)
            {
                Title1 = value;
            }
            //productDisplay constructor 
            public ProductDisplay(string Name, string RefNumber, string quantity, decimal purchasePrice, decimal SellingPrice, double margininprocent)
            {
                Title2 = RefNumber; Title3 = quantity;
                Title4 = purchasePrice.ToString();
                Title5 = SellingPrice.ToString();
                Title6 = margininprocent.ToString("N2");
                Title1 = Name;
            }
            public ProductDisplay(string Itemnumber, string name, string describtion, decimal salesprice, decimal purchaseprice, string location, double margin, int unit, double margininKR, double quantity)
            {
                Title1 = Itemnumber;
                Title2 = name;
                Title3 = describtion.ToString();
                Title4 = salesprice.ToString();
                Title5 = purchaseprice.ToString();
                Title6 = quantity.ToString();
                Title7 = location.ToString();
                Title8 = margin.ToString("N2");
                Title9 = unit.ToString();
                Title10 = margininKR.ToString("N2");
            }


        }


    }
}
