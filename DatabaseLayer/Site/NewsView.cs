using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
    [Table(Name = "NewsViews")]
	public class NewsView
	{
		[Column]
		public int UserId { get; set; }

		[Column]
		public int NewsId { get; set; }

		[Column]
		public DateTime Date { get; set; }
	}
}