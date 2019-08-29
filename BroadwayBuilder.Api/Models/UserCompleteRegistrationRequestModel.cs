using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class UserCompleteRegistrationRequestModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string StateProvince { get; set; }

        public string Country { get; set; }
    }
}