using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Areas.Accidents.Controllers
{
    [Authorize]
	public class RecordsController : Controller
    {
		public ActionResult Index() => View();

		public ActionResult Lists() => View();

		public ActionResult List(int Id) => View(model: Id);

		public ActionResult Details(int Id) => View(model: Id);

		public ActionResult Edit(int Id) => View(model: Id);

		public ActionResult Add()
		{ 
			using (var db = new SiteContext())
			{
				// Удаление старых черновиков
				db.AccidentsRecords
					.Where(x => (DateTime.Today - x.DateCreated).TotalDays >= 7)
					.Where(x => x.IsTemplate)
					.Delete();

				var user = db.Authorize(User);
				if (!user.IsAccidents) return Json(new { Warning = "Нет доступа" });

				int id = db.InsertWithInt32Identity(new AccidentRecord
				{
					UserId = user.UID,
					DateCreated = DateTime.Now,
					Date = DateTime.Today,
					Description = "-",
					DateControl = DateTime.Today,
					IsTemplate = true,
					IsDeleted = false,
				});

				db.Insert(new AccidentLog
				{ 
					Date = DateTime.Now,
					UserId = user.UID,
					AccidentId = id,
					Reason = "Запись создана",
				});

				return RedirectToAction("edit", new { Id = id });
			}
		}

		[HttpPost]
		public ActionResult Edit(int Id, DateTime Date, DateTime DateControl, string Description)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);
				if (!user.IsAccidents) return Json(new { Warning = "Нет доступа" });

				db.AccidentsRecords
					.Where(x => x.Id == Id)
					.Set(x => x.Date, Date)
					.Set(x => x.DateControl, DateControl)
					.Set(x => x.Description, Description)
					.Set(x => x.IsTemplate, false)
					.Update();

				db.Insert(new AccidentLog
				{
					Date = DateTime.Now,
					UserId = user.UID,
					AccidentId = Id,
					Reason = "Запись обновлена: " +
						"Дата [" + Date.ToString("dd.MM.yyyy") + "], " +
						"Описание [" + Description + "], " +
						"Дата ознакомления [" + DateControl.ToString("dd.MM.yyyy") + "]",
				});
			}

			return Json(new { Done = "Запись успешно сохранена", Link = Url.Action("", "records") });
		}

		[HttpPost]
		public ActionResult Delete(int Id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);
				if (!user.IsAccidents) return Json(new { Warning = "Нет доступа" });

				db.AccidentsRecords
					.Where(x => x.Id == Id)
					.Set(x => x.IsDeleted, true)
					.Update();

				db.Insert(new AccidentLog
				{
					Date = DateTime.Now,
					UserId = user.UID,
					AccidentId = Id,
					Reason = "Запись удалена",
				});
			}

			return Json(new { Done = true });
		}

		// actions

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

		[HttpPost]
		public ActionResult SetViewed(int Id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					if (db.AccidentsViews.Count(x => x.UserId == user.UID && x.AccidentId == Id) > 0)
					{ 
						return Json(new { Done = true }); 
					}

					db.Insert(new AccidentView
					{
						UserId = user.UID,
						AccidentId = Id,
						Date = DateTime.Now,
					});
				}

				return Json(new { Done = true });
			}
			catch (Exception e)
			{
				return Json(new { Error = e.Message });
			}
		}

		[HttpPost]
		public ActionResult SetRecordToList(int Id, int ListId)
        {
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);

					var oldRelation = db.AccidentsRecordsLists
						.Where(x => x.ListId == ListId && x.RecordId == Id)
						.FirstOrDefault();

					db.AccidentsRecordsLists
						.Delete(x => x.ListId == ListId && x.RecordId == Id);

					db.Insert(new AccidentRecordList
					{
						ListId = ListId,
						RecordId = Id,
					});
				}

				return Json(new { Done = true });
			}
			catch (Exception e)
			{
				return Json(new { Error = e.Message });
			}
		}
	}
}