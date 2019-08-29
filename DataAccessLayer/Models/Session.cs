using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Session
    {
        public static readonly int MINUTES_UNTIL_EXPIRATION = 30;

        [Required]
        public string Token { get; set; }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string Signature { get; set; }

        #region -- Relationship --

        [Required]
        public int UserId { get; set; }

        public User User { get; set; }

        #endregion
    }
}
