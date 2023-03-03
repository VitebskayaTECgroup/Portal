using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "PhonebookPositions")]
	public class PhonebookPosition
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public int PhonebookGuildId { get; set; } = 0;

		[Column]
		public string Name { get; set; }

		[Column]
		public string Position { get; set; }

		[Column]
		public string PhoneInner { get; set; }

		[Column]
		public string PhoneCity { get; set; }

		[Column]
		public string WindowsName { get; set; }

		[Column]
		public int OrderPriority { get; set; } = 0;


		public string Email()
		{
			if (string.IsNullOrEmpty(WindowsName)) return string.Empty;

			return WindowsName + "@vst.vitebsk.energo.net";
		}

		public string Image(bool isBig = false)
		{
			if (string.IsNullOrEmpty(Name))
			{
				return "http://www.vst.vitebsk.energo.net/content/images/logo.png";
			}

			return "http://www.vst.vitebsk.energo.net/" + (isBig ? "big_" : "") + "photos/" + Name + ".jpg?date=" + DateTime.Now.ToString("ddMMyyyyHHmmss");
		}
	}
}