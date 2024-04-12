using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "OrdersRecords")]
	public class OrderRecord
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public DateTime DateCreated { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public int OrderId { get; set; }

		[Column]
		public string Description { get; set; }

		[Column]
		public string Comment { get; set; }

		[Column]
		public int NumberPlannedOrder { get; set; }

		[Column]
		public int NumberPlannedCommand { get; set; }

		[Column]
		public int NumberUnplannedOrder { get; set; }

		[Column]
		public int NumberUnplannedCommand { get; set; }

		[Column]
		public string ReasonToUndone { get; set; }

		[Column]
		public int AnswerCode { get; set; }

		[Column]
		public int AnswerUserId { get; set; }

		[Column]
		public DateTime? AnswerDate { get; set; }

		public string GetAnswer()
		{
			switch (AnswerCode)
			{
                case 0: return "неизвестно";
                case 1: return "разрешено";
				case 2: return "запрещено";
				default: return "";
			}
		}
	}
}