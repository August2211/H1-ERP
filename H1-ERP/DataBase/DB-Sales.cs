using H1_ERP.DomainModel;
using H1_ERP.Interfaces_s;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DataBase
{
    public partial class DataBase : IRepos<SalesOrderHeader>
    {
        /// <summary>
        /// This method returns a Object of SalesOrderheader type and it does so from the ID parameter of the database eqeuvelant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SalesOrderHeader GetSalesOrderHeaderFromID(int id)
        {

            var querrydata = GetDatafast($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines] WHERE OrderID = {id}");

            List<SalesOrderLine> lines = new List<SalesOrderLine>();
            //Get's all of the orderlines which is connected to the current order. 
            foreach (var s in querrydata.Values)
            {
                int GetProdID = (int)s[1];
                //Calls the method getproductfromid thereby i can instancetiate my object so that i can create an order line.
                Product temp = GetProductFromID(GetProdID);
                SalesOrderLine tempsalesorderline = new SalesOrderLine(temp, Convert.ToUInt16(s[4]));
                lines.Add(tempsalesorderline);

            }
            // After we have all of the orderlines we can create an empty obejct as a representation of the object in the DB. 
            SalesOrderHeader res = new SalesOrderHeader(lines);
            // Lookup in the Order table for the remaning information. 

            var querrydata2 = GetDatafast($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] WHERE OrderID = {id}");

            foreach (var s in querrydata2.Values)
            {
                res.OrderID = Convert.ToUInt32(s[0]);
                res.CustomerID = Convert.ToUInt32(s[1]);
                res.DateOfOrder = Convert.ToDateTime(s[3]);
                res.ExpectedDeliveryDate = Convert.ToDateTime(s[4]);
            }
            return res;
        }
        /// <summary>
        /// Gets all the ids of the current customers and foreach id it finds it calls the method which finds a customer from the id. 
        /// </summary>
        /// <returns></returns>
        public List<SalesOrderHeader> GetAll()
        {

            var products = GetdataFastFromJoinsWithouttheKeyvalueparoftheId($"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] INNER Join [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines]" +
                $"on [dbo].[Sales.OrderLines].OrderID = [dbo].[Sales.Orders].OrderID Inner join  [H1PD021123_Gruppe4].[dbo].[Product]" +
                $"on [dbo].[Product].ProductID =  [dbo].[Sales.OrderLines].ProductID");
            List<SalesOrderHeader> res = new List<SalesOrderHeader>();


            foreach (var orderheaderline in products.Values)
            {
                var checklist = res.Where(x => x.OrderID == Convert.ToUInt32(orderheaderline[0])).ToList();
                if (checklist.Count == 0)
                {
                    List<SalesOrderLine> line = new List<SalesOrderLine>();
                    //get all of the orderlines with matching ID to the orderID 
                    var orderlineswithorderid = products.Values.Select(x => x).Where(x => x.ElementAt(13).ToString() == orderheaderline[0].ToString()).ToList();
                    foreach (var orderline in orderlineswithorderid)
                    {
                        //we select the product with the matching ID 
                        var prod = products.Values.Select(x => x).Where(x => x.ElementAt(9).Equals(orderline[9])).FirstOrDefault();
                        Product product = new Product();
                        product.ProductId = Convert.ToInt32(prod[9]);
                        product.ProductName = prod[15].ToString();
                        product.ProductDescription = prod[16].ToString();
                        product.ProductSalePrice = Convert.ToInt32(prod[17]);
                        product.ProductPurchasePrice = Convert.ToInt32(prod[18]);
                        product.ProductLocation = prod[19].ToString();
                        product.ProductQuantity = Convert.ToInt32(prod[20]);
                        product.ProductUnit = Convert.ToInt32(prod[21]);
                        SalesOrderLine orderLine = new SalesOrderLine(product, Convert.ToUInt16(prod[20].ToString()));
                        orderLine.Id = Convert.ToUInt32(orderline[8]);
                        orderLine.Quantity = Convert.ToUInt16(orderline[20]);
                        line.Add(orderLine);
                    }
                    SalesOrderHeader salesOrder = new SalesOrderHeader(line);
                    salesOrder.OrderID = Convert.ToUInt32(orderheaderline[0]);
                    salesOrder.CustomerID = Convert.ToUInt32(orderheaderline[1]);
                    salesOrder.TotalOrderPrice();
                    salesOrder.Creationtime = Convert.ToDateTime(orderheaderline[3]);
                    salesOrder.CompletionTime = Convert.ToDateTime((DateTime)orderheaderline[4]);
                    salesOrder.Condtion = (Condtion)Convert.ToInt32(orderheaderline[6]);
                    res.Add(salesOrder);
                }
            }
            return res;
        }
        /// <summary>
        /// Inserts a sales ordereheader from an new instance of the obejct 
        /// </summary>
        /// <param name="input"></param>
        public void InsertSalesOrderHeader(SalesOrderHeader input)
        {
            Exec_SQL_Command($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Sales.Orders] (CustomerID,TotalPriceOfOrder,DateOfOrder,ExpectedDeliveryDate) VALUES({input.CustomerID},{input.TotalOrderPrice()},{input.Creationtime},{input.CompletionTime})");
            //find the id of the inserted header by looking at the highest ID
            var Data = GetData("SELECT TOP (1) [OrderID] FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] ORDER BY [OrderID] desc;");
            int OrderID = 1;
            foreach (var Row in Data.Values)
            {
                OrderID = Convert.ToInt32(Row[0]);
            }
            //InsertSalesOrderHeader all of the Orderlines in the table orderlines
            foreach (var OrderLine in input.OrderLines)
            {
                Exec_SQL_Command($"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines] (SinglePrice,OrderQuantity,TotalQuantityPrice,OrderID,ProductID) VALUES({OrderLine.SingleUnitPrice},{OrderLine.Quantity},{OrderLine.TotalPrice},{OrderID},{OrderLine.Product.ProductId})");
            }
        }
        /// <summary>
        /// Updates a salesorderheader with a new instance of the object and the id of the item you want to change
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="NewsalesHeader"></param>
        public void UpdateSalesorderHeader(int ID, SalesOrderHeader NewsalesHeader)
        {
            string sqlCommands = "";
            foreach (var OrderLines in NewsalesHeader.OrderLines)
            {
                sqlCommands += $"UPDATE [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines] SET SinglePrice = {OrderLines.SingleUnitPrice.ToString().Replace(',', '.')}, OrderQuantity = {OrderLines.Quantity.ToString().Replace(',', '.')}, TotalQuantityPrice = {OrderLines.TotalPrice.ToString().Replace(',', '.')} WHERE OrderID = {ID} AND OrderLineID = {OrderLines.Id};\n";
            }
            sqlCommands = $"SET DATEFORMAT dmy;\nUPDATE [H1PD021123_Gruppe4].[dbo].[Sales.Orders] SET TotalPriceOfOrder = {NewsalesHeader.TotalOrderPrice().ToString().Replace(',', '.')}, ExpectedDeliveryDate = CONVERT(datetime,'{NewsalesHeader.CompletionTime}', 103) WHERE OrderID = {ID};\n" + sqlCommands;
            Exec_SQL_Command(sqlCommands);
        }
        /// <summary>
        /// Deletes a instance from the DB with the given ID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteSalesOrderHeaderFromID(int id)
        {
            Exec_SQL_Command($"$DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines] WHERE OrderID = {id};" +
                $" DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] WHERE OrderID = {id};");
        }

        public SalesOrderHeader GetFromID(int id)
        {
            return GetSalesOrderHeaderFromID(id);

        }

        public List<SalesOrderHeader> GetAll(int id)
        {
            return GetAll();
        }

        public void Delete(int id)
        {
            DeleteSalesOrderHeaderFromID(id);
        }

        public void Update(SalesOrderHeader enity)
        {
            UpdateSalesorderHeader((int)enity.OrderID, enity);
        }

        public void Insert(SalesOrderHeader entity)
        {
            InsertSalesOrderHeader(entity);
        }
    }
}