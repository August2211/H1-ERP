using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class Person
    {
        //A new instance of ErrorHandling.
        ErrorHandling.ErrorHandling ErrorHandler = new();

        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private Address _address;

        //Person properties.
        public string FirstName
        {
            get
            {
                return ErrorHandler.IsNull(_firstName);
            }
            set
            {
                _firstName = ErrorHandler.IsNull(value);
            }
        }
        public string LastName
        {
            get
            {
                return ErrorHandler.IsNull(_lastName);
            }
            set
            {
                _lastName = ErrorHandler.IsNull(value);
            }
        }
        public string Email
        {
            get
            {
                return ErrorHandler.IsNull(_email);
            }
            set
            {
                _email = ErrorHandler.IsNull(value);
            }
        }
        public string PhoneNumber
        {
            get
            {
                return ErrorHandler.IsNull(_phoneNumber);
            }
            set
            {
                _phoneNumber = ErrorHandler.IsNull(value);
                { }
            }
        }
        public Address Address
        {
            get
            {
                return ErrorHandler.IsNull(_address);
            }
            set
            {
                _address = ErrorHandler.IsNull(value);
            }
        }

        //Function that returns the full name of a person in "".
        public string FullName()
        {
            return $"\"{ErrorHandler.IsNull(FirstName)} {ErrorHandler.IsNull(LastName)}\"";
        }
    }
}
