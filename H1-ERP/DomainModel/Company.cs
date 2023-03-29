using H1_ERP.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class Company
    {
        ErrorHandling.ErrorHandling errorHandling = new(); //instance for ErrorHandling Class
        private int _Companyid;



        public string _companyName;
        private Address _address;
        public enum currency { DKK = 0, USD = 1, EUR = 2 }
        public currency Currency { get; set; }
       

        public int CompanyID
        {
            get { return errorHandling.IsNull(_Companyid); }
            set { _Companyid = errorHandling.IsNull(value); }
        }
        /*---------------------------------*/
        public Company(string name)
        {
            CompanyName = name; 
        }
        public Company()
        {

        }

        public string CompanyName 
        {

            get
            {
                return errorHandling.IsNull(_companyName);
            }
            set
            {
                _companyName = errorHandling.IsNull(value);

            }
        }
        public Address Address {
            get
            {
                return errorHandling.IsNull(_address);
            }
            set
            {
                _address = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/
       
    }
}
