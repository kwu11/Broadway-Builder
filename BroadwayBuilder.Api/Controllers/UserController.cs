using DataAccessLayer;
using ServiceLayer.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; // For doing async (async Task<>) await
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BroadwayBuilder.Api.Controllers 
{
    public class UserController : ApiController
    {

        [Route("HelloWorld")]
        [HttpGet]

        public string HelloWorld()
        {
            return "Hello World! " + DateTime.Now;
        }

        [Route("user/createuser")]
        [HttpPost]

        public IHttpActionResult CreateUser([FromBody] User user)
        {

            using (var dbcontext = new BroadwayBuilderContext())
            {
                var userService = new UserService(dbcontext);

                try
                {
                    if (user.Username == null)
                    {
                        throw new Exception();
                    }
                    userService.CreateUser(user);
                    dbcontext.SaveChanges();

                    return Content((HttpStatusCode)201, "User Created");

                }
                // Todo: add proper error handling
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.StackTrace);
                    return Content((HttpStatusCode)400, "Must provide a Unique User Name");
                }

            }

        }

        //[HttpGet, Route("user/all")]
        //public IHttpActionResult GetAllUsers()
        //{
        //    using (var dbcontext = new BroadwayBuilderContext())
        //    {
        //        UserService service = new UserService(dbcontext);
        //        try
        //        {
        //            IEnumerable list = service.GetAllUsers();
        //            return Content((HttpStatusCode)200, list);
        //        }
        //        catch (Exception e)
        //        {
        //            return Content((HttpStatusCode)500, "Oops! Something went wrong on our end");
        //        }


        //    }
        //}
    }
}
