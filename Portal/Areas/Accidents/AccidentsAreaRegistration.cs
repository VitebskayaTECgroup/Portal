using System.Web.Mvc;

namespace Portal.Areas.Accidents
{
	public class AccidentsAreaRegistration : AreaRegistration 
	{
		public override string AreaName 
		{
			get 
			{
				return "Accidents";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) 
		{
			context.MapRoute(
				"Accidents_default",
				"Accidents/{controller}/{action}/{id}",
				new { controller = "Records", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}