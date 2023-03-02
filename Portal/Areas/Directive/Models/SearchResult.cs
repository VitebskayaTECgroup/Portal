using DatabaseLayer.Site;

namespace Portal.Areas.Directive.Models
{
	public class SearchResult
	{
		public string PageUrl { get; set; }

		public string Page { get; set; }

		public string Section { get; set; }

		public DirectiveDocument Document { get; set; }
	}
}