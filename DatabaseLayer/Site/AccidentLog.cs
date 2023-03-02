using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "AccidentsLogs")]
	public class AccidentLog
	{
		[Identity]
		public int Id { get; set; }

		[Column]
		public DateTime Date { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public int AccidentId { get; set; }

		[Column]
		public string Reason { get; set; }
	}
}