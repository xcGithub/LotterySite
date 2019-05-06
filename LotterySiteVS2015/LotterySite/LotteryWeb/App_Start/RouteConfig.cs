using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LotteryWeb
{
    public class RouteConfig
    {
        // 将控制器命名空间传入 避免控制器重复 
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

             routes.MapRoute(
                name: "Note",
                url: "note/{action}",
                //defaults: new { controller = "Home", action = "zhuye", id = UrlParameter.Optional }
                defaults: new { controller = "Note", action = "Index" }
                ,namespaces: new string[] { "LotteryWeb.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "zhuye", id = UrlParameter.Optional }
                defaults: new { controller = "Home", action = "folio", id = UrlParameter.Optional }
                ,namespaces: new string[] { "LotteryWeb.Controllers" }
            );
        }

    }
}