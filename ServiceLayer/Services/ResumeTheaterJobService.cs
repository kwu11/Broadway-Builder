using DataAccessLayer;
using System.Linq;
using DataAccessLayer.Models;
using System.Data.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ResumeTheaterJobService
    {
        private readonly BroadwayBuilderContext _dbContext;

        public ResumeTheaterJobService(BroadwayBuilderContext context)
        {
            this._dbContext = context;
        }

        public void CreateResumeTheaterJob(ResumeTheaterJob resumeTheaterJob)
        {
            ResumeTheaterJob checkExistence = _dbContext.ResumeTheaterJobs.Where(job => job.HelpWantedID == resumeTheaterJob.HelpWantedID && job.ResumeID == resumeTheaterJob.ResumeID).FirstOrDefault<ResumeTheaterJob>();
            if (checkExistence != null)
            {
                throw new Exception("User has already applied to this job posting");
            }
            resumeTheaterJob.DateUploaded = DateTime.UtcNow;
            this._dbContext.ResumeTheaterJobs.Add(resumeTheaterJob);
        }

        public IEnumerable GetAllResumeForTheater(int theaterID)
        {
            return _dbContext.ResumeTheaterJobs.Include(job => job.theaterJobPosting)
                .Where(job => job.theaterJobPosting.TheaterID == theaterID)
                .Select(submittedResumes => new
                {
                    HelpWantedID = submittedResumes.HelpWantedID,
                    ResumeID = submittedResumes.ResumeID,
                    DateUploaded = submittedResumes.DateUploaded
                }).ToList();
        }

        public IEnumerable GetAllResumesByHelpWantedID(int helpwantedid)
        {
            return _dbContext.ResumeTheaterJobs.Where(job=>job.HelpWantedID==helpwantedid)
                .Select(submittedResumes => new
            {
                HelpWantedID = submittedResumes.HelpWantedID,
                ResumeID = submittedResumes.ResumeID,
                DateUploaded = submittedResumes.DateUploaded
            }).ToList();
        }

        public IEnumerable<Guid> GetAllResumeGuidsTheater(int theaterid)
        {
            return _dbContext.ResumeTheaterJobs.Include(job => job.userResume).Include(job => job.theaterJobPosting)
                .Where(job => job.theaterJobPosting.TheaterID == theaterid).Select(job => job.userResume.ResumeGuid).ToList<Guid>();
        }

        public IEnumerable<Guid> GetAllResumeGuidsForTheaterJob(int helpwantedid)
        {
            return _dbContext.ResumeTheaterJobs.Include(job => job.userResume)
                .Where(job => job.HelpWantedID == helpwantedid).Select(job => job.userResume.ResumeGuid).ToList<Guid>();
        }

        public void DeleteResumeTheaterJob(ResumeTheaterJob resumeTheaterJob)
        {
            ResumeTheaterJob deleteResumeJob = _dbContext.ResumeTheaterJobs.Find(resumeTheaterJob.ResumeTheaterJobID);
            if (deleteResumeJob != null)
            {
                _dbContext.ResumeTheaterJobs.Remove(deleteResumeJob);
            }
        }
    }
}
