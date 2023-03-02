using DatabaseLayer.Site;
using LinqToDB;
using Portal.Areas.Directive.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Areas.Directive.Controllers
{
	[Authorize]
	public class MainController : Controller
	{
		public ActionResult Index()
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					var pages = db.DirectivePages
						.OrderBy(x => x.OrderValue)
						.Where(x => string.IsNullOrEmpty(x.AllowedGroup) || user.GroupsArray.Contains(x.AllowedGroup))
						.ToList();

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName) + " перешел на корневой раздел",
					});

					return View("Index", pages);
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}

		public ActionResult Page(string pageName)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					var page = db.DirectivePages
						.Where(x => x.Url == pageName)
						.FirstOrDefault();

					if (page == null)
						return Content("Раздел с таким наименованием не определён");
					
					if (!string.IsNullOrEmpty(page.AllowedGroup))
					{
						if (!user.GroupsArray.Contains(page.AllowedGroup.ToLower())) 
							return Content(//"Page: " + pageName + "\n" +
								//"AllowedGroup: " + page.AllowedGroup + "\n" +
								//"GroupsArray: " + string.Join(" ", user.GroupsArray) + "\n" +
								"Доступ запрещён");
					}

					var redactors = db.UsersToPages
						.Where(x => x.PageId == page.Id)
						.Select(x => x.UID);

					page.Redactors = db.Users
						.Where(x => redactors.Contains(x.UID))
						.ToList();

					page.IsRedactor = db.UsersToPages
						.Count(x => x.UID == user.UID && x.PageId == page.Id) > 0;

					page.IsAdmin = user.IsAdmin;

					page.Sections = db.DirectiveSections
						.Where(x => x.PageId == page.Id)
						.OrderBy(x => x.OrderValue)
						.ToList();

					foreach (var sect in page.Sections)
					{
						sect.Documents = db.DirectiveDocuments
							.Where(x => x.SectionId == sect.Id)
							.OrderBy(x => x.OrderValue)
							.ToList();
					}

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName) 
						+ " перешел в раздел [" + page.Id + "] \"" + page.Caption 
						+ "\" как " + (page.IsRedactor ? "редактор" : "пользователь"),
					});

					return View(page.IsRedactor ? "Redactor" : "Page", page);
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}

		public ActionResult Admin(int Id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					if (!user.IsAdmin)
						return Content("У вас нет доступа в этот раздел");

					var page = db.DirectivePages
						.FirstOrDefault(x => x.Id == Id);

					if (page == null)
						return Content("Страница не найдена");

					var model = new AdminPageModel()
					{
						Page = page,
						Users = db.Users
							.Where(x => x.UName != "guest")
							.OrderBy(x => x.DisplayName)
							.ThenBy(x => x.UName)
							.ToList(),
						PageUsers = db.UsersToPages
							.Where(x => x.PageId == Id)
							.Select(x => x.UID)
							.ToList()
					};

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
							+ " перешел в раздел [" + page.Id + "] \"" + page.Caption
							+ "\" как администратор",
					});

					return View("Admin", model);
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!" + e.Message);
			}
		}

		public ActionResult Search()
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName) + " перешел на страницу поиска",
					});

					return View("Search");
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}

		public ActionResult Results(string query)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					var sql = from d in db.DirectiveDocuments.Where(x => x.Name.ToLower().Contains(query.ToLower()))
							  from s in db.DirectiveSections.LeftJoin(x => x.Id == d.SectionId)
							  from p in db.DirectivePages.LeftJoin(x => x.Id == s.PageId)
							  orderby d.Name
							  select new SearchResult
							  {
								  PageUrl = p.Url,
								  Page = p.Caption,
								  Section = s.Caption,
								  Document = d,
							  };

					var model = sql.ToList();

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName) + " выполнил поиск по запросу \"" + query + "\"",
					});

					return View("Results", model);
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}
	}
}