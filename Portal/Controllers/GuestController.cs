using DatabaseLayer.Phonebook;
using DatabaseLayer.Site;
using LinqToDB;
using NPOI.SS.Formula.Functions;
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

		public ActionResult News(int take, int skip = 0)
		{
			using (var db = new SiteContext())
			{
				ViewBag.NewsTags = db.NewsTags
					.ToDictionary(x => x.Token, x => x.Name);

				var news = db.News
					.Where(x => !x.IsTemplate)
					.Where(x => x.DateExpire <= x.DateAdd || x.DateExpire > DateTime.Now)
					.Where(x => skip == 0 || x.Id < skip)
					.OrderByDescending(x => x.DateAdd)
					.Select(x => new News
					{
						Id = x.Id,
						DateAdd = x.DateAdd,
						DateExpire = x.DateExpire,
						Title = x.Title,
						Tags = x.Tags,
						Priority = x.Priority,
						Links = x.Links,
					})
					.Take(take)
					.ToList();

				return View("NewsBlock", news);
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

				var news = query.FirstOrDefault();

				return View("NewsContent", news);
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
			catch
			{
				return Content("?");
			}
		}

		public ActionResult Contacts(string search)
		{
			if (string.IsNullOrEmpty(search)) return Content("");

			return View(model: search);
		}
	}
}