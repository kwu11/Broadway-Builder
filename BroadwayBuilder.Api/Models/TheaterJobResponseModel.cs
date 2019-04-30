using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BroadwayBuilder.Api.Models
{
    public class TheaterJobResponseModel
    {
        public int HelpWantedID { get; set; }
        
        public DateTime DateCreated { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Hours { get; set; }
        [Required]
        public string Requirements { get; set; }
        [Required]
        public string JobType { get; set; }
        public int TheaterID { get; set; }
        
        public TheaterJobResponseModel()
        {
            this.Position = "";
            this.Description = "";
            this.Title = "";
            this.Hours = "";
            this.Requirements = "";
            this.JobType = "";
        }

        public TheaterJobResponseModel(TheaterJobPosting theaterJobPosting)
        {
            this.HelpWantedID = theaterJobPosting.HelpWantedID;
            this.TheaterID = theaterJobPosting.TheaterID;
            this.Position = theaterJobPosting.Position;
            this.Description = theaterJobPosting.Description;
            this.Title = theaterJobPosting.Title;
            this.Hours = theaterJobPosting.Hours;
            this.Requirements = theaterJobPosting.Requirements;
            this.JobType = theaterJobPosting.JobType;
        }
        
    }
}