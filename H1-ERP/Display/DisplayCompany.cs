using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TECHCOOL.UI;
using static H1_ERP.Display.DisplayCompany;

namespace H1_ERP.Display
{

    public partial class DisplayCompany : Screen
    {

        public override string Title { get; set; } = "Company";


        public class CompanyDisplay
        {
            //Properties
            public string CompanyName { get; set; }
            public string Country { get; set; }
            public Company.currency Currency { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string RoadName { get; set; }
            public string StreetNumber { get; set; }
            public string TempCurrency { get; set; }

            public string TempID { get; set; }

            //constructor 
            public CompanyDisplay(string companyName, string country, Company.currency currency, string tempID)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                CompanyName = companyName;
                Country = country;
                Currency = currency;
                TempID = tempID;
            }

            //constructor 
            public CompanyDisplay(string companyName, string country, Company.currency currency, string zipCode, string city, string roadName, string streetNumber)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                CompanyName = companyName;
                Country = country;
                Currency = currency;
                ZipCode = zipCode;
                City = city;
                RoadName = roadName;
                StreetNumber = streetNumber;
            }
        }

      
        protected override void Draw()
        {
            Clear(this);
            Console.ForegroundColor = ConsoleColor.Blue;
            
            //instance for Database
            DataBase.DataBase db = new DataBase.DataBase();

            // instance of the "ListPage" class, specialized to hold elements of type "CompanyDisplay"
            ListPage<CompanyDisplay> listPage = new ListPage<CompanyDisplay>();
          
            // Create a new CompanyDisplay object with the given parameters
            CompanyDisplay displayCopmany = new CompanyDisplay("CompanyName", "Country", Company.currency.DKK, "ZipCode", "City", "RoadName", "StreetNumber");

            // Retrieve a list of all companies from the database
            List<Company> companies = db.GetAllCompany();

            if(companies == null) { Console.Clear(); Console.WriteLine("No companies found!"); return; }

            // Create a new CompanyDisplay object for each company and add it to the listPage
            foreach (var Company in companies)
            {
                listPage.Add(new CompanyDisplay(Company.CompanyName, Company.Address.Country, Company.Currency,Company.CompanyID.ToString()));
            }

            // Add columns to the listPage object for displaying company data
            listPage.AddColumn("CompanyName", "CompanyName", 20);
            listPage.AddColumn("Country", "Country", 20);
            listPage.AddColumn("Currency", "Currency", 10);

            // delete function to remove a company from the database
            Action<CompanyDisplay> Deletefunction = delegate (CompanyDisplay company)
            {   
                // Convert the company's TempID to an integer ID
                int id = Convert.ToInt32(company.TempID);
                db.DeleteCompany(id);   // Delete the company from the database using the ID
                // Create a new DisplayCompany object and display it
                DisplayCompany display = new DisplayCompany();
                Screen.Display(display); 
            };
            // Add a key to the listPage object that calls the Deletefunction method when the F5 key is pressed
            listPage.AddKey(ConsoleKey.F5, Deletefunction);
     
         
            Action<CompanyDisplay> GoBackFunction = delegate (CompanyDisplay display)
            {
                MenuDisplay menuDisplay = new MenuDisplay();
                Screen.Display(menuDisplay);
            };
            // Add a key to the listPage object that calls the GoBackFunction delegate when the Q key is pressed
            listPage.AddKey(ConsoleKey.Q, GoBackFunction);
            // Create a new Form object for CompanyDisplay class
            Form<CompanyDisplay> form = new Form<CompanyDisplay>();
            try
            {
                // Use the listPage object to select a row of data
                var SelectedRow = listPage.Select();
                // If a CompanyDisplay object is selected, create a new ListPage object and add a column for CompanyName
                if (SelectedRow != null && SelectedRow.CompanyName != null)
                {
                    Clear();
                    Console.Clear();

                    ListPage<CompanyDisplay> SelectedCompanyDisplay = new ListPage<CompanyDisplay>();
                    SelectedCompanyDisplay.AddColumn("CompanyName", "CompanyName");
                    SelectedCompanyDisplay.AddColumn("RoadName", "RoadName");
                    SelectedCompanyDisplay.AddColumn("StreetNumber", "StreetNumber");
                    SelectedCompanyDisplay.AddColumn("ZipCode", "ZipCode");
                    SelectedCompanyDisplay.AddColumn("City", "City");
                    SelectedCompanyDisplay.AddColumn("Country", "Country");
                    SelectedCompanyDisplay.AddColumn("Currency", "Currency");
                    Company SelectedCompany = companies.Select(x => x).Where(x => x.CompanyName == SelectedRow.CompanyName).FirstOrDefault();
                    SelectedCompanyDisplay.Add(new CompanyDisplay(SelectedCompany.CompanyName, SelectedCompany.Address.RoadName, SelectedCompany.Currency, SelectedCompany.Address.ZipCode, SelectedCompany.Address.City, SelectedCompany.Address.Country, SelectedCompany.Address.StreetNumber));

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Click 'F1' to edit company.");
                    Console.WriteLine("Click 'F2' to add a new company.");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Click 'Enter' to save.");
                    Console.WriteLine("Click 'Escape' to cancel.");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    SelectedCompanyDisplay.Draw();
                  

                    ConsoleKeyInfo keyInfo = Console.ReadKey();
                       
                    //F1 to edit
                    if (keyInfo.Key == ConsoleKey.F1)
                    {
                                       
                        Form<CompanyDisplay> editor = new Form<CompanyDisplay>();
                        editor.TextBox("CompanyName", "CompanyName");
                        editor.TextBox("RoadName", "RoadName");
                        editor.TextBox("StreetNumber", "StreetNumber");
                        editor.TextBox("ZipCode", "ZipCode");
                        editor.TextBox("City", "City");
                        editor.TextBox("Currency", "TempCurrency");

                        try
                        {
                            Console.SetCursorPosition(42, 10);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("-----Edit Company details-----");
                            Console.SetCursorPosition(38, 12);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            editor.Edit(displayCopmany);
                        

                        }
                        catch (Exception ex) { }


                        while (true)
                        {
                            ConsoleKeyInfo saveKeyInfo = Console.ReadKey();
                            if (saveKeyInfo.Key == ConsoleKey.Enter)
                            {
                                Company newCompany = new Company();
                                Address newAddress = new Address();

                                newAddress.StreetNumber = displayCopmany.StreetNumber;
                                newAddress.City = displayCopmany.City;
                                newAddress.Country = displayCopmany.Country;
                                newAddress.ZipCode = displayCopmany.ZipCode;
                                newAddress.RoadName = displayCopmany.RoadName;

                                newCompany.CompanyName = displayCopmany.CompanyName;

                                newCompany.Currency = (Company.currency)Enum.Parse(typeof(Company.currency), displayCopmany.TempCurrency);


                                newCompany.Address = newAddress;
                                newCompany.CompanyID = SelectedCompany.CompanyID;
                                newAddress.AdressID = SelectedCompany.Address.AdressID;

                                db.UpdateCompany(newCompany);

                                Console.SetCursorPosition(45, 21);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Company edit saved.");
                                Console.ResetColor();
                                break;

                            }
                            else if (saveKeyInfo.Key == ConsoleKey.Escape)
                            {
                                Console.SetCursorPosition(45, 21);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("cCompany edit cancelled.");
                                Console.ResetColor();                               
                                break;

                            }
                        }
                    }

                    ConsoleKeyInfo keyInfo2 = Console.ReadKey();
                 
                    //F2 to add a new company
                    if (keyInfo2.Key == ConsoleKey.F2)
                    {
                                                                    
                        Form<CompanyDisplay> editor = new Form<CompanyDisplay>();
                        editor.TextBox("CompanyName", "CompanyName");
                        editor.TextBox("RoadName", "RoadName");
                        editor.TextBox("StreetNumber", "StreetNumber");
                        editor.TextBox("ZipCode", "ZipCode");
                        editor.TextBox("City", "City");
                        editor.TextBox("Currency", "TempCurrency");

                        try
                        {
                            Console.SetCursorPosition(42,10);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("-----Add a new Company-----");
                            Console.SetCursorPosition(38, 12);
                            Console.ForegroundColor = ConsoleColor.Blue;
                            editor.Edit(displayCopmany);
                        }
                        catch (Exception ex) { }


                        while (true)
                        {
                            ConsoleKeyInfo saveKeyInfo = Console.ReadKey();
                            //Enter to save
                            if (saveKeyInfo.Key == ConsoleKey.Enter)
                            {
                                Company newCompany = new Company();
                                Address newAddress = new Address();

                                newAddress.StreetNumber = displayCopmany.StreetNumber;
                                newAddress.City = displayCopmany.City;
                                newAddress.Country = displayCopmany.Country;
                                newAddress.ZipCode = displayCopmany.ZipCode;
                                newAddress.RoadName = displayCopmany.RoadName;

                                newCompany.CompanyName = displayCopmany.CompanyName;
                                newCompany.Currency = (Company.currency)Enum.Parse(typeof(Company.currency), displayCopmany.TempCurrency);

                                newCompany.Address = newAddress;                              
                                db.InputCompany(newCompany);

                                Console.SetCursorPosition(45, 20);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("New company setup saved.");
                                Console.ResetColor();
                                break;

                            }
                            else if (saveKeyInfo.Key == ConsoleKey.Escape)
                            {
                                
                                Console.SetCursorPosition(45, 20);
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("nNew company setup cancelled.");
                                Console.ResetColor();
                                break;

                            }
                        }
                    }
                }
            }
            catch { }
        }

    }
}
