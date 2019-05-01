using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Resume
    {
        [Key]
        public int ResumeID { get; set; }

        [Required]
        [Index(IsUnique =true)]
        public Guid ResumeGuid { get; set; }

        public int UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public User user { get; set; }
        ICollection<ResumeTheaterJob> resumeTheaterJobs { get; set; }

        public Resume()
        {

        }

        public Resume(int userid,Guid resumeGuid)
        {
            this.UserId = userid;
            this.ResumeGuid = resumeGuid;
        }
    }
}
