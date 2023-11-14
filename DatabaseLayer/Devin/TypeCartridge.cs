using LinqToDB.Mapping;

namespace DatabaseLayer.Devin
{
	[Table(Name = "TypesCartridges")]
	public class TypeCartridge
	{
		[Column]
		public string Id { get; set; }

		[Column]
		public string Name { get; set; }

		[Column]
		public string Type { get; set; }
	}
}