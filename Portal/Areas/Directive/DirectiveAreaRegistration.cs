using System.Web.Mvc;

namespace Portal.Areas.Directive
{
	public class DirectiveAreaRegistration : AreaRegistration 
	{
		public override string AreaName 
		{
			get 
			{
				return "Directive";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context) 
		{
			context.MapRoute("Directive_default", "directive/", new
			{
				controller = "Main",
				action = "Index",
			});

			context.MapRoute("Api", "directive/api/{controller}/{action}", new
			{
				controller = "Main",
				action = "Index",
			});

			context.MapRoute("Search", "directive/search/", new
			{
				controller = "Main",
				action = "Search",
			});

			context.MapRoute("Admin", "directive/admin/{id}", new
			{
				controller = "Main",
				action = "Admin",
				id = UrlParameter.Optional,
			});

			context.MapRoute("Pages", "directive/page/{pageName}", new
			{
				controller = "Main",
				action = "Page",
				pageName = UrlParameter.Optional
			});

			context.MapRoute("Redactors", "directive/redactor/{pageName}", new
			{
				controller = "Main",
				action = "Redactor",
				pageName = UrlParameter.Optional
			});
		}
	}
}