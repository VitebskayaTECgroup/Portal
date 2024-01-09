using DatabaseLayer.Site;
using System.Collections.Generic;

namespace Portal.Models
{
	public class SafetyEditModel
	{
		public AccidentRecord Record { get; set; }

		public List<AccidentView> Views { get; set; } = new List<AccidentView>();

		public List<User> Users { get; set; } = new List<User>();
	}
}