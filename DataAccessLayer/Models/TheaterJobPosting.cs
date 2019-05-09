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
    public class TheaterJobPosting
    {
        [Key]
        public int HelpWantedID { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        [StringLength(20)]
        public string Position { get; set; }
        [Required]
        [StringLength(3000)]
        public string Description { get; set; }
        [Required]
        [StringLength(70)]
        public string Title { get; set; }
        [Required]
        [StringLength(20)]
        public string Hours { get; set; }
        [Required]
        [StringLength(1500)]
        public string Requirements { get; set; }
        [Required]
        [StringLength(20)]
        public string JobType { get; set; }
        [Required]
        public int TheaterID { get; set; }
        public Theater theater { get; set; }

        ICollection<ResumeTheaterJob> resumeTheaterJobs { get; set; }

        public TheaterJobPosting( int theaterID, string position, string description, string title, string hour, string requirement, string jobtype)
        {
            this.TheaterID = theaterID;
            this.Position = position;
            this.Description = description;
            this.Title = title;
            this.Hours = hour;
            this.Requirements = requirement;
            this.JobType = jobtype;
        }
        public TheaterJobPosting()
        {

        }
    }
}
