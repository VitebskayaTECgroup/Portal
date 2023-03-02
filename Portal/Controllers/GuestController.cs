using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[AllowAnonymous]
	public class GuestController : Controller
	{
		public ActionResult Index() => View();

		public ActionResult News(int Id)
		{
			using (var db = new SiteContext())
			{
				var query = from n in db.News
							from u in db.Users.Where(x => x.UID == n.UserId).DefaultIfEmpty()
							where n.Id == Id && n.Priority != "back" && n.GuildId == 0
							select new News
							{
								Id = n.Id,
								Priority = n.Priority,
								DateAdd = n.DateAdd,
								DateExpire = n.DateExpire,
								Title = n.Title,
								Message = n.Message,
								Links = n.Links,
								GuildId = n.GuildId,
								UserName = string.IsNullOrEmpty(n.UserName) ? (string.IsNullOrEmpty(u.DisplayName) ? u.UName : u.DisplayName) : n.UserName,
							};

				var news = query.FirstOrDefault();

				return View("News", news);
			}
		}

		public ActionResult Body(int Id)
		{
			using (var db = new SiteContext())
			{
				var query = from n in db.News
							from u in db.Users.Where(x => x.UID == n.UserId).DefaultIfEmpty()
							where n.Id == Id
							select new News
							{
								Id = n.Id,
								Priority = n.Priority,
								DateAdd = n.DateAdd,
								DateExpire = n.DateExpire,
								Title = n.Title,
								Message = n.Message,
								Links = n.Links,
								UserName = n.UserName,
								Tags = n.Tags ?? "",
								Creator = new User
								{
									UID = u.UID,
									UName = u.UName,
									DisplayName = u.DisplayName,
								}
							};

				var news = query.First();

				ViewBag.Tags = db.NewsTags.ToList();

				return View("NewsBody", news);
			}
		}

		public ActionResult Weather()
		{
			try
			{
				using (var conn = new OdbcConnection())
				{
					conn.ConnectionString = "Driver={SQL Server}; Server=INSQL2; Database=runtime; Uid=wwuser; Pwd=wwuser;";
					conn.Open();

					using (var command = new OdbcCommand("SET QUOTED_IDENTIFIER OFF SELECT vValue FROM v_Live WHERE TagName IN (\"U1_Tnv_3_4\")", conn))
					{
						return Content(command.ExecuteScalar().ToString());

					}
				}
			}
			catch (Exception e)
			{
				return Content(e.Message);
			}
		}

		public JsonResult GetNews(int skip = 0, int count = 10, int guild = 0, string search = null)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var query = from n in db.News
								from u in db.Users.Where(x => x.UID == n.UserId).DefaultIfEmpty()
								where n.Priority != "back"
									&& n.IsTemplate == false
									&& n.GuildId == guild
									&& (n.DateAdd >= n.DateExpire || DateTime.Now <= n.DateExpire)
								orderby n.DateAdd descending
								select new News
								{
									Id = n.Id,
									Priority = n.Priority,
									DateAdd = n.DateAdd,
									DateExpire = n.DateExpire,
									Title = n.Title,
									Message = n.Message,
									Links = n.Links,
									GuildId = n.GuildId,
									UserName = string.IsNullOrEmpty(n.UserName) ? (string.IsNullOrEmpty(u.DisplayName) ? u.UName : u.DisplayName) : n.UserName,
								};

					if (string.IsNullOrEmpty(search))
					{
						query = query
							.Skip(skip)
							.Take(count);
					}
					else
					{
						query = query
							.Where(x => (x.Title + x.Message + x.Links).ToLower().Contains(search.ToLower()));
					}

					var news = query
						.ToList();

					return Json(new { News = news, Error = "" }, JsonRequestBehavior.AllowGet);
				}

			}
			catch (Exception e)
			{
				return Json(new { News = new List<News>(), Error = e }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}