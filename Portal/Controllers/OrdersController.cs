using DatabaseLayer.Site;
using LinqToDB;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using static LinqToDB.Common.Configuration;
using static LinqToDB.Sql;

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

        public ActionResult CopyRecords(int IdOrderWork, int UserId)
        {
            /*
            заметки, могут быть неточными


            DateWork = дата, на которую планируются работы
            GuildWork = подразделение, с которым работаем
            IdOrderWork = Id из таблицы Order - текущий день, нужен для всей работы, найти в Orders по GuildWork и DateWork


            в Orders по GuildWork найти последних 15 дат
            из этих строк забрать Id в массив IdGldMass
            гнать циклом все от самого большого значения к меньшему
            в OrdersRecords найти все записи с OrderId == IdGldMass[i]
            если записей нет - переход на следующий элемент массива
            если запись одна - проверить, пустая ли она (Description == ""); если пустая - переход на следующий элемент массива, если не пустая - копируем и останавливаемся в переборе массива
            если записей больше одной - копируем и останавливаемся в переборе массива

            при копировании использовать 
            новые / действующие     DateCreated = Now, UserId = user, OrderId = IdOrderWork
            старые                  Description, NumberPlannedOrder, NumberPlannedCommand, NumberUnplannedOrder, NumberUnplannedCommand, NumberOfRepairStaff, HeadOfWork, Contractor
            нулевые                 Comment = "", AnswerUserId = 0, AnswerDate = NULL
            проверяемые             AnswerCode = 
            0, если NumberPlannedCommand или NumberUnplannedCommand == 1
            1, если NumberPlannedOrder или NumberUnplannedOrder == 1
            2 в остальных случаях


            найти пустую (Description == "") запись с IdOrderWork и удалить
            */

            using (var db = new SiteContext())
			{
				//забираем название подразделения по Id из таблицы Orders
                string orderSingleGuild = db.Orders.Where(x => x.Id == IdOrderWork).Select(x => x.Guild).FirstOrDefault();

				//по названию подразделения забираем Id последних 15 дней
                var ordersListId = db.Orders.Where(x => x.Guild == orderSingleGuild).OrderByDescending(x => x.Date).Take(15).Select(x => x.Id).ToList();

				//ближайший день с работами = непустой
				int nearId = 0;

				for (int i = 0; i < 15; i++)
				{
					//можно ли копировать день
                    bool canCopyWork = false;
                    
					//день не должен совпадать с тем, на который планирут копию
					if (ordersListId[i] != IdOrderWork)
					{
						//забираем все работы за день
                        var ordersRecordsListById = db.OrdersRecords.Where(x => x.OrderId == ordersListId[i]).ToList();

						//если есть одна запись и она содержит описание - можно копировать
						if (ordersRecordsListById.Count == 1 && ordersRecordsListById.First().Description != "")
						{
							canCopyWork = true;
						}
                        //если записей больше одной - можно копировать
                        if (ordersRecordsListById.Count > 1)
                        {
                            canCopyWork = true;
                        }

                        //если есть одна запись, но она пустая - копировать нельзя
                        if (ordersRecordsListById.Count == 1 && ordersRecordsListById.First().Description == "")
                        {
                            canCopyWork = false;
                        }
                        //если записей меньше одной - копировать нельзя
                        if (ordersRecordsListById.Count < 1)
                        {
                            canCopyWork = false;
                        }
                    }

					//если можно копировать день
                    if (canCopyWork)
                    {
                        //присваиваем Id переменной и выходим из цикла, т.к. остальные дни уже не нужны
                        nearId = ordersListId[i];
                        break;
                    }
                }

				//если нашёлся день, который можно копировать
				if (nearId != 0)
				{
					//за какую дату будут скопированы записи
					//string orderDateToCopy = "Скопированы работы за " + db.Orders.Where(x => x.Id == nearId).Select(x => x.Date).FirstOrDefault().ToString("dd.MM.yyyy");

					//все записи за копируемый день
					var ordersRecordsListToCopy = db.OrdersRecords.Where(x => x.OrderId == nearId).OrderBy(x => x.Id).ToList();

					//прогоняем каждую запись
					foreach (OrderRecord orderRecordToCopy in ordersRecordsListToCopy)
					{
						//создаём новую запись, т.к. часть данных должна быть из старой записи, а часть из новой
						OrderRecord orderRecordNew = new OrderRecord
						{
							//new
							DateCreated = DateTime.Now,
							OrderId = IdOrderWork,
							UserId = UserId,
							//old
							Description = orderRecordToCopy.Description,
                            NumberPlannedCommand = orderRecordToCopy.NumberPlannedCommand,
                            NumberPlannedOrder = orderRecordToCopy.NumberPlannedOrder,
                            NumberUnplannedCommand = orderRecordToCopy.NumberUnplannedCommand,
                            NumberUnplannedOrder = orderRecordToCopy.NumberUnplannedOrder,
                            HeadOfWork = orderRecordToCopy.HeadOfWork,
                            Contractor = orderRecordToCopy.Contractor,
                            NumberOfRepairStaff = orderRecordToCopy.NumberOfRepairStaff,
                            //0
                            Comment = "",
							ReasonToUndone = "",
                            AnswerDate = null,
                            AnswerUserId = 0,
							//calc
                            AnswerCode = 0
						};

                        //т.к. записи "новые", то AnswerCode будет
                        //1 = разрешающим, если это распоряжение
                        //2 = запрещающим, если это наряд
						//0 = неопределённым в остальных случаях
                        if (orderRecordNew.NumberPlannedCommand == 1 || orderRecordNew.NumberUnplannedCommand == 1)
						{
							orderRecordNew.AnswerCode = 1;
						}
						else
						{
							if (orderRecordNew.NumberPlannedOrder == 1 || orderRecordNew.NumberUnplannedOrder == 1)
							{
								orderRecordNew.AnswerCode = 2;
							}
							else
							{ 
								orderRecordNew.AnswerCode = 0;
                            }
                        }

						//добавление записи в базу
						db.Insert(orderRecordNew);
					}

					//Id пустой записи, которую надо удалить
					int idToDel = 0;
                    idToDel = db.OrdersRecords.Where(x => x.OrderId == IdOrderWork).Where(x => x.Description == "").Select(x => x.Id).FirstOrDefault();

					if (idToDel != 0)
					{
						db.OrdersRecords
							.Where(x => x.Id == idToDel)
							.Delete();
					}
                }
				else
				{
                    //возвращаем сообщение, если не получилось выбрать Id прошлого рабочего дня
                    //return "Не удалось получить данные за прошлый рабочий день";
                }
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
                    NumberOfRepairStaff = "",
                    HeadOfWork = "",
                    Contractor = "",
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
                    case "NumberOfRepairStaff":
                        db.OrdersRecords
                            .Where(x => x.Id == RecordId)
                            .Set(x => x.NumberOfRepairStaff, Value)
                            .Update();
                        break;
                    case "HeadOfWork":
                        db.OrdersRecords
                            .Where(x => x.Id == RecordId)
                            .Set(x => x.HeadOfWork, Value)
                            .Update();
                        break;
                    case "Contractor":
                        db.OrdersRecords
                            .Where(x => x.Id == RecordId)
                            .Set(x => x.Contractor, Value)
                            .Update();
                        break;
                    case "AnswerCode":
                        int userID = db.Authorize(User).UID;
						db.OrdersRecords
							.Where(x => x.Id == RecordId)
							.Set(x => x.AnswerCode, value)
							.Set(x => x.AnswerUserId, userID)
							.Set(x => x.AnswerDate, DateTime.Now)
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