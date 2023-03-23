using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace H1_ERP.Display
{
    public class SalesList
    {
        public string Title { get; set; } = "";        
        public SalesList(string title)
        {
            Title = title + " ";
        }
    }
    public class DisplaySalesScreen : Screen
    {
        public override string Title { get; set; } = "Sales Orders";
        protected override void Draw()
        {
            Clear(this);

            ListPage<SalesList> list = new ListPage<SalesList>();

            DataBase.DataBase db = new DataBase.DataBase();
            List<SalesOrderHeader> SalesOrderHeaders = db.GetAll();

            SalesOrderHeaders.ForEach(OrderHeader =>
            {
                List<SalesOrderLine> OrderLines = OrderHeader.OrderLines;
                //string OrderLineString = "";
                //OrderLines.ForEach(Line =>
                //{
                //    OrderLineString += $"Product: {Line.Product} | Unit Price: {Line.SingleUnitPrice} | Quantity: {Line.Quantity} | Total Price: {Line.TotalPrice}";
                //});

                Customer OrderCustomer = db.GetCustomerFromID((int)OrderHeader.CustomerID);

               string CustomerFullName = OrderCustomer.FullName();


                list.Add(new SalesList($"Order Line ID: {OrderHeader.CustomerID} | " +
                    $"Date Of Order: {OrderHeader.Creationtime} | " +
                    $"Expected Delivery Date: {OrderHeader.CompletionTime} |" +
                    $"Customer ID: {OrderHeader.CustomerID} " +
                    $"Full Name: {CustomerFullName} " +
                    $"Total Price: {OrderHeader.TotalOrderPrice()} | " /*+
                    $"{OrderLineString}"*/));
            });

            list.AddColumn("Sales Orders", "Title");

            list.Draw();

            Console.CursorVisible = false;
        }
    }
}
