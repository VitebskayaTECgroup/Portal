using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
	[Table(Name = "NewsTags")]
	public class NewsTags
	{
		[Column]
		public string Token { get; set; }

		[Column]
		public string Name { get; set; }
	}
}