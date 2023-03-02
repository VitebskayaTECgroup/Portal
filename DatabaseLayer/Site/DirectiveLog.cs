using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "DirectiveLogs")]
	public class DirectiveLog
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public DateTime DateTime { get; set; } = DateTime.Now;

		[Column]
		public string Text { get; set; }
	}
}