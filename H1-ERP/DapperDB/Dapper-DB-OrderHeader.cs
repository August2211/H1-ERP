using Dapper;
using H1_ERP.DomainModel;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DapperDB
{
    public partial class DatabaseDapper
    {
        public SalesOrderHeader GetSalesOrderHeader(int ID)
        {
            SalesOrderHeader? OrderHeader = null;

            using (var conn = getConnection())
            {
                conn.Query<SalesOrderHeader, SalesOrderLine, Product, SalesOrderHeader>($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] " +
                $"INNER JOIN [Sales.OrderLines] ON " +
                $"[Sales.OrderLines].OrderID = [Sales.Orders].OrderID " +
                $"INNER JOIN Product ON " +
                $"Product.ProductID = [Sales.OrderLines].ProductID " +
                $"WHERE [Sales.Orders].OrderID = {ID}", (salesOrderheader, SalesOrderLine, Product) =>
                {
                    SalesOrderLine.Product = Product;
                    salesOrderheader.OrderLines = new List<SalesOrderLine>
                    {
                        SalesOrderLine
                    };

                    if (OrderHeader == null)
                    {
                        OrderHeader = salesOrderheader;
                    }
                    else
                    {
                        OrderHeader.OrderLines.Add(SalesOrderLine);
                    }
                    return null;
                },
             splitOn: "OrderID,OrderLineID,ProductID"
             );

                return OrderHeader;
            }
        }

        public List<SalesOrderHeader> GetAllSalesOrderHeaders()
        {
            List<SalesOrderHeader> OrderHeader = new List<SalesOrderHeader>();

            using (var conn = getConnection())
            {
                conn.Query<SalesOrderHeader, SalesOrderLine, Product, SalesOrderHeader>($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] " +
                $"INNER JOIN [Sales.OrderLines] ON " +
                $"[Sales.OrderLines].OrderID = [Sales.Orders].OrderID " +
                $"INNER JOIN Product ON " +
                $"Product.ProductID = [Sales.OrderLines].ProductID "
                , (salesOrderheader, SalesOrderLine, Product) =>
                {
                    SalesOrderLine.Product = Product;

                    if (!OrderHeader.Any(header => header.OrderID == salesOrderheader.OrderID))
                    {
                        salesOrderheader.OrderLines = new List<SalesOrderLine>
                        {
                            SalesOrderLine
                        };
                        OrderHeader.Add(salesOrderheader);
                    }
                    else
                    {
                        OrderHeader.FirstOrDefault(header => header.OrderID == salesOrderheader.OrderID).OrderLines.Add(SalesOrderLine);
                    }
                    return null;
                },
             splitOn: "OrderID,OrderLineID,ProductID"
             );

                return OrderHeader;
            }
        }

        public void DeleteSalesOrderHeaderFromID(int id)
        {
            using (var connection = getConnection())
            {
                connection.Execute($"$DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines] WHERE OrderID = {id};" +
                $" DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] WHERE OrderID = {id};"); 
            }

        }
        public void UpdateSalesOrderHeader(SalesOrderHeader salesOrderHeader)
        {
            using(var conn = getConnection())
            {
                string sqlCommands = "";
                foreach (var OrderLines in salesOrderHeader.OrderLines)
                {
                    sqlCommands += $"UPDATE [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines] SET SinglePrice = {OrderLines.SinglePrice.ToString().Replace(',', '.')}, OrderQuantity = {OrderLines.Quantity.ToString().Replace(',', '.')}, TotalQuantityPrice = {OrderLines.TotalQuantityPrice.ToString().Replace(',', '.')} WHERE OrderID = {salesOrderHeader} AND OrderLineID = {OrderLines.OrderLineID};\n";
                }
                sqlCommands = $"SET DATEFORMAT dmy;\nUPDATE [H1PD021123_Gruppe4].[dbo].[Sales.Orders] SET TotalPriceOfOrder = {salesOrderHeader.TotalOrderPrice().ToString().Replace(',', '.')}, ExpectedDeliveryDate = CONVERT(datetime,'{salesOrderHeader.ExpectedDeliveryDate}', 103) WHERE OrderID = {salesOrderHeader.OrderID};\n" + sqlCommands;
                conn.Execute(sqlCommands);
            }

        }

    }
}
