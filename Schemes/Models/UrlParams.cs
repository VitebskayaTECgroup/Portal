using System.Linq;

namespace Schemes.Models
{
	public class UrlParams
	{
		public int Id { get; set; }

		public string Query { get; set; }

		public string TagsString { get; set; }

		public int[] Tags => TagsString
			.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries)
			.Select(x => int.Parse(x))
			.ToArray();
	}
}