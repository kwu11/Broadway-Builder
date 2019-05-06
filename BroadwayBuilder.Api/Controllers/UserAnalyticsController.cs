using BroadwayBuilder.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BroadwayBuilder.Api.Controllers
{
    [RoutePrefix("api/userAnalytics")]
    public class UserAnalyticsController : ApiController
    {
        
        [Route("NumberOfSuccessfulLogins/{month}/{year}")]
        [HttpGet]
        public IHttpActionResult NumberOfSuccessfulLogins(int month, int year)
        {

            var successfulLoginsPerMonth = new NumberOfSuccessfulLoginsResponseModel()
            {
                Month = month,
                Year = year,
                AverageSuccessfulLogins = 10,
                MinSuccessfulLogins = 2,
                MaxSuccessfulLogins = 15
            };

            return Ok(successfulLoginsPerMonth);

        }


        [Route("SessionLength/{month}/{year}")]
        [HttpGet]
        public IHttpActionResult AverageSessionDuration(int month, int year)
        {

            var successfulLoginsPerMonth = new AverageSessionLengthResponseModel()
            {
                Month = month,
                Year = year,
                AverageSessionLength = 10,
                MinSessionLength = 2,
                MaxSessionLength = 15
            };

            return Ok(successfulLoginsPerMonth);

        }
    }
}
