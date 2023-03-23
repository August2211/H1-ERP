using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class Customer : Person
    {
        //A new instance of ErrorHandling.
        ErrorHandling.ErrorHandling ErrorHandler = new();
        //Customer Properties.
        private int _customerId;
        private DateTime? _lastPurchaseDate;

        public int CustomerId
        {
            get
            {
                //If the CustomerId is null, throw an exception else return the CustomerId.
                return ErrorHandler.IsNull(_customerId);
            }
            set
            {
                //If the CustomerId is null, throw an exception else set the CustomerId.
                _customerId = ErrorHandler.IsNull(value);
            }
        }
        public DateTime? LastPurchaseDate
        {
            get
            {
                return _lastPurchaseDate;
            }
            set
            {
                _lastPurchaseDate = value;
            }
        }
        //Ensure that there is always a customer id.
    
    }
}