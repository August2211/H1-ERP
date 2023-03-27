using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;

namespace H1_ERP.Display
{

    public class DisplayCompany : Screen
    {


        public override string Title { get; set; } = "Company";


        private class CompanyDisplay
        {
            public string Title1 { get; set; }
            public string Title2 { get; set; }
            public string Title3 { get; set; }
            public string Title4 { get; set; }
            public string Title5 { get; set; }
            public string Title6 { get; set; }
            public string Title7 { get; set; }


            public CompanyDisplay(string CompanyName, string Country, string Currency)
            {
                Title1 = CompanyName;
                Title2 = Country;
                Title3 = Currency;
            }


            public CompanyDisplay(string CompanyName, string Street, string HouseNumber, string ZipCode, string City, string Country, string Currency)
            {
                Title1 = CompanyName;
                Title2 = Street;
                Title3 = HouseNumber;
                Title4 = ZipCode;
                Title5 = City;
                Title6 = Country;
                Title7 = Currency;
            }
        }

        protected override void Draw()
        {
            Clear(this);//Clean the screen Gonna draw a list page here
            DataBase.DataBase db = new DataBase.DataBase();

            ListPage<CompanyDisplay> listPage = new ListPage<CompanyDisplay>();
        

            List<Company> companies = new List<Company>();
            foreach (var Company in companies)
            {
                listPage.Add(new CompanyDisplay(Company.CompanyName, Company.Country, Company.Currency));
            }
            listPage.AddColumn("CompanyName", "Title1", 20);
            listPage.AddColumn("City", "Title2", 10);
            listPage.AddColumn("Currency", "Title3", 10);


            Form<CompanyDisplay> form = new Form<CompanyDisplay>();
            var SelectedRow = listPage.Select();
            if (SelectedRow.Title1 != null)
            {
                Clear(this);
                Console.Clear();
                ListPage<CompanyDisplay> SelectedCompanyDisplay = new ListPage<CompanyDisplay>();
                SelectedCompanyDisplay.AddColumn("CompanyName", "Title1");
                SelectedCompanyDisplay.AddColumn("Street", "Title2");
                SelectedCompanyDisplay.AddColumn("HouseNumber", "Title3");
                SelectedCompanyDisplay.AddColumn("ZipCode", "Title4");
                SelectedCompanyDisplay.AddColumn("City", "Title5");
                SelectedCompanyDisplay.AddColumn("Country", "Title6");
                SelectedCompanyDisplay.AddColumn("Currency", "Title7");

                Company SelectedCompany = companies.Select(x => x).Where(x => x.CompanyName == SelectedRow.Title1).FirstOrDefault();
                SelectedCompanyDisplay.Add(new CompanyDisplay(SelectedCompany.CompanyName, SelectedCompany.Street, SelectedCompany.HouseNumber, SelectedCompany.ZipCode, SelectedCompany.City, SelectedCompany.Country, SelectedCompany.Currency));

                if (SelectedRow.Title1 == null)
                {
                    listPage.Draw();

                }








            }
        }
    }
}
