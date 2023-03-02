using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
    [Table(Name = "AccidentsLists")]
	public class AccidentList
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public string Name { get; set; }

		// поля для маппинга

		public AccidentListRoles Role { get; set; }
	}
}