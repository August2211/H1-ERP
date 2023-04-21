using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class SalesOrderLine
    {
        public uint OrderLineID { get;  set; }
        private ushort _quantity; 
        private decimal _singleprice;
        public ushort Quantity { get { return _quantity;   }
            set 
            {
                if (Quantity == 0)
                {
                    throw new Exception("u cannot have a 0 quantatiy of anything");
                }
                else { _quantity = value; }
            } 
        }
        public uint OrderID { get; set; }

       public SalesOrderLine(Product product, ushort quantity) 
       {
            _quantity = quantity;
            SinglePrice = product.ProductSalePrice;
            TotalQuantityPrice = (decimal)(SinglePrice * Quantity);
            Product = product;
       }
        public SalesOrderLine() 
        {
        
        }

        public decimal SinglePrice {   
            get 
            {
                return _singleprice; 
            } 
           private set 
            {
              _singleprice= value;
            }
        }

        public decimal TotalQuantityPrice { get;  set; }

        public Product Product { get; set; }
    }
}
