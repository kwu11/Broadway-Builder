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
using System.Configuration;
using Swashbuckle.Swagger.Annotations;

//get all job posting, edit job posting, delete job posting, create job posting
//http get, put, delete, post
namespace BroadwayBuilder.Api.Controllers
{
    [RoutePrefix("helpwanted")]
    public class HelpWantedController : ApiController
    {
        public HelpWantedController() { }

        /// <summary>
        /// Gets a specified number of theater jobs from a specific theater 
        /// </summary>
        /// <param name="theaterId">Unique theater ID of the specified theater</param>
        /// <param name="currentPage">The currentpage on the frontend</param>
        /// <param name="numberOfItems">The number of theater jobs that the frontend wants</param>
        /// <returns></returns>
        [HttpGet, Route("gettheaterjobs")]
        [SwaggerResponse((HttpStatusCode)200,"The total number of theater jobs and a list containing the specified number of theater jobs wanted.",typeof(TheaterJobResponseList))]
        public IHttpActionResult GetTheaterJobs(int theaterId, int currentPage, int numberOfItems)
        {
            using(var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    TheaterService theaterService = new TheaterService(dbcontext);
                    Theater theater = theaterService.GetTheaterByID(theaterId);
                    if (theater == null)//check if theater exists
                    {
                        throw new DbEntityNotFoundException("There is no record of that Theater in our database");
                    }
                    TheaterJobPostingService service = new TheaterJobPostingService(dbcontext);
                    int count = 0;
                    var list = service.GetAllJobsFromTheater(theaterId, currentPage, numberOfItems,out count);
                    
                    TheaterJobResponseList theaterJobResponseList = new TheaterJobResponseList(count, list);
                    return Content((HttpStatusCode)200, theaterJobResponseList);
                }
                catch (DbEntityNotFoundException e)
                {
                    return Content((HttpStatusCode)404, e.Message);
                }
                catch (Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
            }
        }

        /// <summary>
        /// Gets a specified number of filtered theater jobs from a specific theater 
        /// </summary>
        /// <param name="theaterId">Unique ID of the theater</param>
        /// <param name="jobType">Array of jobtypes to filter</param>
        /// <param name="position">Array of positions to filter</param>
        /// <param name="currentPage">The current page on the frontend</param>
        /// <param name="numberOfItems">The number of theater jobs that are requested</param>
        /// <returns></returns>
        [HttpGet, Route("getfilteredtheaterjobs")]
        [SwaggerResponse((HttpStatusCode)200, "The total number of filtered theater jobs and a list containing the specified number of filtered theater jobs wanted.", typeof(TheaterJobResponseList))]
        public IHttpActionResult GetFilteredTheaterJobs(int theaterId, [FromUri]string[]jobType, [FromUri] string[]position, int currentPage,int numberOfItems)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    TheaterService theaterService = new TheaterService(dbcontext);
                    Theater theater = theaterService.GetTheaterByID(theaterId);
                    if (theater == null)
                    {
                        throw new DbEntityNotFoundException("There is no record of that Theater in our database");
                    }
                    TheaterJobPostingService service = new TheaterJobPostingService(dbcontext);
                    int count = 0;
                    var list = service.FilterTheaterJobPostingsFromTheater(theaterId, jobType, position,currentPage,numberOfItems,out count);
                    TheaterJobResponseList theaterJobResponseList = new TheaterJobResponseList(count, list);
                    return Content((HttpStatusCode)200, theaterJobResponseList);
                }
                catch(DbEntityNotFoundException dbEntityNotFoundException)
                {
                    return Content((HttpStatusCode)404, dbEntityNotFoundException.Message);
                }
                catch(Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
            }
        }

        /// <summary>
        /// Updates a specific theater job
        /// </summary>
        /// <param name="job">theater job that needs to be updated in the database</param>
        /// <returns></returns>
        [HttpPut,Route("edittheaterjob")]
        [SwaggerResponse((HttpStatusCode)202, "A string response saying that the theater job was successfully updated")]
        public IHttpActionResult EditTheaterJob([FromBody]TheaterJobPosting job) 
        {
            using(var dbContext = new BroadwayBuilderContext())
            {
                try
                {
                    //TheaterJobPosting job = jobRequest.GetTheaterJobPosting();
                    TheaterJobPostingService service = new TheaterJobPostingService(dbContext);
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
                        return Content((HttpStatusCode)404, "No such posting exists");//need to edit 
                    }
                }
                //TODO: Log errors - stacktrace, message, source, TheaterJob object
                catch (ZeroAffectedRowsException zeroAffectedRowsException)//custom exception
                {
                    return Content((HttpStatusCode)500, "There appears to be no changes detected. The Theater Job was not updated");
                }
                catch(DbEntityNotFoundException dbEntityNotFoundException)
                {
                    return Content((HttpStatusCode)404, dbEntityNotFoundException.Message);
                }
                catch (DbEntityValidationException dbEntityValidationException)//save was aborted because validation of the entity property values failed
                {
                    return Content((HttpStatusCode)500, "Theater Job could not be updated");
                }
                catch(DbUpdateConcurrencyException dbUpdateConcurrencyException)//concurrency violation; row has been changed in the database since it was queried
                {
                    return Content((HttpStatusCode)500,"An Error has occurred while trying to update the requested theater job posting. Please try again.");
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

        /// <summary>
        /// Deletes the specified theater job from the database
        /// </summary>
        /// <param name="helpWantedId">Unique ID of the theater job</param>
        /// <returns></returns>
        [HttpDelete, Route("deletetheaterjob")]
        [SwaggerResponse((HttpStatusCode)202, "A string response saying that the theater job was successfully deleted")]
        public IHttpActionResult DeleteTheaterJob(int helpWantedId)
        {
            using (var dbContext = new BroadwayBuilderContext())
            {
                TheaterJobPostingService service = new TheaterJobPostingService(dbContext);
                //TheaterJobPosting job = service.GetTheaterJob(helpWantedId);
                try
                {
                    service.DeleteTheaterJob(helpWantedId);
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
                catch (DbEntityNotFoundException dbEntityNotFoundException)
                {
                    return Content((HttpStatusCode)404, dbEntityNotFoundException.Message);
                }
                catch (DbEntityValidationException dbEntityValidationException)
                {
                    return Content((HttpStatusCode)500, "Unable to delete the job posting");
                }
                catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)//concurrency violation; row has been changed in the database since it was queried
                {
                    return Content((HttpStatusCode)500, "An Error has occurred while trying to delete the requested theater job posting. Please try again.");
                }
                catch (DbUpdateException dbUpdateException)
                {
                    return Content((HttpStatusCode)500, "Failed to delete the requested theater job posting.");
                }
                catch (Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
            }
        }

        /// <summary>
        /// Creates a theater job in the database 
        /// </summary>
        /// <param name="theaterJob">The theater job to be created</param>
        /// <returns></returns>
        [HttpPost,Route("createtheaterjob")]
        [SwaggerResponse((HttpStatusCode)201, "The created theater job object")]
        public IHttpActionResult CreateTheaterJob([FromBody]TheaterJobPosting theaterJob)
        {
            using(var dbContext = new BroadwayBuilderContext())
            {
                TheaterJobPostingService jobService = new TheaterJobPostingService(dbContext);
                try
                {
                    //TheaterJobPosting theaterJob = jobRequest.GetTheaterJobPosting();
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
                    TheaterJobResponseModel theaterJobResponseModel = new TheaterJobResponseModel(theaterJob);
                    return Content((HttpStatusCode)201, theaterJobResponseModel);
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

        /// <summary>
        /// Uploads a resume of extension pdf onto the server
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                if(httpRequest.Files.Count > 1 || httpRequest.Files.Count<=0)
                {
                    return Content((HttpStatusCode)406, "Only one file is allowed to be submitted");
                }
                // Grab current file of the request
                var postedFile = httpRequest.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    string extension = Path.GetExtension(postedFile.FileName).ToLower();//get extension of the file
                    if (!allowedFileExtension.Contains(extension))
                    {
                        return Content((HttpStatusCode)406, "That file extension is not allowed to be submitted");
                    }
                    else if(postedFile.ContentLength > maxContentLength)
                    {
                        return Content((HttpStatusCode)406, "That file is too large.");
                    }
                    else
                    {
                        using (var dbContext = new BroadwayBuilderContext())
                        {
                            var userService = new UserService(dbContext);
                            User user = userService.GetUser(userId);
                            if (user == null)//check if user exists
                            {
                                throw new DbEntityNotFoundException("User does not exist");
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
                                    throw new ZeroAffectedRowsException("Failed to add a resume");
                                }
                                resume = userResume;
                            }
                            var subdir = Path.Combine(ConfigurationManager.AppSettings["ResumeDir"], (resume.ResumeGuid.ToString() + "/")); //@"C:\Resumes\"+resume.ResumeGuid;
                            var filePath = Path.Combine(subdir, resume.ResumeGuid.ToString()+".pdf");// subdir+@"\"+resume.ResumeGuid+".pdf";

                            if (!Directory.Exists(subdir))
                            {
                                Directory.CreateDirectory(subdir);
                            }

                            postedFile.SaveAs(filePath);
                            return Content((HttpStatusCode)200, "File Uploaded");
                        }
                        
                    }
                }
                return Content((HttpStatusCode)400, "No file was detected.");
            }
            catch(ZeroAffectedRowsException e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch(DbEntityNotFoundException e)
            {
                return Content((HttpStatusCode)404, e.Message);
            }
            catch(HttpException e)//HttpPostedFile.SaveAs exception
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch(IOException e)//Exception thrown when creating directory
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch (DbUpdateException e)//exception thrown while saving the database
            {
                return Content((HttpStatusCode)500, e.Message);
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                return Content((HttpStatusCode)500, "Unable to delete the job posting");
            }
            catch (Exception e)
            {
                return Content((HttpStatusCode)400, e.Message);
            }
        }

        /// <summary>
        /// Retrieves a resume file of the user
        /// </summary>
        /// <param name="userId">Unique ID of the user</param>
        /// <returns></returns>
        [HttpGet,Route("myresume")]
        [SwaggerResponse((HttpStatusCode)200,"The uri of the resume that is on the server")]
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
                        throw new DbEntityNotFoundException("User does not exist");
                    }
                    var resumeService = new ResumeService(dbContext);
                    Resume resume = resumeService.GetResumeByUserID(userId);
                    if (resume == null)//check if user has already submitted a resume
                    {
                        throw new DbEntityNotFoundException("No resume on file");
                    }
                    var subdir = Path.Combine(ConfigurationManager.AppSettings["ResumeDir"], resume.ResumeGuid.ToString()); 
                    var filePath = Path.Combine(subdir, (resume.ResumeGuid.ToString()+"/"), ".pdf");
                    //var filepath = @"C:\Resumes\" + resume.ResumeGuid + @"\" + resume.ResumeGuid + ".pdf";
                    string url = "";
                    if (File.Exists(filePath))
                    {
                        url = "https://api.broadwaybuilder.xyz/Resumes/"+ resume.ResumeGuid + "/" + resume.ResumeGuid + ".pdf";
                        return Content((HttpStatusCode)200, url);
                    }
                    throw new Exception("No resume on file");
                }
            }
            catch(DbEntityNotFoundException e)
            {
                return Content((HttpStatusCode)404, e.Message);
            }
            catch(Exception e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
        }

        /// <summary>
        /// A user applies to a posted theater job by using a resume that is already on the server.
        /// </summary>
        /// <param name="id">Unique ID of the user</param>
        /// <param name="helpwantedid">Unique ID of the theater job</param>
        /// <returns></returns>
        [HttpPost,Route("apply")] //apply to a theater job
        [SwaggerResponse((HttpStatusCode)200,"A string status message.")]
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
                        throw new DbEntityNotFoundException("No resume on file");
                    }
                    var theaterJobService = new TheaterJobPostingService(dbContext);
                    TheaterJobPosting job = theaterJobService.GetTheaterJob(helpwantedid);
                    if (job == null)//check if job exists
                    {
                        throw new DbEntityNotFoundException("No job on file");
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
            catch(DbEntityNotFoundException e)
            {
                return Content((HttpStatusCode)404, e.Message);
            }
            catch(Exception e)
            {
                return Content((HttpStatusCode)400, e.Message);
            }
        }

        /// <summary>
        /// Gets all the resume uris' for a theater job 
        /// </summary>
        /// <param name="helpwantedId"></param>
        /// <returns></returns>
        [HttpGet,Route("getresumesforjob")] //get all resumes for a single job posting
        public IHttpActionResult GetResumesforTheaterJob(int helpwantedId)
        {
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var theaterJobService = new TheaterJobPostingService(dbContext);
                    if (theaterJobService.GetTheaterJob(helpwantedId) == null)
                    {
                        throw new DbEntityNotFoundException("Theater job does not exist");
                    }
                    var resumeTheaterJobService = new ResumeTheaterJobService(dbContext);
                    var resumeList = resumeTheaterJobService.GetAllResumeGuidsForTheaterJob(helpwantedId);
                    List<string> urlList = new List<string>();
                    foreach (Guid guid in resumeList)
                    {
                        string path = Path.Combine(ConfigurationManager.AppSettings["ResumeDir"], guid.ToString(), guid.ToString() + ".pdf");//@"C:\Resumes\" + guid + @"/" + guid + ".pdf";
                        if (File.Exists(path))
                        {
                            string url = "https://api.broadwaybuilder.xyz/Resumes/" + guid + "/" + guid + ".pdf";
                            urlList.Add(url);
                        }
                        
                    }
                    return Content((HttpStatusCode)200, urlList);
                }
            }
            catch(DbEntityNotFoundException e)
            {
                return Content((HttpStatusCode)404, e.Message);
            }
            catch (Exception e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
        }

    }
}
