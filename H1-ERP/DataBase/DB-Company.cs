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
        Company company = new Company();

        public Company GetCompanyFromID(int id)
        {
            SqlConnection conn = getConnection();
            conn.Open();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {id}";

            SqlCommand commando = new SqlCommand(sql, conn);
            SqlDataReader sqlDataReader = commando.ExecuteReader();
            int rows = 0;
            while (sqlDataReader.Read())
            {
                if (rows < sqlDataReader.FieldCount)
                {

                    sqlDataReader.GetValue(rows);
                    var prod = sqlDataReader;

                    company.CompanyID = (int)prod[0];
                    company.CompanyName = (string)prod[1];
                    company.Street = (string)prod[2];
                    company.HouseNumber = (string)prod[3];
                    company.ZipCode = (string)prod[4];
                    company.City = (string)prod[5];
                    company.Country = (string)prod[6];
                    company.Currency = (string)prod[7];

                    rows++;
                }
            }
            return company;
        }
        //Get all Company and return it to the list 
        public List<Company> GetAllCompany()
        {
            SqlConnection connection = getConnection();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Company]";
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataReader sqlDataReader = command.ExecuteReader();
            List<Company> result = new List<Company>();

           Company company = new Company();

            while (sqlDataReader.Read())
            {
                company.CompanyName=sqlDataReader.GetString(0);
                company.Street=sqlDataReader.GetString(1);
                company.HouseNumber=sqlDataReader.GetString(2);
                company.ZipCode=sqlDataReader.GetString(3);
                company.City=sqlDataReader.GetString(4);
                company.Country=sqlDataReader.GetString(5);
                company.Currency=sqlDataReader.GetString(6);
                result.Add(company);
            }

            return result;
        }  //InsertSalesOrderHeader the Company and put it in dataBase
        public void InputCompany(Company Input)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Company](CompanyName,Street,HouseNumber,ZipCode,City,Country,Currency) VALUES ('{Input.CompanyName}','{Input.Street}','{Input.HouseNumber}','{Input.ZipCode}','{Input.City}','{Input.Country}','{Input.Currency}')";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        //Updata the Company in dataBase
        public void UpdataCompany(Company Input)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"UPDATA [H1PD021123_Gruppe4].[dbo].[Company] set CompanyName = '{Input.CompanyName}', Street = '{Input.Street}',HouseNumber = '{Input.HouseNumber}',ZipCode = '{Input.ZipCode}', City ='{Input.City}', Country'{Input.Country}', Currency='{Input.Currency}')";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
        //DeleteSalesOrderHeaderFromID the Company from dataBase

        public void DeleteCompany(Company Input)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Company] WHERE CompanyID = {Input.CompanyID}";

            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            connection.Close();
        }
    }
}
