using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "PhonebookGuilds")]
	public class PhonebookGuild
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public string Name { get; set; }

		[Column]
		public int OrderPriority { get; set; } = 0;
	}
}