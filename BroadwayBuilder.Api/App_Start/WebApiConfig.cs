using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace BroadwayBuilder.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "GET, POST, DELETE, PUT");//"http://www.broadwaybuilder.xyz", "*", "GET,POST"); // HACK: only using * for testing. will be updated
            config.EnableCors(cors);
        }
    }
}
