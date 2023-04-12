using H1_ERP.DomainModel;
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

            
            
            foreach(Product product in products)
            {
                listPage.Add(new ProductDisplay(product.Name, product.ProductId.ToString(), product.ProductQuantity.ToString(), product.PurchasePrice, product.SellingPrice,(double) product.PurchasePrice / (double)product.SellingPrice * 100)); 

            } 


            listPage.AddColumn("Name", "Title1", 10);
            listPage.AddColumn("Itemnumber", "Title2", products.Select(x => x.Name.Length).Max());
            listPage.AddColumn("Quantity", "Title3", 10);
            listPage.AddColumn("BuyPrice", "Title4", 10);
            listPage.AddColumn("SalePrice", "Title5", 10);
            listPage.AddColumn("Margin %", "Title6", 10);

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
                    SelectedProduct.PurchasePrice, SelectedProduct.Location,(double)SelectedProduct.PurchasePrice / (double)SelectedProduct.SellingPrice * 100,
                    SelectedProduct.Unit,
                    ((double)SelectedProduct.PurchasePrice / (double)SelectedProduct.SellingPrice * 100)* (double)SelectedProduct.PurchasePrice,
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

            public ProductDisplay(string Name, string RefNumber,string quantity, decimal purchasePrice, decimal SellingPrice,double margininprocent) 
            {
              Title2 = RefNumber; Title3 = quantity;
                Title4 = purchasePrice.ToString(); 
                Title5 = SellingPrice.ToString();
                Title6 = margininprocent.ToString("N2");
                Title1 = Name; 
            }
            public ProductDisplay(string Itemnumber, string name , string describtion, decimal salesprice,decimal purchaseprice,string location, double margin, int unit,double margininKR,double quantity ) {
                Title1 = Itemnumber;  
                Title2= name;
                Title3= describtion.ToString();
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
