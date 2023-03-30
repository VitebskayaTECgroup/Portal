using System.Web.Mvc;

namespace Portal.Controllers
{
	[AllowAnonymous]
	public class DefaultController : Controller
	{
		public ActionResult Index()
		{
			if (Request.UserHostAddress.Contains("10.178.9.") || Request.UserHostAddress == "::1")
			{
				return Redirect("~/site/");
			}
			else
			{
				return Redirect("~/guest/");
			}
		}
	}
}