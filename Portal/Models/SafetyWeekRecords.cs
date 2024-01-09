using DatabaseLayer.Site;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Models
{
	public class SafetyWeekRecords
	{
		public bool IsRedactor { get; set; } = false;

		public List<AccidentRecord> Records { get; set; } = new List<AccidentRecord>();

		public List<AccidentView> Views { get; set; } = new List<AccidentView>();

		public SafetyWeekRecords(SiteContext db, User user, DateTime weekDay)
		{
			Records = db.AccidentsRecords
				.Where(x => x.DateControl.Date == weekDay.Date)
				.Where(x => !x.IsTemplate)
				.Where(x => !x.IsDeleted)
				.ToList();

			Views = db.AccidentsViews
				.Where(x => Records.Select(y => y.Id).Contains(x.AccidentId))
				.Where(x => x.UserId == user.UID)
				.ToList();

			var docs = db.AccidentDocs
				.Where(x => Records.Select(y => y.Id).Contains(x.AccidentId))
				.Where(x => x.UserId == user.UID)
				.ToList();

			var listId = 4;
			var redactors = db.Users
				.Where(x => db.AccidentsUsersLists.Where(y => y.ListId == listId && y.Role == AccidentListRoles.Writer).Select(y => y.UserId).Contains(x.UID)
					|| Records.Select(y => y.UserId).Contains(x.UID))
				.ToList();

			#pragma warning disable IDE0075 // Упростить условное выражение
			IsRedactor = user.UName == "void"
				? false
				: redactors.Select(x => x.UID).Contains(user.UID);
			#pragma warning restore IDE0075 // Упростить условное выражение

			foreach (var record in Records)
			{
				record.Creator = redactors.FirstOrDefault(x => x.UID == record.UserId);
				record.IsWatched = IsRedactor || Views.Any(x => x.AccidentId == record.Id);
				record.Docs = docs.Where(x => x.AccidentId == record.Id).ToList();
			}
		}
	}
}