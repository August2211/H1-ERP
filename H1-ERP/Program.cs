using H1_ERP.DataBase;
using H1_ERP.Display;
using H1_ERP.DomainModel;
using System.Data.Entity;
using System.Runtime.Loader;
using TECHCOOL.UI;
using H1_ERP.DataBase;
using H1_ERP.DapperDB;
using Org.BouncyCastle.Asn1.X509;

namespace H1_ERP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //MenuDisplay menuDisplay = new MenuDisplay();
            //Screen.Display(menuDisplay);
            DatabaseDapper database = new DatabaseDapper();
            var gg =  database.GetProductFromID(4); 
        }
    }
}