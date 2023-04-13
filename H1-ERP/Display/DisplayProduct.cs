using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
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
            DataBase.DataBase dataBase = new DataBase.DataBase();
            Clear(this);
            ListPage<ProductDisplay> listPage = new ListPage<ProductDisplay>();
            List<Product> products = dataBase.GetAllProduct();



            foreach (Product product in products)
            {
                listPage.Add(new ProductDisplay(product.Name, product.ProductId.ToString(), product.ProductQuantity.ToString(), product.PurchasePrice, product.SellingPrice, (double)product.PurchasePrice / (double)product.SellingPrice * 100));

            }


            listPage.AddColumn("Name", "Title1", 10);
            listPage.AddColumn("Itemnumber", "Title2", products.Select(x => x.Name.Length).Max());
            listPage.AddColumn("Quantity", "Title3", 10);
            listPage.AddColumn("BuyPrice", "Title4", 10);
            listPage.AddColumn("SalePrice", "Title5", 10);
            listPage.AddColumn("Margin %", "Title6", 10);

            Action<ProductDisplay> editFunction = delegate (ProductDisplay product)
            {
                var data = dataBase.GetDatafast($"SELECT * FROM [dbo].[Product] WHERE ProductID = {product.Title2}");

                listPage = new ListPage<ProductDisplay>();

                listPage.AddColumn("Edit", "Title1", 30);
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[0].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[1].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[2].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[3].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[4].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[5].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[6].ToString()));
                listPage.Add(new ProductDisplay(data.ElementAt(0).Value[7].ToString()));


                Clear(this);
                var selectedProduct = listPage.Select();
                Console.SetCursorPosition(1, Console.GetCursorPosition().Top + 4);
                Console.Write("                              ");
                Console.SetCursorPosition(1, Console.GetCursorPosition().Top);
                string newValue = Console.ReadLine();
                string[] values = new string[8];
                values[0] = data.ElementAt(0).Value[0].ToString();
                values[1] = data.ElementAt(0).Value[1].ToString();
                values[2] = data.ElementAt(0).Value[2].ToString();
                values[3] = data.ElementAt(0).Value[3].ToString();
                values[4] = data.ElementAt(0).Value[4].ToString();
                values[5] = data.ElementAt(0).Value[5].ToString();
                values[6] = data.ElementAt(0).Value[6].ToString();
                values[7] = data.ElementAt(0).Value[7].ToString();

                if (Console.GetCursorPosition().Top - 5 == 0)
                {
                    Clear(this);
                    Console.WriteLine("You cannot edit the ID");
                    Console.ReadKey();
                }
                else
                {

                    values[Console.GetCursorPosition().Top - 5] = newValue;

                    dataBase.Exec_SQL_Command($"UPDATE [dbo].[Product] SET ProductName = '{values[1]}', ProductDescription = '{values[2]}', ProductSalePrice = '{values[3]}', ProductPurchasePrice = '{values[4]}', ProductLocation = '{values[5]}', ProductQuantity = '{values[6]}', ProductUnit = '{values[7]}' WHERE ProductID = '{values[0]}'");
                    dataBase.Exec_SQL_Command($"UPDATE [dbo].[Sales.OrderLines] SET TotalQuantityPrice = (SinglePrice * OrderQuantity) WHERE ProductID = '{values[0]}'");
                    dataBase.Exec_SQL_Command($"UPDATE [dbo].[Sales.Orders] SET TotalPriceOfOrder = (SELECT SUM(Total) FROM [dbo].[Sales.OrderLines] WHERE OrderID = {values[0]}) WHERE OrderID = {values[0]}");
                    Clear(this);
                    Console.WriteLine("Product edited");
                    Console.ReadKey();
                }
            };

            Action<ProductDisplay> removeFunction = delegate (ProductDisplay product)
            {
                var data = dataBase.GetDatafast($"DELETE FROM [dbo].[Product] WHERE ProductID = {product.Title2}");
            };

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

            var SelectedRow = listPage.Select();

            if (SelectedRow == null)
            {
                Clear(this);
                MenuDisplay menu = new MenuDisplay();
                Screen.Display(menu);
            }
            if (SelectedRow.Title1 != null)
            {
                Title = "Product Details";
                Clear(this);
                Console.Clear();
                ListPage<ProductDisplay> SelectedCustomerDisplay = new ListPage<ProductDisplay>();
                SelectedCustomerDisplay.AddColumn("Itemnumber", "Title1");
                SelectedCustomerDisplay.AddColumn("Name", "Title2");
                SelectedCustomerDisplay.AddColumn("Description", "Title3");
                SelectedCustomerDisplay.AddColumn("SalesPrice", "Title4");
                SelectedCustomerDisplay.AddColumn("BuyPrice", "Title5");
                SelectedCustomerDisplay.AddColumn("Quantity", "Title6");
                SelectedCustomerDisplay.AddColumn("Location", "Title7");
                SelectedCustomerDisplay.AddColumn("Avance %", "Title8");
                SelectedCustomerDisplay.AddColumn("Unit", "Title9");
                SelectedCustomerDisplay.AddColumn("Avance KR", "Title10");
                Product SelectedProduct = products.Select(x => x).Where(x => x.ProductId == Convert.ToInt32(SelectedRow.Title2)).FirstOrDefault();
                SelectedCustomerDisplay.Add(new ProductDisplay(
                    SelectedProduct.ProductId.ToString(),
                    SelectedProduct.Name,
                    SelectedProduct.Description,
                    SelectedProduct.SellingPrice,
                    SelectedProduct.PurchasePrice, SelectedProduct.Location, (double)SelectedProduct.PurchasePrice / (double)SelectedProduct.SellingPrice * 100,
                    SelectedProduct.Unit,
                    ((double)SelectedProduct.PurchasePrice / (double)SelectedProduct.SellingPrice * 100) * (double)SelectedProduct.PurchasePrice,
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
