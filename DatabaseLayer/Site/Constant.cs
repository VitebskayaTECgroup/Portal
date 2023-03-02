using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
	[Table(Name = "Constants")]
	public class Constant
	{
		[Column]
		public string Keyword;

		[Column]
		public string Value;
	}
}