using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace BroadwayBuilder.Api.Models
{
    public class ProductionResponseModel
    {
        // Info: productionId is nullable in code for data validation purposes.
        public int? ProductionID { get; set; }
        [Required]
        public string ProductionName { get; set; }
        [Required]
        public string DirectorFirstName { get; set; }
        [Required]
        public string DirectorLastName { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string StateProvince { get; set; }
        [Required] 
        public string Country { get; set; }
        [Required]
        public string Zipcode { get; set; }

        public int TheaterID { get; set; }

        public List<ProductionDateTimeResponseModel> DateTimes { get; set; }

    }
}