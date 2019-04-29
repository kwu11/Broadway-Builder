using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Exceptions
{
    class ProductionDateTimeNotFoundException : Exception
    {
        public ProductionDateTimeNotFoundException() : base() { }

        public ProductionDateTimeNotFoundException(string message) : base(message) { }
    }
}
