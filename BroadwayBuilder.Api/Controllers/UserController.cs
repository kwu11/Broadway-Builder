using DataAccessLayer;
using ServiceLayer.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ServiceLayer.KFC_API_Services;
using BroadwayBuilder.Api.Models;
using DataAccessLayer.Models;
using System.Collections;
using Swashbuckle.Swagger.Annotations;
using System.Linq;
using ServiceLayer.Exceptions;

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

        [HttpPut, Route("updateUser")]
        public IHttpActionResult UpdateUser([FromBody] User user)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    var userService = new UserService(dbcontext);
                    var updatedUser = userService.UpdateUser(user);
                    if (updatedUser != null)
                    {
                        dbcontext.SaveChanges();
                    }
                    else
                    {
                        throw new Exception();
                    }
                    return Content((HttpStatusCode)200, user);
                }
                catch
                {
                    return Content((HttpStatusCode)404, "The user could not be found");
                }
            }
        }

        [HttpDelete, Route("deleteUser")]
        public IHttpActionResult DeleteUser([FromBody] User user)
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                try
                {
                    var userService = new UserService(dbcontext);
                    userService.DeleteUser(user);
                    dbcontext.SaveChanges();
                    return Content((HttpStatusCode)200, "User Successfully Deleted");
                }
                catch
                {
                    return Content((HttpStatusCode)404, "The user could not be found");
                }
            }
        }

        [HttpGet, Route("all")]
        public IHttpActionResult GetAllUsers()
        {
            using (var dbcontext = new BroadwayBuilderContext())
            {
                UserService service = new UserService(dbcontext);
                try
                {
                    var usersList = service.GetAllUsers()
                        .Select(user => new UserResponseModel()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            StreetAddress = user.StreetAddress,
                            City = user.City,
                            StateProvince = user.StateProvince,
                            Country = user.Country,
                            IsEnabled = user.IsEnabled,
                            Username = user.Username,
                            UserId = user.UserId
                        }).ToList();

                    return Content((HttpStatusCode)200, usersList);
                }
                catch (Exception e)
                {
                    return Content((HttpStatusCode)500, "Oops! Something went wrong on our end");
                }
            }
        }



        [HttpGet, Route("registrationstatus")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(bool))]
        public IHttpActionResult GetUserRegistrationStatus()
        {
            var authHeaderValue = Request.Headers.GetValues("Authorization").FirstOrDefault();
            if (authHeaderValue == null)
            {
                return BadRequest("No Authorization Header");
            }

            var token = authHeaderValue.Split(' ')[1];

            using (var _db = new BroadwayBuilderContext())
            {
                UserService userService = new UserService(_db);

                var user = userService.GetUserByToken(token);

                return Ok(user.IsComplete);
            }
        }

        [HttpPut, Route("completeregistration")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public IHttpActionResult UserCompleteRegistration([FromBody] UserCompleteRegistrationRequestModel userData)
        {
            var token = ControllerHelper.GetTokenFromAuthorizationHeader(Request.Headers);

            using (var _db = new BroadwayBuilderContext())
            {
                UserService userService = new UserService(_db);

                var user = userService.GetUserByToken(token);

                if (user.IsComplete)
                {
                    return BadRequest("User is already registered");
                }

                user.FirstName = userData.FirstName;
                user.LastName = userData.LastName;
                user.City = userData.City;
                user.StreetAddress = userData.StreetAddress;
                user.StateProvince = userData.StateProvince;
                user.Country = userData.Country;
                user.IsComplete = true;
                user.IsEnabled = true;

                userService.UpdateUser(user);

                _db.SaveChanges();
            }

            return Ok();
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

                    User user;
                    try
                    {
                        user = userService.GetUser(request.Email);
                    }
                    catch (UserNotFoundException ex)
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

                        // Everyone starts off as a general user
                        userService.AddUserRole(user.UserId, DataAccessLayer.Enums.RoleEnum.GeneralUser);
                    }

                    // User was found, so login user
                    Session session = new Session()
                    {
                        UserId = user.UserId,
                        Token = Guid.NewGuid().ToString(),
                        Signature = request.Signature,
                        CreatedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                        UpdatedAt = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                    };

                    _dbcontext.Sessions.Add(session);
                    _dbcontext.SaveChanges();
                    //Logging Usage
                    //TODO: possibly change the userid argument for LogUsage
                    LoggerHelper.LogUsage("Login", user.UserId);
                    var redirectURL = $"https://www.broadwaybuilder.xyz/#/login?token={session.Token}";
                    return Redirect(redirectURL);
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                    //TODO: LogError
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
        [HttpPost,Route("applogout")]
        public IHttpActionResult LogoutFromApp()
        {
            string token = ControllerHelper.GetTokenFromAuthorizationHeader(Request.Headers);
            if(token == null)
            {
                return Unauthorized();
            }
            try
            {
                using (var dbContext = new BroadwayBuilderContext())
                {
                    dbContext.Sessions.Remove(dbContext.Sessions
                        .Where(session => session.Token == token).First());
                    dbContext.SaveChanges();
                    return Ok("User has successfully logged out.");
                }
            }
            catch(Exception e)
            {
                return InternalServerError(e);
            }

        }
        [HttpPost]
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

        [HttpPut]
        [Route("elevate/{userId}")]
        public IHttpActionResult ElevateUser([FromUri] int userId)
        {
            var token = ControllerHelper.GetTokenFromAuthorizationHeader(Request.Headers);

            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var authorizationService = new AuthorizationService(dbcontext);

                    var userService = new UserService(dbcontext);

                    var requestingUser = userService.GetUserByToken(token);

                    var isAuthorized = authorizationService.HasPermission(requestingUser, DataAccessLayer.Enums.PermissionsEnum.UpgradeGeneralUserToTheaterAdmin);

                    if (!isAuthorized)
                    {
                        return Unauthorized();
                    }

                    var isTheaterAdmin = userService.HasUserRole(userId, DataAccessLayer.Enums.RoleEnum.TheaterAdmin);

                    if (!isTheaterAdmin)
                    {
                        userService.AddUserRole(userId, DataAccessLayer.Enums.RoleEnum.TheaterAdmin);
                        dbcontext.SaveChanges();
                    }

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("downgrade/{userId}")]

        public IHttpActionResult DowngradeUser([FromUri] int userId)
        {
            var token = ControllerHelper.GetTokenFromAuthorizationHeader(Request.Headers);

            try
            {
                using (var dbcontext = new BroadwayBuilderContext())
                {
                    var authorizationService = new AuthorizationService(dbcontext);

                    var userService = new UserService(dbcontext);

                    var requestingUser = userService.GetUserByToken(token);

                    var isAuthorized = authorizationService.HasPermission(requestingUser, DataAccessLayer.Enums.PermissionsEnum.DowngradeTheaterAdminToGeneralUser);

                    if (!isAuthorized)
                    {
                        return Unauthorized();
                    }

                    var isTheaterAdmin = userService.HasUserRole(userId, DataAccessLayer.Enums.RoleEnum.TheaterAdmin);

                    if (isTheaterAdmin)
                    {
                        userService.RemoveUserRole(userId, DataAccessLayer.Enums.RoleEnum.TheaterAdmin);
                        dbcontext.SaveChanges();
                    }

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("getrole")]
        public IHttpActionResult GetUserRole()
        {
            var token = ControllerHelper.GetTokenFromAuthorizationHeader(Request.Headers);
            if (token == null)
            {
                return Unauthorized();
            }
            try
            {
                using(var dbcontext = new BroadwayBuilderContext())
                {
                    var userService = new UserService(dbcontext);

                    var userId = userService.GetUserByToken(token).UserId;

                    var roles = userService.GetUserRoles(userId)
                        .Select(o => Enum.GetName(typeof(DataAccessLayer.Enums.RoleEnum), o))
                        .ToList();

                    return Ok(roles);
                }
            } 
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
