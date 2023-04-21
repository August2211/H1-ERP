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
                Exec_SQL_Command($"INSERT INTO [dbo].[Customer.Adress] (RoadName, StreetNumber, ZipCode, City, Country) VALUES('Nej Vej{uniqueNumber}', '123{uniqueNumber}', '321{uniqueNumber}', 'NejCity{uniqueNumber}', 'NejCountry{uniqueNumber}')");
                var CompanyAddressID = GetData("SELECT TOP(1) AdressID FROM [dbo].[Customer.Adress] ORDER BY AdressID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Customer.Adress] (RoadName, StreetNumber, ZipCode, City, Country) VALUES('Nej Vej{uniqueNumber}', '123{uniqueNumber}', '321{uniqueNumber}', 'NejCity{uniqueNumber}', 'NejCountry{uniqueNumber}')");
                var EmployeeAddressID = GetData("SELECT TOP(1) AdressID FROM [dbo].[Customer.Adress] ORDER BY AdressID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Customers.Person] (FirstName, LastName, Email, PhoneNumber, AdressID) VALUES ('John{uniqueNumber}', 'Doe{uniqueNumber}', 'johndoe@gmail.com{uniqueNumber}', '+123456789{uniqueNumber}', {AddressID})" +
                $"INSERT INTO [dbo].[Company] (CompanyName, Currency, AdressID) VALUES ('CompanyName{uniqueNumber}', {Currency}, {CompanyAddressID})");
                var PersonID = GetData("SELECT TOP(1) PersonID FROM [dbo].[Customers.Person] ORDER BY PersonID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Customer.Customers] (LastPurchaseDate, PersonID) VALUES ('2023-03-23 16:03:00', {PersonID})" +
                $"INSERT INTO [dbo].[Customers.Person] (FirstName, LastName, Email, PhoneNumber, AdressID) VALUES ('{SalesNames[uniqueNumber]}', '{SalesSurNames[uniqueNumber]}', '{SalesNames[uniqueNumber]}{SalesSurNames[uniqueNumber]}@WANK.com{uniqueNumber}', '+123456789{uniqueNumber}', {EmployeeAddressID})");
                var EmployeePersonID = GetData("SELECT TOP(1) PersonID FROM [dbo].[Customers.Person] ORDER BY PersonID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Company.Employees] VALUES (GETDATE(), GETDATE()+3000, {uniqueNumber * 1000}, {EmployeePersonID})");
                var CustomerID = GetData("SELECT TOP(1) CustomerID FROM [dbo].[Customer.Customers] ORDER BY CustomerID desc").Values.ElementAt(0)[0];
                var EmployeeID = GetData("SELECT TOP(1) Id FROM [dbo].[Company.Employees] ORDER BY Id desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Sales.Orders] (CustomerID, TotalPriceOfOrder, DateOfOrder, ExpectedDeliveryDate, Comments, Condition, SalesPerson) VALUES ({CustomerID}, 5{uniqueNumber}.99, '2023-03-23 16:03:00', '2023-03-30 16:03:00', 'Please deliver to the front door{uniqueNumber}.', {condition}, '{EmployeeID}')");
                var OrderID = GetData("SELECT TOP(1) OrderID FROM [dbo].[Sales.Orders] ORDER BY OrderID desc").Values.ElementAt(0)[0];
                Exec_SQL_Command($"INSERT INTO [dbo].[Product] (ProductName, ProductDescription, ProductSalePrice, ProductPurchasePrice, ProductLocation, ProductQuantity, ProductUnit) VALUES ('Product{uniqueNumber}', 'ProductDescription{uniqueNumber}', 2{uniqueNumber}, 3{uniqueNumber}, 'lll{uniqueNumber}', 5{uniqueNumber}, {uniqueNumber})");
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

        public void CreateTables()
        {
            Exec_SQL_Command("\r\n\r\n\r\n/****** Object:  Table [dbo].[Customer.Adress]    Script Date: 13-04-2023 09:40:07 ******/\r\nSET ANSI_NULLS ON\r\n\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\n\r\nCREATE TABLE [dbo].[Customer.Adress](\r\n    [AdressID] [int] IDENTITY(1,1) NOT NULL,\r\n    [RoadName] [text] NOT NULL,\r\n    [StreetNumber] [nvarchar](15) NOT NULL,\r\n    [ZipCode] [nchar](15) NOT NULL,\r\n    [City] [nvarchar](85) NOT NULL,\r\n    [Country] [nvarchar](60) NOT NULL,\r\n CONSTRAINT [PK_Customer.Adress] PRIMARY KEY CLUSTERED \r\n(\r\n    [AdressID] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\n\r\n\r\n\r\n\r\n\r\n/****** Object:  Table [dbo].[Customers.Person]    Script Date: 13-04-2023 09:39:32 ******/\r\nSET ANSI_NULLS ON\r\n\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\n\r\nCREATE TABLE [dbo].[Customers.Person](\r\n    [PersonID] [int] IDENTITY(1,1) NOT NULL,\r\n    [FirstName] [text] NOT NULL,\r\n    [LastName] [text] NOT NULL,\r\n    [Email] [text] NOT NULL,\r\n    [PhoneNumber] [nvarchar](15) NOT NULL,\r\n    [AdressID] [int] NOT NULL,\r\n CONSTRAINT [PK_Customers.Person] PRIMARY KEY CLUSTERED \r\n(\r\n    [PersonID] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\n\r\n\r\nALTER TABLE [dbo].[Customers.Person]  WITH CHECK ADD  CONSTRAINT [FK_AdressID_PersonID] FOREIGN KEY([AdressID])\r\nREFERENCES [dbo].[Customer.Adress] ([AdressID])\r\n\r\n\r\nALTER TABLE [dbo].[Customers.Person] CHECK CONSTRAINT [FK_AdressID_PersonID]\r\n\r\n\r\n\r\n\r\n\r\n/****** Object:  Table [dbo].[Company]    Script Date: 13-04-2023 09:40:22 ******/\r\nSET ANSI_NULLS ON\r\n\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\n\r\nCREATE TABLE [dbo].[Company](\r\n    [CompanyID] [int] IDENTITY(1,1) NOT NULL,\r\n    [CompanyName] [nvarchar](50) NOT NULL,\r\n    [Currency] [int] NOT NULL,\r\n    [AdressID] [int] NOT NULL,\r\n CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED \r\n(\r\n    [CompanyID] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n\r\nALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_dboCompany_ParentTable] FOREIGN KEY([AdressID])\r\nREFERENCES [dbo].[Customer.Adress] ([AdressID])\r\n\r\n\r\nALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_dboCompany_ParentTable]\r\n\r\n\r\n\r\n\r\n\r\n/****** Object:  Table [dbo].[Customer.Customers]    Script Date: 13-04-2023 09:39:47 ******/\r\nSET ANSI_NULLS ON\r\n\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\n\r\nCREATE TABLE [dbo].[Customer.Customers](\r\n    [CustomerID] [int] IDENTITY(1,1) NOT NULL,\r\n    [LastPurchaseDate] [datetime] NULL,\r\n    [PersonID] [int] NOT NULL,\r\n CONSTRAINT [PK_Customer.Customers] PRIMARY KEY CLUSTERED \r\n(\r\n    [CustomerID] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n\r\nALTER TABLE [dbo].[Customer.Customers]  WITH CHECK ADD  CONSTRAINT [FK_PersonID_CustomerID] FOREIGN KEY([PersonID])\r\nREFERENCES [dbo].[Customers.Person] ([PersonID])\r\n\r\n\r\nALTER TABLE [dbo].[Customer.Customers] CHECK CONSTRAINT [FK_PersonID_CustomerID]\r\n\r\n\r\n\r\n\r\n\r\n/****** Object:  Table [dbo].[Company.Employees]    Script Date: 14-04-2023 08:56:02 ******/\r\nSET ANSI_NULLS ON\r\n\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\n\r\nCREATE TABLE [dbo].[Company.Employees](\r\n    [OrderLineID] [int] IDENTITY(1,1) NOT NULL,\r\n    [EmploymentDate] [date] NOT NULL,\r\n    [RetirementDate] [date] NOT NULL,\r\n    [Salary] [int] NOT NULL,\r\n    [PersonID] [int] NOT NULL,\r\n CONSTRAINT [PK_Company.Employees] PRIMARY KEY CLUSTERED \r\n(\r\n    [OrderLineID] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n\r\nALTER TABLE [dbo].[Company.Employees]  WITH CHECK ADD  CONSTRAINT [FK_dboCompanyEmployees_ParentTable] FOREIGN KEY([PersonID])\r\nREFERENCES [dbo].[Customers.Person] ([PersonID])\r\n\r\n\r\nALTER TABLE [dbo].[Company.Employees] CHECK CONSTRAINT [FK_dboCompanyEmployees_ParentTable]\r\n\r\n\r\n\r\n\r\n\r\n/****** Object:  Table [dbo].[Sales.Orders]    Script Date: 14-04-2023 08:55:25 ******/\r\nSET ANSI_NULLS ON\r\n\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\n\r\nCREATE TABLE [dbo].[Sales.Orders](\r\n    [OrderID] [int] IDENTITY(1,1) NOT NULL,\r\n    [CustomerID] [int] NOT NULL,\r\n    [TotalPriceOfOrder] [decimal](9, 2) NOT NULL,\r\n    [DateOfOrder] [datetime] NOT NULL,\r\n    [ExpectedDeliveryDate] [datetime] NOT NULL,\r\n    [Comments] [text] NULL,\r\n    [Condition] [tinyint] NOT NULL,\r\n    [SalesPerson] [int] NOT NULL,\r\n CONSTRAINT [PK_Sales.Orders] PRIMARY KEY CLUSTERED \r\n(\r\n    [OrderID] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\n\r\n\r\nALTER TABLE [dbo].[Sales.Orders]  WITH CHECK ADD  CONSTRAINT [FK_CustomerID_Sales_OrdersCustomerID] FOREIGN KEY([CustomerID])\r\nREFERENCES [dbo].[Customer.Customers] ([CustomerID])\r\n\r\n\r\nALTER TABLE [dbo].[Sales.Orders] CHECK CONSTRAINT [FK_CustomerID_Sales_OrdersCustomerID]\r\n\r\n\r\nALTER TABLE [dbo].[Sales.Orders]  WITH CHECK ADD  CONSTRAINT [FK_dboSalesOrders_ParentTable] FOREIGN KEY([SalesPerson])\r\nREFERENCES [dbo].[Company.Employees] ([OrderLineID])\r\n\r\n\r\nALTER TABLE [dbo].[Sales.Orders] CHECK CONSTRAINT [FK_dboSalesOrders_ParentTable]\r\n\r\n\r\nALTER TABLE [dbo].[Sales.Orders]  WITH CHECK ADD  CONSTRAINT [CHK_Condition] CHECK  (([Condition]>=(0) AND [Condition]<=(4)))\r\n\r\n\r\nALTER TABLE [dbo].[Sales.Orders] CHECK CONSTRAINT [CHK_Condition]\r\n\r\n\r\n\r\n\r\n\r\n/****** Object:  Table [dbo].[Product]    Script Date: 13-04-2023 09:39:11 ******/\r\nSET ANSI_NULLS ON\r\n\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\n\r\nCREATE TABLE [dbo].[Product](\r\n    [ProductID] [int] IDENTITY(1,1) NOT NULL,\r\n    [ProductName] [nvarchar](50) NOT NULL,\r\n    [ProductDescription] [text] NOT NULL,\r\n    [ProductSalePrice] [money] NOT NULL,\r\n    [ProductPurchasePrice] [money] NOT NULL,\r\n    [ProductLocation] [nvarchar](50) NOT NULL,\r\n    [ProductQuantity] [int] NOT NULL,\r\n    [ProductUnit] [int] NOT NULL,\r\n CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED \r\n(\r\n    [ProductID] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]\r\n\r\n\r\n\r\n\r\n\r\n/****** Object:  Table [dbo].[Sales.OrderLines]    Script Date: 13-04-2023 09:38:44 ******/\r\nSET ANSI_NULLS ON\r\n\r\n\r\nSET QUOTED_IDENTIFIER ON\r\n\r\n\r\nCREATE TABLE [dbo].[Sales.OrderLines](\r\n    [OrderLineID] [int] IDENTITY(1,1) NOT NULL,\r\n    [ProductID] [int] NOT NULL,\r\n    [SinglePrice] [decimal](9, 2) NOT NULL,\r\n    [OrderQuantity] [nchar](10) NOT NULL,\r\n    [TotalQuantityPrice] [decimal](9, 2) NOT NULL,\r\n    [OrderID] [int] NOT NULL,\r\n CONSTRAINT [PK_Sales.OrderLines] PRIMARY KEY CLUSTERED \r\n(\r\n    [OrderLineID] ASC\r\n)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]\r\n) ON [PRIMARY]\r\n\r\n\r\nALTER TABLE [dbo].[Sales.OrderLines]  WITH CHECK ADD  CONSTRAINT [FK_dboSalesOrderlines_ParentTable] FOREIGN KEY([OrderID])\r\nREFERENCES [dbo].[Sales.Orders] ([OrderID])\r\n\r\n\r\nALTER TABLE [dbo].[Sales.OrderLines] CHECK CONSTRAINT [FK_dboSalesOrderlines_ParentTable]\r\n\r\n\r\nALTER TABLE [dbo].[Sales.OrderLines]  WITH CHECK ADD  CONSTRAINT [FK_ProductId_Orders_ProductID] FOREIGN KEY([ProductID])\r\nREFERENCES [dbo].[Product] ([ProductID])\r\n\r\n\r\nALTER TABLE [dbo].[Sales.OrderLines] CHECK CONSTRAINT [FK_ProductId_Orders_ProductID]\r\n");
        }

        public void RefreshData(int amountToAdd)
        {
            DeleteAllData();
            BulkAddData(amountToAdd);
        }
    }
}