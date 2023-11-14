using LinqToDB.Mapping;
using System;
using System.Web.Mvc;

namespace DatabaseLayer.Site
{
	[Table(Name = "News")]
	public class News
	{
		[Column, Identity]
		public int Id { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public string UserName { get; set; }

		[Column]
		public int GuildId { get; set; }

	
		[Column]
		public DateTime DateAdd { get; set; }

		[Column]
		public DateTime DateExpire { get; set; }


		[Column]
		public string Title { get; set; }

		[Column, AllowHtml]
		public string Message { get; set; } = "";

		[Column]
		public string Links { get; set; } = "";

		[Column]
		public string Priority { get; set; }

		[Column]
		public bool IsTemplate { get; set; } = false;

		[Column]
		public string Tags { get; set; } = "";


		public User Creator { get; set; }

		public NewsGuild Guild { get; set; }

		public string GuildName { get; set; }

		public bool IsWatched { get; set; }

		public bool IsHide { get; set; }

		public bool IsPinned { get; set; }

		public bool IsRedactor { get; set; }

		public bool IsTimed => DateAdd.Date < DateExpire.Date;

		public int DaysRemains => (DateExpire.Date - DateTime.Now.Date).Days;
	}
}