using LinqToDB.Mapping;

namespace DatabaseLayer.Devin
{
	[Table(Name="AidaDescriptions")]
	public class AidaDescription
	{
		[Column]
		public string Name { get; set; }

		[Column]
		public string Description { get; set; }
	}
}