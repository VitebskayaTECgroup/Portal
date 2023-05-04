using LinqToDB.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Site
{
	[Table(Name = "DocsSections")]
	public class DocsSection : IDocsPageElement
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public int PageId { get; set; } = 0;

		[Column, NotNull]
		public int SectionId { get; set; } = 0;

		[Column]
		public string Title { get; set; }

		[Column, NotNull]
		public int OrderValue { get; set; } = 0;


		public List<DocsSection> Sections { get; set; } = new List<DocsSection>();

		public List<DocsDocument> Documents { get; set; } = new List<DocsDocument>();

		public List<IDocsPageElement> Elements { get; set; } = new List<IDocsPageElement>();

		public void LoadSectionsFromAll(List<DocsSection> allSections)
		{
			Sections = allSections
				.Where(x => x.SectionId == Id)
				.ToList();

			foreach (var subMenu in Sections)
			{
				subMenu.LoadSectionsFromAll(allSections);
			}
		}
	}
}