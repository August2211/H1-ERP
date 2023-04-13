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
            public string CompanyName { get; set; }
            public string Country { get; set; }
            public Company.currency Currency { get; set; }
            public string ZipCode { get; set; }
            public string City { get; set; }
            public string RoadName { get; set; }
            public string StreetNumber { get; set; }
            public string TempCurrency { get; set; }


            public CompanyDisplay(string companyName, string country, Company.currency currency)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                CompanyName = companyName;
                Country = country;
                Currency = currency;

            }


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

        public void newLine()
        {


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();

        }
        protected override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Clear(this);
            DataBase.DataBase db = new DataBase.DataBase();

            ListPage<CompanyDisplay> listPage = new ListPage<CompanyDisplay>();

            CompanyDisplay displayCopmany = new CompanyDisplay("CompanyName", "Country", Company.currency.DKK, "ZipCode", "City", "RoadName", "StreetNumber");


            List<Company> companies = db.GetAllCompany();
            foreach (var Company in companies)
            {
                listPage.Add(new CompanyDisplay(Company.CompanyName, Company.Address.Country, Company.Currency));
            }
            listPage.AddColumn("CompanyName", "CompanyName", 20);
            listPage.AddColumn("Country", "Country", 20);
            listPage.AddColumn("Currency", "Currency", 10);

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            Form<CompanyDisplay> form = new Form<CompanyDisplay>();
            try
            {
                var SelectedRow = listPage.Select();

                if (SelectedRow.CompanyName != null)
                {
                    Clear(this);
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

                    SelectedCompanyDisplay.Draw();
                    if (SelectedRow.CompanyName == null)
                    {
                        listPage.Draw();


                    }


                    if (keyInfo.Key == ConsoleKey.F1)
                    {
                        newLine();
                        Console.ForegroundColor = ConsoleColor.Blue;

                        Form<CompanyDisplay> editor = new Form<CompanyDisplay>();
                        editor.TextBox("CompanyName", "CompanyName");
                        editor.TextBox("RoadName", "RoadName");
                        editor.TextBox("StreetNumber", "StreetNumber");
                        editor.TextBox("ZipCode", "ZipCode");
                        editor.TextBox("City", "City");
                        editor.TextBox("Currency", "TempCurrency");

                        try
                        {
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

                                newLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("New company setup saved.");
                                Console.ResetColor();
                                break;

                            }
                            else if (saveKeyInfo.Key == ConsoleKey.Escape)
                            {
                                newLine();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("New company setup cancelled.");
                                Console.ResetColor();
                                break;

                            }
                        }
                    }



                    if (keyInfo.Key == ConsoleKey.F2)
                    {
                        newLine();
                        Console.ForegroundColor = ConsoleColor.Blue;

                        Form<CompanyDisplay> editor = new Form<CompanyDisplay>();
                        editor.TextBox("CompanyName", "CompanyName");
                        editor.TextBox("RoadName", "RoadName");
                        editor.TextBox("StreetNumber", "StreetNumber");
                        editor.TextBox("ZipCode", "ZipCode");
                        editor.TextBox("City", "City");
                        editor.TextBox("Currency", "TempCurrency");

                        try
                        {
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
                               

                                db.InputCompany(newCompany);

                                newLine();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("New company setup saved.");
                                Console.ResetColor();
                                break;

                            }
                            else if (saveKeyInfo.Key == ConsoleKey.Escape)
                            {
                                newLine();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("New company setup cancelled.");
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
