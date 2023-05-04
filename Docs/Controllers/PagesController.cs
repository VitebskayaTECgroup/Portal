using DatabaseLayer.Site;
using LinqToDB;
using LinqToDB.Tools;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Docs.Controllers
{
	public class PagesController : Controller
	{
		[AllowAnonymous]
		public ActionResult Index(int Id)
		{
			using (var db = new SiteContext())
			{
				var page = db.DocsPages
					.FirstOrDefault(x => x.Id == Id);

				if (page == null)
				{
					return Content("Страница не найдена");
				}

				var user = db.Authorize(User);

				ViewBag.Title = page.Name;
				ViewBag.IsDocsAdmin = user.IsDocsAdmin;

				var sections = db.DocsSections
					.Where(x => x.PageId == page.Id)
					.ToList();

				var documents = db.DocsDocuments
					.Where(x => x.PageId == page.Id)
					.Where(x => !x.IsTemplate)
					.ToList();

				foreach (var section in sections)
				{
					section.Documents = documents
						.Where(x => x.SectionId == section.Id)
						.ToList();
				}

				var firstRankSections = sections
					.Where(x => x.SectionId == 0)
					.ToList();

				foreach (var section in firstRankSections)
				{
					section.LoadSectionsFromAll(sections);
				}

				page.Elements.AddRange(firstRankSections);
				page.Elements.AddRange(documents.Where(x => x.SectionId == 0));

				return View("Page", page);
			}
		}

		public ActionResult Redactor(int Id)
		{
			using (var db = new SiteContext())
			{
				var page = db.DocsPages
					.FirstOrDefault(x => x.Id == Id);

				if (page == null)
				{
					return Content("Страница не найдена");
				}

				var user = db.Authorize(User);

				ViewBag.Title = page.Name;
				ViewBag.IsDocsAdmin = user.IsDocsAdmin;

				var sections = db.DocsSections
					.Where(x => x.PageId == page.Id)
					.ToList();

				var documents = db.DocsDocuments
					.Where(x => x.PageId == page.Id)
					.Where(x => !x.IsTemplate)
					.ToList();

				foreach (var doc in documents)
				{
					doc.IsRedactor = user.IsDocsAdmin;
				}

				foreach (var section in sections)
				{
					section.Documents = documents
						.Where(x => x.SectionId == section.Id)
						.ToList();
				}

				var firstRankSections = sections
					.Where(x => x.SectionId == 0)
					.ToList();

				foreach (var section in firstRankSections)
				{
					section.LoadSectionsFromAll(sections);
				}

				page.Elements.AddRange(firstRankSections);
				page.Elements.AddRange(documents.Where(x => x.SectionId == 0));

				return View(page);
			}
		}

		public ActionResult Add(string name, int menuId)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.DocsPages
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
					db.DocsPages
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
					db.DocsPages
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