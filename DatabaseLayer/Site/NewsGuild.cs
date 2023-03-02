using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
	[Table(Name = "NewsGuilds")]
	public class NewsGuild
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public string Name { get; set; }

		[Column]
		public bool IsPrivate { get; set; }
	}
}