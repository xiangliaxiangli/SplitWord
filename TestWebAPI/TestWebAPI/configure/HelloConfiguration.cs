using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TestWebAPI.configure
{
    public class HelloConfiguration
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();
            // New code  solve the cros problem
            config.EnableCors();
            //api/{controller}/{id}
            //api/{controller}/{action}/{id}  可以通过action name 来访问
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}