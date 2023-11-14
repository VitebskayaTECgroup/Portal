using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "Guests")]
	public class Guest
	{
		[Column]
		public DateTime Date { get; set; }

		[Column]
		public string Ip { get; set; }

		[Column]
		public string UserAgent { get; set; }
	}
}