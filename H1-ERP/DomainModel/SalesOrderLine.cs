using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class SalesOrderLine
    {

        public ushort Quantity { get { return Quantity; }
            set 
            {
                 if(Quantity == 0)
                 {
                        throw new Exception("u cannot have a 0 quantatiy of anything"); 
                 }
            } 
        }

       public SalesOrderLine(Product product, ushort quantity) 
       {
            Quantity = quantity;
            TotalPrice = (decimal)(SingleUnitPrice * Quantity);
            Product = product;
       }

        public decimal SingleUnitPrice {   
            get 
            {
                return SingleUnitPrice; 
            } 
            set 
            {
                if (SingleUnitPrice < 0)
                {
                    throw new InvalidDataException("a price cannot be negative");
                }
                else SingleUnitPrice = SingleUnitPrice;
            }
        }

        public decimal TotalPrice { get; private set; }

        public Product Product { get;private set; }
                    
    }
}
