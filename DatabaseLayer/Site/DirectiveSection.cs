using LinqToDB.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Site
{
	[Table(Name = "DirectiveSections")]
	public class DirectiveSection
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public int PageId { get; set; } = 0;

		[Column]
		public int SectionId { get; set; } = 0;

		[Column]
		public string Caption { get; set; }

		[Column]
		public int OrderValue { get; set; } = 1;

		// fields

		public List<DirectiveSection> Sections { get; set; } = new List<DirectiveSection>();

		public List<DirectiveDocument> Documents { get; set; } = new List<DirectiveDocument>();

		// methods

		public void Load(List<DirectiveSection> all)
		{
			Sections = all
				.Where(x => x.SectionId == Id)
				.OrderBy(x => x.OrderValue)
				.ToList();

			foreach (var section in Sections)
			{
				section.Load(all);
			}
		}
	}
}