using DatabaseLayer.Site;
using LinqToDB;
using Newtonsoft.Json;
using Portal.Areas.Directive.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Areas.Directive.Controllers
{
	[Authorize]
	public class PageController : Controller
	{
		public ActionResult Sort(string json)
		{
			try
			{
				var model = JsonConvert.DeserializeObject<SortedModel>(json);

				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					bool isRedactor = user.IsAdmin || db.UsersToPages
						.Count(x => x.UID == user.UID && x.PageId == model.PageId) > 0;

					if (!isRedactor)
						return Content("У вас нет доступа к редактированию этой страницы");

					foreach (var doc in model.Documents)
					{
						db.DirectiveDocuments
							.Where(x => x.Id == doc.Id)
							.Set(x => x.OrderValue, doc.Order)
							.Set(x => x.SectionId, doc.SectionId)
							.Update();
					}

					foreach (var sect in model.Sections)
					{
						db.DirectiveSections
							.Where(x => x.Id == sect.Id)
							.Set(x => x.OrderValue, sect.Order)
							.Update();
					}

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
						+ " выполнил сортировку на странице [" + model.PageId + "]",
					});

					return Content("done");
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}

		public ActionResult SetUsers(int pageId, string users)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					if (!user.IsAdmin)
						return Content("У вас нет разрешения на редактирование списка пользователей");

					string[] oldUsers = db.UsersToPages
						.Where(x => x.PageId == pageId)
						.Select(x => "[" + x.UID + "]")
						.ToArray();

					db.UsersToPages
						.Where(x => x.PageId == pageId)
						.Delete();

					foreach (var uid in JsonConvert.DeserializeObject<int[]>(users))
					{
						db.Insert(new UsersToPages
						{
							PageId = pageId,
							UID = uid,
						});
					}

					string[] newUsers = db.UsersToPages
						.Where(x => x.PageId == pageId)
						.Select(x => "[" + x.UID + "]")
						.ToArray();

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
						+ " изменил список редакторов страницы [" + pageId + "] с { "
						+ string.Join(", ", oldUsers)
						+ " } на { "
						+ string.Join(", ", newUsers)
						+ " }",
					});

					return Content("done");
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}
	}
}