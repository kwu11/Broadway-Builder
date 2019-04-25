using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class ValidationResultModel
    {
        public bool ValidationSuccessful { get; set; }

        public HashSet<string> Reasons { get; set; }
    }
}