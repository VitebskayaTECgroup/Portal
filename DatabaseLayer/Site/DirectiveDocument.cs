using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "DirectiveDocuments")]
	public class DirectiveDocument
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public int SectionId { get; set; }

		[Column]
		public int PageId { get; set; }

		[Column]
		public string FilePath { get; set; }

		[Column]
		public string Name { get; set; }

		[Column]
		public DateTime WhenUpdated { get; set; }

		[Column]
		public int OrderValue { get; set; }

		// fields

		public string FileUrl => FilePath.Replace(@"\\web", "http://www.vst.vitebsk.energo.net").Replace("\\", "/");
	}
}