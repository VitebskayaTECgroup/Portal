using DatabaseLayer.Checkpoint;
using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Linq;

namespace Schreduler.Actions
{
	public static class Checkpoint
	{
		public static void Load()
		{
			Console.WriteLine("Checkpoint => start");

			using (var db = new SiteContext())
			{
				// Инфа с проходной, чтобы отслеживать, кто есть на работе

				var identities = db.Persons.Select(x => x.TabId).ToList();

				using (var db2 = new CheckpointContext())
				{
					var tabs = db2.OWNERS
						.ToDictionary(x => x.OW_ID, x => int.TryParse(x.OW_TABNUM.Trim(), out int i) ? i : 0);

					var events = db2.EVENTS
						.Where(x => x.EV_TSTAMP > DateTime.Today.AddDays(-1) && x.EV_OW_ID != null)
						.OrderBy(x => x.EV_TSTAMP)
						.ToList()
						.Select(x => new
						{
							x.EV_TSTAMP,
							x.EV_ADDR,
							TabId = tabs[x.EV_OW_ID.Value]
						})
						.ToList()
						.GroupBy(x => x.TabId)
						.Select(g => new
						{
							TabId = g.Key,
							OnWork = (g.OrderByDescending(x => x.EV_TSTAMP).FirstOrDefault()?.EV_ADDR ?? 0) == 1
						})
						.ToList();

					foreach (var ev in events)
					{
						Console.WriteLine(ev.TabId + " | " + ev.OnWork);

						db.Persons
							.Where(x => x.TabId == ev.TabId)
							.Set(x => x.OnWork, ev.OnWork)
							.Update();
					}
				}
			}

			Console.WriteLine("Checkpoint => stop");
		}
	}
}