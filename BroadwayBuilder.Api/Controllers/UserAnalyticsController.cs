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
        public IHttpActionResult SessionLength(int month, int year)
        {

            var sessionLength = new SessionLengthResponseModel()
            {
                Month = month,
                Year = year,
                AverageSessionLength = 4,
                MinSessionLength = 1,
                MaxSessionLength = 10
            };

            return Ok(sessionLength);

        }

        [Route("SuccessfulVsFailedLogins/{month}/{year}")]
        [HttpGet]
        public IHttpActionResult SuccessfulVsFailedLogins(int month, int year)
        {

            var successVsFailedLogins = new SuccessfulVsFailedLoginsResponseModel()
            {
                Month = month,
                Year = year,
                SuccessfulLogins = 20,
                FailedLogins = 5
            };

            return Ok(successVsFailedLogins);

        }
    }
}
