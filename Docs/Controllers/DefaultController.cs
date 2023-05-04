using DatabaseLayer.Site;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Docs.Controllers
{
	public class DefaultController : Controller
	{
		[AllowAnonymous]
		public ActionResult Menu(bool isDocsAdmin = false)
		{
			return View(isDocsAdmin);
		}

		public ActionResult Tree()
		{
			return View();
		}

		public ActionResult Search()
		{
			return View();
		}

		public ActionResult SearchResult(string query)
		{
			using (var db = new SiteContext())
			{
				if (string.IsNullOrEmpty(query)) return View(new List<DocsDocument>());

				var model = db.DocsDocuments
					.Where(x => (x.Text + x.TagsString).ToLower().Contains(query.ToLower()))
					.OrderBy(x => x.Text)
					.ToList();

				return View(model);
			}
		}
	}
}