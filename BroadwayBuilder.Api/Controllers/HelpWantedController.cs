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

namespace BroadwayBuilder.Api.Controllers
{
    [RoutePrefix("helpwanted")]
    public class HelpWantedController : ApiController
    {
        //public HelpWantedController() { }

        /// <summary>
        /// Gets a specified number of theater jobs from a specific theater 
        /// </summary>
        /// <param name="theaterId">Unique theater ID of the specified theater</param>
        /// <param name="currentPage">The currentpage on the frontend</param>
        /// <param name="numberOfItems">The number of theater jobs that the frontend wants</param>
        /// <returns></returns>
        [HttpGet, Route("gettheaterjobs")]
        [SwaggerResponse((HttpStatusCode)200,"Gets total number of theater jobs and a list containing the specified number of theater jobs wanted.",typeof(TheaterJobResponseList))]
        public IHttpActionResult GetTheaterJobs(int theaterId, int currentPage, int numberOfItems)
        {
            using(var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    TheaterService theaterService = new TheaterService(dbcontext);
                    Theater theater = theaterService.GetTheaterByID(theaterId);
                    if (theater == null)//check if theater exists; null if there is no record in the DB
                    {
                        //throw new DbEntityNotFoundException("There is no record of that Theater in our database");
                        //use Stacktrace class then log?
                        return Content((HttpStatusCode)404, "There is no record of that Theater in our database");
                    }
                    TheaterJobPostingService service = new TheaterJobPostingService(dbcontext);
                    int count = 0; //variable to be used for GetAllJobsFromTheater to get the count of Theater job postings found from query 
                    var list = service.GetAllJobsFromTheater(theaterId, currentPage, numberOfItems,out count);
                    
                    TheaterJobResponseList theaterJobResponseList = new TheaterJobResponseList(count, list); //create a response model containing the count and List of theater jobs to reduce JSON Hijacking
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
                         return Content((HttpStatusCode)404, "There is no record of that Theater in our database");
                    }
                    TheaterJobPostingService service = new TheaterJobPostingService(dbcontext);
                    int count = 0;
                    var list = service.FilterTheaterJobPostingsFromTheater(theaterId, jobType, position,currentPage,numberOfItems,out count);
                    TheaterJobResponseList theaterJobResponseList = new TheaterJobResponseList(count, list);
                    return Content((HttpStatusCode)200, theaterJobResponseList);
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
        [SwaggerResponse((HttpStatusCode)200, "A string response saying that the theater job was successfully updated")]
        public IHttpActionResult EditTheaterJob([FromBody]TheaterJobPosting job) 
        {
            using(var dbContext = new BroadwayBuilderContext())
            {
                try
                {
                    TheaterJobPostingService service = new TheaterJobPostingService(dbContext);
                    if (job != null)
                    {
                        service.UpdateTheaterJob(job);
                        var results = dbContext.SaveChanges();
                        if (results > 0)
                        {
                            return Content((HttpStatusCode)200, "Updated Job Posting");
                        }

                        //throw new ZeroAffectedRowsException();
                        return Content((HttpStatusCode)500, "There appears to be no changes detected. The Theater Job was not updated");
                    }
                    else
                    {
                        return Content((HttpStatusCode)400, "No such posting exists");
                    }
                }
                //TODO: Log errors - stacktrace, message, source, TheaterJob object
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
        [SwaggerResponse((HttpStatusCode)200, "A string response saying that the theater job was successfully deleted")]
        public IHttpActionResult DeleteTheaterJob(int helpWantedId)
        {
            using (var dbContext = new BroadwayBuilderContext())
            {
                TheaterJobPostingService service = new TheaterJobPostingService(dbContext);
                try
                {
                    service.DeleteTheaterJob(helpWantedId);
                    var results = dbContext.SaveChanges();
                    if (results > 0)
                    {
                        return Content((HttpStatusCode)200, "Successfully Deleted Job Posting");
                    }
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
                    if (theaterJob == null)
                    {
                        return Content((HttpStatusCode)400, "That job posting does not exist");
                    }
                    jobService.CreateTheaterJob(theaterJob);
                    var results = dbContext.SaveChanges();
                    if (results <= 0)
                    {
                        return Content((HttpStatusCode)500, "There appears to be no additions made. The Job posting was not created");
                    }
                    TheaterJobResponseModel theaterJobResponseModel = new TheaterJobResponseModel(theaterJob);
                    return Content((HttpStatusCode)201, theaterJobResponseModel);
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
            List<string> allowedFileExtension = new List<string> { ".pdf" };
            //Business Rule - only one file allowed to submit
            int maxFileCount = 1;
            // Max file size is 1MB
            const int maxContentLength = 1024 * 1024 * 1;

            try
            {
                //get the content, headers, etc the full request of the current http request
                var httpRequest = HttpContext.Current.Request;
                var fileValidator = new FileValidator();
                //Validate the submitted file to verify that it complies with Business Rules
                var validationResult = fileValidator.ValidateFiles(httpRequest.Files, allowedFileExtension, maxContentLength, maxFileCount);
                if (!validationResult.ValidationSuccessful)//if one or more business rules were violated
                {
                    var errorMessage = string.Join("\n", validationResult.Reasons);
                    return Content((HttpStatusCode)406, errorMessage);
                }
                // Grab current file of the request
                var postedFile = httpRequest.Files[0];
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var userService = new UserService(dbContext);
                    User user = userService.GetUser(userId);
                    if (user == null)//check if user exists
                    {
                        return Content((HttpStatusCode)404, "There is no record of that User.");
                    }
                    var resumeService = new ResumeService(dbContext);
                    Resume resume = resumeService.GetResumeByUserID(userId);
                    if (resume == null)//check if user has already submitted a resume
                    {
                        Resume userResume = new Resume(userId, Guid.NewGuid());
                        resumeService.CreateResume(userResume);
                        var result = dbContext.SaveChanges();
                        if (result <= 0)
                        {
                            return Content((HttpStatusCode)500, "Failed to add a resume onto our database");
                        }
                        resume = userResume;
                    }
                    //Folder path of the user
                    var subdir = Path.Combine(ConfigurationManager.AppSettings["ResumeDir"], (resume.ResumeGuid.ToString() + "/")); //@"C:\Resumes\"+resume.ResumeGuid;
                    //Filepath of the submitted file
                    var filePath = Path.Combine(subdir, resume.ResumeGuid.ToString() + ".pdf");// subdir+@"\"+resume.ResumeGuid+".pdf";
                    
                    if (!Directory.Exists(subdir))//check if the directory exists
                    {
                        Directory.CreateDirectory(subdir);//create the directory if it doesnt exist
                    }
                    //saves file onto the specified file path and overwrites any file that may exist in that shares the same path
                    postedFile.SaveAs(filePath);
                    return Content((HttpStatusCode)200, "File Uploaded");
                }
            }
            catch(HttpException e)//HttpPostedFile.SaveAs exception
            {
                return Content((HttpStatusCode)500, "Unable to save the file onto our file system.");
            }
            catch(IOException e)//Exception thrown when creating directory
            {
                return Content((HttpStatusCode)500, "Unable to delete the job posting");
            }
            catch (DbUpdateException e)//exception thrown while saving the database
            {
                return Content((HttpStatusCode)500, "Unable to delete the job posting");
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

        [HttpPut, Route("uploaduserresume")]
        public IHttpActionResult UploadUserResume()
        {
            string token = ControllerHelper.GetTokenFromAuthorizationHeader(Request.Headers);
            if (token == null)
            {
                return Unauthorized();
            }
            //A list in case we want to accept more than one file type
            List<string> allowedFileExtension = new List<string> { ".pdf" };
            //Business Rule - only one file allowed to submit
            int maxFileCount = 1;
            // Max file size is 1MB
            const int maxContentLength = 1024 * 1024 * 1;

            try
            {
                //get the content, headers, etc the full request of the current http request
                var httpRequest = HttpContext.Current.Request;
                var fileValidator = new FileValidator();
                //Validate the submitted file to verify that it complies with Business Rules
                var validationResult = fileValidator.ValidateFiles(httpRequest.Files, allowedFileExtension, maxContentLength, maxFileCount);
                if (!validationResult.ValidationSuccessful)//if one or more business rules were violated
                {
                    var errorMessage = string.Join("\n", validationResult.Reasons);
                    return Content((HttpStatusCode)406, errorMessage);
                }
                // Grab current file of the request
                var postedFile = httpRequest.Files[0];
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var userService = new UserService(dbContext);
                    var user = userService.GetUserByToken(token);
                    if (user == null)//check if user exists
                    {
                        return Content((HttpStatusCode)404, "User does not exist");
                    }
                    var resumeService = new ResumeService(dbContext);
                    Resume resume = resumeService.GetResumeByUserID(user.UserId);
                    if (resume == null)//check if user has already submitted a resume
                    {
                        Resume userResume = new Resume(user.UserId, Guid.NewGuid());
                        resumeService.CreateResume(userResume);
                        var result = dbContext.SaveChanges();
                        if (result <= 0)
                        {
                            return Content((HttpStatusCode)500, "Failed to add a resume onto our database");
                        }
                        resume = userResume;
                    }
                    //Folder path of the user
                    var subdir = Path.Combine(ConfigurationManager.AppSettings["ResumeDir"], (resume.ResumeGuid.ToString() + "/")); //@"C:\Resumes\"+resume.ResumeGuid;
                    //Filepath of the submitted file
                    var filePath = Path.Combine(subdir, resume.ResumeGuid.ToString() + ".pdf");// subdir+@"\"+resume.ResumeGuid+".pdf";

                    if (!Directory.Exists(subdir))//check if the directory exists
                    {
                        Directory.CreateDirectory(subdir);//create the directory if it doesnt exist
                    }
                    //saves file onto the specified file path and overwrites any file that may exist in that shares the same path
                    postedFile.SaveAs(filePath);
                    return Content((HttpStatusCode)200, "File Uploaded");
                }
            }
            catch (HttpException e)//HttpPostedFile.SaveAs exception
            {
                return Content((HttpStatusCode)500, "Unable to save the file onto our file system.");
            }
            catch (IOException e)//Exception thrown when creating directory
            {
                return Content((HttpStatusCode)500, "Unable to delete the job posting");
            }
            catch (DbUpdateException e)//exception thrown while saving the database
            {
                return Content((HttpStatusCode)500, "Unable to delete the job posting");
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
                        return Content((HttpStatusCode)404, "User does not exist");
                    }
                    var resumeService = new ResumeService(dbContext);
                    Resume resume = resumeService.GetResumeByUserID(userId);
                    if (resume == null)//check if user has already submitted a resume
                    {
                        return Content((HttpStatusCode)404, "No resume on file");
                    }
                    var subdir = Path.Combine(ConfigurationManager.AppSettings["ResumeDir"], resume.ResumeGuid.ToString()); 
                    var filePath = Path.Combine(subdir, (resume.ResumeGuid.ToString()+".pdf"));
                    string url = "";
                    if (File.Exists(filePath))//check if the file exists in the specified path
                    {
                        //virtual directory of the file
                        url = ConfigurationManager.AppSettings["ApiResumeDir"] + resume.ResumeGuid + "/" + resume.ResumeGuid + ".pdf";
                        return Content((HttpStatusCode)200, url);
                    }
                    return Content((HttpStatusCode)404, "No resume on file");
                }
            }
            catch(Exception e)
            {
                return Content((HttpStatusCode)500, e.Message);
            }
        }

        [HttpGet, Route("getuserresume")]
        [SwaggerResponse((HttpStatusCode)200, "The uri of the resume that is on the server")]
        public IHttpActionResult GetUserResume()
        {
            string token = ControllerHelper.GetTokenFromAuthorizationHeader(Request.Headers);
            if (token == null)
            {
                return Unauthorized();
            }
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var userService = new UserService(dbContext);
                    var user = userService.GetUserByToken(token);
                    if (user == null)//check if user exists
                    {
                        return Content((HttpStatusCode)404, "User does not exist");
                    }
                    var resumeService = new ResumeService(dbContext);
                    Resume resume = resumeService.GetResumeByUserID(user.UserId);
                    if (resume == null)//check if user has already submitted a resume
                    {
                        return Content((HttpStatusCode)404, "No resume on file");
                    }
                    var subdir = Path.Combine(ConfigurationManager.AppSettings["ResumeDir"], resume.ResumeGuid.ToString());
                    var filePath = Path.Combine(subdir, (resume.ResumeGuid.ToString() + ".pdf"));
                    string url = "";
                    if (File.Exists(filePath))//check if the file exists in the specified path
                    {
                        //virtual directory of the file
                        url = ConfigurationManager.AppSettings["ApiResumeDir"] + resume.ResumeGuid + "/" + resume.ResumeGuid + ".pdf";
                        return Content((HttpStatusCode)200, url);
                    }
                    return Content((HttpStatusCode)404, "No resume on file");
                }
            }
            catch (Exception e)
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
                    if (resume == null)//check if user has already submitted a resume; null
                    {
                        return Content((HttpStatusCode)404, "No resume on file");
                    }
                    var theaterJobService = new TheaterJobPostingService(dbContext);
                    TheaterJobPosting job = theaterJobService.GetTheaterJob(helpwantedid);
                    if (job == null)//check if job exists
                    {
                        return Content((HttpStatusCode)404, "No job on file");
                    }

                    var resumeJobPosting = new ResumeTheaterJob(job.HelpWantedID,resume.ResumeID);
                    var resumeJobService = new ResumeTheaterJobService(dbContext);
                    resumeJobService.CreateResumeTheaterJob(resumeJobPosting);
                    var result = dbContext.SaveChanges();
                    if (result > 0)//check if any rows were affected in the database
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

        [HttpPost, Route("userapply")] //apply to a theater job
        [SwaggerResponse((HttpStatusCode)200, "A string status message.")]
        public IHttpActionResult UserApplyToJob(int helpwantedid)
        {
            string token = ControllerHelper.GetTokenFromAuthorizationHeader(Request.Headers);
            if (token == null)
            {
                return Unauthorized();
            }
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var userService = new UserService(dbContext);
                    var user = userService.GetUserByToken(token);
                    if(user == null)
                    {
                        return Content(HttpStatusCode.NotFound, "User was not found within the database");
                    }
                    var resumeService = new ResumeService(dbContext);
                    Resume resume = resumeService.GetResumeByUserID(user.UserId);
                    if (resume == null)//check if user has already submitted a resume; null
                    {
                        return Content((HttpStatusCode)404, "No resume on file");
                    }
                    var theaterJobService = new TheaterJobPostingService(dbContext);
                    TheaterJobPosting job = theaterJobService.GetTheaterJob(helpwantedid);
                    if (job == null)//check if job exists
                    {
                        return Content((HttpStatusCode)404, "No job on file");
                    }

                    var resumeJobPosting = new ResumeTheaterJob(job.HelpWantedID, resume.ResumeID);
                    var resumeJobService = new ResumeTheaterJobService(dbContext);
                    resumeJobService.CreateResumeTheaterJob(resumeJobPosting);
                    var result = dbContext.SaveChanges();
                    if (result > 0)//check if any rows were affected in the database
                    {
                        return Content((HttpStatusCode)200, "Successfully Applied!");
                    }
                    return Content((HttpStatusCode)500, "Wasn't able to successfully apply");
                }
            }
            catch (Exception e)
            {
                return Content((HttpStatusCode)400, e.Message);
            }
        }

        /// <summary>
        /// Gets all the resume uris' for a theater job 
        /// </summary>
        /// <param name="helpwantedId"></param>
        /// <returns></returns>
        [HttpGet,Route("getresumesforjob")] 
        public IHttpActionResult GetResumesforTheaterJob(int helpwantedId)
        {
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    var theaterJobService = new TheaterJobPostingService(dbContext);
                    if (theaterJobService.GetTheaterJob(helpwantedId) == null)
                    {
                        return Content((HttpStatusCode)404, "Theater job does not exist");
                    }
                    var resumeTheaterJobService = new ResumeTheaterJobService(dbContext);
                    var resumeList = resumeTheaterJobService.GetAllResumeGuidsForTheaterJob(helpwantedId);
                    List<string> urlList = new List<string>();
                    foreach (Guid guid in resumeList)
                    {
                        string path = Path.Combine(ConfigurationManager.AppSettings["ResumeDir"], guid.ToString(), guid.ToString() + ".pdf");
                        if (File.Exists(path))
                        {
                            string url = ConfigurationManager.AppSettings["ApiResumeDir"] + guid + "/" + guid + ".pdf";
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

        [HttpGet,Route("log")]
        public IHttpActionResult Log(int id)
        {
            try
            {
                LogService logService = new LogService();
                //logService.TestLogger();
                DateTime old = new DateTime(2019,4,20);
                DateTime dateTime = DateTime.UtcNow;
                var list = logService.GetErrorLogs(old,dateTime);
                return Content(HttpStatusCode.OK,list);
            }
            catch(Exception e)
            {
                LogService logService2 = new LogService();
                var context = HttpContext.Current;
                logService2.LogError(context,e);
                return Ok("Check Atlas!");
            }
        }

    }
}
