using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1_ERP.ErrorHandling
{
    public class ErrorHandling
    {
        public void IsNull(string input)
        {
            if (input == null)
                throw new InvalidDataException("This field cannot be null");
        }
    }
}
