using DatabaseLayer.Site;
using Docs.Models;
using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;

namespace Docs.Controllers
{
	public class SectionsController : Controller
	{
		public ActionResult Add(string title, int pageId, int sectionId)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var maxOrder = db.DocsSections
						.Where(x => x.PageId == pageId && x.SectionId == sectionId)
						.Select(x => x.OrderValue)
						.DefaultIfEmpty(0)
						.Max();

					db.DocsSections
						.Value(x => x.Title, title)
						.Value(x => x.PageId, pageId)
						.Value(x => x.SectionId, sectionId)
						.Value(x => x.OrderValue, ++maxOrder)
						.Insert();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Edit(string title, int id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.DocsSections
						.Where(x => x.Id == id)
						.Set(x => x.Title, title)
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
					db.DocsSections
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

		public ActionResult Reorder(string json)
		{
			try
			{
				var list = JsonConvert.DeserializeObject<List<ReorderModel>>(json);
				var orderValue = 0;

				using (var db = new SiteContext())
				{
					foreach (var el in list)
					{
						if (el.IsSection)
						{
							db.DocsSections
								.Where(x => x.Id == el.Id)
								.Set(x => x.OrderValue, orderValue++)
								.Update();
						}
						else
						{
							db.DocsDocuments
								.Where(x => x.Id == el.Id)
								.Set(x => x.OrderValue, orderValue++)
								.Update();
						}
					}
				}

				return Json(new { Done = true });
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Move(int id, int sectionId)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.DocsSections
						.Where(x => x.Id == id)
						.Set(x => x.SectionId, sectionId)
						.Update();
				}

				return Json(new { Done = true });
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}
	}
}