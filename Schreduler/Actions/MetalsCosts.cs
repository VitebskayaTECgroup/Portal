using Dapper;
using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace Schreduler.Actions
{
	public class MetalsCosts
	{
		public static void Load()
		{

			Console.WriteLine("MetalsCosts => start");

			using (var conn = new OleDbConnection(Program.DBF))
			{
				var metals = conn.Query<Metals>("SELECT PERIOD, NAIM, CENA, SKID_PROC FROM drag.DBF ORDER BY PERIOD, NAIM");
				var list = metals.ToList();

				using (var db = new SqlConnection(Program.Site))
				{
					db.Execute("DELETE FROM MetalsCosts");
					db.Execute("INSERT INTO MetalsCosts (Date, Name, Cost, Discount) VALUES (@PERIOD, @NAIM, @CENA, @SKID_PROC)", metals);
				}
			}

			Console.WriteLine("MetalsCosts => stop");
		}
	}

	public class Metals
	{
		public DateTime PERIOD { get; set; }

		public string NAIM { get; set; }

		public float CENA { get; set; }

		public float SKID_PROC { get; set; }
	}
}