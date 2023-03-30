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
			=> GetTable<User>();

		public ITable<Person> Persons
			=> GetTable<Person>();

		public ITable<Vacation> Vacations
			=> GetTable<Vacation>();

		public ITable<Guest> Guests
			=> GetTable<Guest>();

		public ITable<Appeal> Appeals
			=> GetTable<Appeal>();

		public ITable<Violation> Violations
			=> GetTable<Violation>();

		public ITable<UsersToGuilds> UsersToGuilds
			=> GetTable<UsersToGuilds>();


		public ITable<News> News
			=> GetTable<News>();

		public ITable<NewsGuild> NewsGuilds
			=> GetTable<NewsGuild>();

		public ITable<NewsView> NewsViews
			=> GetTable<NewsView>();

		public ITable<NewsHide> NewsHides
			=> GetTable<NewsHide>();

		public ITable<NewsPin> NewsPins
			=> GetTable<NewsPin>();

		public ITable<NewsTags> NewsTags
			=> GetTable<NewsTags>();


		public ITable<MetalsCost> MetalsCosts
			=> GetTable<MetalsCost>();

		public ITable<Constant> Constants
			=> GetTable<Constant>();

		public ITable<Notification> Notes
			=> GetTable<Notification>();

		public ITable<Error> Errors
			=> GetTable<Error>();


		public ITable<Request> AutoRequests
			=> GetTable<Request>();

		public ITable<Car> AutoCars
			=> GetTable<Car>();

		public ITable<Driver> AutoDrivers
			=> GetTable<Driver>();

		public ITable<Order> Orders
			=> GetTable<Order>();

		public ITable<OrderRecord> OrdersRecords
			=> GetTable<OrderRecord>();

		// Директива №1

		public ITable<DirectivePage> DirectivePages
			=> GetTable<DirectivePage>();

		public ITable<DirectiveSection> DirectiveSections
			=> GetTable<DirectiveSection>();

		public ITable<DirectiveDocument> DirectiveDocuments
			=> GetTable<DirectiveDocument>();

		public ITable<DirectiveLog> DirectiveLogs
			=> GetTable<DirectiveLog>();

		public ITable<UsersToPages> UsersToPages
			=> GetTable<UsersToPages>();

		public ITable<DirectiveDocumentView> DirectiveDocumentsViews
			=> GetTable<DirectiveDocumentView>();

		// Час ТБ

		public ITable<AccidentRecord> AccidentsRecords
			=> GetTable<AccidentRecord>();

		public ITable<AccidentLog> AccidentsLogs
			=> GetTable<AccidentLog>();

		public ITable<AccidentView> AccidentsViews
			=> GetTable<AccidentView>();

		public ITable<AccidentList> AccidentsLists
			=> GetTable<AccidentList>();

		public ITable<AccidentRecordList> AccidentsRecordsLists
			=> GetTable<AccidentRecordList>();

		public ITable<AccidentUserList> AccidentsUsersLists
			=> GetTable<AccidentUserList>();

		// Телефонный справочник

		public ITable<PhonebookGuild> PhonebookGuilds
			=> GetTable<PhonebookGuild>();

		public ITable<PhonebookPosition> PhonebookPositions
			=> GetTable<PhonebookPosition>();


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
	}
}