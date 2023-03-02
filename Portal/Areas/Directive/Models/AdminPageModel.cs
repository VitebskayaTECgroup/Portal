using DatabaseLayer.Site;
using System.Collections.Generic;

namespace Portal.Areas.Directive.Models
{
	public class AdminPageModel
	{
		public DirectivePage Page { get; set; }

		public List<User> Users { get; set; }

		public List<int> PageUsers { get; set; }
	}
}