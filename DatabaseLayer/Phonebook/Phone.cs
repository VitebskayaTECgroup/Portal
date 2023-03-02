using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Phonebook
{
	[Table(Name = "Телефоны")]
	public class Phone
	{
		[Column(Name = "Id"), NotNull]
		public int Id { get; set; }

		[Column(Name = "Человек")]
		public string Name { get; set; } = "";

		[Column(Name = "Цех")]
		public string Guild { get; set; } = "";

		[Column(Name = "Объект")]
		public string Position { get; set; } = "";

		[Column(Name = "Внутренний")]
		public string InnerPhone { get; set; } = "";

		[Column(Name = "Городской")]
		public string OuterPhone { get; set; } = "";

		[Column(Name = "Email")]
		public string Email { get; set; } = "";

		[Column(Name = "Вес")]
		public int OrderPriority { get; set; } = 1;

		[Column(Name = "ВесЦеха")]
		public int GuildPriority { get; set; } = 1;

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
