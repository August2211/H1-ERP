using H1_ERP.ErrorHandling;

namespace H1_ERP.DomainModel
{
    public class Address
    {

        //A new instance of ErrorHandling.
        ErrorHandling.ErrorHandling ErrorHandler = new();

        //Address properties.
        private string _roadName;
        private string _streetNumber;
        private string _zipCode;
        private string _city;
        private string _country;

        public uint AdressID { get;  set; }
        public string RoadName
        {
            get
            {
                //If the road name is null, throw an exception else return the road name.
                return ErrorHandler.IsNull(_roadName);
            }
            set
            {
                //If the road name is null, throw an exception else set the road name.
                _roadName = ErrorHandler.IsNull(value);
            }
        }
        public string StreetNumber
        {
            get
            {
                return ErrorHandler.IsNull(_streetNumber);
            }
            set
            {
                _streetNumber = ErrorHandler.IsNull(value);
            }
        }
        public string ZipCode
        {
            get
            {
                return ErrorHandler.IsNull(_zipCode);
            }
            set
            {
                _zipCode = ErrorHandler.IsNull(value);
            }
        }
        public string City
        {
            get
            {
                return ErrorHandler.IsNull(_city);
            }
            set
            {
                _city = ErrorHandler.IsNull(value);
            }
        }
        public string Country
        {
            get
            {
                return ErrorHandler.IsNull(_country);
            }
            set
            {
                _country = ErrorHandler.IsNull(value);
            }
        }
    }
}