using Dapper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;

namespace Schreduler.Actions
{
	public static class Devices
    {
        public static void Load()
        {
            Console.WriteLine("Devices => start");

            var objects = new List<Object1C>();
            using (var conn = new OleDbConnection(Program.DBF))
            {
                objects.AddRange(conn.Query<Object1C>(@"SELECT
                        INV        AS [Inventory],
                        NAIM       AS [Description],
                        CEH        AS [Guild],
                        PLACE      AS [SubDivision],
                        MOL        AS [Mol],
                        BAL_ST     AS [BalanceCost],
                        AMORT_ST   AS [DepreciationCost],
                        AU         AS [Gold],
                        AG         AS [Silver],
                        PT         AS [Platinum],
                        PD         AS [Palladium],
                        MPG        AS [Mpg],
                        DATA_VVODA AS [Date]
                    FROM os.DBF"));

                objects.AddRange(conn.Query<Object1C>(@"SELECT 
                        COD    AS [InventoryNew], 
                        COD2   AS [Inventory], 
                        SCHET  AS [Account],
                        NAIM   AS [Description], 
                        KOL    AS [Rest],
                        STOIM  AS [RestCost],
                        DPRIH  AS [Date],
                        MESTO  AS [Location],
                        MOL    AS [Mol],
                        ED_IZM AS [Unit]
                    FROM mater.DBF"));
            }
            foreach (var obj in objects) obj.Process();

            using (var conn = new SqlConnection(Program.Devin))
            {
                conn.Open();

                // Заменяем данные из 1С
                conn.Execute("DELETE FROM Objects1C");
                conn.Execute("INSERT INTO Objects1C (Inventory, Description, Guild, SubDivision, Mol, Account, Location, Unit, Date, Rest, BalanceCost, DepreciationCost, RestCost, Gold, Silver, Platinum, Palladium, Mpg) VALUES (@InventoryNew, @Description, @Guild, @SubDivision, @Mol, @Account, @Location, @Unit, @Date, @Rest, @BalanceCost, @DepreciationCost, @RestCost, @Gold, @Silver, @Platinum, @Palladium, @Mpg)", objects);

                // Переопределяем МОЛы
                var devices = conn.Query<string>("SELECT Inventory FROM Devices GROUP BY Inventory").ToList();
                foreach (var inventory in devices)
                {
                    conn.Execute("UPDATE Devices SET InventoryOld = @I WHERE Inventory = @I", new { I = inventory });

                    var mols = conn.Query<string>("SELECT Mol FROM Objects1C WHERE Inventory = @I", new { I = inventory }).ToList();
                    if (mols.Count == 1)
                    {
                        conn.Execute("UPDATE Devices SET Mol = @M WHERE Inventory = @I", new { I = inventory, M = mols[0] });
                    }
                }

                conn.Close();
            }

            Console.WriteLine("Devices => stop");
        }
    }

    public class Object1C
    {
        public string Inventory { get; set; }

        public string InventoryNew { get; set; }

        public string Description { get; set; }

        public string Guild { get; set; }

        public string SubDivision { get; set; }

        public string Mol { get; set; }

        public string Account { get; set; }

        public string Location { get; set; }

        public string Unit { get; set; }

        public DateTime? Date { get; set; }

        public DateTime DateImported { get; set; }

        public int Rest { get; set; }

        public float BalanceCost { get; set; }

        public float DepreciationCost { get; set; }

        public float RestCost { get; set; }

        public float Gold { get; set; }

        public float Silver { get; set; }

        public float Platinum { get; set; }

        public float Palladium { get; set; }

        public float Mpg { get; set; }

        public bool IsHide { get; set; }

        public bool IsChecked { get; set; }

        public void Process()
        {
            if (!string.IsNullOrEmpty(Inventory)) Inventory = Inventory.Trim(); else Inventory = "";
            if (!string.IsNullOrEmpty(InventoryNew)) InventoryNew = InventoryNew.Trim(); else InventoryNew = Inventory;
            if (!string.IsNullOrEmpty(Description)) Description = Description.Trim(); else Description = "";
            if (!string.IsNullOrEmpty(Guild)) Guild = Guild.Trim(); else Guild = "";
            if (!string.IsNullOrEmpty(SubDivision)) SubDivision = SubDivision.Trim(); else SubDivision = "";
            if (!string.IsNullOrEmpty(Mol)) Mol = Mol.Trim(); else Mol = "";
            if (!string.IsNullOrEmpty(Account)) Account = Account.Trim(); else Account = "";
            if (!string.IsNullOrEmpty(Location)) Location = Location.Trim(); else Location = "";
            if (!string.IsNullOrEmpty(Unit)) Unit = Unit.Trim(); else Unit = "";

            if (Date?.Year < 1990) Date = null;

            if (Rest > 1)
            {
                RestCost /= Rest;
            }
        }
    }
}