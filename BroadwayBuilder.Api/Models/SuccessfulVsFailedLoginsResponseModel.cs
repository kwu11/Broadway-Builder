using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class SuccessfulVsFailedLoginsResponseModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int SuccessfulLogins { get; set; }
        public int FailedLogins { get; set; }
    }
}