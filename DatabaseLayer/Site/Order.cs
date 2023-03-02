using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "Orders")]
	public class Order
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public DateTime Date { get; set; }

		[Column]
		public string Guild { get; set; }

		[Column]
		public int NumberPersonal { get; set; }

		[Column]
		public int NumberMasters { get; set; }
	}
}