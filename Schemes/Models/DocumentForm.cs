using DatabaseLayer.Site;
using System.Collections.Generic;

namespace Schemes.Models
{
	public class DocumentForm
	{
		public SchemeDocument Document { get; set; }

		public List<SchemeTag> Tags { get; set; }
	}
}