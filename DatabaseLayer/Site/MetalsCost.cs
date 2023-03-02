using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "MetalsCosts")]
	public class MetalsCost
	{
		[Column, DataType(LinqToDB.DataType.DateTime)]
		public DateTime Date;

		[Column]
		public string Name;

		[Column]
		public float Cost;

		[Column]
		public float Discount;
	}
}