using DatabaseLayer.Site;
using LinqToDB;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[Authorize]
    public class AutoController : Controller
    {
        public ActionResult Index() => View();

        public ActionResult List() => View();

        public ActionResult Drivers() => View();

        public ActionResult Cars() => View();

        public ActionResult Form(int? Id) => View("Form", model: Id ?? 0);

        public ActionResult Copy(int Id)
        {
            ViewBag.IsCopy = true;
            return Form(Id);
        }

        public ActionResult AnswerForm(int? Id) => View(model: Id ?? 0);


        // Заявки на автомобили

        [HttpPost]
        public string Create()
        {
            var request = new Request();

            var errors = CheckForm(ref request, Request.Form);
            if (errors.Count != 0)
            {
                return "<div class='auto_errors'>" + string.Join("<br />", errors) + "</div>";
            }

            using (var db = new SiteContext())
            {
                var user = db.Authorize(User);

                request.CreationUserId = user.UID;
                request.CreationDate = DateTime.Now;

                int id = db.InsertWithInt32Identity(request);

                var admins = db.Users.Where(x => x.UClass.Contains("auto_prov")).Select(x => x.UID).ToList();

                foreach (var uid in admins)
                {
                    db.Insert(new Notification
                    {
                        N_UID = uid,
                        N_Watched = false,
                        N_DateCreated = DateTime.Now,
                        N_DateWatched = null,
                        N_App = "Заказ авто",
                        N_Note = user.DisplayName + " создал запрос на предоставление авто на " + request.Date.ToString("d MMMM yyyy"),
                        N_CreateUID = user.UID,
                        N_Link = Url.Action("index", "auto") + "?id=" + id
                    });
                }

                return request.Date.ToString("dd.MM.yyyy");
            }
        }

        [HttpPost]
        public string Update(int Id)
        {
            var request = new Request();

            var errors = CheckForm(ref request, Request.Form);
            if (errors.Count != 0)
            {
                return "<div class='auto_errors'>" + string.Join("<br />", errors) + "</div>";
            }

            using (var db = new SiteContext())
            {
                var user = db.Authorize(User);

                db.AutoRequests
                    .Where(x => x.Id == Id)
                    .Set(x => x.CreationUserId, user.UID)
                    .Set(x => x.CreationDate, DateTime.Now)
                    .Set(x => x.Date, request.Date)
                    .Set(x => x.TimeStart, request.TimeStart)
                    .Set(x => x.TimeEnd, request.TimeEnd)
                    .Set(x => x.Location, request.Location)
                    .Set(x => x.Target, request.Target)
                    .Update();

                return request.Date.ToString("dd.MM.yyyy");
            }
        }

        [HttpPost]
        public void Delete(int Id)
        {
            using (var db = new SiteContext())
            {
                db.AutoRequests.Delete(x => x.Id == Id);
                db.Notes.Delete(x => x.N_App == "Заказ авто" && x.N_Link.Contains("?id=" + Id));
            }
        }

        [HttpPost]
        public string Answer([Bind(Include = "Id,Comment")] Request request)
        {
            request.DecisionCode = int.TryParse(Request.Form.Get("answerCode") ?? "", out int i) ? i : 0;

            using (var db = new SiteContext())
            {
                var user = db.Authorize(User);

                if (request.DecisionCode == 1)
                {
                    var errors = new List<string>();

                    if (string.IsNullOrEmpty(Request.Form.Get("acceptDate")))
                    {
                        errors.Add("Необходимо указать дату, на которую предоставляется машина");
                    }
                    else if (!DateTime.TryParse(Request.Form.Get("acceptDate"), out DateTime d))
                    {
                        errors.Add("Дата, на которую предоставляется машина, введена в неверном формате. Ожидается формат дд.мм.гггг");
                    }
                    else if (d < DateTime.Today)
                    {
                        errors.Add("Указанная дата уже прошла");
                    }
                    else
                    {
                        request.DateAccepted = d;
                    }

                    if (!int.TryParse(Request.Form.Get("carId"), out i))
                    {
                        errors.Add("Не передан идентификатор выбранной машины");
                    }
                    else if (i == 0)
                    {
                        errors.Add("Необходимо выбрать машину");
                    }
                    else
                    {
                        request.CarId = i;
                    }

                    if (!int.TryParse(Request.Form.Get("driverId"), out i))
                    {
                        errors.Add("Не передан идентификатор выбранного водителя");
                    }
                    else if (i == 0)
                    {
                        errors.Add("Необходимо выбрать водителя");
                    }
                    else
                    {
                        request.DriverId = i;
                    }

                    if (errors.Count != 0)
                    {
                        return "<div class='auto_errors'>" + string.Join("<br />", errors) + "</div>";
                    }

                    db.AutoRequests
                        .Where(x => x.Id == request.Id)
                        .Set(x => x.DecisionCode, request.DecisionCode)
                        .Set(x => x.Comment, request.Comment)
                        .Set(x => x.DecisionUserId, user.UID)
                        .Set(x => x.DecisionDate, DateTime.Now)
                        .Set(x => x.CarId, request.CarId)
                        .Set(x => x.DriverId, request.DriverId)
                        .Set(x => x.DateAccepted, request.DateAccepted)
                        .Update();
                }
                else if (request.DecisionCode == -1)
                {
                    db.AutoRequests
                        .Where(x => x.Id == request.Id)
                        .Set(x => x.DecisionCode, request.DecisionCode)
                        .Set(x => x.Comment, request.Comment)
                        .Set(x => x.DecisionUserId, user.UID)
                        .Set(x => x.DecisionDate, DateTime.Now)
                        .Update();
                }
                else
                {
                    db.AutoRequests
                        .Where(x => x.Id == request.Id)
                        .Set(x => x.DecisionCode, request.DecisionCode)
                        .Set(x => x.Comment, request.Comment)
                        .Update();
                }

                return "";
            }
        }

        public string Print()
        {
            using (var db = new SiteContext())
            {
                var query = from r in db.AutoRequests
                            from d in db.AutoDrivers.LeftJoin(x => x.Id == r.DriverId)
                            from c in db.AutoCars.LeftJoin(x => x.Id == r.CarId)
                            where r.Date == DateTime.Today && r.DecisionCode == 1
                            select new
                            {
                                Driver = d.Name,
                                Car = c,
                                r.Location,
                            };

                var model = query.ToList();

                string templatePath = Server.MapPath("/content/documents/auto.xls");
                string outputPath = Server.MapPath($@"/content/documents/Заявка на авто от {DateTime.Now.ToLongDateString()}.xls");

                HSSFWorkbook book;
                using (var fs = System.IO.File.OpenRead(templatePath))
                {
                    book = new HSSFWorkbook(fs);
                }
                ISheet sheet = book.GetSheetAt(0);

                sheet.GetRow(6).GetCell(6).SetCellValue("\"______\"____________" + DateTime.Now.Year + " г.");
                sheet.GetRow(10).GetCell(0).SetCellValue("на " + DateTime.Now.ToString("d MMMM yyyy") + " года");

                int currentRow = 15;
                int lastRow = currentRow + model.Count;
                int i = 3;

                foreach (var req in model)
                {
                    IRow row = sheet.CopyRow(14, currentRow);

                    row.GetCell(1).SetCellValue(i++);
                    row.GetCell(2).SetCellValue(req.Driver);
                    row.GetCell(3).SetCellValue(req.Car.Model + " (" + req.Car.Number + ")");
                    row.GetCell(4).SetCellValue(req.Car.Type);
                    row.GetCell(5).SetCellValue(req.Location);

                    currentRow++;
                }

                string path = $"";

                using (var file = new FileStream(outputPath, FileMode.Create))
                {
                    book.Write(file);
                    file.Close();
                }

                return path;
            }
        }

        private List<string> CheckForm(ref Request request, System.Collections.Specialized.NameValueCollection form)
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(form.Get("data")))
            {
                errors.Add("Необходимо указать дату, на которую нужна машина");
            }
            else if (!DateTime.TryParse(form.Get("data"), out DateTime d))
            {
                errors.Add("Дата, на которую нужна машина, введена в неверном формате. Ожидается формат дд.мм.гггг");
            }
            else if (d < DateTime.Today)
            {
                errors.Add("Указанная дата уже прошла");
            }
			else if (d.Date == DateTime.Now.Date.AddDays(1) && DateTime.Now.Hour > 15)
			{
				errors.Add("Создание заявки на выбранную дату разрешено только до 16:00");
			}
			else
            {
                request.Date = d;
            }

            if (string.IsNullOrEmpty(form.Get("start")))
            {
                errors.Add("Необходимо указать время отправления");
            }
            else if (!DateTime.TryParse(form.Get("start"), out DateTime d))
            {
                errors.Add("Время отправления введено в неверном формате. Ожидается формат чч:мм");
            }
			else if (d.Hour > 16 || d.Hour < 8)
			{
				errors.Add("Время отправления выходит за рамки рабочего дня (с 8:00 до 16:00)");
			}
			else
            {
                request.TimeStart = d;
            }

            if (string.IsNullOrEmpty(form.Get("end")))
            {
                request.TimeEnd = DateTime.Parse("17:00");
            }
            else if (!DateTime.TryParse(form.Get("end"), out DateTime d))
            {
                errors.Add("Время возвращения введено в неверном формате. Ожидается формат чч:мм");
            }
            else if (d.Hour > 17 || d.Hour < 8)
            {
                errors.Add("Время возвращения выходит за рамки рабочего дня (с 8:00 до 17:00)");
            }
            else if (d <= request.TimeStart)
            {
                errors.Add("Время возвращения должно быть позже времени отправления");
            }
            else
            {
                request.TimeEnd = d;
            }

            if (string.IsNullOrEmpty(form.Get("adress")))
            {
                errors.Add("Необходимо ввести адрес");
            }
            else
            {
                request.Location = form.Get("adress");
            }

            if (string.IsNullOrEmpty(form.Get("target")))
            {
                errors.Add("Необходимо указать причину заказа авто");
            }
            else
            {
                request.Target = form.Get("target");
            }

            return errors;
        }


        // Список водителей

        [HttpPost]
        public void CreateDriver([Bind(Include = "Name,DefaultCarId,PhoneNumber")] Driver driver)
        {
            using (var db = new SiteContext())
            {
                db.Insert(driver);
            }
        }

        [HttpPost]
        public void SaveDriver([Bind(Include = "Id,Name,DefaultCarId,PhoneNumber")] Driver driver)
        {
            using (var db = new SiteContext())
            {
                db.AutoDrivers
                    .Where(x => x.Id == driver.Id)
                    .Set(x => x.Name, driver.Name)
                    .Set(x => x.PhoneNumber, driver.PhoneNumber)
                    .Set(x => x.DefaultCarId, driver.DefaultCarId)
                    .Update();
            }
        }

        [HttpPost]
        public void DeleteDriver(int Id)
        {
            using (var db = new SiteContext())
            {
                db.AutoDrivers.Delete(x => x.Id == Id);
            }
        }


        // Список автомобилей

        [HttpPost]
        public void CreateCar([Bind(Include = "Model,Number,Type")] Car car)
        {
            using (var db = new SiteContext())
            {
                db.Insert(car);
            }
        }

        [HttpPost]
        public void SaveCar([Bind(Include = "Id,Model,Number,Type")] Car car)
        {
            using (var db = new SiteContext())
            {
                db.AutoCars
                    .Where(x => x.Id == car.Id)
                    .Set(x => x.Model, car.Model)
                    .Set(x => x.Number, car.Number)
                    .Set(x => x.Type, car.Type)
                    .Update();
            }
        }

        [HttpPost]
        public void DeleteCar(int Id)
        {
            using (var db = new SiteContext())
            {
                db.AutoCars.Delete(x => x.Id == Id);
            }
        }
    }
}