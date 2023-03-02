using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
    [Table(Name = "AccidentsRecordsLists")]
	public class AccidentRecordList
	{
		[Column]
		public int ListId { get; set; }

		[Column]
		public int RecordId { get; set; }
	}
}