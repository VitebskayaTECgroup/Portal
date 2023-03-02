using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Areas.Accidents.Controllers
{
    [Authorize]
    public class ListController : Controller
    {
        public ActionResult Index() => View();

        public ActionResult Edit(int Id) => View(model: Id);

		public ActionResult Add() => View();

		[HttpPost]
		public ActionResult Add(string Name)
        {
			try
            {
				using (var db = new SiteContext())
                {
					var user = db.Authorize(User);
					if (!user.IsAdmin) return Json(new { Error = "Недостаточно прав для создания списка!" });

					var id = db.InsertWithInt32Identity(new AccidentList
					{
						Name = Name,
					});

					return Json(new { Done = true, Id = id });
                }
            }
			catch (Exception e)
            {
				return Json(new { Error = e.Message });
            }
        }

		[HttpPost]
		public ActionResult Edit(int Id, string Name)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);
					if (!user.IsAdmin) return Content("Ошибка! Нет разрешения");

					db.AccidentsLists
						.Where(x => x.Id == Id)
						.Set(x => x.Name, Name)
						.Update();

					return Content("Список изменён успешно");
				}
			}
			catch (Exception e)
			{
				return Content("Ошибка выполнения: " + e.Message);
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
					if (!user.IsAdmin) return Content("Ошибка! Нет разрешения");

					db.AccidentsLists
						.Where(x => x.Id == Id)
						.Delete();

					return Content("Список удалён успешно");
				}
			}
			catch (Exception e)
			{
				return Content("Ошибка выполнения: " + e.Message);
			}
		}
		
		[HttpPost]
		public ActionResult SetUserListRelation(int Id, int UserId, int Role)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);
					if (!user.IsAdmin) return Content("Ошибка! Нет разрешения");


					var l = db.AccidentsLists
						.Where(x => x.Id == Id)
						.FirstOrDefault();

					if (l == null) return Content("Список, с которым устанавливается связь, не найден!");


					var u = db.Users
						.Where(x => x.UID == UserId)
						.FirstOrDefault();

					if (u == null) return Content("Пользователь, для которого устанавливается связь, не найден!");


					var oldRelation = db.AccidentsUsersLists
						.Where(x => x.ListId == Id && x.UserId == UserId)
						.FirstOrDefault() ?? new AccidentUserList { Role = AccidentListRoles.NonSet };

					db.AccidentsUsersLists
						.Where(x => x.ListId == Id && x.UserId == UserId)
						.Delete();

					db.Insert(new AccidentUserList
					{
						ListId = Id,
						UserId = UserId,
						Role = (AccidentListRoles)Role,
					});

					return Content("Связь пользователя \"" + u.DisplayName + "#" + u.UID +
						"\" со списком \"" + l.Name +
						"\" изменена с \"" + AccidentUserList.RoleDescription(oldRelation.Role) +
						"\" на \"" + AccidentUserList.RoleDescription((AccidentListRoles)Role) + "\"");
				}
			}
			catch (Exception e)
			{
				return Content("Ошибка выполнения: " + e.Message);
			}
		}
	}
}