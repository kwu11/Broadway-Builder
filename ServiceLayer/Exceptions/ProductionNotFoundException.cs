using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Exceptions
{
    public class ProductionNotFoundException : Exception
    {
        public ProductionNotFoundException() : base() { }

        public ProductionNotFoundException(string message) : base(message) { }
    }
}
