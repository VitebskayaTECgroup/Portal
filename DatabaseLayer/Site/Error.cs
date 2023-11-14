using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "Errors")]
	public class Error
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public string Page { get; set; }

		[Column]
		public string User { get; set; }

		[Column, DataType(LinqToDB.DataType.DateTime)]
		public DateTime Date { get; set; }

		[Column]
		public string StackTrace { get; set; }
	}
}