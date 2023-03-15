using AngleSharp.Parser.Html;
using Dapper;
using System;
using System.Data.SqlClient;
using System.Net;

namespace Schreduler.Actions
{
	public static class NBRB
	{
		public static void Parse()
		{
			Console.WriteLine("NBRB => start");

			string raw = new WebClient().DownloadString("http://www.nbrb.by/statistics/rates/ratesDaily/");
			var parser = new HtmlParser();
			var document = parser.Parse(raw);

			using (var conn = new SqlConnection(Program.Site))
			{
				conn.Open();

				var rows = document.QuerySelector(".currencyTable").GetElementsByTagName("tr");

				foreach (var row in rows)
				{
					try
					{
						string key = row.QuerySelector(".country")?.TextContent ?? null;
						string value = row.QuerySelector(".curCours div")?.TextContent ?? null;

						if (key != null)
						{
							key = key.Trim();

							if (key == "Доллар США")
								conn.Execute("UPDATE Constants SET Value = @Value WHERE Keyword = @Keyword", new { Keyword = "USD", Value = value.Trim() });
							if (key == "Евро")
								conn.Execute("UPDATE Constants SET Value = @Value WHERE Keyword = @Keyword", new { Keyword = "EUR", Value = value.Trim() });
							if (key == "Российский рубль")
								conn.Execute("UPDATE Constants SET Value = @Value WHERE Keyword = @Keyword", new { Keyword = "RUB", Value = value.Trim() });
						}
					}
					catch (Exception) { }
				}
			}

			Console.WriteLine("NBRB => stop");
		}
	}
}