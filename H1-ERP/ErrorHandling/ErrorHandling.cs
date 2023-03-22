using H1_ERP.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.ErrorHandling
{
    public class ErrorHandling
    {
        //If the input is null, throw an exception else return the input.
        public string IsNull(string input)
        {
            if (input == null)
                throw new InvalidDataException("This field cannot be null");
            else return input;
        }
        //Allows int to be used.
        public int IsNull(int input)
        {
            if (input == null)
                throw new InvalidDataException("This field can¨t be null");
            else return input;
        }
        public Address IsNull(Address input)
        {
            if (input.Country == null || input.StreetNumber == null || input.City == null || input.RoadName == null || input.ZipCode == null)
                throw new InvalidDataException("This field cannot be null");
            else return input;
        }
        public DateTime IsNull(DateTime input)
        {
            if (input == null)
                throw new InvalidDataException("This field cannot be null");
            else return input;
        }
        public double IsNull(double input)
        {
            if (input == null)
                throw new InvalidDataException("This field can't be null");
            else return input;
        }
    }
}
