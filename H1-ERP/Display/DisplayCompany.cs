using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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


        public class CompanyDisplay
        {
            public string Title1 { get; set; }
            public string Title2 { get; set; }
            public Company.currency Title3 { get; set; }
            public string Title4 { get; set; }
            public string Title5 { get; set; }
            public string Title6 { get; set; }
            public string Title7 { get; set; }


            public CompanyDisplay(string CompanyName, string Country, Company.currency currency)
            {
                Title1 = CompanyName;
                Title2 = Country;
                Title3 = currency;
            }


            public CompanyDisplay(string CompanyName, string Street, Company.currency currency, string ZipCode, string City, string Country, string HouseNumber)
            {
                Title1 = CompanyName;
                Title2 = Street;
                Title3 = currency;
                Title4 = ZipCode;
                Title5 = City;
                Title6 = Country;
                Title7 = HouseNumber;
            }
        }

        protected override void Draw()
        {
            Clear(this);//Clean the screen Gonna draw a list page here
            DataBase.DataBase db = new DataBase.DataBase();

            ListPage<CompanyDisplay> listPage = new ListPage<CompanyDisplay>();

         


            List<Company> companies = db.GetAllCompany();
            foreach (var Company in companies)
            {
                listPage.Add(new CompanyDisplay(Company.CompanyName, Company.Country, Company.Currency));
            }
            listPage.AddColumn("CompanyName", "Title1", 20);
            listPage.AddColumn("Country", "Title2", 10);
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
                SelectedCompanyDisplay.Add(new CompanyDisplay(SelectedCompany.CompanyName, SelectedCompany.Street, SelectedCompany.Currency, SelectedCompany.ZipCode, SelectedCompany.City, SelectedCompany.Country, SelectedCompany.HouseNumber));

                SelectedCompanyDisplay.Draw();
                if (SelectedRow.Title1 == null)
                {
                    listPage.Draw();

                }

            }
        }
        public class EditCompanyDisplay : Screen
        {
            public override string Title { get; set; } = "Edit Company";
            protected override void Draw()
            {
                Clear(this);
               Company company = new Company("new Company");
               
                Form<Company> editor = new Form<Company>();
                    
                editor.TextBox("Company", "Title");
                editor.IntBox("CompanyName", "Priority");
                editor.IntBox("Street", "Priority");
                editor.IntBox("HouseNumber", "Priority");
                editor.IntBox("ZipCode", "Priority");
                editor.IntBox("City", "Priority");
                editor.IntBox("Country", "Priority");
                editor.SelectBox("Currency", "Priority");

                editor.Edit(company);
                Clear(this);


            }
        }
    }
}
