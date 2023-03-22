using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    internal enum Condtion
    {
        None = 0,
        Created = 1,
        Confirmed = 2,
        Pakagede =3,
        done = 4,
    }
    public class SalesOrderHeader
    {
        public SalesOrderHeader(List<SalesOrderLine> input)
        {
            Condtion = Condtion.Created;
            CustomerID = 1; 
            Creationtime = DateTime.Now; 
            CompletionTime = DateTime.Now.AddDays(3);
            OrderLines = input;
            OrderID = 2; 
        }
        
        public uint OrderID { get; set; }

        public DateTime Creationtime { get; private set; }

        public DateTime CompletionTime { get;  set; }

        public uint CustomerID { get; set; }

        internal Condtion Condtion { get; set; }

         public List<SalesOrderLine> OrderLines { get;  private set; }

        /// <summary>
        /// This is a method for calculating the total price of the wholde order
        /// </summary>
        /// <returns></returns>
        public decimal TotalOrderPrice(List<SalesOrderLine> input)
        {
            return input.Select(x => x.TotalPrice).ToArray().Sum();
        }
    }
}
