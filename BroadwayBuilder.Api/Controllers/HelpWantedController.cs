using BroadwayBuilder.Api.Models;
using DataAccessLayer;
using DataAccessLayer.Models;
using ServiceLayer.Exceptions;
using ServiceLayer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

//get all job posting, edit job posting, delete job posting, create job posting
//http get, put, delete, post
namespace BroadwayBuilder.Api.Controllers
{
    [RoutePrefix("helpwanted")]
    public class HelpWantedController : ApiController
    {
        public HelpWantedController() { }

        [HttpGet, Route("length")]
        public IHttpActionResult GetTheaterJobsCount(int theaterid)
        {
            using (var dbContext = new BroadwayBuilderContext())
            {
                try
                {
                    HelpWantedService service = new HelpWantedService(dbContext);
                    return Content((HttpStatusCode)200, service.GetTheaterJobsCount(theaterid));
                }
                catch
                {
                    return Content((HttpStatusCode)404, "Unable to get count of job postings for theater " + theaterid);
                }
            }
        }

        [HttpGet, Route("gettheaterjobs")]
        public IHttpActionResult GetTheaterJobs(int theaterId, int currentPage, int numberOfItems)//needs to be changed to string for encryption purposes
        {
            using(var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    HelpWantedService service = new HelpWantedService(dbcontext);
                    int count = 0;
                    var list = service.GetAllJobsFromTheater(theaterId, currentPage, numberOfItems,out count);
                    if(list == null)
                    {
                        throw new NullNotFoundException();
                    }
                    
                    return Content((HttpStatusCode)200, (new { Count = count,JobPosting = list}));
                }
                catch (NullNotFoundException)
                {
                    return Content((HttpStatusCode)404, "Unable to find any jobs related to that Theater");
                }
                catch (Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
            }
        }

        [HttpGet, Route("getfilteredtheaterjobs")]
        public IHttpActionResult GetFilteredTheaterJobs(int theaterId, [FromUri]string[]jobType, [FromUri] string[]position, int currentPage,int numberOfItems)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    HelpWantedService service = new HelpWantedService(dbcontext);
                    int count = 0;
                    var list = service.FilterTheaterJobPostingsFromTheater(theaterId, jobType, position,currentPage,numberOfItems,out count);
                    return Content((HttpStatusCode)200, new {Count = count,JobPostings = list });
                }
                catch(Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
            }
        }

        [HttpPut,Route("edittheaterjob")]
        public IHttpActionResult EditTheaterJob([FromBody]TheaterJobPosting job) 
        {
            using(var dbContext = new BroadwayBuilderContext())
            {
                try
                {
                    HelpWantedService service = new HelpWantedService(dbContext);
                    //TheaterJobPosting job = service.GetTheaterJob(helpwantedid);
                    if (job != null)
                    {
                        service.UpdateTheaterJob(job);
                        var results = dbContext.SaveChanges();
                        if (results > 0)
                        {
                            return Content((HttpStatusCode)202, "Updated Job Posting");//not sure to return object or just string response
                        }

                        throw new ZeroAffectedRowsException();
                    }
                    else
                    {
                        return Content((HttpStatusCode)500, "NO such posting exists");//need to edit 
                    }
                }
                //TODO: Log errors - stacktrace, message, source, TheaterJob object
                catch (ZeroAffectedRowsException zeroAffectedRowsException)//custom exception
                {
                    return Content((HttpStatusCode)500, "There appears to be no changes detected. The Theater Job was not updated");
                }
                catch (DbEntityValidationException)//save was aborted because validation of the entity property values failed
                {
                    return Content((HttpStatusCode)500, "Theater Job could not be updated");
                }
                catch(DbUpdateConcurrencyException dbUpdateConcurrencyException)//concurrency violation; row has been changed in the database since it was queried
                {
                    return Content((HttpStatusCode)500,"An Error has occurred while trying to update the requested theater job posting");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    return Content((HttpStatusCode)500, "Failed to update the requested theater job posting.");
                }
                catch (Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
                
            }
        }

        [HttpDelete, Route("deletetheaterjob")]
        public IHttpActionResult DeleteTheaterJob(int helpWantedId)
        {
            using (var dbContext = new BroadwayBuilderContext())
            {
                HelpWantedService service = new HelpWantedService(dbContext);
                TheaterJobPosting job = service.GetTheaterJob(helpWantedId);
                try
                {
                    service.DeleteTheaterJob(job);
                    if (job == null)
                    {
                        return Content((HttpStatusCode)404, "Job Listing does not exist in the database");
                    }
                    var results = dbContext.SaveChanges();
                    if (results > 0)
                    {
                        return Content((HttpStatusCode)202, "Successfully Deleted Job Posting");
                    }
                    else
                    {
                        throw new ZeroAffectedRowsException();
                    }
                }
                catch (ZeroAffectedRowsException)
                {
                    return Content((HttpStatusCode)500, "There appears to be no changes made in the database. The job posting wasn't deleted");
                }
                catch (DbEntityValidationException)
                {
                    return Content((HttpStatusCode)500, "Unable to delete the job posting");
                }
                catch (Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
            }
        }

        [HttpPost,Route("createtheaterjob")]
        public IHttpActionResult CreateTheaterJob([FromBody]TheaterJobPosting theaterJob)
        {
            using(var dbContext = new BroadwayBuilderContext())
            {
                HelpWantedService jobService = new HelpWantedService(dbContext);
                try
                {
                    if (theaterJob == null)
                    {
                        return Content((HttpStatusCode)400, "That job posting does not exist");
                    }
                    jobService.CreateTheaterJob(theaterJob);
                    var results = dbContext.SaveChanges();
                    if (results <= 0)
                    {
                        throw new ZeroAffectedRowsException();
                    }
                    return Content((HttpStatusCode)201, theaterJob);
                    //return Content((HttpStatusCode)201, "Theater Job Posting Created");
                }
                catch (ZeroAffectedRowsException)
                {
                    return Content((HttpStatusCode)500, "There appears to be no additions made. The Job posting was not created");
                }
                catch (DbEntityValidationException)
                {
                    return Content((HttpStatusCode)500,"Unable to create the requested job posting");
                }
                catch(Exception e)//needs to be updated
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
            }
        }

        [HttpPost, Route("createproductionjob")]
        public IHttpActionResult CreateProductionJob([FromBody]ProductionJobPosting productionJob)
        {
            using (var dbContext = new BroadwayBuilderContext())
            {
                ProductionJobService jobService = new ProductionJobService(dbContext);
                try
                {
                    if (productionJob == null)
                    {
                        return Content((HttpStatusCode)404,"The job posting does not exist");
                    }
                    jobService.CreateProductionJob(productionJob);
                    var results = dbContext.SaveChanges();
                    if (results > 0)
                    {
                        return Content((HttpStatusCode)201, "Production Job Posting Created");
                    }
                    else
                    {
                        throw new ZeroAffectedRowsException();
                    }
                }
                catch (ZeroAffectedRowsException)
                {
                    return Content((HttpStatusCode)500, "There appears to be no changes made. The job posting was not created");
                }
                catch (DbEntityValidationException)
                {
                    return Content((HttpStatusCode)500,"Unable to created the requested job posting");
                }
                catch (Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
            }
        }

        [HttpPut,Route("uploadresume")]
        public IHttpActionResult UploadResume(int userId)
        {
            //A list in case we want to accept more than one file type
            IList<string> allowedFileExtension = new List<string> { ".pdf" };
            
            // Max file size is 1MB
            const int maxContentLength = 1024 * 1024 * 1;

            try
            {
                //get the content, headers, etc the full request of the current http request
                var httpRequest = HttpContext.Current.Request;
                if(httpRequest.Files.Count > 1)
                {
                    return Content((HttpStatusCode)401, "Only one file is allowed to be submitted");
                }
                // Grab current file of the request
                var postedFile = httpRequest.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(postedFile.FileName).ToLower();//get extension of the file
                    if (!allowedFileExtension.Contains(extension))
                    {
                        throw new Exception(extension);
                    }
                    else if(postedFile.ContentLength > maxContentLength)
                    {
                        throw new Exception("File Exceeds file limit");
                    }
                    else
                    {
                        using (var dbContext = new BroadwayBuilderContext())
                        {
                            var userService = new UserService(dbContext);
                            User user = userService.GetUser(userId);
                            if (user == null)//check if user exists
                            {
                                throw new Exception("User does not exist");
                            }
                            var resumeService = new ResumeService(dbContext);
                            Resume resume = resumeService.GetResumeByUserID(userId);
                            if (resume == null)//check if user has already submitted a resume
                            {
                                Resume userResume = new Resume(userId, Guid.NewGuid());
                                resumeService.CreateResume(userResume);
                                var result = dbContext.SaveChanges();
                                if(result <= 0)
                                {
                                    throw new Exception("failed to add a resume");
                                }
                                resume = userResume;
                            }
                            var subdir = @"C:\Resumes\"+resume.ResumeGuid;
                            var filePath = subdir+@"\"+resume.ResumeGuid+".pdf";

                            if (!Directory.Exists(subdir))
                            {
                                Directory.CreateDirectory(subdir);
                            }

                            postedFile.SaveAs(filePath);
                            return Content((HttpStatusCode)200, "File Uploaded");
                        }
                        
                    }
                }
                throw new Exception("something went wrong");
            }
            catch(HttpException e)//HttpPostedFile.SaveAs exception
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch(IOException e)//Exception thrown when creating directory
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch(DbUpdateException e)//exception thrown while saving the database
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch(Exception e)
            {
                return Content((HttpStatusCode)400, e.Message);
            }
        }

        [HttpGet,Route("myresume")]
        public IHttpActionResult GetResume(int userId)
        {
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var userService = new UserService(dbContext);
                    User user = userService.GetUser(userId);
                    if (user == null)//check if user exists
                    {
                        throw new Exception("User does not exist");
                    }
                    var resumeService = new ResumeService(dbContext);
                    Resume resume = resumeService.GetResumeByUserID(userId);
                    if (resume == null)//check if user has already submitted a resume
                    {
                        throw new Exception("No resume on file");
                    }
                    var filepath = @"C:\Resumes\" + resume.ResumeGuid + @"\" + resume.ResumeGuid + ".pdf";
                    string url = "";
                    if (File.Exists(filepath))
                    {
                        url = "https://api.broadwaybuilder.xyz/Resumes/"+ resume.ResumeGuid + "/" + resume.ResumeGuid + ".pdf";
                        return Content((HttpStatusCode)200, url);
                    }
                    throw new Exception("No resume on file");
                }
            }
            catch(Exception e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
        }

        [HttpPost,Route("apply")] //apply to a theater job
        public IHttpActionResult ApplyToJob([FromUri]int id,[FromUri]int helpwantedid)
        {
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var resumeService = new ResumeService(dbContext);
                    Resume resume = resumeService.GetResumeByUserID(id);
                    if (resume == null)//check if user has already submitted a resume
                    {
                        throw new Exception("No resume on file");
                    }
                    var theaterJobService = new HelpWantedService(dbContext);
                    TheaterJobPosting job = theaterJobService.GetTheaterJob(helpwantedid);
                    if (job == null)//check if job exists
                    {
                        throw new Exception("No job on file");
                    }

                    var resumeJobPosting = new ResumeTheaterJob(job.HelpWantedID,resume.ResumeID);
                    var resumeJobService = new ResumeTheaterJobService(dbContext);
                    resumeJobService.CreateResumeTheaterJob(resumeJobPosting);
                    var result = dbContext.SaveChanges();
                    if (result > 0)
                    {
                        return Content((HttpStatusCode)200, "Successfully Applied!");
                    }
                    return Content((HttpStatusCode)500, "Wasn't able to successfully apply");
                }
            }
            catch(Exception e)
            {
                return Content((HttpStatusCode)400, e.Message);
            }
        }

        [HttpGet,Route("getResume")] // all resumes submitted to the theater -- dont recommend this
        public IHttpActionResult GetResumesForAllTheaterJobs(int theaterid)
        {
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var theaterService = new TheaterService(dbcontext);
                    if (theaterService.GetTheaterByID(theaterid) == null)
                    {
                        throw new Exception("theater does not exist");
                    }
                    var resumetheaterjobservice = new ResumeTheaterJobService(dbcontext);
                    var resumelist = resumetheaterjobservice.GetAllResumeGuidsTheater(theaterid);
                    List<string> urlList = null;
                    foreach(Guid guid in resumelist)
                    {
                        string url = "https://api.broadwaybuilder.xyz/Resumes/" + guid + "/" + guid + ".pdf";
                        urlList.Add(url);
                    }
                    return Content((HttpStatusCode)200, urlList);
                }
            }
            catch(Exception e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
        }

        [HttpGet,Route("getresumesforjob")] //get all resumes for a single job posting
        public IHttpActionResult GetResumesforTheaterJob(int helpwantedId)
        {
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var theaterJobService = new HelpWantedService(dbContext);
                    if (theaterJobService.GetTheaterJob(helpwantedId) == null)
                    {
                        throw new Exception("Theater job does not exist");
                    }
                    var resumeTheaterJobService = new ResumeTheaterJobService(dbContext);
                    var resumeList = resumeTheaterJobService.GetAllResumeGuidsForTheaterJob(helpwantedId);
                    List<string> urlList = new List<string>();
                    foreach (Guid guid in resumeList)
                    {
                        string path = @"C:\Resumes\" + guid + @"/" + guid + ".pdf";
                        if (File.Exists(path))
                        {
                            string url = "https://api.broadwaybuilder.xyz/Resumes/" + guid + "/" + guid + ".pdf";
                            urlList.Add(url);
                        }
                        
                    }
                    return Content((HttpStatusCode)200, urlList);
                }
            }
            catch (Exception e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
        }

    }
}
