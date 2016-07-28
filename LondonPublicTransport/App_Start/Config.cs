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
            routes.MapHttpRoute(name: "WebApiRoute", routeTemplate: "transportapi/{controller}");
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