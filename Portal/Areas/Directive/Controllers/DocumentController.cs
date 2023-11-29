using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Areas.Directive.Controllers
{
	[Authorize]
	public class DocumentController : Controller
	{
		public ActionResult Add(string name, int sectionId)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					var section = db.DirectiveSections
						.FirstOrDefault(x => x.Id == sectionId);

					if (section == null)
						return Content("Раздел не найден");

					bool isRedactor = user.IsAdmin || db.UsersToPages
						.Count(x => x.UID == user.UID && x.PageId == section.PageId) > 0;

					if (!isRedactor)
						return Content("У вас нет доступа к редактированию этой страницы");

					var file = Request.Files.Count > 0 ? Request.Files[ 0 ] : null;

					if (file == null)
						return Content("Файл не передан");

					string folderPath = @"\\web\files\Директива №1\" + sectionId;

					if (db.DirectiveDocuments.Count(x => x.SectionId == section.Id && x.Name == name) > 0)
						return Content("Файл с таким наименованием уже есть в данном разделе");

					if (!Directory.Exists(folderPath))
						Directory.CreateDirectory(folderPath);

					string filePath = (folderPath + "\\" + file.FileName)
						.Replace("+", "");

					if (System.IO.File.Exists(filePath))
						return Content("данный файл уже есть в данном разделе");

					file.SaveAs(filePath);

					if (string.IsNullOrEmpty(name)) 
						name = file.FileName.Substring(0, file.FileName.LastIndexOf('.'));

					int id = db.InsertWithInt32Identity(new DirectiveDocument
					{
						SectionId = sectionId,
						Name = name.Replace("+", ""),
						FilePath = filePath,
						OrderValue = 0,
						WhenUpdated = DateTime.Now,
					});

					db.DirectiveDocumentsViews
						.Value(x => x.UserId, user.UID)
						.Value(x => x.DocumentId, id)
						.Value(x => x.WhenSeen, DateTime.Now)
						.Insert();

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
						+ " добавил файл [" + id + "] \"" + file.FileName 
						+ "\" в раздел [" + sectionId + "] \"" + section.Caption + "\""
						+ "\" с наименованием \"" + name + "\"",
					});

					return Content("done");
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}

		public ActionResult Edit(int id, string name)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					var doc = db.DirectiveDocuments
						.FirstOrDefault(x => x.Id == id);

					if (doc == null)
						return Content("Файл не найден");

					var section = db.DirectiveSections
						.FirstOrDefault(x => x.Id == doc.SectionId);

					if (section == null)
						return Content("Раздел не найден");

					bool isRedactor = user.IsAdmin || db.UsersToPages
						.Count(x => x.UID == user.UID && x.PageId == section.PageId) > 0;

					if (!isRedactor)
						return Content("У вас нет доступа к редактированию этой страницы");

					if (db.DirectiveDocuments.Count(x => x.SectionId == section.Id && x.Id != id && x.Name == name) > 0)
						return Content("В данном разделе уже есть другой файл с таким наименованием");

					string oldName = db.DirectiveDocuments
						.Where(x => x.Id == id)
						.FirstOrDefault()?.Name ?? "";

					db.DirectiveDocuments
						.Where(x => x.Id == id)
						.Set(x => x.Name, name)
						.Update();

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
						+ " изменил наименование файла [" + id + "] c \"" + oldName
						+ "\" на  [" + name + "] \"" + section.Caption + "\"",
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

					var doc = db.DirectiveDocuments
						.FirstOrDefault(x => x.Id == id);

					if (doc == null)
						return Content("Файл не найден");

					var section = db.DirectiveSections
						.FirstOrDefault(x => x.Id == doc.SectionId);

					if (section == null)
						return Content("Раздел не найден");

					bool isRedactor = user.IsAdmin || db.UsersToPages
						.Count(x => x.UID == user.UID && x.PageId == section.PageId) > 0;

					if (!isRedactor)
						return Content("У вас нет доступа к редактированию этой страницы");

					string folderPath = @"\\web\files\Директива №1\" + section.Id;

					if (System.IO.File.Exists(doc.FilePath))
					{
						System.IO.File.Delete(doc.FilePath);
					}

					string oldName = db.DirectiveDocuments
						.Where(x => x.Id == id)
						.FirstOrDefault()?.Name ?? "";

					db.DirectiveDocuments
						.Delete(x => x.Id == id);

					db.Insert(new DirectiveLog
					{
						Text = "Пользователь " + (user.DisplayName ?? user.UName)
						+ " удалил файл [" + id + "] c наименованием \"" + oldName + "\"",
					});

					return Content("done");
				}
			}
			catch (Exception e)
			{
				return Content("Произошла ошибка!\n" + e.Message);
			}
		}

		public ActionResult SetViewed(int docId)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					if (!db.DirectiveDocumentsViews.Any(x => x.UserId == user.UID && x.DocumentId == docId))
					{
						db.DirectiveDocumentsViews
							.Value(x => x.UserId, user.UID)
							.Value(x => x.DocumentId, docId)
							.Value(x => x.WhenSeen, DateTime.Now)
							.Insert();
					}

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