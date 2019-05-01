using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Exceptions
{
    public class NullNotFoundException : Exception
    {
        public NullNotFoundException() { }
        public NullNotFoundException(string message) : base(message) { }
        public NullNotFoundException(string message, Exception innerException) : base(message,innerException) { }
    }

    public class ZeroAffectedRowsException:Exception
    {
        public ZeroAffectedRowsException() { }
        public ZeroAffectedRowsException(string message) : base(message) { }
    }

    public class DbEntityNotFoundException : Exception
    {
        public DbEntityNotFoundException() { }
        public DbEntityNotFoundException(string message) : base(message) { }
        public DbEntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
