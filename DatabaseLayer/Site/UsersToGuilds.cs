using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
	[Table]
	public class UsersToGuilds
	{
		[Column]
		public string UserName { get; set; }

		[Column]
		public int GuildId { get; set; }
	}
}