using H1_ERP.DomainModel;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DataBase
{
    public partial class DataBase
    {
        public void BulkAddData(int amountToAdd)
        {
            int Currency = 0;
            int uniqueNumber = 0;
            int condition = 0;

            string[] SalesNames = new string[10];
            SalesNames[0] = "Torben";
            SalesNames[1] = "Kurt";
            SalesNames[2] = "Søren";
            SalesNames[3] = "Morten";
            SalesNames[4] = "Benny";
            SalesNames[5] = "Lars";
            SalesNames[6] = "Peter";
            SalesNames[7] = "Jens";
            SalesNames[8] = "Edvart";
            SalesNames[9] = "Lasse";

            string[] SalesSurNames = new string[10];
            SalesSurNames[0] = "Jensen";
            SalesSurNames[1] = "Kristensen";
            SalesSurNames[2] = "Hansen";
            SalesSurNames[3] = "Nielsen";
            SalesSurNames[4] = "Petersen";
            SalesSurNames[5] = "Andersen";
            SalesSurNames[6] = "Madsen";
            SalesSurNames[7] = "Larsen";
            SalesSurNames[8] = "Jørgensen";
            SalesSurNames[9] = "Pedersen";

            for (int i = 0; i < amountToAdd; i++)
            {
                Exec_SQL_Command($"INSERT INTO [dbo].[Customer.Adress] (RoadName, StreetNumber, ZipCode, City, Country) VALUES('Nej Vej{uniqueNumber}', '123{uniqueNumber}', '321{uniqueNumber}', 'NejCity{uniqueNumber}', 'NejCountry{uniqueNumber}')");
                var AddressID = GetData("SELECT TOP(1) AdressID FROM [dbo].[Customer.Adress] ORDER BY AdressID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Customers.Person] (FirstName, LastName, Email, PhoneNumber, AdressID) VALUES ('John{uniqueNumber}', 'Doe{uniqueNumber}', 'johndoe@gmail.com{uniqueNumber}', '+123456789{uniqueNumber}', {AddressID})" +
                $"INSERT INTO [dbo].[Company] (CompanyName, Currency, AdressID) VALUES ('CompanyName{uniqueNumber}', {Currency}, {AddressID})");
                var PersonID = GetData("SELECT TOP(1) PersonID FROM [dbo].[Customers.Person] ORDER BY PersonID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Customer.Customers] (LastPurchaseDate, PersonID) VALUES ('2023-03-23 16:03:00', {PersonID})" +
                $"INSERT INTO [dbo].[Customers.Person] (FirstName, LastName, Email, PhoneNumber, AdressID) VALUES ('{SalesNames[uniqueNumber]}', '{SalesSurNames[uniqueNumber]}', '{SalesNames[uniqueNumber]}{SalesSurNames[uniqueNumber]}@WANK.com{uniqueNumber}', '+123456789{uniqueNumber}', {AddressID})");
                var EmployeePersonID = GetData("SELECT TOP(1) PersonID FROM [dbo].[Customers.Person] ORDER BY PersonID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Company.Employees] VALUES (GETDATE(), GETDATE()+3000, {uniqueNumber * 1000}, {EmployeePersonID})");
                var CustomerID = GetData("SELECT TOP(1) CustomerID FROM [dbo].[Customer.Customers] ORDER BY CustomerID desc").Values.ElementAt(0)[0];
                var EmployeeID = GetData("SELECT TOP(1) Id FROM [dbo].[Company.Employees] ORDER BY Id desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Sales.Orders] (CustomerID, TotalPriceOfOrder, DateOfOrder, ExpectedDeliveryDate, Comments, Condition, SalesPerson) VALUES ({CustomerID}, 5{uniqueNumber}.99, '2023-03-23 16:03:00', '2023-03-30 16:03:00', 'Please deliver to the front door{uniqueNumber}.', {condition}, '{EmployeeID}')");
                var OrderID = GetData("SELECT TOP(1) OrderID FROM [dbo].[Sales.Orders] ORDER BY OrderID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Product] (ProductName, ProductDescription, ProductSalePrice, ProductPurchasePrice, ProductLocation, ProductQuantity, ProductUnit) VALUES ('Product{uniqueNumber}', 'Description{uniqueNumber}', 2{uniqueNumber}, 3{uniqueNumber}, 'lll{uniqueNumber}', 5{uniqueNumber}, {uniqueNumber})");
                var ProductID = GetData($"SELECT TOP(1) ProductID FROM [dbo].[Product] ORDER BY ProductID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Sales.OrderLines] (ProductID, SinglePrice, OrderQuantity, TotalQuantityPrice, OrderID) VALUES ({ProductID}, 1{uniqueNumber}.99, '5{uniqueNumber}', 5{uniqueNumber}.95, {OrderID})" +
                $"INSERT INTO [dbo].[Sales.OrderLines] (ProductID, SinglePrice, OrderQuantity, TotalQuantityPrice, OrderID) VALUES ({ProductID}, 1{uniqueNumber}.99, '5{uniqueNumber}', 5{uniqueNumber}.95, {OrderID})" +
                $"INSERT INTO [dbo].[Sales.OrderLines] (ProductID, SinglePrice, OrderQuantity, TotalQuantityPrice, OrderID) VALUES ({ProductID}, 1{uniqueNumber}.99, '5{uniqueNumber}', 5{uniqueNumber}.95, {OrderID})");
                uniqueNumber++;
                condition++;
                if (condition > 4)
                {
                    condition = 0;
                }
                else if (Currency > 4)
                {
                    Currency = 0;
                }
                if (uniqueNumber > 9)
                {
                    uniqueNumber = 0;
                }
            }
        }

        public void DeleteAllData()
        {
            Exec_SQL_Command("DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines];" +
                "DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders];" +
                "DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Customers];" +
                "DELETE FROM [H1PD021123_Gruppe4].[dbo].[Company.Employees];" +
                "DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person];" +
                "DELETE FROM [H1PD021123_Gruppe4].[dbo].[Company];" +
                "DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress];" +
                "DELETE FROM [H1PD021123_Gruppe4].[dbo].[Product];");
        }

        public void RefreshData(int amountToAdd)
        {
            DeleteAllData();
            BulkAddData(amountToAdd);
        }
    }
}