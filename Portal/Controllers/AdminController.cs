using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		public ActionResult Index() => RedirectToAction("Errors");

		public ActionResult Errors() => View();

		public ActionResult Users() => View();

		public ActionResult Guests() => View();

		public ActionResult News() => View();

		public ActionResult NewsGuilds() => View();

		public ActionResult Granties() => View();

		public ActionResult Vacations() => View();

		public ActionResult UserDetails(string UserName)
		{
			using (var db = new SiteContext())
			{
				var user = db.Users.FirstOrDefault(x => x.UName == UserName);

				return View(user);
			}
		}


		[HttpPost]
		public ActionResult UserDetails(string UserName, string Nick, string Granties, string Guilds)
		{
			using (var db = new SiteContext())
			{
				db.Users
					.Where(x => x.UName == UserName)
					.Set(x => x.Nick, Nick)
					.Set(x => x.UClass, Granties)
					.Update();

				var guilds = Guilds
					.Split(';')
					.Select(x => int.TryParse(x, out int i) ? i : 0)
					.Where(x => x != 0)
					.ToList();

				db.UsersToGuilds
					.Where(x => x.UserName == UserName)
					.Delete();

				foreach (var guild in guilds)
				{
					db.UsersToGuilds
						.Value(x => x.UserName, UserName)
						.Value(x => x.GuildId, guild)
						.Insert();
				}
			}

			return Content("");
		}

		[HttpPost]
		public ActionResult News(string mode, string uname)
		{
			using (var db = new SiteContext())
			{
				if (mode == "add")
				{
					db.Users
						.Where(x => x.UName == uname)
						.Set(x => x.UClass, x => x.UClass + " site_red")
						.Update();
				}
				else
				{
					db.Users
						.Where(x => x.UName == uname)
						.Set(x => x.UClass, x => x.UClass.Replace("site_red", "").Replace("  ", " "))
						.Update();
				}
			}

			return Content("");
		}

		[HttpPost]
		public ActionResult DeleteError(int Id)
		{
			using (var db = new SiteContext())
			{
				return Content(db.Errors.Delete(x => x.Id == Id).ToString());
			}
		}

		[HttpPost]
		public ActionResult DeleteAllErrors()
		{
			using (var db = new SiteContext())
			{
				return Content(db.Errors.Delete().ToString());
			}
		}

		[HttpPost]
		public ActionResult UserDelete(string UserName)
		{
			using (var db = new SiteContext())
			{
				db.Users
					.Where(x => x.UName == UserName)
					.Delete();

				return Content("");
			}
		}

		[HttpPost]
		public string AddGrantie(int Id, string Key)
		{
			try
			{
				using (var db = new SiteContext())
				{
					string uclass = db.Users.Where(x => x.UID == Id).Select(x => x.UClass).FirstOrDefault() ?? "";

					switch (Key)
					{
						case "ascuka":
						case "auto_prov":
						case "buh":
						case "checkpoint":
						case "devin":
						case "mnemonic":
						case "z_admin":
						case "phonebook":
							uclass = uclass + " " + Key;
							break;
						case "site_red":
						case "site_admin":
							uclass = uclass.Replace("site_admin", "").Replace("site_red", "") + " " + Key;
							break;
						case "labours_user":
						case "labours_guildmaster":
						case "labours_subuser":
						case "labours_reviewer":
						case "labours_worksecurity":
						case "labours_admin":
						case "labours_watch":
						case "labours_stats":
							uclass = uclass
								.Replace("labours_user", "")
								.Replace("labours_guildmaster", "")
								.Replace("labours_subuser", "")
								.Replace("labours_reviewer", "")
								.Replace("labours_worksecurity", "")
								.Replace("labours_admin", "")
								.Replace("labours_watch", "")
								.Replace("labours_stats", "")
								+ " " + Key;
							break;
						case "record_user":
						case "record_admin":
							uclass = uclass.Replace("record_user", "").Replace("record_admin", "") + " " + Key;
							break;
						case "works_user":
						case "works_admin":
							uclass = uclass.Replace("works_user", "").Replace("works_admin", "") + " " + Key;
							break;
						case "z_watch":
						case "z_prov":
						case "z_omts":
						case "z_user":
						case "z_exes":
							uclass = uclass.Replace("z_prov", "").Replace("z_omts", "").Replace("z_watch", "").Replace("z_user", "").Replace("z_exes", "") + " " + Key;
							break;
						case "cartridges":
							uclass = uclass + " " + Key;
							break;
						case "order_guest":
						case "order_user":
						case "order_nss":
							uclass = uclass.Replace("order_nss", "").Replace("order_user", "").Replace("order_guest", "") + " " + Key;
							break;
					}

					db.Users
						.Where(x => x.UID == Id)
						.Set(x => x.UClass, uclass)
						.Update();
				}

				return "";
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		[HttpPost]
		public string RemoveGrantie(int Id, string Key)
		{
			try
			{
				using (var db = new SiteContext())
				{
					string uclass = db.Users.Where(x => x.UID == Id).Select(x => x.UClass).FirstOrDefault() ?? "";

					switch (Key)
					{
						case "site_admin":
						case "site_red":
						case "ascuka":
						case "auto_prov":
						case "buh":
						case "checkpoint":
						case "devin":
						case "mnemonic":
						case "record_user":
						case "z_user":
						case "labours_user":
						case "labours_guildmaster":
						case "labours_subuser":
						case "labours_reviewer":
						case "labours_worksecurity":
						case "labours_admin":
						case "labours_watch":
						case "labours_stats":
						case "works_user":
						case "works_admin":
						case "order_guest":
						case "order_user":
						case "order_nss":
						case "phonebook":
							uclass = uclass.Replace(Key, "");
							break;
						case "record_admin":
							uclass = uclass.Replace(Key, "record_user");
							break;
						case "z_watch":
						case "z_prov":
						case "z_omts":
						case "z_exec":
						case "z_admin":
							uclass = uclass.Replace(Key, "z_user");
							break;
						case "cartridges":
							uclass = uclass.Replace(Key, "");
							break;
					}

					db.Users
						.Where(x => x.UID == Id)
						.Set(x => x.UClass, uclass.Trim())
						.Update();
				}

				return "";
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		[HttpPost]
		public ActionResult PinUserToGuild(string UserName, int GuildId)
		{
			using (var db = new SiteContext())
			{
				if (db.UsersToGuilds.Count(x => x.UserName == UserName && x.GuildId == GuildId) == 0)
				{
					db.UsersToGuilds
						.Value(x => x.GuildId, GuildId)
						.Value(x => x.UserName, UserName)
						.Insert();
				}

				return Content("");
			}
		}

		[HttpPost]
		public ActionResult UnpinUserToGuild(string UserName, int GuildId)
		{
			using (var db = new SiteContext())
			{
				db.UsersToGuilds
					.Where(x => x.UserName == UserName && x.GuildId == GuildId)
					.Delete();

				return Content("");
			}
		}

		[HttpPost]
		public ActionResult AddGuild(string Name, bool IsPrivate)
		{
			using (var db = new SiteContext())
			{
				db.NewsGuilds
					.Value(x => x.Name, Name)
					.Value(x => x.IsPrivate, IsPrivate)
					.Insert();

				return Content("");
			}
		}

		[HttpPost]
		public ActionResult SaveGuild(int GuildId, string Name, bool IsPrivate)
		{
			using (var db = new SiteContext())
			{
				db.NewsGuilds
					.Where(x => x.Id == GuildId)
					.Set(x => x.Name, Name)
					.Set(x => x.IsPrivate, IsPrivate)
					.Update();

				return Content("");
			}
		}

		[HttpPost]
		public ActionResult DelGuild(int GuildId)
		{
			using (var db = new SiteContext())
			{
				db.NewsGuilds
					.Where(x => x.Id == GuildId)
					.Delete();

				db.UsersToGuilds
					.Where(x => x.GuildId == GuildId)
					.Delete();

				return Content("");
			}
		}

		public ActionResult CheckAD()
		{
			try
			{
				using (var db = new SiteContext())
				{
					db.RescanUsers();
				}

				return Json(new { Done = true });
			}
			catch (Exception e)
			{
				return Json(new { Error = e.Message, e.StackTrace });
			}
		}
	}
}