using DatabaseLayer.Site;
using LinqToDB;
using Portal.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[Authorize]
	public class SafetyController : Controller
	{
		public ActionResult Index() => View();

		public ActionResult Week(DateTime? date) => View(model: date ?? DateTime.Today);

		public ActionResult SetDocViewed(int recordId, string Link)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				if (!db.AccidentDocs.Any(x => x.AccidentId == recordId && x.UserId == user.UID && x.Link == Link))
				{
					db.AccidentDocs
						.Value(x => x.AccidentId, recordId)
						.Value(x => x.UserId, user.UID)
						.Value(x => x.Link, Link)
						.Value(x => x.WhenViewed, DateTime.Now)
						.Insert();
				}
			}

			return Json(new { Done = true });
		}

		public ActionResult SetAccidentViewed(int recordId)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				if (!db.AccidentsViews.Any(x => x.AccidentId == recordId && x.UserId == user.UID))
				{
					db.AccidentsViews
						.Value(x => x.AccidentId, recordId)
						.Value(x => x.UserId, user.UID)
						.Value(x => x.Date, DateTime.Now)
						.Insert();
				}

				return Json(new { Done = true });
			}
		}

		public ActionResult Edit(int id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);
				if (!user.IsAccidents) return Json(new { Error = "Нет доступа" });

				var record = db.AccidentsRecords
					.Where(x => x.Id == id)
					.FirstOrDefault();

				if (record == null) return Json(new { Error = "Запись не найдена" });

				record.Creator = db.Users.FirstOrDefault(x => x.UID == record.UserId);

				var model = new SafetyEditModel
				{
					Record = record,
					Users = db.Users.Where(x => x.UClass.Contains("user")).ToList(),
					Views = db.AccidentsViews.Where(x => x.AccidentId == record.Id).ToList(),
				};

				return View(model);
			}
		}


		public ActionResult Create()
		{
			int id = 0;

			var date = DateTime.TryParse(Request.QueryString.Get("date"), out DateTime dd) ? dd : DateTime.Today;

			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);
				if (!user.IsAccidents) return Json(new { Error = "Нет доступа" });

				id = db.AccidentsRecords
					.Value(x => x.IsTemplate, true)
					.Value(x => x.DateCreated, DateTime.Now)
					.Value(x => x.DateControl, date)
					.Value(x => x.Date, DateTime.Today)
					.Value(x => x.Description, string.Empty)
					.Value(x => x.UserId, user.UID)
					.Value(x => x.IsDeleted, false)
					.InsertWithInt32Identity() ?? 0;
			}

			return RedirectToAction(nameof(Edit), new { Id = id });
		}

		[HttpPost]
		public ActionResult Update(AccidentRecord record)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);
				if (!user.IsAccidents) return Json(new { Error = "Нет доступа" });

				db.AccidentsRecords
					.Where(x => x.Id == record.Id)
					.Set(x => x.IsTemplate, false)
					.Set(x => x.Date, record.Date)
					.Set(x => x.DateControl, record.DateControl)
					.Set(x => x.Description, record.Description)
					.Update();

				if (!db.AccidentsRecordsLists.Any(x => x.RecordId == record.Id && x.ListId == 4))
				{
					db.AccidentsRecordsLists
						.Value(x => x.RecordId, record.Id)
						.Value(x => x.ListId, 4)
						.Insert();
				}

				return Json(new { Done = true });
			}
		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				if (!user.IsAccidents) return Json(new { Error = "Нет доступа" });

				db.AccidentsRecords
					.Where(x => x.Id == id)
					.Set(x => x.IsDeleted, true)
					.Update();
			}

			return Json(new { Done = true });
		}

		[HttpPost]
		public ActionResult Upload(int Id, HttpPostedFileBase file)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);
					if (!user.IsAccidents) return Json(new { Warning = "Нет доступа" });

					if (file != null)
					{
						// получаем имя файла
						string fileName = Path.GetFileName(file.FileName);

						// узнаем расширение
						string fileType = fileName.Substring(fileName.LastIndexOf('.') + 1);

						// конвертация документов word в pdf
						if (fileType == "doc" || fileType == "docx")
						{

						}

						if (!Directory.Exists(@"\\web\Files\Час ТБ\" + Id))
						{
							Directory.CreateDirectory(@"\\web\Files\Час ТБ\" + Id);
						}

						file.SaveAs(@"\\web\Files\Час ТБ\" + Id + "\\" + fileName);

						db.Insert(new AccidentLog
						{
							Date = DateTime.Now,
							UserId = user.UID,
							AccidentId = Id,
							Reason = "Прикреплён файл " + fileName,
						});

						return Json(new { Done = fileName });
					}
					else
					{
						return Json(new { Warn = "Переданный файл пуст" });
					}
				}
			}
			catch (Exception e)
			{
				return Json(new { Error = e.Message, e.StackTrace });
			}
		}

		[HttpPost]
		public ActionResult Remove(int Id, string Name)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);
					if (!user.IsAccidents) return Json(new { Warning = "Нет доступа" });

					if (Directory.Exists(@"\\web\Files\Час ТБ\" + Id))
					{
						if (System.IO.File.Exists(@"\\web\Files\Час ТБ\" + Id + "\\" + Name))
						{
							System.IO.File.Delete(@"\\web\Files\Час ТБ\" + Id + "\\" + Name);
						}
					}

					db.Insert(new AccidentLog
					{
						Date = DateTime.Now,
						UserId = user.UID,
						AccidentId = Id,
						Reason = "Откреплён файл " + Name,
					});

					return Json(new { Done = true });
				}
			}
			catch (Exception e)
			{
				return Json(new { Error = e.Message, e.StackTrace });
			}
		}
	}
}