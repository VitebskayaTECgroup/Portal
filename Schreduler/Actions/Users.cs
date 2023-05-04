using DatabaseLayer.Site;
using System;

namespace Schreduler.Actions
{
	public class Users
	{
		public static void Load()
		{
			Console.WriteLine("Users => start");

			using (var db = new SiteContext())
			{
				db.RescanUsers();
			}

			Console.WriteLine("Users => stop");
		}
	}
}