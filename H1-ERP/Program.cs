using H1_ERP.DataBase;
using H1_ERP.Display;
using H1_ERP.DomainModel;
using System.Data.Entity;
using System.Runtime.Loader;
using TECHCOOL.UI;
using H1_ERP.DataBase;

namespace H1_ERP
{
    internal class Program
    {
        static void Main(string[] args)
        {


             
            MenuDisplay menu = new MenuDisplay();

            Screen.Display(menu); 

            DisplayCustomer display = new DisplayCustomer();
            Screen.Display(display);   

        }
    }
}