using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
	[Table(Name = "SchemesTags")]
	public class SchemeTag
	{
		[Column, Identity]
		public int Id { get; set; } = 0;

		[Column, NotNull]
		public string Text { get; set; } = string.Empty;
	}
}