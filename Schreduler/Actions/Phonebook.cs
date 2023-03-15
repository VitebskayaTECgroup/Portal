using DatabaseLayer.Site;
using LinqToDB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Schreduler.Actions
{
	public static class Phonebook
    {
		public static void Load()
		{
			Console.WriteLine("Phonebook => start");

			var persons = new List<Person>();
			var vacations = new List<Vacation>();
			DateTime d;
			int i;

			for (int k = 1; k < 5; k++)
			{
				string raw = new WebClient().DownloadString("http://phone.vitebsk.energo.net/users?page=" + k + "&size=50&org=%D0%92%D0%B8%D1%82%D0%B5%D0%B1%D1%81%D0%BA%D0%B0%D1%8F%20%D0%A2%D0%AD%D0%A6");

				var scheme = JsonConvert.DeserializeObject<Scheme>(raw);

				foreach (var xxx in scheme.hits)
				{
					Console.WriteLine(xxx.name);

					foreach (var yyy in xxx.data)
					{
						Console.WriteLine("\t" + yyy.name);

						foreach (var zzz in yyy.data)
						{
							var person = new Person
							{
								TabId = int.TryParse(zzz._source.tn, out i) ? i : 0,
								Family = zzz._source.fio.Replace("  ", " ").Replace("  ", " ").Trim(),
								Department = yyy.name,
								Position = zzz._source.dolgn,
								Phone = zzz._source.telefrab,
								PhoneInner = zzz._source.telef,
								OnWork = false,
								Email = zzz._source.email,
								BirthDate = DateTime.TryParse(zzz._source.dtrogd, out d) ? d : DateTime.MinValue,
								OrderDepartment = zzz._source.order_podr,
								OrderPosition = zzz._source.order_dolgn,
							};

							var vacation = new Vacation
							{
								TabId = person.TabId,
								DateStart = DateTime.TryParse(zzz._source.dtotpn, out d) ? d : DateTime.MinValue,
								DateEnd = DateTime.TryParse(zzz._source.dtotpk, out d) ? d : DateTime.MinValue
							};

							persons.Add(person);
							vacations.Add(vacation);

							Console.WriteLine(person.TabId + " " + person.Family + " " + person.Phone + " " + person.PhoneInner);
						}
					}
				}
			}

			Console.WriteLine("Persons: " + persons.Count);
			Console.WriteLine("Vacations: " + vacations.Count);

			using (var db = new SiteContext())
			{
				foreach (var person in persons)
				{
					if (person.TabId > 0)
					{
						if (db.Persons.Count(x => x.TabId == person.TabId) > 0)
						{
							db.Persons
								.Where(x => x.TabId == person.TabId)
								.Set(x => x.Department, person.Department)
								.Set(x => x.Email, person.Email)
								.Set(x => x.Position, person.Position)
								.Set(x => x.Phone, person.Phone)
								.Set(x => x.PhoneInner, person.PhoneInner)
								.Set(x => x.OrderDepartment, person.OrderDepartment)
								.Set(x => x.OrderPosition, person.OrderPosition)
								.Update();

							Console.WriteLine("Update: " + person.TabId);
						}
						else
						{
							db.Insert(person);

							Console.WriteLine("Insert: " + person.TabId);
						}
					}
				}

				db.Persons
					.Where(x => !persons.Select(y => y.TabId).Contains(x.TabId))
					.Set(x => x.Department, string.Empty)
					.Set(x => x.Email, string.Empty)
					.Set(x => x.Position, string.Empty)
					.Set(x => x.Phone, string.Empty)
					.Set(x => x.PhoneInner, string.Empty)
					.Set(x => x.OrderDepartment, 0)
					.Set(x => x.OrderPosition, 0)
					.Update();

				foreach (var vacation in vacations)
				{
					if (vacation.IsValid)
					{
						if (db.Vacations.Count(x => x.TabId == vacation.TabId && x.DateStart == vacation.DateStart && x.DateEnd == vacation.DateEnd) == 0)
						{
							db.Insert(vacation);

							Console.WriteLine("Vacation: " + vacation.TabId);
						}
					}
				}
			}

			Console.WriteLine("Phonebook => stop");
		}
	}

    public class Scheme
	{
        public List<Hit> hits;
        public int total;
	}

    public class Hit
	{
        public List<HitData> data;
        public string name;
	}

    public class HitData
	{
        public List<HitSubData> data;
        public string name;
	}

    public class HitSubData
	{
        public HitSource _source;
    }

    public class HitSource
    {
        public string dolgn;
        public string dtotpk;
        public string dtotpn;
        public string dtrogd;
        public string email;
        public string fio;
        public int order_dolgn;
        public int order_podr;
        public string telef;
        public string telefkod;
        public string telefrab;
        public string tn;
    }
}