using DataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class TheaterJobService
    {
        private readonly BroadwayBuilderContext _dbContext;

        public TheaterJobService(BroadwayBuilderContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void CreateTheaterJob(TheaterJobPosting theaterJob)
        {
            theaterJob.DateCreated = DateTime.Now;
            _dbContext.TheaterJobPostings.Add(theaterJob);
        }

        public TheaterJobPosting GetTheaterJob(TheaterJobPosting theaterJob)
        {
            return _dbContext.TheaterJobPostings.Find(theaterJob.HelpWantedID);
        }

        public TheaterJobPosting GetTheaterJob(int helpwantedid)
        {
            return _dbContext.TheaterJobPostings.Find(helpwantedid);
        }

        public int GetTheaterJobsCount(int theaterid)
        {
            return _dbContext.TheaterJobPostings.Where(job => job.TheaterID == theaterid).Count();
            
        }


        public IEnumerable GetAllJobsFromTheater(int theaterid, int startingPoint, int numberOfItems)
        {
            return _dbContext.TheaterJobPostings.OrderByDescending(job => job.DateCreated).Where(job => job.TheaterID == theaterid).Skip(startingPoint).Take(numberOfItems).Select(job => new
            {
                Title = job.Title,
                Position = job.Position,
                JobType = job.JobType,
                Hours = job.Hours,
                Description = job.Description,
                Requirements = job.Requirements,
                DateCreated = job.DateCreated,
                HelpWantedId = job.HelpWantedID,
                TheaterId = job.TheaterID
            }).ToList();
        }

        public IEnumerable FilterTheaterJobPostingFromTheater(int theaterid,string title,string Postion,string Hours, string description, string requirements,DateTime date)
        {
            //IQueryable allJobsFromTheater = GetAllJobsForTheater(theaterid);
            var list = _dbContext.TheaterJobPostings.Where(job => job.TheaterID == theaterid)
                    .Select(job => new
                    {
                        Title = job.Title,
                        Position = job.Position,
                        Hours = job.Hours,
                        Description = job.Description,
                        Requirements = job.Requirements,
                        DateCreated = job.DateCreated,
                        HelpWantedId = job.HelpWantedID,
                        TheaterId = job.TheaterID
                    });
            if (String.IsNullOrEmpty(title))
            {
                list = list.Where(job=>job.Title == title);
            }
            if (String.IsNullOrEmpty(Postion))
            {
                list = list.Where(job => job.Position == Postion);
            }
            return list;
            
        }

        public void UpdateTheaterJob(TheaterJobPosting updatedTheaterJob)
        {
            _dbContext.Entry(updatedTheaterJob).State = EntityState.Modified;
        }

        public void DeleteTheaterJob(TheaterJobPosting theaterJob)
        {
            TheaterJobPosting jobToRemove = _dbContext.TheaterJobPostings.Find(theaterJob.HelpWantedID);
            if (jobToRemove != null)
            {
                _dbContext.TheaterJobPostings.Remove(jobToRemove);
            }
        }
    }
}
