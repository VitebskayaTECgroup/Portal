using DatabaseLayer.Devin;
using DatabaseLayer.Site;
using LinqToDB;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[Authorize]
	public class SiteController : Controller
	{
		public ActionResult Index() => View();

		public ActionResult Birthdays() => View();


		public void UpdateNick(string Nick)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);
				db.Users
					.Where(x => x.UID == user.UID)
					.Set(x => x.Nick, Nick ?? user.Nick)
					.Update();
			}
		}

		public ActionResult Notes()
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				var model = db.Notes
					.Where(x => x.N_UID == user.UID)
					.Where(x => !x.N_Watched)
					.ToList();

				return View(model);
			}
		}

		public void NoteSee(int Id)
		{
			using (var db = new SiteContext())
			{
				db.Notes
					.Where(x => x.N_ID == Id)
					.Set(x => x.N_Watched, true)
					.Set(x => x.N_DateWatched, DateTime.Now)
					.Update();
			}
		}

		public void NoteSeeAll()
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				db.Notes
					.Where(x => x.N_UID == user.UID)
					.Set(x => x.N_Watched, true)
					.Set(x => x.N_DateWatched, DateTime.Now)
					.Update();
			}
		}

		[HttpPost]
		public void Error(string error, string page)
		{
			using (var db = new SiteContext())
			{
				db.Insert(new Error
				{
					Page = page,
					StackTrace = error,
					Date = DateTime.Now,
					User = User.Identity.Name,
				});
			}
		}

		public string CorruptionDateUpdate(string date, string target)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.Constants
						.Where(x => x.Keyword == "CorruptionDate")
						.Set(x => x.Value, date)
						.Update();


					db.Constants
						.Where(x => x.Keyword == "CorruptionTarget")
						.Set(x => x.Value, target)
						.Update();
				}

				return "Данные изменены";
			}
			catch (Exception e)
			{
				return "Ошибка! " + e.Message;
			}
		}

		public ActionResult MetalCosts(string date = "")
		{
			DateTime _date = DateTime.TryParse(date, out DateTime d) ? d : DateTime.Today;

			using (var db = new SiteContext())
			{
				var metals = db.MetalsCosts
					.ToList()
					.GroupBy(x => x.Name)
					.Select(g => new
					{
						Name = g.Key.Trim(),
						Values = g
							.Select(x => new
							{
								x.Date,
								x.Cost,
								x.Discount,
							})
							.OrderByDescending(x => x.Date)
							.FirstOrDefault(x => x.Date <= _date)
					})
					.Select(x => new MetalsCost
					{
						Name = x.Name,
						Date = x.Values.Date,
						Cost = x.Values.Cost,
						Discount = x.Values.Discount,
					})
					.ToList();

				ViewBag.Date = metals.Max(x => x.Date);

				return View("MetalCosts", metals);
			}
		}

		public JsonResult PrintReport()
		{
			string name = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_ADDR"]).HostName.ToUpper();
			name = name.Substring(0, name.IndexOf('.'));

			using (var db = new DevinContext())
			{
				var computer = db.Devices
					.Where(x => x.Name.ToUpper() == name && x.Type == "CMP")
					.FirstOrDefault();

				if (computer == null)
				{
					return Json(new
					{
						Error = "Компьютер не найден в базе",
						name,
						SV = Request.ServerVariables.AllKeys.Select(x => x + " = " + Request.ServerVariables[x]).ToList(),
					});
				}

				var devices = db.Devices
					.Where(x => x.Type != "CMP" && x.ComputerId == computer.Id)
					.ToList();

				computer.Object1C = db.Objects1C
					.Where(x => x.Inventory == computer.Inventory)
					.FirstOrDefault();

				foreach (var device in devices)
				{
					device.Object1C = db.Objects1C
						.Where(x => x.Inventory == device.Inventory)
						.FirstOrDefault();
				}

				var workplace = db.WorkPlaces
					.Where(x => x.Id == computer.PlaceId)
					.FirstOrDefault();

				XSSFWorkbook book;
				using (var fs = new FileStream(Server.MapPath(Url.Action("documents", "content") + "\\report.xlsx"), FileMode.Open, FileAccess.Read))
				{
					book = new XSSFWorkbook(fs);
				}
				var sheet = book.GetSheetAt(0);

				var now = DateTime.Now.ToString("dd.MM.yyyy");
				sheet.GetRow(4).GetCell(9).SetCellValue(workplace?.Location ?? computer.Location ?? "");
				sheet.GetRow(12).GetCell(0).SetCellValue(now);

				int step = 0;

				sheet.CopyRow(8, 9 + step);
				var row = sheet.GetRow(9 + step);
				row.GetCell(0).SetCellValue(computer.Inventory);
				row.GetCell(1).SetCellValue(computer.Object1C.Description ?? "");
				row.GetCell(2).SetCellValue(computer.SerialNumber ?? "");
				row.GetCell(3).SetCellValue(computer.PublicName);
				row.GetCell(4).SetCellValue(computer.DateInstall.ToString("dd.MM.yyyy"));
				row.GetCell(5).SetCellValue(computer.Object1C.Mol ?? computer.Mol);
				row.GetCell(6).SetCellValue(now);
				step++;

				foreach (var device in devices)
				{
					sheet.CopyRow(8, 9 + step);
					row = sheet.GetRow(9 + step);
					row.GetCell(0).SetCellValue(device.Inventory);
					row.GetCell(1).SetCellValue(device.Object1C.Description ?? "");
					row.GetCell(2).SetCellValue(device.SerialNumber ?? "");
					row.GetCell(3).SetCellValue(device.PublicName);
					row.GetCell(4).SetCellValue(device.DateInstall.ToString("dd.MM.yyyy"));
					row.GetCell(5).SetCellValue(device.Object1C.Mol ?? computer.Mol);
					row.GetCell(6).SetCellValue(now);
					step++;
				}

				sheet.GetRow(8).Height = 0;
				string file = "Карточка_учета_оргтехники_" + name + ".xlsx";

				using (var fs = new FileStream(Server.MapPath(Url.Action("documents", "content") + "\\" + file), FileMode.OpenOrCreate, FileAccess.Write))
				{
					book.Write(fs);
				}

				return Json(new
				{
					Good = "Карточка учета вычислительной техники на рабочем месте \"" + name + "\" создана",
					Link = Url.Action("documents", "content") + "/" + file + "?r=" + new Random().Next()
				}, JsonRequestBehavior.AllowGet);
			}
		}

		public JsonResult PrintReportMol(string Mol)
		{
			using (var db = new DevinContext())
			{
				var query = from d in db.Devices
							from c in db.Objects1C.LeftJoin(x => x.Inventory == d.Inventory)
							from p in db.WorkPlaces.LeftJoin(x => x.Id == d.PlaceId)
							where d.Mol == Mol && !d.IsOff
							select new
							{
								d.Id,
								d.Name,
								d.Inventory,
								d.DateInstall,
								d.SerialNumber,
								Description = d.PublicName ?? c.Description,
								Guild = c.Guild,
								Location = p.Location ?? "не определено",
							};

				var devices = query
					.ToList()
					.GroupBy(x => x.Id)
					.Select(g => g.FirstOrDefault())
					.OrderBy(x => x.Location)
					.ToList();

				var guild = devices
					.Select(x => x.Guild)
					.FirstOrDefault();

				XSSFWorkbook book;
				using (var fs = new FileStream(Server.MapPath(Url.Action("documents", "content") + "\\mol.xlsx"), FileMode.Open, FileAccess.Read))
				{
					book = new XSSFWorkbook(fs);
				}
				var sheet = book.GetSheetAt(0);

				sheet.GetRow(4).GetCell(1).SetCellValue(Mol);
				sheet.GetRow(4).GetCell(6).SetCellValue(guild);
				sheet.GetRow(10).GetCell(0).SetCellValue(DateTime.Today.ToString("dd.MM.yyyy"));
				sheet.GetRow(10).GetCell(6).SetCellValue(Mol);

				int step = 0;
				foreach (var device in devices)
				{
					sheet.CopyRow(8, 9 + step);
					var row = sheet.GetRow(9 + step);
					row.GetCell(0).SetCellValue(device.Inventory);
					row.GetCell(1).SetCellValue(device.Description);
					row.GetCell(2).SetCellValue(device.SerialNumber);
					row.GetCell(3).SetCellValue(device.Location);
					row.GetCell(4).SetCellValue(device.DateInstall.ToString("dd.MM.yyyy"));
					step++;
				}

				sheet.GetRow(8).Height = 0;
				string file = "Карточка_учета_оргтехники_по_МОЛ_" + Mol.Replace('.', '_') + ".xlsx";

				using (var fs = new FileStream(Server.MapPath(Url.Action("documents", "content") + "\\" + file), FileMode.OpenOrCreate, FileAccess.Write))
				{
					book.Write(fs);
				}

				return Json(new
				{
					Good = "Карточка учета вычислительной техники на рабочем месте \"" + Mol + "\" создана",
					Link = Url.Action("documents", "content") + "/" + file + "?r=" + new Random().Next()
				}, JsonRequestBehavior.AllowGet);
			}
		}
	}
}