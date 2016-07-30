using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;

namespace LondonPublicTransport.App_Start
{
    public class RoutesConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(null, "", "~/Pages/PublicTransport.aspx");
            //routes.MapHttpRoute(name: "WebApiRoute", routeTemplate: "transportapi/{controller}");
            routes.MapHttpRoute(name: "WebApiRoute", routeTemplate: "transportapi/{controller}/{lat}/{lng}/{radius}",
                defaults: new { lat = RouteParameter.Optional, lng = RouteParameter.Optional , radius = RouteParameter.Optional });
        }
    }

    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/jquery").Include("~/scripts/jquery-{version}.js"));
        }
    }
}