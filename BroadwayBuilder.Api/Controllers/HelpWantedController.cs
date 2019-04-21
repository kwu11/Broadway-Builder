using DataAccessLayer;
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
        public IHttpActionResult GetThaterJobsCount(int theaterid)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    TheaterJobService service = new TheaterJobService(dbcontext);
                    return Content((HttpStatusCode)200, service.GetTheaterJobsCount(theaterid));
                }
                catch
                {
                    return Content((HttpStatusCode)404, "Unable to get count of job postings for theater " + theaterid);
                }
            }
        }

        [HttpGet,Route("{theaterid}")]
        public IHttpActionResult GetTheaterJobs(int theaterid, int startingPoint, int numberOfItems)//needs to be changed to string for encryption purposes
        {
            using(var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    TheaterJobService service = new TheaterJobService(dbcontext);
                    var list = service.GetAllJobsFromTheater(theaterid, startingPoint, numberOfItems);
                    if(list == null)
                    {
                        throw new NullNotFoundException();
                    }
                    return Content((HttpStatusCode)200, list);
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

        [HttpPut,Route("edittheaterjob")]
        public IHttpActionResult EditTheaterJob(TheaterJobPosting job) 
        {
            using(var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    TheaterJobService service = new TheaterJobService(dbcontext);
                    //TheaterJobPosting job = service.GetTheaterJob(helpwantedid);
                    if (job != null)
                    {
                        service.UpdateTheaterJob(job);
                        var results = dbcontext.SaveChanges();
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
                catch (ZeroAffectedRowsException)
                {
                    return Content((HttpStatusCode)500, "There appears to be no changes detected. The Theater Job was not updated");
                }
                catch (DbEntityValidationException)
                {
                    return Content((HttpStatusCode)500, "Theater Job could not be updated");
                }
                catch (Exception e)
                {
                    return Content((HttpStatusCode)400, e.Message);
                }
                
            }
        }

        [HttpDelete, Route("deletetheaterjob/{helpWantedId}")]
        public IHttpActionResult DeleteTheaterJob(int helpWantedId)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                TheaterJobService service = new TheaterJobService(dbcontext);
                TheaterJobPosting job = service.GetTheaterJob(helpWantedId);
                try
                {
                    service.DeleteTheaterJob(job);
                    if (job == null)
                    {
                        return Content((HttpStatusCode)404, "That Job Listing does not exist");
                    }
                    var results = dbcontext.SaveChanges();
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
            using(var dbcontext = new BroadwayBuilderContext())
            {
                TheaterJobService jobService = new TheaterJobService(dbcontext);
                try
                {
                    if (theaterJob == null)
                    {
                        return Content((HttpStatusCode)400, "That job posting does not exist");
                    }
                    jobService.CreateTheaterJob(theaterJob);
                    var results = dbcontext.SaveChanges();
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
            using (var dbcontext = new BroadwayBuilderContext())
            {
                ProductionJobService jobService = new ProductionJobService(dbcontext);
                try
                {
                    if (productionJob == null)
                    {
                        return Content((HttpStatusCode)404,"The job posting does not exist");
                    }
                    jobService.CreateProductionJob(productionJob);
                    var results = dbcontext.SaveChanges();
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

        [HttpPut,Route("{userid}/uploadresume")]
        public IHttpActionResult uploadResume(int userid)
        {
            //A list in case we want to accept more than one file type
            IList<string> AllowedFileExtension = new List<string> { ".pdf" };
            
            // Max file size is 1MB
            const int MaxContentLength = 1024 * 1024 * 1;

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
                    if (!AllowedFileExtension.Contains(extension))
                    {
                        throw new Exception(extension);
                    }
                    else if(postedFile.ContentLength > MaxContentLength)
                    {
                        throw new Exception("File Exceeds file limit");
                    }
                    else
                    {
                        using (var dbcontext = new BroadwayBuilderContext())
                        {
                            var userService = new UserService(dbcontext);
                            User user = userService.GetUser(userid);
                            if (user == null)//check if user exists
                            {
                                throw new Exception("User does not exist");
                            }
                            var resumeService = new ResumeService(dbcontext);
                            Resume resume = resumeService.GetResumeByUserID(userid);
                            if (resume == null)//check if user has already submitted a resume
                            {
                                Resume userResume = new Resume(userid, Guid.NewGuid());
                                resumeService.CreateResume(userResume);
                                var result = dbcontext.SaveChanges();
                                if(result <= 0)
                                {
                                    throw new Exception("failed to add a resume");
                                }
                                resume = userResume;
                            }
                            var subdir = @"C:\Resumes\"+resume.ResumeID;
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

        [HttpGet,Route("myresume/{id}")]
        public IHttpActionResult GetResume(int id)
        {
            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var userService = new UserService(dbcontext);
                    User user = userService.GetUser(id);
                    if (user == null)//check if user exists
                    {
                        throw new Exception("User does not exist");
                    }
                    var resumeService = new ResumeService(dbcontext);
                    Resume resume = resumeService.GetResumeByUserID(id);
                    if (resume == null)//check if user has already submitted a resume
                    {
                        throw new Exception("No resume on file");
                    }
                    var filepath = @"C:\Resumes\" + resume.ResumeID + @"\" + resume.ResumeGuid + ".pdf";
                    string url = "";
                    if (!Directory.Exists(filepath))
                    {
                        url = "https://api.broadwaybuilder.xyz/"+ resume.ResumeGuid + ".pdf";
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

    }
}
