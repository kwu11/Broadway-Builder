using DataAccessLayer;
using ServiceLayer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.KFC_API_Services;
using BroadwayBuilder.Api.Models;
using DataAccessLayer.Models;
using Swashbuckle.Swagger.Annotations;
using System.Linq;

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
        public IHttpActionResult LoginFromSSO([FromBody] LoginRequestModel request)
        {
            using (var _dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                   
                    ControllerHelper.ValidateLoginRequestModel(ModelState, request);

                    Guid userSsoId = ControllerHelper.ParseAndCheckId(request.SSOUserId);

                    SignatureService signatureService = new SignatureService();
                    // Check if the signature is invalid
                    if (!signatureService.IsValidClientRequest(request.SSOUserId, request.Email, request.Timestamp, request.Signature))
                    {
                        return Content(HttpStatusCode.Unauthorized, "Invalid Signature Token");
                    }

                    // Now we have to get a user (check if it exists)
                    UserService userService = new UserService(_dbcontext);

                    var user = userService.GetUser(request.Email);

                    // User is not found, register user as a new user
                    if (user == null)
                    {
                        var newUser = new User()
                        {
                            UserGuid = userSsoId,
                            Username = request.Email,
                            DateCreated = DateTime.UtcNow,
                            IsEnabled = false,
                            IsComplete = false
                        };
                        userService.CreateUser(newUser);
                        user = newUser;
                    }

                    // User was found, so login user
                    Session session = new Session()
                    {
                        UserId = user.UserId,
                        Token = Guid.NewGuid().ToString()
                    };

                    _dbcontext.Sessions.Add(session);
                    _dbcontext.SaveChanges();

                    var redirectURL = "https://www.broadwaybuilder.xyz/#/login/?token=" + session.Token;
                    return Redirect(redirectURL);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
        }

        [HttpPost, Route("logout")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult LogoutFromSSO([FromBody] LogoutRequestModel request)
        {
            using (var _dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    ControllerHelper.ValidateLoginRequestModel(ModelState, request);

                    //Guid userSsoId = ControllerHelper.ParseAndCheckId(request.SSOUserId);

                    //SignatureService signatureService = new SignatureService();
                    //if (!signatureService.IsValidClientRequest(request.SSOUserId, request.Email, request.Timestamp, request.Signature))
                    //{
                    //    return Content(HttpStatusCode.Unauthorized, "Invalid Signature Token");
                    //}

                    _dbcontext.Sessions.Remove(_dbcontext.Sessions
                        .Where(o => o.Signature == request.Signature)
                        .First());
                    _dbcontext.SaveChanges();

                    return Ok("User logged out");
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
        }

        [HttpDelete]
        [Route("delete")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult DeleteUserFromSSO([FromBody] LoginRequestModel request)
        {
            using (var _dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    ControllerHelper.ValidateLoginRequestModel(ModelState, request);

                    Guid userSsoId = ControllerHelper.ParseAndCheckId(request.SSOUserId);

                    SignatureService signatureService = new SignatureService();
                    if (!signatureService.IsValidClientRequest(request.SSOUserId, request.Email, request.Timestamp, request.Signature))
                    {
                        return Content(HttpStatusCode.Unauthorized, "Invalid Signature Token");
                    }

                    UserService userService = new UserService(_dbcontext);
                    var user = userService.GetUser(request.Email);
                    userService.DeleteUser(request.Email);

                    _dbcontext.Sessions.RemoveRange(_dbcontext.Sessions.Where(o => o.UserId == user.UserId));
                    _dbcontext.SaveChanges();

                    return Ok("User deleted");
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
        }
    }
}
