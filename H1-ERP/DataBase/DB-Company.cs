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
    
            Company company = new Company();
            company.CompanyID = id;
            Address address = new Address();
            SqlConnection conn = getConnection();
            var SelectedCompany = GetData($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {id}", conn);
             company.CompanyID = Convert.ToInt32(SelectedCompany.ElementAt(0).Value[2]);
             company.Currency = (Company.currency)Convert.ToInt32(SelectedCompany.ElementAt(2).Value[2]);
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {company.CompanyID}";
            var SelectedCompanies = GetData(sql, conn);
            foreach ( var row in SelectedCompanies.Values)
                {
                    company.CompanyID = Convert.ToInt32(SelectedCompany.ElementAt(0).Value[0]);
                    company.CompanyName = row[1].ToString();                 
                    company.Currency = (Company.currency)Convert.ToInt32(SelectedCompany.ElementAt(2).Value[2]);             
           
            }
            var addressID = GetData($"SELECT AdressID FROM[H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {company.CompanyID}", conn);
            var TheAdress = GetData($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress] WHERE AdressID = {addressID.ElementAt(0).Value[0]}", conn);
          foreach( var adress in TheAdress.Values) 
            {
                address.AdressID = Convert.ToUInt32(adress.ElementAt(0));
                address.RoadName = adress.ElementAt(1).ToString();
                address.StreetNumber = adress.ElementAt(2).ToString();
                address.ZipCode = adress.ElementAt(3).ToString();
                address.City = adress.ElementAt(4).ToString();
                address.Country = adress.ElementAt(5).ToString();

            }
            if (address.AdressID != 0)
            {
                company.Address = address;
            }
            conn.Close();
            return company;
        }
        //Get all Company and return it to the list 
        public List<Company> GetAllCompany()
        {
            SqlConnection connection = getConnection();
            List<Company> result = new List<Company>();
            var AllCompanies = GetData("SELECT * FROM [dbo].[Company] INNER JOIN [dbo].[Customer.Adress] ON [dbo].[Company].AdressID = [dbo].[Customer.Adress].AdressID");
               
                foreach(var row in AllCompanies.Values) 
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
                connection.Close();
            return result;

        }  //InsertSalesOrderHeader the Company and put it in dataBase
        public void InputCompany(Company Input)
        {
            SqlConnection connection = getConnection();
            string sql2 = $"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Customer.Adress] (RoadName,StreetNumber,ZipCode,City,Country) VALUES ('{Input.Address.RoadName}','{Input.Address.StreetNumber}', '{Input.Address.ZipCode}','{Input.Address.City}', '{Input.Address.Country}')";
            SqlCommand sqlCommand2 = new SqlCommand(sql2, connection);
            sqlCommand2.ExecuteNonQuery();

            var AddressID = GetData("SELECT TOP(1) AdressID FROM [dbo].[Customer.Adress] ORDER BY AdressID desc").Values.ElementAt(0)[0];

            SqlConnection connection2 = getConnection();
            string sql = $"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Company](CompanyName,Currency,AdressID) VALUES ('{Input.CompanyName}','{(int)Input.Currency}','{AddressID}')";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        //Updata the Company in dataBase
        public void UpdateCompany(Company Input)
        {
            SqlConnection connection = getConnection();
           string sql2 = $"UPDATE [H1PD021123_Gruppe4].[dbo].[Customer.Adress] set RoadName = '{Input.Address.RoadName}',  StreetNumber='{Input.Address.StreetNumber}', ZipCode='{Input.Address.ZipCode}', City= '{Input.Address.City}', Country='{Input.Address.Country}' WHERE AdressID='{Input.Address.AdressID}'";
            SqlCommand sqlCommand2 = new SqlCommand(sql2, connection);
            sqlCommand2.ExecuteNonQuery();

            string sql = $"UPDATE [H1PD021123_Gruppe4].[dbo].[Company] set CompanyName = '{Input.CompanyName}',  Currency='{((int)Input.Currency)}', AdressID='{Input.Address.AdressID}'WHERE CompanyID='{Input.CompanyID}'";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        //DeleteSalesOrderHeaderFromID the Company from dataBase

        public void DeleteCompany(int Input)
        {
            SqlConnection connection = getConnection();
            string sql = $"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {Input}";

            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
