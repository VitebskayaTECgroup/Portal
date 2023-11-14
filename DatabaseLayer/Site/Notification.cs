using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "Notifications")]
	public class Notification
	{
		[Column, Identity]
		public int N_ID { get; set; }

		[Column]
		public int N_UID { get; set; }

		[Column]
		public bool N_Watched { get; set; }

		[Column, DataType(LinqToDB.DataType.DateTime)]
		public DateTime N_DateCreated { get; set; }

		[Column, DataType(LinqToDB.DataType.DateTime)]
		public DateTime? N_DateWatched { get; set; }

		[Column]
		public string N_App { get; set; }

		[Column]
		public string N_Note { get; set; }

		[Column]
		public int N_CreateUID { get; set; }

		[Column]
		public string N_Link { get; set; }
	}
}