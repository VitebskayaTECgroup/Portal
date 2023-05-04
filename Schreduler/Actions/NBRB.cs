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
						string key = row.QuerySelector(".curAmount")?.TextContent ?? null;
						string value = row.QuerySelector(".curCours div")?.TextContent ?? null;

						if (key != null)
						{
							key = key.Trim();

							if (key.Contains("USD"))
							{
								conn.Execute("UPDATE Constants SET Value = @Value WHERE Keyword = @Keyword", new { Keyword = "USD", Value = value.Trim() });
								Console.WriteLine("USD: " + value.Trim());
							}
							if (key.Contains("EUR"))
							{
								conn.Execute("UPDATE Constants SET Value = @Value WHERE Keyword = @Keyword", new { Keyword = "EUR", Value = value.Trim() });
								Console.WriteLine("EUR: " + value.Trim());
							}
							if (key.Contains("RUB"))
							{
								conn.Execute("UPDATE Constants SET Value = @Value WHERE Keyword = @Keyword", new { Keyword = "RUB", Value = value.Trim() });
								Console.WriteLine("RUB: " + value.Trim());
							}
						}
					}
					catch (Exception) { }
				}
			}

			Console.WriteLine("NBRB => stop");
		}
	}
}