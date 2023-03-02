using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
	[Table(Name = "NewsHides")]
	public class NewsHide
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public int NewsId { get; set; }
	}
}