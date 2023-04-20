using H1_ERP.DataBase;
using H1_ERP.Display;
using H1_ERP.DomainModel;
using System.Data.Entity;
using System.Runtime.Loader;
using TECHCOOL.UI;
using H1_ERP.DataBase;
using H1_ERP.DapperDB;

namespace H1_ERP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MenuDisplay menuDisplay = new MenuDisplay();
            //Screen.Display(menuDisplay);
            DatabaseDapper<Customer> database = new DatabaseDapper<Customer>();
            var nemt = database.GetAllSingleEntities<Customer>("SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customer.Customers]"); 

        }
    }
}