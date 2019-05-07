using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Exceptions
{
   public class InvalidGuidException : Exception
    {
        public InvalidGuidException() : base() { }

        public InvalidGuidException(string message) : base(message) { }
    }
}
