using LinqToDB;
using LinqToDB.Data;
using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;

namespace DatabaseLayer.Site
{
	public class SiteContext : DataConnection
	{
		public SiteContext() : base("Site") { }

		public ITable<User> Users
			=> this.GetTable<User>();

		public ITable<Person> Persons
			=> this.GetTable<Person>();

		public ITable<Vacation> Vacations
			=> this.GetTable<Vacation>();

		public ITable<Guest> Guests
			=> this.GetTable<Guest>();

		public ITable<Appeal> Appeals
			=> this.GetTable<Appeal>();

		public ITable<Violation> Violations
			=> this.GetTable<Violation>();

		public ITable<UsersToGuilds> UsersToGuilds
			=> this.GetTable<UsersToGuilds>();


		public ITable<News> News
			=> this.GetTable<News>();

		public ITable<NewsGuild> NewsGuilds
			=> this.GetTable<NewsGuild>();

		public ITable<NewsView> NewsViews
			=> this.GetTable<NewsView>();

		public ITable<NewsHide> NewsHides
			=> this.GetTable<NewsHide>();

		public ITable<NewsPin> NewsPins
			=> this.GetTable<NewsPin>();

		public ITable<NewsTags> NewsTags
			=> this.GetTable<NewsTags>();


		public ITable<MetalsCost> MetalsCosts
			=> this.GetTable<MetalsCost>();

		public ITable<Constant> Constants
			=> this.GetTable<Constant>();

		public ITable<Notification> Notes
			=> this.GetTable<Notification>();

		public ITable<Error> Errors
			=> this.GetTable<Error>();


		public ITable<Request> AutoRequests
			=> this.GetTable<Request>();

		public ITable<Car> AutoCars
			=> this.GetTable<Car>();

		public ITable<Driver> AutoDrivers
			=> this.GetTable<Driver>();

		public ITable<Order> Orders
			=> this.GetTable<Order>();

		public ITable<OrderRecord> OrdersRecords
			=> this.GetTable<OrderRecord>();

		// Директива №1

		public ITable<DirectivePage> DirectivePages
			=> this.GetTable<DirectivePage>();

		public ITable<DirectiveSection> DirectiveSections
			=> this.GetTable<DirectiveSection>();

		public ITable<DirectiveDocument> DirectiveDocuments
			=> this.GetTable<DirectiveDocument>();

		public ITable<DirectiveLog> DirectiveLogs
			=> this.GetTable<DirectiveLog>();

		public ITable<UsersToPages> UsersToPages
			=> this.GetTable<UsersToPages>();

		public ITable<DirectiveDocumentView> DirectiveDocumentsViews
			=> this.GetTable<DirectiveDocumentView>();

		// Час ТБ

		public ITable<AccidentRecord> AccidentsRecords
			=> this.GetTable<AccidentRecord>();

		public ITable<AccidentLog> AccidentsLogs
			=> this.GetTable<AccidentLog>();

		public ITable<AccidentView> AccidentsViews
			=> this.GetTable<AccidentView>();

		public ITable<AccidentList> AccidentsLists
			=> this.GetTable<AccidentList>();

		public ITable<AccidentRecordList> AccidentsRecordsLists
			=> this.GetTable<AccidentRecordList>();

		public ITable<AccidentUserList> AccidentsUsersLists
			=> this.GetTable<AccidentUserList>();

		// Телефонный справочник

		public ITable<PhonebookGuild> PhonebookGuilds
			=> this.GetTable<PhonebookGuild>();

		public ITable<PhonebookPosition> PhonebookPositions
			=> this.GetTable<PhonebookPosition>();

		// Документы

		public ITable<DocsMenu> DocsMenu
			=> this.GetTable<DocsMenu>();

		public ITable<DocsPage> DocsPages
			=> this.GetTable<DocsPage>();

		public ITable<DocsSection> DocsSections
			=> this.GetTable<DocsSection>();

		public ITable<DocsDocument> DocsDocuments
			=> this.GetTable<DocsDocument>();

		// Схемы

		public ITable<SchemeContainer> SchemeContainers
			=> this.GetTable<SchemeContainer>();

		public ITable<SchemeDocument> SchemeDocuments
			=> this.GetTable<SchemeDocument>();

		public ITable<SchemeTag> SchemeTags
			=> this.GetTable<SchemeTag>();

		public User Authorize(IPrincipal principal) => Authorize(principal.Identity.Name);

		public User Authorize(string win)
		{
			win = win.ToLower().Replace("vst\\", "");

			if (Users.Count(x => x.UName == win) == 0)
			{
				int uid = this.InsertWithInt32Identity(new User
				{
					UName = win,
					UClass = "user",
					Groups = "",
					DisplayName = "",
					Last = DateTime.Now,
					Nick = ""
				});

				foreach (var id in News.Select(x => x.Id).ToList())
				{
					this.Insert(new NewsView { Date = DateTime.Now, NewsId = id, UserId = uid });
				}

				foreach (var id in AccidentsRecords.Select(x => x.Id).ToList())
				{
					this.Insert(new AccidentView { Date = DateTime.Now, AccidentId = id, UserId = uid });
				}

				RescanUsers(win);
			}

			Users
				.Where(x => x.UName == win)
				.Set(x => x.Last, DateTime.Now)
				.Update();

			return Users.FirstOrDefault(x => x.UName == win);
		}

		public void RescanUsers(string uname = "")
		{
			var names = Users
				.Where(x => uname == "" || uname == x.UName)
				.OrderBy(x => x.UName)
				.Select(x => x.UName).ToList();

			foreach (var name in names)
			{
				using (var context = new PrincipalContext(ContextType.Domain))
				{
					var usr = UserPrincipal.FindByIdentity(context, "vst\\" + name);
					if (usr != null)
					{
						var groups = usr.GetGroups().Select(x => x.Name).ToArray();

						Users
							.Where(x => x.UName == name)
							.Set(x => x.DisplayName, usr.DisplayName)
							.Set(x => x.Groups, string.Join("|", groups))
							.Update();
					}
				}
			}
		}

		public bool HasUnviewedDocsInDirective(User user)
		{
			var unviewedDocs = DirectiveDocumentsViews
				.Where(x => x.UserId == user.UID)
				.Select(x => x.DocumentId)
				.ToList();

			var sections = DirectiveDocuments
				.Where(x => !unviewedDocs.Contains(x.Id))
				.Select(x => x.SectionId)
				.ToList();

			var pages = DirectiveSections
				.Where(x => sections.Contains(x.Id))
				.Select(x => x.PageId)
				.ToList();

			if (!pages.Any()) return false;

			var pagesWithUnviewed = DirectivePages
				.Where(x => string.IsNullOrEmpty(x.AllowedGroup) || user.GroupsArray.Contains(x.AllowedGroup))
				.Where(x => pages.Contains(x.Id))
				.Any();

			return pagesWithUnviewed;
		}
	}
}