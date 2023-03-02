using DatabaseLayer.Site;
using LinqToDB;
using NPOI.HSSF.UserModel;
using Portal.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [Authorize]
	public class ViolationsController : Controller
    {
        // views

        public ActionResult Index() => View();

        public ActionResult Search([Bind(Include = "Search,Exact,From,To,FS1,FS2,FS3,FS4,FS5,FS6,FS7,FS8,FS9,FVD1,FVD2,FV1,FV2,FV3,FV4,FV5,FV6,FV7,FV8,FV9,Sort,Revert")] ViolationsFilter filter)
        {
            if (!Request.QueryString.HasKeys())
			{
                filter.FS1 = true;
                filter.FS2 = true;
                filter.FS3 = true;
                filter.FS6 = true;
                filter.FS7 = true;

                filter.FVD2 = true;
                filter.FV1 = true;
                filter.FV3 = true;
                filter.FV7 = true;
            }

            return View(filter); 
        }


        // crud

        public ActionResult Create()
        {
            using (var db = new SiteContext())
			{
                var user = db.Authorize(User);
                if (!user.IsViolationsAdmin) return Content("нет доступа");

                ViewBag.User = user;
			}

            return View();
        }

		public ActionResult Details(int Id)
		{
            using (var db = new SiteContext())
			{
                var user = db.Authorize(User);
                if (!user.IsViolationsUser) return Content("нет доступа");

                ViewBag.User = user;

                var violation = db.Violations
                    .FirstOrDefault(x => x.Id == Id);

                if (violation == null) return Content("Запись не найдена");

                return View(violation);
			}
        }
        
        public ActionResult Edit(int Id)
        {
            using (var db = new SiteContext())
            {
                var user = db.Authorize(User);
                if (!user.IsViolationsAdmin) return Content("нет доступа");

                ViewBag.User = user;

                var violation = db.Violations
                    .FirstOrDefault(x => x.Id == Id);

                if (violation == null) return Content("Запись не найдена");

                return View(violation);
            }
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "DateViolation,PersonName,PersonPosition,PersonDepartment,HeadName,ProcessName,Description,Result,OrderId,JudgeName")] Violation violation)
        {
            try
            {
                using (var db = new SiteContext())
                {
                    var user = db.Authorize(User);
                    if (!user.IsViolationsAdmin) return Json(new { Error = "нет доступа" });

                    violation.DateCreated = DateTime.Now;
                    violation.IsDeleted = false;

                    int id = db.InsertWithInt32Identity(violation);
                }

                return Json(new { Done = "Запись создана! Будет выполнено перенаправление на главную страницу", Link = Url.Action("", "violations") });
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Edit(int Id, [Bind(Include = "DateViolation,PersonName,PersonPosition,PersonDepartment,HeadName,ProcessName,Description,Result,OrderId,JudgeName")] Violation violation)
        {
            try
            {
                using (var db = new SiteContext())
                {
                    var user = db.Authorize(User);
                    if (!user.IsViolationsAdmin) return Json(new { Error = "нет доступа" });

                    db.Violations
                        .Where(x => x.Id == Id)
                        .Set(x => x.DateViolation, violation.DateViolation)
                        .Set(x => x.PersonName, violation.PersonName)
                        .Set(x => x.PersonPosition, violation.PersonPosition)
                        .Set(x => x.PersonDepartment, violation.PersonDepartment)
                        .Set(x => x.HeadName, violation.HeadName)
                        .Set(x => x.ProcessName, violation.ProcessName)
                        .Set(x => x.Description, violation.Description)
                        .Set(x => x.Result, violation.Result)
                        .Set(x => x.OrderId, violation.OrderId)
                        .Set(x => x.JudgeName, violation.JudgeName)
                        .Update();
                }

                return Json(new { Done = "Запись изменена!", Link = Url.Action("details", "violations", new { Id }) });
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message });
            }
        }

        [HttpPost]
        public ActionResult Delete(int Id)
        {
            try
            {
                using (var db = new SiteContext())
                {
                    var user = db.Authorize(User);
                    if (!user.IsViolationsAdmin) return Json(new { Error = "нет доступа" });

                    db.Violations
                        .Where(x => x.Id == Id)
                        .Set(x => x.IsDeleted, true)
                        .Update();

                    return Json(new { Done = "Запись удалена! Будет выполнено перенаправление на главную страницу", Link = Url.Action("", "violations") });
                }
            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message });
            }
        }


        // export

        public ActionResult Export(int Id)
        {
            using (var db = new SiteContext())
            {
                var person = db.Persons
                    .FirstOrDefault(x => x.TabId == Id);

                var violations = db.Violations
                    .Where(x => x.TabId == Id)
                    .OrderByDescending(x => x.DateCreated)
                    .ToList();

                var book = new HSSFWorkbook();
                var sheet = book.CreateSheet("Отчет о нарушениях");

                sheet.CreateRow(0).CreateCell(0).SetCellValue("Сотрудник: " + person.Family);
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 3));

                var header = sheet.CreateRow(1);
                header.CreateCell(0).SetCellValue("Дата");
                header.CreateCell(1).SetCellValue("№ приказа");
                header.CreateCell(2).SetCellValue("Нарушение");
                header.CreateCell(3).SetCellValue("Меры");

                sheet.SetColumnWidth(0, 12 * 256);
                sheet.SetColumnWidth(1, 16 * 256);
                sheet.SetColumnWidth(2, 21 * 256);
                sheet.SetColumnWidth(3, 35 * 256);

                for (int i = 0; i < violations.Count; i++)
                {
                    var row = sheet.CreateRow(i + 2);
                    row.CreateCell(0).SetCellValue(violations[i].DateCreated.ToString("dd.MM.yyyy"));
                    row.CreateCell(1).SetCellValue(violations[i].OrderId);
                    row.CreateCell(2).SetCellValue(violations[i].Description);
                    row.CreateCell(3).SetCellValue(violations[i].Result);

                    row.GetCell(0).CellStyle.WrapText = true;
                    row.GetCell(1).CellStyle.WrapText = true;
                    row.GetCell(2).CellStyle.WrapText = true;
                    row.GetCell(3).CellStyle.WrapText = true;
                }

                string path = $"Нарушения {person.Family}.xls";
                using (var file = new FileStream(Server.MapPath(@"/content/documents/Нарушения " + person.Family + ".xls"), FileMode.Create))
                {
                    book.Write(file);
                    file.Close();
                }

                return Content("/content/documents/" + path);
            }
        }
    }
}