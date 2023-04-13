using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public enum Condtion
    {
        None = 0,
        Created = 1,
        Confirmed = 2,
        Pakagede =3,
        done = 4,
    }
    public class SalesOrderHeader
    {
        private uint _orderId; 
        public SalesOrderHeader(List<SalesOrderLine> input)
        {
            Condtion = Condtion.Created;
            Creationtime = DateTime.Now; 
            CompletionTime = DateTime.Now.AddDays(3);
            OrderLines = input; 
        }
        
        public uint OrderID { get;  set; }

        public DateTime Creationtime { get;  set; }

        public DateTime CompletionTime { get;  set; }

        public uint CustomerID { get; set; }

        public Condtion Condtion { get; set; }

         public List<SalesOrderLine> OrderLines { get;  private set; }

        public Condtion condition
        {
            get => default;
            set
            {
            }
        }

        public SalesOrderLine salesOrderLine
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// This is a method for calculating the total price of the wholde order
        /// </summary>
        /// <returns></returns>
        public decimal TotalOrderPrice()
        {
            return this.OrderLines.Select(x => x.TotalPrice).ToArray().Sum();
        }
    }
}
