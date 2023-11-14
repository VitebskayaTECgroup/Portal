using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
	[Table(Name = "Personal")]
	public class Person
	{
		[Column, NotNull]
		public int TabId { get; set; }

		[Column]
		public string Family { get; set; }

		[Column]
		public string Guild { get; set; }

		[Column]
		public DateTime BirthDate { get; set; }

		[Column]
		public bool OnWork { get; set; }

		[Column]
		public string Position { get; set; }

		[Column]
		public string Phone { get; set; }

		[Column]
		public string PhoneInner { get; set; }

		[Column]
		public string Department { get; set; }

		[Column]
		public string Email { get; set; }

		[Column]
		public int? OrderPosition { get; set; }

		[Column]
		public int? OrderDepartment { get; set; }


		// поля для модуля "Дни рождений"

		public string[] Data => Family.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

		public string LastName => Data[0].Trim();

		public string FirstName => Data[1].Trim();

		public string MiddleName => Data[2].Trim();

		public int CalculateAge()
		{
			DateTime temp = BirthDate;
			int age = 0;
			while ((temp = temp.AddYears(1)) < DateTime.Now)
				age++;
			return age;
		}

		public void Sanitaze()
		{
			Family = Family.Trim();
			Guild = Guild.Trim();

			string s = "";
			char[] chars = Family.ToLower().ToCharArray();

			for (int i = 0; i < chars.Length; i++)
			{
				if (i == 0)
				{
					s += chars[i].ToString().ToUpper();
				}
				else if (chars[i - 1] == ' ' || chars[i - 1] == '-')
				{
					s += chars[i].ToString().ToUpper();
				}
				else
				{
					s += chars[i].ToString();
				}
			}

			Family = s;
		}

		public string Image(bool isBig = false)
		{
			if (string.IsNullOrEmpty(Family))
			{
				return "http://www.vst.vitebsk.energo.net/content/images/logo.png";
			}

			return "http://www.vst.vitebsk.energo.net/" + (isBig ? "big_" : "") + "photos/" + Family + ".jpg";
		}

		public static string GetPhone(string phone)
		{
			string buff = "";

			int k = 0;
			for (int i = phone.Length - 1; i >= 0; i--)
			{
				if (k % 2 == 0 && k > 0) buff = "-" + buff;
				k++;
				buff = phone[i] + buff;
			}

			return buff;
		}
	}
}