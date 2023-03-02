using LinqToDB.Mapping;

namespace DatabaseLayer.Devin
{
	[Table(Name = "WriteoffsMarks")]
	public class WriteoffMark
	{
		[Column]
		public string DateMark { get; set; }

		[Column]
		public int Number { get; set; }
	}
}