using LinqToDB.Mapping;

namespace DatabaseLayer.Devin
{
	[Table(Name = "Relation_Officials_Employees")]
	public class Relation_Official_Employee
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public int OfficialId { get; set; }

		[Column]
		public int EmployeeId { get; set; }

		[Column]
		public int Weight { get; set; }
	}
}