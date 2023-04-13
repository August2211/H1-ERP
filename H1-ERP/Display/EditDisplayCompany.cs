using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using static H1_ERP.DomainModel.Company;

namespace H1_ERP.Display
{
    public partial class DisplayCompany 
    {
        //public class EditCompanyDisplay : Screen
        //{

        //    public string Title1 { get; set; } = "";
        //    public string Title2 { get; set; } = "";
        //    public string Title3 { get; set; } = "";
        //    public string Title4 { get; set; } = "";
        //    public string Title5 { get; set; } = "";
        //    public string Title6 { get; set; } = "";
        //    public Company.currency Title7 { get; set; }




        //    public EditCompanyDisplay(string CompanyName, string RoadName, string StreetNumber, string ZipCode, string City, string Country,  Company.currency currency)
        //    {
        //        Title1 = CompanyName;
        //        Title2 = RoadName;
        //        Title3 = StreetNumber;
        //        Title4 = ZipCode;
        //        Title5 = City;
        //        Title6 = Country;
        //        Title7 = currency;
        //    }

        //    public EditCompanyDisplay()
        //    {
        //    }

        //    public override string Title { get; set; } = "Edit Company";
        //    protected override void Draw()
        //    {
        //        Clear(this);
        //        DataBase.DataBase db = new DataBase.DataBase();

        //       EditCompanyDisplay editCompanyDisplay  = new EditCompanyDisplay();

        //        ListPage<EditCompanyDisplay> listPage = new ListPage<EditCompanyDisplay>();
        //        List<Company> companies = db.GetAllCompany();
        //        foreach(var company in  companies)
        //        {
        //            listPage.Add(new EditCompanyDisplay(company.CompanyName, company.Address.RoadName, company.Address.StreetNumber, company.Address.ZipCode,company.Address.City , company.Address.Country, company.Currency));

        //        }
        //        Form<EditCompanyDisplay> editor = new Form<EditCompanyDisplay>();

        //        editor.TextBox("CompanyName", "CompanyName");

        //        editor.Edit(editCompanyDisplay);
                
        //        Clear(this);

        //    }
        //}
    }
}
