using LinqToDB.Mapping;
using System.Collections.Generic;

namespace DatabaseLayer.Site
{
	[Table(Name = "DirectiveSections")]
	public class DirectiveSection
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public int PageId { get; set; }

		[Column]
		public string Caption { get; set; }

		[Column]
		public int OrderValue { get; set; }

		// fields

		public List<DirectiveDocument> Documents { get; set; } = new List<DirectiveDocument>();
	}
}