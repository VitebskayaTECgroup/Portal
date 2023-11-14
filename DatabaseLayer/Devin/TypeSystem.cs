using LinqToDB.Mapping;

namespace DatabaseLayer.Devin
{
	[Table(Name = "TypesSystems")]
	public class TypeSystem
	{
		[Column]
		public string Id { get; set; }

		[Column]
		public string Name { get; set; }
	}
}