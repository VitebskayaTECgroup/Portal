using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
	[Table(Name = "UsersToPages")]
	public class UsersToPages
	{
		[Column]
		public int PageId { get; set; }

		[Column]
		public int UID { get; set; }
	}
}