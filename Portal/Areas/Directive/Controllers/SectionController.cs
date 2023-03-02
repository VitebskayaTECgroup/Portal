using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Areas.Directive.Controllers
{
	[Authorize]
	public class SectionController : Controller
	{
		public ActionResult Add(int pageId, string caption)
		{
			try 
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					bool isRedactor = user.IsAdmin || db.UsersToPages
						.Count(x => x.UID == user.UID && x.PageId == pageId) > 0;

					if (!isRedactor)
						return Content("У вас нет доступа к редактированию этой страницы");

					if (db.DirectiveSections.Count(x => x.PageId == pageId && x.Caption == caption) > 0)
						return Content("На странице уже есть другой раздел с таким наименованием");

					int id = db.InsertWithInt32Identity(new DirectiveSection
					{
						PageId = pageId,
						Caption = caption,
						OrderValue = 0,
					});

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
						+ " добавил раздел [" + id + "] \"" + caption
						+ "\" на страницу [" + pageId + "]",
					});

					return Content("done");
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}

		public ActionResult Edit(int id, string caption)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);
					int pageId = db.DirectiveSections
						.Where(x => x.Id == id)
						.Select(x => x.PageId)
						.DefaultIfEmpty(0)
						.FirstOrDefault();

					if (pageId == 0)
						return Content("Страница не найдена");

					bool isRedactor = user.IsAdmin || db.UsersToPages
						.Count(x => x.UID == user.UID && x.PageId == pageId) > 0;

					if (!isRedactor)
						return Content("У вас нет доступа к редактированию этой страницы");

					if (db.DirectiveSections.Count(x => x.PageId == pageId && x.Id != id && x.Caption == caption) > 0)
						return Content("На странице уже есть другой раздел с таким наименованием");

					string oldCaption = db.DirectiveSections
						.Where(x => x.Id == id)
						.FirstOrDefault()?.Caption ?? "";

					db.DirectiveSections
						.Where(x => x.Id == id)
						.Set(x => x.Caption, caption)
						.Update();

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
						+ " изменил наименование раздела [" + id + "] на странице [" + pageId + "] c \"" + oldCaption
						+ "\" на  [" + caption + "]",
					});

					return Content("done");
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}

		public ActionResult Del(int id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);
					int pageId = db.DirectiveSections
						.Where(x => x.Id == id)
						.Select(x => x.PageId)
						.DefaultIfEmpty(0)
						.FirstOrDefault();

					if (pageId == 0)
						return Content("Страница не найдена");

					bool isRedactor = user.IsAdmin || db.UsersToPages
						.Count(x => x.UID == user.UID && x.PageId == pageId) > 0;

					if (!isRedactor)
						return Content("У вас нет доступа к редактированию этой страницы");

					string oldCaption = db.DirectiveSections
						.Where(x => x.Id == id)
						.FirstOrDefault()?.Caption ?? "";

					db.DirectiveDocuments
						.Where(x => x.SectionId == id)
						.Delete();

					db.DirectiveSections
						.Where(x => x.Id == id)
						.Delete();

					if (Directory.Exists(@"\\web\files\Директива №1\" + id))
						Directory.Delete(@"\\web\files\Директива №1\" + id, true);

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
						+ " удалил раздел [" + id + "] \"" + oldCaption
						+ "\" на странице  [" + pageId + "]",
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