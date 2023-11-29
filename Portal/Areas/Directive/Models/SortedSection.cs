namespace Portal.Areas.Directive.Models
{
	public class SortedSection
	{
		public int Id { get; set; }

		public int SectionId { get; set; } = 0;

		public int Order { get; set; } = 1;
	}
}