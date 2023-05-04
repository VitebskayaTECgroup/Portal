using LinqToDB.Mapping;
using System.Collections.Generic;

namespace DatabaseLayer.Site
{
	[Table(Name = "DocsDocuments")]
	public class DocsDocument : IDocsPageElement
	{
		[Column, Identity]
		public int Id { get; set; } = 0;

		[Column, NotNull]
		public int PageId { get; set; } = 0;

		[Column, NotNull]
		public int SectionId { get; set; } = 0;

		[Column, NotNull]
		public string Type { get; set; } = "text";

		[Column]
		public string Caption { get; set; }

		[Column]
		public string Text { get; set; }

		[Column]
		public string Link { get; set; }

		[Column]
		public string TagsString { get; set; }

		[Column]
		public string ViewMode { get; set; }

		[Column, NotNull]
		public bool IsTemplate { get; set; } = true;

		[Column, NotNull]
		public int OrderValue { get; set; } = 0;


		public bool IsRedactor { get; set; } = false;

		public static Dictionary<string, string> Types = new Dictionary<string, string>
		{
			{ "text", "Текст" },
			{ "document", "Документ" },
			{ "link", "Ссылка" },
			{ "image", "Изображение" },
		};
	}
}