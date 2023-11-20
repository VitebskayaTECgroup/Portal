using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[Authorize]
	public class SafetyController : Controller
	{
		public ActionResult Index() => View();

		public ActionResult Week(DateTime? date) => View(model: date ?? DateTime.Today);

		public ActionResult Accident(int Id = 0) => View(model: Id);

		public ActionResult SetDocViewed(int recordId, string Link)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				if (!db.AccidentDocs.Any(x => x.AccidentId == recordId && x.UserId == user.UID && x.Link == Link))
				{
					db.AccidentDocs
						.Value(x => x.AccidentId, recordId)
						.Value(x => x.UserId, user.UID)
						.Value(x => x.Link, Link)
						.Value(x => x.WhenViewed, DateTime.Now)
						.Insert();
				}
			}

			return Json(new { Done = true });
		}

		public ActionResult SetAccidentViewed(int recordId)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				if (!db.AccidentsViews.Any(x => x.AccidentId == recordId && x.UserId == user.UID))
				{
					db.AccidentsViews
						.Value(x => x.AccidentId, recordId)
						.Value(x => x.UserId, user.UID)
						.Value(x => x.Date, DateTime.Now)
						.Insert();
				}

				return Json(new { Done = true });
			}
		}

		public ActionResult Form()
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		public ActionResult Edit(AccidentRecord record)
		{
			return View();
		}

		public ActionResult Delete()
		{
			return View();
		}

		public ActionResult SetAsSeen()
		{
			return View();
		}
	}
}