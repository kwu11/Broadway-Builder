using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ResumeTheaterJob
    {
        [Key]
        public int ResumeTheaterJobID { get; set; }
        public int HelpWantedID { get; set; }
        public int ResumeID { get; set; }
        [Required]
        public DateTime DateUploaded { get; set; }
        public TheaterJobPosting theaterJobPosting { get; set; }
        public Resume userResume { get; set; }
        

        public ResumeTheaterJob(int helpid, int resumeid) {
            this.HelpWantedID = helpid;
            this.ResumeID = resumeid;
        }

        public ResumeTheaterJob()
        {

        }
    }
}
