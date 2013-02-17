using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EventBooking
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

	        routes.MapRoute(
		        name: "SessionsForActivity",
		        url: "Sessions/Index/{activityId}",
                 defaults: new {controller = "Sessions", action="Index",  activityId = UrlParameter.Optional }
		        );

	        routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}