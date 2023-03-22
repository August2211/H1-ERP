using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class Customer : Person
    {
        //Customer Properties.
        public int CustomerId { get; set; }
        public DateTime LastPurchaseDate { get; set; }
    }
}
