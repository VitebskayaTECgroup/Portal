using LinqToDB.Mapping;
using System.Collections.Generic;

namespace DatabaseLayer.Site
{
	[Table(Name = "DocsPages")]
	public class DocsPage
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public string Name { get; set; }

		[Column, NotNull]
		public int MenuId { get; set; } = 0;


		public List<IDocsPageElement> Elements { get; set; } = new List<IDocsPageElement>();
	}
}