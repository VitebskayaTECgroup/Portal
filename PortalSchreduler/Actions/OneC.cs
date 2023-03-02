using Dapper;
using DatabaseLayer.Site;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;

namespace PortalSchreduler.Actions
{
	public static class OneC
    {
        public static void Load()
        {
            Console.WriteLine("OneC => start");

            // Загрузка данных с выгрузки 1С
            var personal = new List<Person>();

            using (var db = new OleDbConnection(ConfigurationManager.ConnectionStrings["Dbf"].ToString()))
            {
                string sql = @"SELECT
                        TABNOMER AS [TabId],
                        FAMILY   AS [Family],
                        PODR     AS [Guild],
                        DATAROJD AS [BirthDate]
                    FROM T2Vvod.DBF";

                personal = db.Query<Person>(sql).ToList();
            }

            personal = personal
                .Where(x => x.TabId != 0)
                .Where(x => !string.IsNullOrEmpty(x.Guild))
                .Where(x => !string.IsNullOrEmpty(x.Family))
                .ToList();

            foreach (var person in personal)
            {
                person.Sanitaze();
            }

            using (var db = new SiteContext())
            {
                db.Persons
                    .Where(x => !personal.Select(k => k.TabId).Contains(x.TabId))
                    .Delete();

                foreach (var person in personal)
                {
                    if (db.Persons.Count(x => x.TabId == person.TabId) == 0)
                    {
                        db.Insert(new Person
                        {
                            TabId = person.TabId,
                            Family = person.Family.Replace("  ", " ").Replace("  ", " ").Trim(),
                            BirthDate = person.BirthDate,
                            Guild = person.Guild,
                            OnWork = false,
                        });
                    }
                    else
					{
                        db.Persons
                            .Where(x => x.TabId == person.TabId)
                            .Set(x => x.BirthDate, person.BirthDate)
                            .Set(x => x.Family, person.Family.Replace("  ", " ").Replace("  ", " ").Trim())
                            .Set(x => x.Guild, person.Guild)
                            .Update();
                    }
                }
            }

            Console.WriteLine("OneC => stop");
        }
    }
}