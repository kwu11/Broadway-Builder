using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class LogoutRequestModel
    {
        [Required]
        public string Signature { get; set; }
    }
}