using LinqToDB.Mapping;
using System.Collections.Generic;

namespace DatabaseLayer.Site
{
	[Table(Name = "DirectivePages")]
	public class DirectivePage
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public string Url { get; set; }

		[Column]
		public string Caption { get; set; }

		[Column]
		public int OrderValue { get; set; }

		[Column]
		public string AllowedGroup { get; set; }

		// fields

		public bool IsRedactor { get; set; } = false;

		public bool IsAdmin { get; set; } = false;

		public bool HasUnviewedDocs { get; set; } = false;

		public List<User> Redactors { get; set; } = new List<User>();

		public List<DirectiveSection> Sections { get; set; } = new List<DirectiveSection>();

		public List<DirectiveDocument> Documents { get; set; } = new List<DirectiveDocument>();

		public List<int> Viewed { get; set;} = new List<int>();
	}
}