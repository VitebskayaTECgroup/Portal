using DatabaseLayer.Site;
using LinqToDB;
using Schemes.Models;
using System.IO;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Schemes.Controllers
{
	public class TagsController : Controller
	{
		public ActionResult Add(string Text)
		{
			if (string.IsNullOrEmpty(Text)) return Json(new { Error = "Название тега не указано" });

			using (var db = new SiteContext())
			{
				var id = db.SchemeTags
					.Value(x => x.Text, Text)
					.InsertWithInt32Identity();

				if (!id.HasValue) return Json(new { Error = "Не получен идентификатор нового тега" });

				return Json(new { Done = id.Value });
			}
		}
	}
}