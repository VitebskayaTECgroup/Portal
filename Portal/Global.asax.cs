using LinqToDB.Data;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			#if DEBUG
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (s, s1, tl) => Debug.WriteLine(s + " | " + tl);
			#endif
		}
	}
}