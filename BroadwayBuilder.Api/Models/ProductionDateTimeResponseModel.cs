using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class ProductionDateTimeResponseModel
    {
        
        public int ProductionDateTimeId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

    }
}