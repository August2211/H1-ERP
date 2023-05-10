using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {

        public Company GetCompanyFromID(int id)
        {

            //we intizialise the individual objects needed for the Company object
            Company company = new Company();
            company.CompanyID = id;
            Address address = new Address();

            // we get data from 2 table's using a inner join  
            var SelectedCompany = GetData($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Company] INNER JOIN [Customer.Adress] ON " +
                $"[Customer.Adress].AdressID = Company.AdressID " +
                $"WHERE CompanyID = {id}");
            // we assign each value to our obejct 
            company.CompanyID = Convert.ToInt32(SelectedCompany.ElementAt(0).Value[0]);
            company.CompanyName = SelectedCompany.ElementAt(0).Value[1].ToString();
            company.Currency = (Company.currency)Convert.ToInt32(SelectedCompany.ElementAt(0).Value[2]);


            address.AdressID = Convert.ToUInt32(SelectedCompany.ElementAt(0).Value[3]);
            address.RoadName = SelectedCompany.ElementAt(0).Value[5].ToString();
            address.StreetNumber = SelectedCompany.ElementAt(0).Value[6].ToString();
            address.ZipCode = SelectedCompany.ElementAt(0).Value[7].ToString();
            address.City = SelectedCompany.ElementAt(0).Value[8].ToString();
            address.Country = SelectedCompany.ElementAt(0).Value[9].ToString();

            //we make a check to ensure the company has an adress
            if (address.AdressID != 0)
            {
                company.Address = address;
            }
            return company;
        }
        //Get all Company and return it to the list 
        public List<Company> GetAllCompany()
        {
            List<Company> result = new List<Company>();
            //Get the data from the 2 table's and join them together 
            var AllCompanies = GetData("SELECT * FROM [dbo].[Company] INNER JOIN [dbo].[Customer.Adress] ON [dbo].[Company].AdressID = [dbo].[Customer.Adress].AdressID");


            // after the data is joined we take the indvidual value's and set them for our indvidual objects
            foreach (var row in AllCompanies.Values)
            {
                Company companies = new Company();
                companies.CompanyID = Convert.ToInt32(row[0]);
                companies.CompanyName = row[1].ToString();
                companies.Currency = (Company.currency)Convert.ToInt32(row[2]);
                Address address = new Address();
                address.AdressID = Convert.ToUInt32(row[4]);
                address.RoadName = row[5].ToString();
                address.StreetNumber = row[6].ToString();
                address.ZipCode = row[7].ToString();
                address.City = row[8].ToString();
                address.Country = row[9].ToString();

                companies.Address = address;
                result.Add(companies);
            }
            // we return the given company in the database 
            return result;

        }  //InsertSalesOrderHeader the Company and put it in dataBase
        public void InputCompany(Company Input)
        {
            // We Insert the data for a new adress in the table and afterwards we take select the ID for the given adress and insert in to the company table 
            var AddressID = GetData($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customer.Adress] (RoadName,StreetNumber,ZipCode,City,Country) VALUES ('{Input.Address.RoadName}','{Input.Address.StreetNumber}', '{Input.Address.ZipCode}','{Input.Address.City}', '{Input.Address.Country}') " +
                $"SELECT TOP(1) AdressID FROM [dbo].[Customer.Adress] ORDER BY AdressID desc").Values.ElementAt(0)[0];

           Exec_SQL_Command($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Company](CompanyName,Currency,AdressID) VALUES ('{Input.CompanyName}','{(int)Input.Currency}','{AddressID}')");
        }
        //Updata the Company in dataBase
        public void UpdateCompany(Company Input)
        {
            // we take the company obejct and update all the values in the database 
            Exec_SQL_Command($"UPDATE [H1PD021123_Gruppe4].[dbo].[Customer.Adress] set RoadName = '{Input.Address.RoadName}',  StreetNumber='{Input.Address.StreetNumber}'," +
                             $" ZipCode='{Input.Address.ZipCode}', City= '{Input.Address.City}', Country='{Input.Address.Country}' WHERE AdressID='{Input.Address.AdressID}' " +
                            $"UPDATE [H1PD021123_Gruppe4].[dbo].[Company] set CompanyName = '{Input.CompanyName}',  Currency='{((int)Input.Currency)}', 0dressID='{Input.Address.AdressID}'WHERE CompanyID='{Input.CompanyID}'"); 
        }
        //DeleteSalesOrderHeaderFromID the Company from dataBase

        public void DeleteCompany(int Input)
        {
           Company company = new Company();
            // Firstly we get the adressID from the database 
            company.Address.AdressID = (uint)GetData($"SELECT AdressID FROM [H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {Input}").ElementAt(0).Value[0];
            //delete in the order beneath to avoid errors
            Exec_SQL_Command($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {Input}");
            Exec_SQL_Command($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] WHERE AdressID = {company.Address.AdressID}");

        }
    }
}
