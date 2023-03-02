using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
	[Table(Name = "NewsPins")]
	public class NewsPin
	{
		[Column]
		public int UserId { get; set; }

		[Column]
		public int NewsId { get; set; }
	}
}