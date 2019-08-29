using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class LoginRequestModel
    {
        [Required]
        public string SSOUserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public long Timestamp { get; set; }
        [Required]
        public string Signature { get; set; }
    }
}