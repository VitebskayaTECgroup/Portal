using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "AccidentsViews")]
	public class AccidentView
	{
		[Identity]
		public int Id { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public int AccidentId { get; set; }

		[Column]
		public DateTime Date { get; set; }
	}
}