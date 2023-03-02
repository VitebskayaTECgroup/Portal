using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[AllowAnonymous]
	public class DocsController : Controller
	{
		public ActionResult Index() => Redirect("~/docs/О_филиале/Официальное_наименование/");

		public ActionResult Page(string section, string page)
		{
			ViewBag.OnlyIntranet = Request.UserHostAddress.Contains("10.178.9.");
			return View((section + "/" + page).Replace('_', ' ').Replace(",", ""));
		}

		public string Mail()
		{
			string grant = Request.Form.Get("grant") ?? "";

			// Настройка сообщения
			var mail = new MailMessage
			{
				Subject = "Сообщение",
				Body = grant != "" ?
				"ЭЛЕКТРОННОЕ ОБРАЩЕНИЕ\n" +
					"\nПолное наименование юридического лица: " + Request.Form.Get("fio") +
					"\nФамилия, имя, отчество лица, уполномоченного в установленном порядке подписывать обращения: " + grant +
					"\nАдрес места нахождения юридического лица: " + Request.Form.Get("adress") +
					"\nEmail для ответа: " + Request.Form.Get("email") +
					"\nСообщение: \n\n" + Request.Form.Get("body")
				:
				"ЭЛЕКТРОННОЕ ОБРАЩЕНИЕ\n" +
					"\nФамилия, имя, отчество гражданина: " + Request.Form.Get("fio") +
					"\nАдрес места жительства либо места пребывания гражданина: " + Request.Form.Get("adress") +
					"\nEmail для ответа: " + Request.Form.Get("email") +
					"\nСообщение: \n\n" + Request.Form.Get("body"),
				From = new MailAddress("1okno@vst.vitebsk.energo.net") // 1, должно быть равно с 2
			};

			// Прикрепленные файлы
			string[] files = (Request.Form.Get("files") ?? "").Split(new string [] { "<>" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string file in files)
			{
				mail.Attachments.Add(new Attachment(@"\\web\files\Входящие\" + file));
			}

			// Конечный адрес
			mail.To.Add("secretary@VST.VITEBSK.ENERGO.NET");

			// Сервер отправки
			using (var smtp = new SmtpClient())
			{
				smtp.Host = "mail.vst.vitebsk.energo.net";
				smtp.Credentials = new NetworkCredential("1okno@vst.vitebsk.energo.net", "14abZMu+"); // 2, должно быть равно с 1

				// Отправка
				smtp.Send(mail);
			}

			return "Сообщение отправлено";
		}

		public string MailFile()
		{
			foreach (string f in Request.Files.AllKeys)
			{
				var file = Request.Files[f];

				string[] file_splitted = file.FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
				string result = file_splitted[file_splitted.GetLength(0) - 1];

				file_splitted = result.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
				string[] normal_files = { "pdf", "docx", "doc", "rtf", "txt", "odt", "zip", "rar", "png", "tiff", "jpeg", "jpg", "ppt", "pptx" };
				bool normal = false;
				foreach (string normal_file in normal_files)
				{
					if (normal_file == file_splitted[file_splitted.GetLength(0) - 1]) normal = true;
				}

				if (normal)
				{
					file.SaveAs(@"\\web\files\Входящие\" + result);

					Response.Write("<script>parent.onUpload('" + result + "');</script>");
				}
				else
				{
					Response.Write("<script>alert('Запрещенное разрешение файла!');</script>");
				}
			}

			return "";
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

		public ActionResult Appeal(string text)
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.Insert(new Appeal
					{
						Date = DateTime.Now,
						Ip = Request.UserHostAddress,
						Text = text,
					});
				}

				return Content("Ваше обращение зафиксировано");
			}
			catch
			{
				return Content("Произошла ошибка. Воспользуйтесь электронной почтой и отправьте обращение на адрес vst@vitebsk.energo.net");
			}
		}
	}
}