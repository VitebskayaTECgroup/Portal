using LinqToDB.Mapping;
using System;
using System.Collections.Generic;

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

		public User Creator { get; set; } = null;

		public List<AccidentDocView> Docs {  get; set; } = new List<AccidentDocView>();

		public List<AccidentView> Views { get; set; } = new List<AccidentView>();

		public DateTime GetWeek()
		{
			var day = (int)DateControl.DayOfWeek;
			var monday = DateControl.AddDays(day == 1 ? 0 : day > 1 ? (-1 * (day - 1)) : -6);
			return monday;
		}
	}
}