namespace Portal.Models
{
	public class LiveFolder
	{
		public string Name { get; set; }

		public string RelativePath { get; set; }

		public bool WriteImageBox { get; set; }

		public bool WriteImageCaptions { get; set; }

		public bool NoImages { get; set; } = false;

		public bool SortByDate { get; set; } = false;

		public bool SortByName { get; set; } = false;
	}
}