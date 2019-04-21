using DataAccessLayer;
using ServiceLayer.Exceptions;
using ServiceLayer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public IHttpActionResult GetThaterJobsCount()
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    TheaterJobService service = new TheaterJobService(dbcontext);
                    return Content((HttpStatusCode)200, service.GetTheaterJobsCount());
                }
                catch
                {
                    return Content((HttpStatusCode)404, "Unable to get count of job postings");
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

    }
}
