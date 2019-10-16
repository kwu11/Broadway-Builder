using DataAccessLayer;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BroadwayBuilder.Api.Controllers
{
    [RoutePrefix("api/healthcheck")]
    public class HealthCheckController : ApiController
    {
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, type: null)]
        public async Task<IHttpActionResult> HealthCheck()
        {
            try
            {
                using (var context = new BroadwayBuilderContext())
                {
                    if (!context.Database.Exists())
                    {
                        return Content(HttpStatusCode.InternalServerError, "DB is down");
                    }
                }

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync("http://ui.broadwaybuilder.xyz");
                    if (!response.IsSuccessStatusCode)
                    {
                        return Content(HttpStatusCode.InternalServerError, "SPA is down");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Unknown error occurred");
            }

            return Ok();
        }
    }
}
