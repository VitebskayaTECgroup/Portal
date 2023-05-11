using LinqToDB.Mapping;
using System.Linq;

namespace DatabaseLayer.Site
{
	[Table(Name = "SchemesDocuments")]
	public class SchemeDocument
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public int ContainerId { get; set; } = 0;

		[Column, NotNull]
		public string Link { get; set; } = string.Empty;

		[Column, NotNull]
		public string Name { get; set; } = string.Empty;

		[Column, NotNull]
		public string TagsString { get; set; } = string.Empty;

		public int[] Tags => TagsString
			.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
			.Select(x => int.Parse(x))
			.ToArray();
	}
}