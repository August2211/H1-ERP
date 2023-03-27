using H1_ERP.DomainModel;
using Org.BouncyCastle.Utilities.Collections;
using System;
using System.Collections.Generic;
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
            int uniqueNumber = 0;
            int condition = 0;
            for (int i = 0; i < amountToAdd; i++)
            {
                Exec_SQL_Command($"INSERT INTO [dbo].[Customer.Adress] (RoadName, StreetNumber, ZipCode, City, Country) VALUES('Nej Vej{uniqueNumber}', '123{uniqueNumber}', '321{uniqueNumber}', 'NejCity{uniqueNumber}', 'NejCountry{uniqueNumber}')");
                var AddressID = GetData("SELECT TOP(1) AdressID FROM [dbo].[Customer.Adress] ORDER BY AdressID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Customers.Person] (FirstName, LastName, Email, PhoneNumber, AdressID) VALUES ('John{uniqueNumber}', 'Doe{uniqueNumber}', 'johndoe@example.com{uniqueNumber}', '+123456789{uniqueNumber}', {AddressID})");
                var PersonID = GetData("SELECT TOP(1) PersonID FROM [dbo].[Customers.Person] ORDER BY PersonID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Customer.Customers] (LastPurchaseDate, PersonID) VALUES ('2023-03-23 16:03:00', {PersonID})");
                var CustomerID = GetData("SELECT TOP(1) CustomerID FROM [dbo].[Customer.Customers] ORDER BY CustomerID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Sales.Orders] (CustomerID, TotalPriceOfOrder, DateOfOrder, ExpectedDeliveryDate, Comments, Condition) VALUES ({CustomerID}, 5{uniqueNumber}.99, '2023-03-23 16:03:00', '2023-03-30 16:03:00', 'Please deliver to the front door{uniqueNumber}.', {condition})");
                var OrderID = GetData("SELECT TOP(1) OrderID FROM [dbo].[Sales.Orders] ORDER BY OrderID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Product] (ProductName, ProductDescription, ProductSalePrice, ProductPurchasePrice, ProductLocation, ProductQuantity, ProductUnit) VALUES ('Product{uniqueNumber}', 'Description{uniqueNumber}', 2{uniqueNumber}, 3{uniqueNumber}, 'Location{uniqueNumber}', 5{uniqueNumber}, {uniqueNumber})");
                var ProductID = GetData($"SELECT TOP(1) ProductID FROM [dbo].[Product] ORDER BY ProductID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Sales.OrderLines] (ProductID, SinglePrice, OrderQuantity, TotalQuantityPrice, OrderID) VALUES ({ProductID}, 1{uniqueNumber}.99, '5{uniqueNumber}', 5{uniqueNumber}.95, {OrderID})");
                Exec_SQL_Command($"INSERT INTO [dbo].[Company] (CompanyName, Street, HouseNumber, zipCode, City, Country, Currency) VALUES ('CompanyName{uniqueNumber}', 'Street{uniqueNumber}', {uniqueNumber}, 'zipCode{uniqueNumber}', 'City{uniqueNumber}', 'Country{uniqueNumber}', 'Currency{uniqueNumber}')");
                uniqueNumber++;
                condition++;
                if (condition > 4)
                {
                    condition = 0;
                }
            }
        }

        public void DeleteAllData()
        {
            Exec_SQL_Command("DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines]");
            Exec_SQL_Command("DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders]");
            Exec_SQL_Command("DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Customers]");
            Exec_SQL_Command("DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customers.Person]");
            Exec_SQL_Command("DELETE FROM [H1PD021123_Gruppe4].[dbo].[Customer.Adress]");
            Exec_SQL_Command("DELETE FROM [H1PD021123_Gruppe4].[dbo].[Company]");
            Exec_SQL_Command("DELETE FROM [H1PD021123_Gruppe4].[dbo].[Product]");
        }

        public void RefreshData(int amountToAdd)
        {
            DeleteAllData();
            BulkAddData(amountToAdd);
        }
    }
}