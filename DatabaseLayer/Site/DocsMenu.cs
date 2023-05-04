using LinqToDB.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Site
{
	[Table(Name = "DocsMenu")]
	public class DocsMenu
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column, NotNull]
		public string Name { get; set; }

		[Column, NotNull]
		public int MenuId { get; set; }

		public List<DocsMenu> SubMenuList { get; set; } = new List<DocsMenu>();

		public List<DocsPage> Pages { get; set; } = new List<DocsPage>();

		public void LoadSubMenuFromAllList(List<DocsMenu> allMenuList)
		{
			SubMenuList = allMenuList
				.Where(x => x.MenuId == Id)
				.ToList();

			foreach (var subMenu in SubMenuList)
			{
				subMenu.LoadSubMenuFromAllList(allMenuList);
			}
		}
	}
}