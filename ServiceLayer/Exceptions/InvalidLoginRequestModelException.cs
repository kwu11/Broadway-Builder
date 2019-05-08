using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Exceptions
{
   public class InvalidLoginRequestModelException : Exception
    {
        public InvalidLoginRequestModelException() : base() { }

        public InvalidLoginRequestModelException(string message) : base(message) { }
    
    }
}
