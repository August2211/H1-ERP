using H1_ERP.DataBase;
using H1_ERP.Display;
using H1_ERP.DomainModel;
using System.Data.Entity;
using System.Runtime.Loader;
using TECHCOOL.UI;

namespace H1_ERP
{
    internal class Program
    {
        static void Main(string[] args)
        {


            test Test = new test();
            Test.start();
            DisplayCustomer customer = new DisplayCustomer();
            DisplayCompany companies = new DisplayCompany();


            Screen.Display(companies);


            {
                Console.WriteLine("Hello, World!");

                DataBase.DataBase dataBase = new DataBase.DataBase();

                Product product = dataBase.GetProductFromID(2);


                SalesOrderLine orderLine = new SalesOrderLine(product, 4);
                List<SalesOrderLine> orders = new List<SalesOrderLine>();
                DataBase.DataBase hej = new DataBase.DataBase();
                hej.GetProductFromID(2);
                orders.Add(orderLine);
                SalesOrderHeader hej2 = new SalesOrderHeader(orders);
            }
        }
    }
}