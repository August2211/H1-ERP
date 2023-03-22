﻿using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class SalesOrderLine
    {
        public uint Id { get; private set; }
        private ushort _quantity; 
        private decimal _singleprice;
        public ushort Quantity { get { return _quantity;   }
            set 
            {
                 if(Quantity == 0)
                 {
                        throw new Exception("u cannot have a 0 quantatiy of anything"); 
                 } 

                 _quantity = value;
            } 
        }

       public SalesOrderLine(Product product, ushort quantity) 
       {
            Id = 2;
            _quantity = quantity;
            SingleUnitPrice = product.SalePrice;
            TotalPrice = (decimal)(SingleUnitPrice * Quantity);
            Product = product;

       }

        public decimal SingleUnitPrice {   
            get 
            {
                return _singleprice; 
            } 
           private set 
            {
              _singleprice= value;
            }
        }

        public decimal TotalPrice { get; private set; }

        public Product Product { get;private set; }
                    
    }
}
