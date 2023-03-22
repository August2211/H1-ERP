using H1_ERP.DataBase;
using H1_ERP.DomainModel;
using System.Data.Entity;

namespace H1_ERP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            DataBase.DataBase datab= new DataBase.DataBase();
            Product product = datab.GetFromID(2);

            //Product product = new Product();
            //product.ProductId
            //    = 1;
            //product.Name = "Test";
            //product.Unit = 2;
            //product.Description = "Test";
            //product.ProductQuantity = 23;
            //product.SalePrice = 12.64m;
            //product.PurchasePrice = 5; 
            //product.Location = "sweeden";
            SalesOrderLine orderLine = new SalesOrderLine(product,4);
            List<SalesOrderLine> orders = new List<SalesOrderLine>();
            DataBase.DataBase hej = new DataBase.DataBase();
            hej.GetfromID(2); 
            orders.Add(orderLine);
           SalesOrderHeader hej2 = new SalesOrderHeader(orders); 
        }
    }
}