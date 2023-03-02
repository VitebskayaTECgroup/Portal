using System.Web.Mvc;
using System.Web.Routing;

namespace Portal
{
	public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Docs", "docs/{section}/{page}", new {
                controller = "Docs",
                action = "Page",
                section = UrlParameter.Optional,
                page = UrlParameter.Optional
            });

            routes.MapRoute(
                name: "Default", 
                url: "{controller}/{action}/{id}", 
                defaults: new {
                    controller = "Default",
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}