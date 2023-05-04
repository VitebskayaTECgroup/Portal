using LinqToDB.Data;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace Docs
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			RouteTable.Routes.MapRoute(
				name: "Pages",
				url: "page/{id}",
				defaults: new { controller = "Pages", action = "Index", id = UrlParameter.Optional }
			);
			RouteTable.Routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Default", action = "Menu", id = UrlParameter.Optional }
			);

			#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (s, s1, tl) => Debug.WriteLine(s + " | " + tl);
			#endif
		}
	}
}
