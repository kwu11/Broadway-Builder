using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class NumberOfSuccessfulLoginsResponseModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int AverageSuccessfulLogins { get; set; }
        public int MinSuccessfulLogins { get; set; }
        public int MaxSuccessfulLogins { get; set; }
    }
}