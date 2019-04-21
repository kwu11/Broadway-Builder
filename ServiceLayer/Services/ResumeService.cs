using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ResumeService
    {
        private readonly BroadwayBuilderContext _dbContext;

        public ResumeService(BroadwayBuilderContext context)
        {
            this._dbContext = context;
        }

        public void CreateResume(Resume resume)
        {
            resume.DateCreated = DateTime.Now;
            _dbContext.Resumes.Add(resume);
        }

        public Resume GetResumeById(int resumeID)
        {
            return _dbContext.Resumes.Find(resumeID);
        }

        public Resume GetResumeByUserID(int userid)
        {
            return _dbContext.Resumes.Where(resume => resume.UserId == userid).FirstOrDefault();
        }

        public void DeleteResume(Resume resume)
        {
            Resume Findresume = _dbContext.Resumes.Find(resume.ResumeID);
            if (Findresume != null)
            {
                _dbContext.Resumes.Remove(Findresume);
            }
        }

        public Resume UpdateResume(Resume resume)
        {
            Resume Findresume = _dbContext.Resumes.Find(resume.ResumeID);
            if (Findresume != null)
            {
                Findresume.ResumeGuid = resume.ResumeGuid;
            }

            return Findresume;
        }

        public Guid GetGuid(int resumeid)
        {
            return _dbContext.Resumes.Where(resume => resume.ResumeID == resumeid).Select(resume => resume.ResumeGuid).FirstOrDefault<Guid>();
        }

    }
}
