using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
	[Authorize]
	public class NewsController : Controller
	{
		public ActionResult Index() => View();

		public ActionResult List(int take = 0, int skip = 0, bool hided = false, bool expired = false, bool pinned = true, int before = 0)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				ViewBag.NewsTags = db.NewsTags
					.ToDictionary(x => x.Token, x => x.Name);

				var pinnedGuilds = db.UsersToGuilds
					.Where(x => x.UserName == user.UName)
					.Select(x => x.GuildId)
					.ToList();

				var query = from n in db.News
							from u in db.Users.Where(x => x.UID == n.UserId).DefaultIfEmpty()
							from g in db.NewsGuilds.LeftJoin(x => x.Id == n.GuildId)
							from p in db.NewsPins.LeftJoin(x => x.NewsId == n.Id && x.UserId == user.UID).DefaultIfEmpty(new NewsPin { NewsId = n.Id, UserId = -1 })
							where
								!n.IsTemplate
								// фильтр, убирающий просроченные новости
								&& (expired || (n.DateExpire <= n.DateAdd || n.DateExpire >= DateTime.Now.Date))
								// фильтр, убирающий скрытые новости
								&& (hided || db.NewsHides.Where(x => x.NewsId == n.Id && x.UserId == user.UID).Count() == 0)
								// фильтр на новостные каналы
								&& (n.GuildId == 0 || !g.IsPrivate || pinnedGuilds.Contains(n.GuildId))
							select new News
							{
								Id = n.Id,
								Priority = n.Priority,
								DateAdd = n.DateAdd,
								DateExpire = n.DateExpire,
								Title = n.Title,
								Message = n.Message,
								Links = n.Links,
								UserName = n.UserName,
								Tags = n.Tags ?? "",
								Creator = new User
								{
									UID = u.UID,
									UName = u.UName,
									DisplayName = u.DisplayName,
								},
								Guild = g ?? new NewsGuild { Id = 0, Name = "Общий", IsPrivate = false },
								IsWatched = db.NewsViews.Where(x => x.NewsId == n.Id && x.UserId == user.UID).Count() > 0,
								IsHide = false,
								IsRedactor = n.UserId == user.UID || user.IsAdmin,
								IsPinned = p.UserId > 0,
								WatchedCount = db.NewsViews.Where(x => x.NewsId == n.Id).Count()
                            };

				query = query
					.OrderByDescending(x => x.IsPinned)
					.ThenByDescending(x => x.DateAdd);

				if (skip > 0) query = query.Where(x => x.Id < skip);
				if (take > 0) query = query.Take(take);
				if (before > 0) query = query.Where(x => x.Id > before);
				if (!pinned) query = query.Where(x => !x.IsPinned).OrderByDescending(x => x.DateAdd);

				return View("NewsBlock", query.ToList());
			}
		}

		public ActionResult Body(int Id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				var query = from n in db.News
							from u in db.Users.Where(x => x.UID == n.UserId).DefaultIfEmpty()
							where n.Id == Id
							select new News
							{
								Id = n.Id,
								Priority = n.Priority,
								DateAdd = n.DateAdd,
								DateExpire = n.DateExpire,
								Title = n.Title,
								Message = n.Message,
								Links = n.Links,
								UserName = n.UserName,
								Tags = n.Tags ?? "",
								Creator = new User
								{
									UID = u.UID,
									UName = u.UName,
									DisplayName = u.DisplayName,
								},
								IsWatched = db.NewsViews.Where(x => x.NewsId == n.Id && x.UserId == user.UID).Count() > 0,
							};

				var news = query.First();

				if (news.GuildId != 0)
				{
					news.GuildName = db.NewsGuilds.FirstOrDefault(x => x.Id == news.GuildId)?.Name ?? "";
				}

				ViewBag.Tags = db.NewsTags.ToList();

				return View("NewsContent", news);
			}
		}

		public ActionResult Create()
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				int id = db.InsertWithInt32Identity(new News
				{
					UserId = user.UID,
					Priority = "low",
					DateAdd = DateTime.Now,
					DateExpire = DateTime.Now.AddMinutes(-1),
					UserName = string.Empty,
					IsTemplate = true,
					GuildId = 0,
				});

				return RedirectToAction(nameof(Edit), new { Id = id });
			}
		}

		public ActionResult Edit(int Id) => View("Edit", model: Id);


		[HttpPost]
		public ActionResult Edit(
			[Bind(Include = "Id,Title,Message,Priority,GuildId")] News news,
			string[] links,
			bool IsCustomDate,
			string DateAdd,
			bool IsTimed,
			string DateExpire,
			bool IsUsername,
			string Username,
			string Tags
		)
		{
			try
			{
				using (var db = new SiteContext())
				{
					var user = db.Authorize(User);
					if (user == null) return Json(new { UserNotFound = true });

					var date = DateTime.Now;

					var wasTemplate = db.News
						.Where(x => x.Id == news.Id)
						.Select(x => x.IsTemplate)
						.FirstOrDefault();

					db.News
						.Where(x => x.Id == news.Id)
						.Set(x => x.IsTemplate, true)
						.Update();

					db.News
						.Where(x => x.Id == news.Id)
						.Set(x => x.Title, news.Title)
						.Set(x => x.Message, news.Message)
						.Set(x => x.Links, string.Join(";", links ?? new string[0]))
						.Set(x => x.Priority, news.Priority)
						.Set(x => x.GuildId, news.GuildId)
						.Set(x => x.Tags, Tags ?? "")
						.Update();

					if (IsCustomDate)
					{
						db.News
							.Where(x => x.Id == news.Id)
							.Set(x => x.DateAdd, DateTime.TryParse(DateAdd, out DateTime d) ? d : date)
							.Update();
					}

					if (IsTimed)
					{
						db.News
							.Where(x => x.Id == news.Id)
							.Set(x => x.DateExpire, DateTime.TryParse(DateExpire, out DateTime d) ? d : date)
							.Update();
					}
					else
					{
						db.News
							.Where(x => x.Id == news.Id)
							.Set(x => x.DateExpire, x => x.DateAdd.AddMinutes(-1))
							.Update();
					}

					if (IsUsername && !string.IsNullOrEmpty(Username))
					{
						db.News
							.Where(x => x.Id == news.Id)
							.Set(x => x.UserName, Username)
							.Update();
					}
					else if (wasTemplate)
					{
						db.News
							.Where(x => x.Id == news.Id)
							.Set(x => x.UserName, string.Empty)
							.Update();
					}

					db.News
						.Where(x => x.Id == news.Id)
						.Set(x => x.IsTemplate, false)
						.Update();

					return Json(new { Done = true });
				}
			}
			catch (Exception e)
			{
				return Json(new { Error = true, Data = e.Message + "\n" + e.Source + "\n" + e.StackTrace });
			}
		}

		[HttpPost]
		public ActionResult Delete(int Id)
		{
			try
			{
				using (var db = new SiteContext())
				{
					int i = db.News
						.Where(x => x.Id == Id)
						.Delete();

					if (i > 0) return Json(new { Done = true });
					return Json(new { NotFound = true });
				}
			}
			catch (Exception e)
			{
				return Json(new { Error = true, Data = e.Message + "\n" + e.Source + "\n" + e.StackTrace });
			}
		}


		// Addons

		[HttpPost]
		public ActionResult UploadFile(int Id)
		{
			foreach (string f in Request.Files.AllKeys)
			{
				try
				{
					var raw = Request.Files[f];

					string[] Params = raw.FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
					string file = Params[Params.Length - 1];

					string type = file.Substring(file.LastIndexOf(".") + 1).ToLower();
					bool allow = false;
					string[] allowedTypes = new string[] { "jpg", "png", "gif", "jpeg", "tiff", "tif", "txt", "doc", "docx", "rtf", "zip", "xls", "xlsx", "pdf", "bmp", "avi", "mp4", "ppt", "pptx" };

					for (int i = 0; i < allowedTypes.Length; i++)
					{
						if (allowedTypes[i].ToLower() == type)
						{
							allow = true;
							break;
						}
					}

					if (!allow)
					{
						Response.Write($@"<script>alert('Файлы такого типа запрещены к загрузке на сервер.');</script>");
					}
					else
					{
						if (!Directory.Exists(@"\\web\Files\Новости\" + Id)) Directory.CreateDirectory(@"\\web\Files\Новости\" + Id);

						var link = "http://www.vst.vitebsk.energo.net/files/Новости/" + Id + "/" + file;
						raw.SaveAs(@"\\web\Files\Новости\" + Id + "\\" + file);

						Response.Write($@"<script>
							var div = document.createElement('div');
							div.innerHTML = ""<table><tr><th><span>{file}</span><a href='{link}'>{link}</a><input name='Links[]' class='hide' value='{file}' /></th><td width='120px'><button onclick='removeFile(this)'><span class='material-icons'>remove_circle_outline</span><span class='form-button-caption'>Удалить</span></button></td></tr></table>"";
							parent.document.getElementById('form-files').appendChild(div);
						</script>");
					}
				}
				catch
				{
					return Content("");
				}
			}

			return Content("");
		}

		public void Hide(int Id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				if (db.NewsHides.Count(x => x.NewsId == Id && x.UserId == user.UID) == 0)
				{
					db.Insert(new NewsHide
					{
						NewsId = Id,
						UserId = user.UID,
					});
				}
			}
		}

		public void Show(int Id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				db.NewsHides
					.Delete(x => x.NewsId == Id && x.UserId == user.UID);
				
			}
		}

		public void Pin(int Id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				if (db.NewsPins.Count(x => x.NewsId == Id && x.UserId == user.UID) == 0)
				{
					db.Insert(new NewsPin
					{
						NewsId = Id,
						UserId = user.UID,
					});
				}
			}
		}

		public void Unpin(int Id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				db.NewsPins
					.Where(x => x.NewsId == Id && x.UserId == user.UID)
					.Delete();
			}
		}

		public void Watch(int Id)
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				if (db.NewsViews.Count(x => x.UserId == user.UID && x.NewsId == Id) == 0)
				{
					db.Insert(new NewsView
					{
						Date = DateTime.Now,
						NewsId = Id,
						UserId = user.UID,
					});
				}
			}
		}

		public void WatchAll()
		{
			using (var db = new SiteContext())
			{
				var user = db.Authorize(User);

				var watched = db.NewsViews
					.Where(x => x.UserId == user.UID)
					.Select(x => x.NewsId)
					.ToList();

				var news = db.News
					.Where(x => !watched.Contains(x.Id))
					.Select(x => x.Id)
					.ToList();

				foreach (var id in news)
				{
					db.Insert(new NewsView
					{
						Date = DateTime.Now,
						NewsId = id,
						UserId = user.UID,
					});
				}
			}
		}
	}
}