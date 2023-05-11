using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "Users")]
	public class User
	{
		[Column, Identity]
		public int UID { get; set; }

		[Column]
		public string UName { get; set; }

		[Column]
		public string UClass { get; set; }

		[Column]
		public DateTime Last { get; set; }

		[Column]
		public string Nick { get; set; }

		[Column]
		public string DisplayName { get; set; }

		[Column]
		public string Groups { get; set; }


		// Granties

		public bool IsAdmin => UClass.Contains("site_admin");

		public bool IsAutoHead => UClass.Contains("auto_prov");

		public bool IsViolationsUser => UClass.Contains("record_");

		public bool IsViolationsAdmin => UClass.Contains("record_admin");

		public bool IsWorksUser => IsWorksAdmin || UClass.Contains("works_user");

		public bool IsWorksAdmin => UClass.Contains("works_admin");

		public bool IsOrderUser => UClass.Contains("order_user");

		public bool IsOrderNSS => UClass.Contains("order_nss");

		public bool IsOrderViewer => UClass.Contains("order_");

		public bool IsAccidents => Groups.Contains("group_accidents");

		public bool IsPhonebookAdmin => UClass.Contains("phonebook");

		public bool IsDocsAdmin => UClass.Contains("docs_admin") || IsAdmin;

		public bool IsSchemeAdmin => UClass.Contains("scheme_admin");

		// fields

		public string[] GroupsArray => Groups.ToLower().Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
	}
}