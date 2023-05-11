using DatabaseLayer.Site;
using LinqToDB;
using Schemes.Models;
using System.IO;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Schemes.Controllers
{
	public class DocumentsController : Controller
	{
		public ActionResult Form(int Id = 0, int ContainerId = 0)
		{
			using (var db = new SiteContext())
			{
				if (Id == 0)
				{
					Id = db.SchemeDocuments
						.Value(x => x.ContainerId, ContainerId)
						.Value(x => x.Name, "")
						.Value(x => x.TagsString, "")
						.Value(x => x.Link, "")
						.InsertWithInt32Identity() ?? 0;

					if (Id == 0) return Content("Ошибка: не получен Id нового документа");
				}

				var model = new DocumentForm
				{
					Tags = db.SchemeTags.ToList(),
					Document = db.SchemeDocuments.FirstOrDefault(x => x.Id == Id),
				};

				if (model.Document == null) return Content("Ошибка: не найден документ по Id = " + Id);

				return View(model);
			}
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

				if (Directory.Exists(@"\\web\Files\Схемы\" + id)) Directory.Delete(@"\\web\Files\Схемы\" + id, true);
				Directory.CreateDirectory(@"\\web\Files\Схемы\" + id);
				document.SaveAs(@"\\web\Files\Схемы\" + id + "\\" + file);

				using (var db = new SiteContext())
				{
					db.SchemeDocuments
						.Where(x => x.Id == id)
						.Set(x => x.Link, file)
						.Update();
				}

				return Json(new { Done = file });
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}

		public ActionResult Edit(int Id, string Name, string TagsString)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var link = db.SchemeDocuments
						.Where(x => x.Id == Id)
						.Select(x => x.Link)
						.FirstOrDefault();

					if (string.IsNullOrEmpty(link)) return Json(new { Error = "Не добавлен документ!" });

					db.SchemeDocuments
						.Where(x => x.Id == Id)
						.Set(x => x.Name, Name)
						.Set(x => x.TagsString, TagsString)
						.Update();

					return Json(new { Done = "Документ сохранён" });
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
					db.SchemeDocuments
						.Where(x => x.Id == Id)
						.Delete();

					if (Directory.Exists(@"\\web\Files\Схемы\" + Id)) Directory.Delete(@"\\web\Files\Схемы\" + Id, true);

					return Json(new { Done = "Документ удалён" });
				}
			}
			catch (Exception ex)
			{
				return Json(new { Error = ex.Message });
			}
		}
	}
}