using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "AccidentsDocs")]
	public class AccidentDocView
	{
		[Identity]
		public int Id { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public int AccidentId { get; set; }

		[Column]
		public string Link { get; set; }

		[Column]
		public DateTime WhenViewed { get; set; }
	}
}