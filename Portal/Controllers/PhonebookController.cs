using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
	public class PhonebookController : Controller
	{
		public ActionResult Index() => View();

		public ActionResult Redactor() => View();

		public ActionResult Create(int Id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.PhonebookPositions
						.Value(x => x.Name, string.Empty)
						.Value(x => x.Position, "Новая запись")
						.Value(x => x.PhonebookGuildId, Id)
						.Value(x => x.PhoneInner, string.Empty)
						.Value(x => x.PhoneCity, string.Empty)
						.Value(x => x.WindowsName, string.Empty)
						.Value(x => x.OrderPriority, 0)
						.Insert();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Edit(int Id, string Name, string Position, string PhoneInner, string PhoneCity, string WindowsName)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.PhonebookPositions
						.Where(x => x.Id == Id)
						.Set(x => x.Name, Name)
						.Set(x => x.Position, Position)
						.Set(x => x.PhoneInner, PhoneInner)
						.Set(x => x.PhoneCity, PhoneCity)
						.Set(x => x.WindowsName, WindowsName)
						.Update();

					return Json(new { Done = true });
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
					db.PhonebookPositions
						.Where(x => x.Id == Id)
						.Delete();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult CreateGuild()
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.PhonebookGuilds
						.Value(x => x.Name, string.Empty)
						.Value(x => x.OrderPriority, 0)
						.Insert();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult EditGuild(PhonebookGuild guild)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.PhonebookGuilds
						.Where(x => x.Id == guild.Id)
						.Set(x => x.Name, guild.Name)
						.Update();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult DeleteGuild(int Id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.PhonebookGuilds
						.Where(x => x.Id == Id)
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