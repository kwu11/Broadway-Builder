using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class TheaterUser
    {
        [Key]
        [Column(Order = 1)]
        public int TheaterID { get; set; }
        public Theater Theater { get; set; }
   
        [Key]
        [Column(Order = 2)]
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        public bool IsDisabled { get; set; }
    }
}
