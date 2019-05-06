using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class AverageSessionLengthResponseModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int AverageSessionLength { get; set; }
        public int MinSessionLength { get; set; }
        public int MaxSessionLength { get; set; }
    }
}