using DataAccessLayer;
using ServiceLayer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.KFC_API_Services;
using BroadwayBuilder.Api.Models;
using DataAccessLayer.Models;

namespace BroadwayBuilder.Api.Controllers 
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        [HttpPost, Route("createuser")]
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

        [HttpGet, Route("{token}")]
        public User GetUser(string token)
        {
            using (var _db = new BroadwayBuilderContext())
            {
                UserService userService = new UserService(_db);

                // Get Session
                var session = _db.Sessions.Find(token);

                // Get User
                return userService.GetUser(session.UserId);
            }
        }

        [HttpPost, Route("login")]
        public HttpResponseMessage LoginFromSSO([FromBody] LoginRequestPaylod request)
        {
            using (var _db = new BroadwayBuilderContext())
            {
                try
                {
                    // TODO: Validate, Parse, and Check Id ...

                    SignatureService signatureService = new SignatureService();
                    // Check if the signature is invalid
                    if (!signatureService.IsValidClientRequest(request.SSOUserId, request.Email, request.Timestamp, request.Signature))
                    {
                        var response401 = new HttpResponseMessage();
                        response401.StatusCode = HttpStatusCode.Unauthorized;
                        response401.Content = new StringContent("Invalid Signature Token");
                        return response401;
                    }

                    // Now we have to get a user (check if it exists)
                    UserService userService = new UserService(_db);

                    var user = userService.GetUser(request.Email);

                    // User is not found, so register that new user
                    if (user == null)
                    {
                        // TODO: Fill in user attributes
                        User newUser = new User();
                        userService.CreateUser(newUser);
                        user = newUser;
                    }
                    // User was found, so login user
                    Session session = new Session();
                    session.UserId = user.UserId;
                    session.Token = Guid.NewGuid().ToString();

                    _db.Sessions.Add(session);

                    _db.SaveChanges();

                    HttpResponseMessage response = new HttpResponseMessage();

                    var redirectURL = "https://www.broadwaybuilder.xyz/#/login/?token=" + session.Token;
                    response = Request.CreateResponse(HttpStatusCode.SeeOther);
                    response.Headers.Location = new Uri(redirectURL);
                    return response;
                }
                catch (Exception e)
                {
                    var response = new HttpResponseMessage();
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Content = new StringContent(e.Message);
                    return response;
                }
            }
        }
    }
}
