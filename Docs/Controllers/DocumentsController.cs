using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Docs.Controllers
{
	public class DocumentsController : Controller
	{
		public ActionResult Form(int id, int pageid = 0, int sectionId = 0)
		{
			return View("Form", new DocsDocument { Id = id, PageId = pageid, SectionId = sectionId });
		}

		public ActionResult Add(int pageId, int sectionId)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var maxOrder = db.DocsDocuments
						.Where(x => x.PageId == pageId && x.SectionId == sectionId)
						.Select(x => x.OrderValue)
						.DefaultIfEmpty(0)
						.Max();

					var id = db.DocsDocuments
						.Value(x => x.PageId, pageId)
						.Value(x => x.SectionId, sectionId)
						.Value(x => x.Caption, string.Empty)
						.Value(x => x.Link, string.Empty)
						.Value(x => x.Text, string.Empty)
						.Value(x => x.Type, "text")
						.Value(x => x.TagsString, string.Empty)
						.Value(x => x.ViewMode, string.Empty)
						.Value(x => x.IsTemplate, true)
						.Value(x => x.OrderValue, ++maxOrder)
						.InsertWithInt32Identity() ?? 0;

					return Form(id, pageId, sectionId);
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Edit(int id, string caption, string text, string type, string tagsString, string viewMode)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.DocsDocuments
						.Where(x => x.Id == id)
						.Set(x => x.Caption, caption)
						.Set(x => x.Text, text)
						.Set(x => x.Type, type)
						.Set(x => x.TagsString, tagsString)
						.Set(x => x.ViewMode, viewMode)
						.Set(x => x.IsTemplate, false)
						.Update();

					return Json(new { Done = true });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Delete(int id)
		{
			return View("Delete", id);
		}

		public ActionResult Upload(int id)
		{
			try
			{
				if (Request.Files.Count == 0) throw new Exception("Файл не получен");

				var document = Request.Files[0];
				var file = document.FileName.Contains('\\')
					? document.FileName.Substring(document.FileName.LastIndexOf('\\') + 1)
					: document.FileName;
				string fileType = file.Substring(file.LastIndexOf(".") + 1).ToLower();
				string[] allowedTypes = new string[] { "jpg", "png", "gif", "jpeg", "tiff", "tif", "txt", "doc", "docx", "rtf", "zip", "xls", "xlsx", "pdf", "bmp", "avi", "mp4", "ppt", "pptx" };

				if (!allowedTypes.Contains(fileType)) throw new Exception("Файлы такого типа не разрешены");

				if (Directory.Exists(@"\\web\Files\Docs\" + id)) Directory.Delete(@"\\web\Files\Docs\" + id, true);
				Directory.CreateDirectory(@"\\web\Files\Docs\" + id);
				document.SaveAs(@"\\web\Files\Docs\" + id + "\\" + file);
				
				using (var db = new SiteContext())
				{
					db.DocsDocuments
						.Where(x => x.Id == id)
						.Set(x => x.Link, file)
						.Update();
				}

				return Json(new { Done = "Файл загружен", Name = file, Link = "http://www.vst.vitebsk.energo.net/files/docs/" + id + "/" + file });
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
					db.DocsDocuments
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

		public ActionResult Move(int id, int sectionId)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.DocsDocuments
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