using AngleSharp.Html.Parser;
using Dapper;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PortalSchreduler.Actions
{
	public class MeteoBy
	{
		//static public async Task Parse()
		//{
			//Console.WriteLine("MeteoBy => start");

			//var raw = await HtmlLoader.LoadAsync("https://meteo.by/vitebsk/");
			//var document = new HtmlParser().ParseDocument(raw);

			//var tables = document.QuerySelectorAll("ul.b-weather");

			//foreach (var table in tables)
			//{
			//	string[] digits = table
			//		.QuerySelector(".temp")
			//		.QuerySelector("nobr")
			//		.TextContent
			//		.Trim()
			//		.Replace("\n", " ")
			//		.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			//	string icon = table.QuerySelector(".icon").TextContent.Trim().Replace("\n", " ");
			//	string row = "(" + digits[0] + " ... " + digits[1] + ") °C - " + icon;

			//	switch (icon)
			//	{
			//		case 0: WeatherNight = row; break;
			//		case 1: WeatherMorning = row; break;
			//		case 2: WeatherDay = row; break;
			//		case 3: WeatherEvening = row; break;
			//	}

			//	i++;
			//}

			//string WeatherImageSource = document.QuerySelector(".weather").QuerySelector("img").GetAttribute("src").Replace("/images", "/site/content/images/").Replace("//", "/");

			//using (var conn = new SqlConnection(Program.Site))
			//{
			//	conn.Open();
			//	conn.Execute("UPDATE Constants SET Value = @WeatherNight       WHERE Keyword = 'WeatherNight'      ", new { WeatherNight });
			//	conn.Execute("UPDATE Constants SET Value = @WeatherMorning     WHERE Keyword = 'WeatherMorning'    ", new { WeatherMorning });
			//	conn.Execute("UPDATE Constants SET Value = @WeatherDay         WHERE Keyword = 'WeatherDay'        ", new { WeatherDay });
			//	conn.Execute("UPDATE Constants SET Value = @WeatherEvening     WHERE Keyword = 'WeatherEvening'    ", new { WeatherEvening });
			//	conn.Execute("UPDATE Constants SET Value = @WeatherImageSource WHERE Keyword = 'WeatherImageSource'", new { WeatherImageSource });
			//}

			//Console.WriteLine("MeteoBy => stop");
		//}
	}
}