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

        [Required]
        public int TheaterID { get; set; }
        
        public TheaterJobResponseModel()
        {
            this.Position = "";
            this.Description = "";
            this.Title = "";
            this.Hours = "";
            this.Requirements = "";
            this.JobType = "";
            this.DateCreated = DateTime.Now;
        }

        public TheaterJobResponseModel(int theaterID, string position, string description, string title, string hour, string requirement, string jobtype)
        {
            this.TheaterID = theaterID;
            this.Position = position;
            this.Description = description;
            this.Title = title;
            this.Hours = hour;
            this.Requirements = requirement;
            this.JobType = jobtype;
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
            this.DateCreated = theaterJobPosting.DateCreated;
        }

        public TheaterJobPosting GetTheaterJobPosting()
        {
            TheaterJobPosting theaterJobPosting = 
                new TheaterJobPosting(this.HelpWantedID,this.Position,
                this.Description,this.Title,this.Hours,this.Requirements,
                this.JobType);
            return theaterJobPosting;
        }
        
    }

    public class TheaterJobResponseList
    {
        public int count;
        public IEnumerable theaterJobList;

        public TheaterJobResponseList(int count, IEnumerable jobList)
        {
            this.count = count;
            theaterJobList = jobList;
        }
    }
}