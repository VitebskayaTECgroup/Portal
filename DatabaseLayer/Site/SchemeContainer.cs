using LinqToDB.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Site
{
	[Table(Name = "SchemesContainers")]
	public class SchemeContainer
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public int ContainerId { get; set; } = 0;

		[Column, NotNull]
		public string Name { get; set; }

		[Column, NotNull]
		public string Description { get; set; }


		public List<SchemeContainer> Containers { get; set; } = new List<SchemeContainer>();

		public List<SchemeDocument> Documents { get; set; } = new List<SchemeDocument>();

		public void Load(List<SchemeContainer> list)
		{
			Containers = list
				.Where(x => x.ContainerId == Id)
				.ToList();

			foreach (var sub in Containers)
			{
				sub.Load(list);
			}
		}
	}
}