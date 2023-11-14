using LinqToDB.Mapping;

namespace DatabaseLayer.Checkpoint
{
	[Table]
	public class OWNERS
	{
		[Column, Identity]
		public int OW_ID { get; set; }

		[Column]
		public string OW_LNAME { get; set; }

		[Column]
		public string OW_FNAME { get; set; }

		[Column]
		public string OW_MNAME { get; set; }

		[Column]
		public string OW_TABNUM { get; set; }

		[Column]
		public int? OW_JOB { get; set; }

		[Column]
		public int? OW_DEP { get; set; }

		[Column]
		public int? OW_DEF_GROUP { get; set; }

		[Column]
		public int OW_TYPE { get; set; } = 0;

		[Column]
		public int OW_ARCHIVE { get; set; } = 0;

		[Column]
		public string OW_EXT_ID { get; set; }
	}
}