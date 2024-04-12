using DatabaseLayer.Site;
using LinqToDB;
using NPOI.HSSF.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[Authorize]
	public class OrdersController : Controller
    {
		public ActionResult Index() => View();

        public ActionResult DelRecord(int Id)
        {
			using (var db = new SiteContext())
			{
				db.OrdersRecords
					.Where(x => x.Id == Id)
					.Delete();
			}

			return Json(new { Done = true });
		}

        public ActionResult AddRecord(int Id, int UserId)
		{
            using (var db = new SiteContext())
			{
				db.Insert(new OrderRecord
				{
					DateCreated = DateTime.Now,
					OrderId = Id,
					UserId = UserId,
					Description = "",
					Comment = "",
					NumberPlannedCommand = 0,
					NumberPlannedOrder = 0,
					NumberUnplannedCommand = 0,
					NumberUnplannedOrder = 0,
					ReasonToUndone = "",
					AnswerCode = 0, //1
					AnswerDate = null,
					AnswerUserId = 0,
				});
			}

            return Json(new { Done = true });
		}

		public ActionResult Save(int OrderId, int RecordId, string Name, string Value)
		{
			int value = int.TryParse(Value, out int i) ? i : 0;

			using (var db = new SiteContext())
			{
                //запись считается не новой
				bool isNewRecord = false;
				//если изменяется тип - "наряд" или "распоряжение", то определяем новая ли запись
				if (Name.Contains("Command") || Name.Contains("Order"))
				{
					//забрать AnswerCode для текущей работы
					int recordAnswer = db.OrdersRecords.Where(x => x.Id == RecordId).Select(x => x.AnswerCode).FirstOrDefault();
					//если AnswerCode == 0 (Неизвестно), то запись новая
					if (recordAnswer == 0)
					{
						isNewRecord = true;
					}
				}

                switch (Name)
				{
					case "Description":
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.Description, Value)
							.Update();
						break;
					case "Comment":
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.Comment, Value)
							.Update();
						break;
					case "NumberPlannedOrder":
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.NumberPlannedOrder, value)
							.Update();
                        // если запись новая, добавить запрет, т.к. это наряд
                        if (isNewRecord)
                        {
                            db.OrdersRecords
                                .Where(x => x.Id == RecordId)
                                .Set(x => x.AnswerCode, 2)
                                .Update();
                        }
                        break;
					case "NumberPlannedCommand":
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.NumberPlannedCommand, value)
							.Update();
						// если запись новая, добавить разрешение, т.к. это распоряжение
						if (isNewRecord)
						{
                            db.OrdersRecords
                                .Where(x => x.Id == RecordId)
                                .Set(x => x.AnswerCode, 1)
                                .Update();
                        }
                        break;
					case "NumberUnplannedOrder":
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.NumberUnplannedOrder, value)
							.Update();
                        // если запись новая, добавить запрет, т.к. это наряд
                        if (isNewRecord)
                        {
                            db.OrdersRecords
                                .Where(x => x.Id == RecordId)
                                .Set(x => x.AnswerCode, 2)
                                .Update();
                        }
                        break;
					case "NumberUnplannedCommand":
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.NumberUnplannedCommand, value)
							.Update();
                        // если запись новая, добавить разрешение, т.к. это распоряжение
                        if (isNewRecord)
                        {
                            db.OrdersRecords
                                .Where(x => x.Id == RecordId)
                                .Set(x => x.AnswerCode, 1)
                                .Update();
                        }
                        break;
					case "ReasonToUndone":
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.ReasonToUndone, Value)
							.Update();
						break;
					case "AnswerCode":
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.AnswerCode, value)
							.Update();
						break;

					case "NumberPersonal":
						db.Orders
							.Where(x => x.Id == OrderId)
							.Set(x => x.NumberPersonal, value)
							.Update();
						break;
					case "NumberMasters":
						db.Orders
							.Where(x => x.Id == OrderId)
							.Set(x => x.NumberMasters, value)
							.Update();
						break;
				}
			}

			return Json(new { Done = true });
		}

		public ActionResult SendToFTP(string Date)
		{
			try
			{
				var date = DateTime.TryParse(Date, out DateTime d) ? d : DateTime.Today;
				var path = Server.MapPath(Url.Action("documents", "content") + "/orders.xls");

				// Открываем и редактируем файл про информацию в локальной папке
				HSSFWorkbook book;
				using (var fs = System.IO.File.OpenRead(path))
				{
					book = new HSSFWorkbook(fs);
				}
				HSSFSheet sheet = (HSSFSheet)book.GetSheetAt(0);
				var row = sheet.GetRow(3);
				row.GetCell(0).SetCellValue(Date);

				using (var db = new SiteContext())
				{
					var orders = db.Orders.Where(x => x.Date == date).Select(x => x.Id).ToList();

					var ordersRecords = db.OrdersRecords
						.Where(x => orders.Contains(x.OrderId))
						.ToList();

					var planned = 0;
					var unplanned = 1;

					foreach (var record in ordersRecords)
					{
						planned += record.NumberPlannedCommand;
						planned += record.NumberPlannedOrder;

						unplanned += record.NumberUnplannedCommand;
						unplanned += record.NumberUnplannedOrder;
					}

					row.GetCell(1).SetCellValue(planned + unplanned);
					row.GetCell(2).SetCellValue(0);
				}

				using (var file = new FileStream(path, FileMode.OpenOrCreate))
				{
					book.Write(file);
					file.Close();
				}

				// Отправляем его в FTP
				using (var client = new WebClient())
				{
					client.Credentials = new NetworkCredential("vst_gcu", "ntgkj09");
					client.UploadFile("ftp://vst_gcu@ftp.vitebsk.energo.net/CDS_oper_info/План работ2.xls", WebRequestMethods.Ftp.UploadFile, path);
				}

				// Обновляем дату последней загрузки
				using (var db = new SiteContext())
				{
					db.Constants
						.Where(x => x.Keyword == "OrdersFTPDate")
						.Set(x => x.Value, DateTime.Now.ToString("d MMMM yyyy в HH:mm"))
						.Update();
				}

				// Пишем о завершении
				return Json(new { Done = true });
			}
			catch (Exception e)
			{
				return Json(new { 
					Error = true, 
					e = new 
					{
						e.Message,
						e.Source,
						e.StackTrace,
					} 
				});
			}
		}
	}
}