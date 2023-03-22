using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.DomainModel
{
    public class Person
    {
        //Person properties.
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }

        //Function that returns the full name of a person in "".
        public string FullName()
        {
            return $"\"{FirstName} {LastName}\"";
        }
    }
}
