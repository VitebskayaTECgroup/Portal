using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Schemes.Controllers
{
	[Authorize]
	public class ContainersController : Controller
	{
		public ActionResult Create(int ContainerId, string Name)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.SchemeContainers
						.Value(x => x.ContainerId, ContainerId)
						.Value(x => x.Name, Name)
						.Value(x => x.Description, "")
						.Insert();

					return Json(new { Done = "Раздел добавлен" });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Edit(int Id, string Name)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.SchemeContainers
						.Where(x => x.Id == Id)
						.Set(x => x.Name, Name)
						.Update();

					return Json(new { Done = "Раздел изменён" });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Delete(int Id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.SchemeContainers
						.Where(x => x.Id == Id)
						.Delete();

					return Json(new { Done = "Раздел удалён" });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}
	}
}