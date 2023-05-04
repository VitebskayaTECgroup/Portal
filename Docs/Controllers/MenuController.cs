using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Docs.Controllers
{
	public class MenuController : Controller
	{
		public ActionResult Add(string name, int menuId)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.DocsMenu
						.Value(x => x.Name, name)
						.Value(x => x.MenuId, menuId)
						.Insert();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Edit(string name, int id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.DocsMenu
						.Where(x => x.Id == id)
						.Set(x => x.Name, name)
						.Update();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Remove(int id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.DocsMenu
						.Where(x => x.Id == id)
						.Delete();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}
	}
}