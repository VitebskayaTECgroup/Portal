using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "Violations")]
	public class Violation
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public DateTime DateCreated { get; set; }

		[Column]
		public DateTime DateViolation { get; set; }

		[Column]
		public string PersonName { get; set; }

		[Column]
		public string PersonPosition { get; set; }

		[Column]
		public string PersonDepartment { get; set; }

		[Column]
		public string HeadName { get; set; }

		[Column]
		public string ProcessName { get; set; }

		[Column]
		public string Description { get; set; }

		[Column]
		public string Result { get; set; }

		[Column]
		public string OrderId { get; set; }

		[Column]
		public int? TabId { get; set; }

		[Column]
		public string JudgeName { get; set; }

		[Column]
		public bool IsDeleted { get; set; }
	}
}