using DataAccessLayer;
using ServiceLayer.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class TheaterJobPostingService
    {
        private readonly BroadwayBuilderContext _dbContext;

        public TheaterJobPostingService(BroadwayBuilderContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public void CreateTheaterJob(TheaterJobPosting theaterJob)
        {
            theaterJob.DateCreated = DateTime.UtcNow;
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

        public IEnumerable GetAllJobsFromTheater(int theaterid, int currentPage, int numberOfItems,out int count)
        {
            // Starting point of query to get data from the theater job posting table
            var startingPoint = numberOfItems * (currentPage - 1);

            var list = _dbContext.TheaterJobPostings
                // Order by newest data first
                .OrderByDescending(job => job.DateCreated)
                // Gets jobs for just a specific theater
                .Where(job => job.TheaterID == theaterid)
                // Select specific data for the response
                .Select(job => new
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
            count = list.Count;
            var paginatedList = list.Skip(startingPoint).Take(numberOfItems);
            return paginatedList;
        }

        public IEnumerable FilterTheaterJobPostingsFromTheater(int theaterId, string[] jobType, string[] Postion,int currentPage, int numberOfItems,out int count)
        {
            var startingPoint = numberOfItems * (currentPage - 1);
            var list = _dbContext.TheaterJobPostings
                .OrderByDescending(job=>job.DateCreated)
                .Where(job=>job.TheaterID == theaterId)
                .Where(job => jobType.Contains(job.JobType))
                .Where(job => Postion.Contains(job.Position))
                .Select(job => new
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
            count = list.Count;
            var paginatedList = list.Skip(startingPoint).Take(numberOfItems);
            return paginatedList;
        }

        public void UpdateTheaterJob(TheaterJobPosting updatedTheaterJob)
        {
            TheaterJobPosting theaterJobPostingToUpdate = _dbContext.TheaterJobPostings.Find(updatedTheaterJob.HelpWantedID);
            
            if (theaterJobPostingToUpdate!=null)
            {
                theaterJobPostingToUpdate.Position = updatedTheaterJob.Position;
                theaterJobPostingToUpdate.Description = updatedTheaterJob.Description;
                theaterJobPostingToUpdate.Title = updatedTheaterJob.Title;
                theaterJobPostingToUpdate.Hours = updatedTheaterJob.Hours;
                theaterJobPostingToUpdate.Requirements = updatedTheaterJob.Requirements;
                theaterJobPostingToUpdate.JobType = updatedTheaterJob.JobType;
            }
            else
            {
                throw new DbEntityNotFoundException("There is no record of the specified job posting in our records.");
            }


        }

        public void DeleteTheaterJob(int helpWantedId)
        {
            TheaterJobPosting jobToRemove = _dbContext.TheaterJobPostings.Find(helpWantedId);
            if (jobToRemove != null)
            {
                _dbContext.TheaterJobPostings.Remove(jobToRemove);
            }
            else
            {
                throw new DbEntityNotFoundException("There is no record of the specified job posting in our records.");
            }
        }
    }
}
