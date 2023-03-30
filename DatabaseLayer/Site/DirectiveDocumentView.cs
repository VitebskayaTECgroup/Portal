using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "DirectiveDocumentsViews")]
	public class DirectiveDocumentView
	{
		[Column]
		public int DocumentId { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public DateTime WhenSeen { get; set; }
	}
}