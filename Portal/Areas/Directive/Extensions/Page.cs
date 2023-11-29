using DatabaseLayer.Site;
using System;
using System.Linq;

namespace Portal.Areas.Directive.Extensions
{
	public static class Page
	{
		public static void LoadContent(this DirectivePage page, SiteContext db)
		{
			var sections = db.DirectiveSections
				.Where(x => x.PageId == page.Id)
				.ToList();

			var documents = db.DirectiveDocuments
				.Where(x => x.PageId == page.Id || sections.Select(s => s.Id).Contains(x.SectionId))
				.ToList();

			foreach (var sect in sections)
			{
				sect.Documents = documents
					.Where(x => x.SectionId == sect.Id)
					.OrderBy(x => x.OrderValue)
					.ToList();
			}

			page.Sections = sections
				.Where(x => x.SectionId == 0)
				.OrderBy(x => x.OrderValue)
				.ToList();

			page.Documents = documents
				.Where(x => x.SectionId == 0)
				.OrderBy(x => x.OrderValue)
				.ToList();

			foreach (var section in page.Sections)
			{
				section.Load(sections);
			}
		}
	}
}