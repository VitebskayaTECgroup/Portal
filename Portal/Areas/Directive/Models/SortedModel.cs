using System.Collections.Generic;

namespace Portal.Areas.Directive.Models
{
	public class SortedModel
	{
		public int PageId { get; set; }

		public List<SortedSection> Sections { get; set; } = new List<SortedSection>();

		public List<SortedDocument> Documents { get; set; } = new List<SortedDocument>();
	}
}