using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "AccidentsRecords")]
	public class AccidentRecord
	{
		[Identity]
		public int Id { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public DateTime DateCreated { get; set; }

		[Column]
		public DateTime Date { get; set; }
		
		[Column]
		public string Description { get; set; }

		[Column]
		public DateTime DateControl { get; set; }

		[Column]
		public bool IsTemplate { get; set; }

		[Column]
		public bool IsDeleted { get; set; }


		// fields

		public bool IsWatched { get; set; } = false;

		public AccidentView Watch { get; set; } = null;

		public DateTime GetWeek()
		{
			var day = (int)DateControl.DayOfWeek;
			var monday = DateControl.AddDays(day == 1 ? 0 : day > 1 ? (-1 * (day - 1)) : -6);
			return monday;
		}
	}
}