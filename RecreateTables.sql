USE [master]
GO

/****** Object:  Table [dbo].[Customer.Adress]    Script Date: 13-04-2023 09:40:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer.Adress](
    [AdressID] [int] IDENTITY(1,1) NOT NULL,
    [RoadName] [text] NOT NULL,
    [StreetNumber] [nvarchar](15) NOT NULL,
    [ZipCode] [nchar](15) NOT NULL,
    [City] [nvarchar](85) NOT NULL,
    [Country] [nvarchar](60) NOT NULL,
 CONSTRAINT [PK_Customer.Adress] PRIMARY KEY CLUSTERED 
(
    [AdressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [master]
GO

/****** Object:  Table [dbo].[Customers.Person]    Script Date: 13-04-2023 09:39:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customers.Person](
    [PersonID] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] [text] NOT NULL,
    [LastName] [text] NOT NULL,
    [Email] [text] NOT NULL,
    [PhoneNumber] [nvarchar](15) NOT NULL,
    [AdressID] [int] NOT NULL,
 CONSTRAINT [PK_Customers.Person] PRIMARY KEY CLUSTERED 
(
    [PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Customers.Person]  WITH CHECK ADD  CONSTRAINT [FK_AdressID_PersonID] FOREIGN KEY([AdressID])
REFERENCES [dbo].[Customer.Adress] ([AdressID])
GO

ALTER TABLE [dbo].[Customers.Person] CHECK CONSTRAINT [FK_AdressID_PersonID]
GO

USE [master]
GO

/****** Object:  Table [dbo].[Company]    Script Date: 13-04-2023 09:40:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Company](
    [CompanyID] [int] IDENTITY(1,1) NOT NULL,
    [CompanyName] [nvarchar](50) NOT NULL,
    [Currency] [int] NOT NULL,
    [AdressID] [int] NOT NULL,
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
    [CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK_dboCompany_ParentTable] FOREIGN KEY([AdressID])
REFERENCES [dbo].[Customer.Adress] ([AdressID])
GO

ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK_dboCompany_ParentTable]
GO

USE [master]
GO

/****** Object:  Table [dbo].[Customer.Customers]    Script Date: 13-04-2023 09:39:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Customer.Customers](
    [CustomerID] [int] IDENTITY(1,1) NOT NULL,
    [LastPurchaseDate] [datetime] NULL,
    [PersonID] [int] NOT NULL,
 CONSTRAINT [PK_Customer.Customers] PRIMARY KEY CLUSTERED 
(
    [CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Customer.Customers]  WITH CHECK ADD  CONSTRAINT [FK_PersonID_CustomerID] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Customers.Person] ([PersonID])
GO

ALTER TABLE [dbo].[Customer.Customers] CHECK CONSTRAINT [FK_PersonID_CustomerID]
GO

USE [master]
GO

/****** Object:  Table [dbo].[Company.Employees]    Script Date: 14-04-2023 08:56:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Company.Employees](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [EmploymentDate] [date] NOT NULL,
    [RetirementDate] [date] NOT NULL,
    [Salary] [int] NOT NULL,
    [PersonID] [int] NOT NULL,
 CONSTRAINT [PK_Company.Employees] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Company.Employees]  WITH CHECK ADD  CONSTRAINT [FK_dboCompanyEmployees_ParentTable] FOREIGN KEY([PersonID])
REFERENCES [dbo].[Customers.Person] ([PersonID])
GO

ALTER TABLE [dbo].[Company.Employees] CHECK CONSTRAINT [FK_dboCompanyEmployees_ParentTable]
GO

USE [master]
GO

/****** Object:  Table [dbo].[Sales.Orders]    Script Date: 14-04-2023 08:55:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sales.Orders](
    [OrderID] [int] IDENTITY(1,1) NOT NULL,
    [CustomerID] [int] NOT NULL,
    [TotalPriceOfOrder] [decimal](9, 2) NOT NULL,
    [DateOfOrder] [datetime] NOT NULL,
    [ExpectedDeliveryDate] [datetime] NOT NULL,
    [Comments] [text] NULL,
    [Condition] [tinyint] NOT NULL,
    [SalesPerson] [int] NOT NULL,
 CONSTRAINT [PK_Sales.Orders] PRIMARY KEY CLUSTERED 
(
    [OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Sales.Orders]  WITH CHECK ADD  CONSTRAINT [FK_CustomerID_Sales_OrdersCustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer.Customers] ([CustomerID])
GO

ALTER TABLE [dbo].[Sales.Orders] CHECK CONSTRAINT [FK_CustomerID_Sales_OrdersCustomerID]
GO

ALTER TABLE [dbo].[Sales.Orders]  WITH CHECK ADD  CONSTRAINT [FK_dboSalesOrders_ParentTable] FOREIGN KEY([SalesPerson])
REFERENCES [dbo].[Company.Employees] ([Id])
GO

ALTER TABLE [dbo].[Sales.Orders] CHECK CONSTRAINT [FK_dboSalesOrders_ParentTable]
GO

ALTER TABLE [dbo].[Sales.Orders]  WITH CHECK ADD  CONSTRAINT [CHK_Condition] CHECK  (([Condition]>=(0) AND [Condition]<=(4)))
GO

ALTER TABLE [dbo].[Sales.Orders] CHECK CONSTRAINT [CHK_Condition]
GO

USE [master]
GO

/****** Object:  Table [dbo].[Product]    Script Date: 13-04-2023 09:39:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
    [ProductID] [int] IDENTITY(1,1) NOT NULL,
    [ProductName] [nvarchar](50) NOT NULL,
    [ProductDescription] [text] NOT NULL,
    [ProductSalePrice] [money] NOT NULL,
    [ProductPurchasePrice] [money] NOT NULL,
    [ProductLocation] [nvarchar](50) NOT NULL,
    [ProductQuantity] [int] NOT NULL,
    [ProductUnit] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
    [ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [master]
GO

/****** Object:  Table [dbo].[Sales.OrderLines]    Script Date: 13-04-2023 09:38:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sales.OrderLines](
    [OrderLineID] [int] IDENTITY(1,1) NOT NULL,
    [ProductID] [int] NOT NULL,
    [SinglePrice] [decimal](9, 2) NOT NULL,
    [OrderQuantity] [nchar](10) NOT NULL,
    [TotalQuantityPrice] [decimal](9, 2) NOT NULL,
    [OrderID] [int] NOT NULL,
 CONSTRAINT [PK_Sales.OrderLines] PRIMARY KEY CLUSTERED 
(
    [OrderLineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Sales.OrderLines]  WITH CHECK ADD  CONSTRAINT [FK_dboSalesOrderlines_ParentTable] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Sales.Orders] ([OrderID])
GO

ALTER TABLE [dbo].[Sales.OrderLines] CHECK CONSTRAINT [FK_dboSalesOrderlines_ParentTable]
GO

ALTER TABLE [dbo].[Sales.OrderLines]  WITH CHECK ADD  CONSTRAINT [FK_ProductId_Orders_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO

ALTER TABLE [dbo].[Sales.OrderLines] CHECK CONSTRAINT [FK_ProductId_Orders_ProductID]
GO