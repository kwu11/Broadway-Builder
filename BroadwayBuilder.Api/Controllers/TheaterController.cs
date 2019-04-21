using DataAccessLayer;
using ServiceLayer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BroadwayBuilder.Api.Controllers
{
    public class TheaterController : ApiController
    {
        [Route("theater/createtheater")]
        [HttpPost]

        public IHttpActionResult CreateTheater([FromBody] Theater theater)
        {

            using (var dbcontext = new BroadwayBuilderContext())
            {
                var theaterService = new TheaterService(dbcontext);

                try
                {
                    if (theater.TheaterName == null)
                    {
                        throw new Exception();
                    }
                    theaterService.CreateTheater(theater);
                    dbcontext.SaveChanges();

                    return Content((HttpStatusCode)201,"Theater Created");

                }
                // Todo: add proper error handling
                catch (Exception e)
                {
                    return Content((HttpStatusCode)400, "Must provide a Theater Name");
                }

            }

        }

        [HttpGet, Route("theater/{theatername}")]
        public IHttpActionResult GetTheaterByName(string theatername)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                TheaterService service = new TheaterService(dbcontext);
                try
                {
                    Theater theater = service.GetTheaterByName(theatername);
                    if(theater == null)
                    {
                        throw new Exception();
                    }
                    return Content((HttpStatusCode)200,theater);
                }
                catch (Exception)
                {
                    return Content((HttpStatusCode)404, "The Theater could not be found");
                }
            }
        }

        [HttpGet,Route("theater/get/{theaterid}")]
        public IHttpActionResult GetTheaterById(int theaterid)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                TheaterService service = new TheaterService(dbcontext);
                try
                {
                    Theater theater = service.GetTheaterByID(theaterid);
                    if (theater == null)
                    {
                        throw new Exception();
                    }
                    return Content((HttpStatusCode)200, theater);
                }
                catch (Exception)
                {
                    return Content((HttpStatusCode)404, "The Theater could not be found");
                }
            }
        }

        [HttpGet,Route("theater/all")]
        public IHttpActionResult GetAllTheaters()
        {
            using(var dbcontext = new BroadwayBuilderContext())
            {
                TheaterService service = new TheaterService(dbcontext);
                try
                {
                    IEnumerable list = service.GetAllTheaters();
                    return Content((HttpStatusCode)200,list);
                }
                catch(Exception e)
                {
                    return Content((HttpStatusCode)500,"Oops! Something went wrong on our end");
                }
                

            }
        }

        [HttpPut,Route("theater/updateTheater")]
        public IHttpActionResult UpdateTheater([FromBody] Theater theater)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    var theaterService = new TheaterService(dbcontext);
                    var updatedTheater = theaterService.UpdateTheater(theater);
                    if (updatedTheater != null)
                    {
                        dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception();
                    }
                    return Content((HttpStatusCode)200, theater);
                }
                catch
                {
                    return Content((HttpStatusCode)404, "The theater could not be found");
                }
            }
        }
        
        [HttpDelete, Route("theater/deleteTheater")]
        public IHttpActionResult DeleteTheater([FromBody] Theater theater)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    var theaterService = new TheaterService(dbcontext);
                    theaterService.DeleteTheater(theater);
                    dbcontext.SaveChanges();
                    return Content((HttpStatusCode)200, "Theater Successfully Deleted");
                }
                catch
                {
                    return Content((HttpStatusCode)404, "The theater could not be found");
                }
            }
        }
    }
}
