using Schemes.Models;
using System.Web.Mvc;

namespace Schemes.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		public ActionResult Index(int Id = 0, string query = "", string tagsString = "") => View(new UrlParams
		{
			Id = Id,
			Query = query,
			TagsString = tagsString
		});
	}
}