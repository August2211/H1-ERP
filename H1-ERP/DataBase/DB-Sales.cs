﻿using H1_ERP.DomainModel;
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
    public partial class DataBase
    {
        /// <summary>
        /// This method return a Object of SalesOrderheader type and it does so form the ID parameter of the database eqeuvelant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SalesOrderHeader GetSalesOrderHeaderFromID(int id)
        {
            SqlConnection conn = getConnection(); 
            conn.Open();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.OrderLines] WHERE OrderID = {id}"; 
            SqlCommand commando = new SqlCommand(sql, conn);
            SqlDataReader sqlDataReader = commando.ExecuteReader();
            int rows = 0; 

            
            List<SalesOrderLine> lines = new List<SalesOrderLine>();
            //gets all of the orderlines which is connected to the current order 
            while(sqlDataReader.Read())
            {
                int GetProdID = (int)sqlDataReader[1];
                //calls the method getproductfromid thereby i can instancetiate my obejct so that i can create an order line 
                Product temp = GetProductFromID(GetProdID);
                SalesOrderLine tempsalesorderline = new SalesOrderLine(temp, Convert.ToUInt16(sqlDataReader[4]));
                lines.Add(tempsalesorderline);

            }
            // after we have all of the orderlines we can create an empty obejct as a representation of the obejct in th DB  
            SalesOrderHeader res = new SalesOrderHeader(lines);

            sqlDataReader.Close();
            // lookup in the Order table for the remaning information 
            commando =new SqlCommand( $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] WHERE OrderID = {id}",conn);
            sqlDataReader = commando.ExecuteReader();
            rows = 0; 
            while(sqlDataReader.Read())
            {
                while(rows < sqlDataReader.FieldCount)
                {
                    res.OrderID = Convert.ToUInt32(sqlDataReader[0]);
                    res.CustomerID = Convert.ToUInt32(sqlDataReader[1]);
                    res.Creationtime = Convert.ToDateTime(sqlDataReader[3]);
                    res.CompletionTime = Convert.ToDateTime(sqlDataReader[4]);
                    rows++;
                }

            }


            conn.Close(); 
            return res; 
        }  
        /// <summary>
        /// Gets all the ids of the current customers and foreach id it finds it calls the method which finds a customer from the id 
        /// </summary>
        /// <returns></returns>
        public List<SalesOrderHeader> GetAll()
        {

            SqlConnection connection = getConnection();
            connection.Open();
            string sql = $"SELECT * FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders]";
            List<SalesOrderHeader> res = new List<SalesOrderHeader>();
            SqlCommand command = new SqlCommand(sql, connection); 
            SqlDataReader reader = command.ExecuteReader();
            List<int> ints = new List<int>();

            while (reader.Read())
            {
                ints.Add((int)reader[0]);
            }
            foreach(var s in ints)
            {
               var orderheader = GetSalesOrderHeaderFromID(s); 
               res.Add(orderheader);
            }
             

            return res; 
        }
        public void Insert(SalesOrderHeader input)
        {
            SqlConnection connection = getConnection();
            connection.Open(); 

            string sql = $"INSERT INTO [H1PD021123_Gruppe4].[dbo].[Sales.Orders] (CustomerID,TotalPriceOfOrder,DateOfOrder,ExpectedDeliveryDate) VALUES({input.CustomerID},{input.TotalOrderPrice()},{input.Creationtime},{input.CompletionTime})";
           SqlCommand command = new SqlCommand(sql, connection);

           int i =  command.ExecuteNonQuery(); 
    
        }

        public void Update(SalesOrderLine input, int id)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            SqlCommand command = new SqlCommand($"UPDATE [H1PD021123_Gruppe4].[dbo].[Sales.Orders] SET SinglePrice = {input.SingleUnitPrice}, OrderQuantity = {input.Quantity}, TotalQuantityPrice = {input.TotalPrice} WHERE OrderID = {id} ",connection);
            command.ExecuteNonQuery(); 
        }

        public void Delete(int id)
        {
            SqlConnection connection = getConnection();
            connection.Open();
            SqlCommand command = new SqlCommand($"DELETE FROM [H1PD021123_Gruppe4].[dbo].[Sales.Orders] WHERE OrderID = {id}", connection);
            command.ExecuteNonQuery();

        }


    }
}
