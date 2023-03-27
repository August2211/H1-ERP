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

       

        private string _companyName;
        private string _street;
        private string _houseNumber;
        private string _zipCode;
        private string _city;
        private string _country;
        private string _currency;

        public int CompanyID
        {
            get { return errorHandling.IsNull(_Companyid); }
            set { _Companyid = errorHandling.IsNull(value); }
        }
        /*---------------------------------*/

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
        /*---------------------------------*/
        public string Street
        {

            get
            {
                return errorHandling.IsNull(_street);
            }
            set
            {
                _street = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/
        public string HouseNumber
        {

            get
            {
                return errorHandling.IsNull(_houseNumber);
            }
            set
            {
                _houseNumber = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/
        public string ZipCode
        {

            get
            {
                return errorHandling.IsNull(_zipCode);
            }
            set
            {
                _zipCode = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/
        public string City
        {

            get
            {
                return errorHandling.IsNull(_city);
            }
            set
            {
                _city = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/
        public string Country
        {

            get
            {
                return errorHandling.IsNull(_country);
            }
            set
            {
                _country = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/
        public string Currency
        {
            
            get
            {
                return errorHandling.IsNull(_currency);
            }
            set
            {
                _currency = errorHandling.IsNull(value);

            }
        }
        /*---------------------------------*/
    }
}
