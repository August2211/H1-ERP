﻿using H1_ERP.DataBase;
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
            DisplayCustomer customer= new DisplayCustomer();    
            DisplayCompany companies= new DisplayCompany();


            Screen.Display(companies);   


        }
    }
}