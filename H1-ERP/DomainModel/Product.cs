using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class Product
    {
        //Product properties.
        /*--------------------------------*/

        private int orderId;

        public int OrderId        
        {
            get
            {
                if (orderId == null)
                {
                  

                }
            }
            
            set { orderId = value; }
        }

        //public int OrederID { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public double SalePrice { get; set; }
        //public double purchasePrice { get; set; }
        //public string Location { get; set; }
        //public double quantity { get; set; }
        //public int unit { get; set; }
        /*---------------------------------*/
    }
}