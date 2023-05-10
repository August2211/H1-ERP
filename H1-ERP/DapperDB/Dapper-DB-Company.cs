﻿using Dapper;
using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TECHCOOL.UI;
using TECHCOOL;

namespace H1_ERP.DapperDB
{
    public partial class DatabaseDapper
    {
        public Company GetCompanyFromID(int ID)
        {
            Company? Company = null;
            using (var conn = getConnection())
            {
                    conn.Query<Company, Address, Company>($"SELECT * FROM Company INNER JOIN [Customer.Adress] ON  " +
                    $"[Customer.Adress].AdressID = [Company].AdressID " +
                    $"WHERE [Company].CompanyID = {ID}", (company, address) =>
                    {
                        Company = company;
                        Company.Address = address;
                        return null;
                    }, splitOn: "CompanyID,AdressID");
                return Company;
            }
        }
        public List<Company> GetAllCompanies()
        {
            List<Company> Companies = new List<Company>();
            using (var conn = getConnection())
            {
                conn.Query<Company, Address, Company>("SELECT * FROM Company INNER JOIN[Customer.Adress] ON " +
                    "[Customer.Adress].AdressID = [Company].AdressID", (company, address) =>
                    {
                        company.Address = address;
                        Companies.Add(company);
                        return null;
                    }, splitOn: "CompanyID,AdressID");
            }
            return Companies;
        }
        public void DeleteCompany(int ID)
        {
            using (var conn = getConnection())
            {
                conn.Query($"DELETE FROM Company WHERE CompanyID = {ID}");
            }
        }
    }
}
