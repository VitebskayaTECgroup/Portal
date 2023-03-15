using Schreduler.Actions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schreduler
{
	internal class Program
	{
		public static string Site = @"Data Source=WEB\SQLEXPRESS; Initial Catalog=Site; Persist Security Info=True; User ID=Site; Password=Site";

		public static string Devin = @"Data Source=WEB\SQLEXPRESS; Initial Catalog=Everest; Persist Security Info=True; User ID=user_everesr; Password=EveresT10";

		public static string DBF = @"Provider=vfpoledb; Data Source=\\10.178.9.42\out; Extended Properties=dBASE IV; User ID=Admin; Password=";

		static void Main(string[] args)
		{
#if DEBUG
			NBRB.Parse();

			OneC.Load();
			Users.Load();
			Checkpoint.Load();
			Phonebook.Load();

			Devices.Load();
			MetalsCosts.Load();

			Images.Process();

			Console.WriteLine("... done.");
			Console.ReadLine();
#else
			// интернет-ресурсы
			try { NBRB.Parse(); } catch (Exception e) { Err(e); }

			// пользователи и сотрудники
			try { OneC.Load(); } catch (Exception e) { Err(e); }
			try { Users.Load(); } catch (Exception e) { Err(e); }
			try { Checkpoint.Load(); } catch (Exception e) { Err(e); }
			try { Phonebook.Load(); } catch (Exception e) { Err(e); }

			// основные средства и малооценка, стоимость драгметаллов
			try { Devices.Load(); } catch (Exception e) { Err(e); }
			try { MetalsCosts.Load(); } catch (Exception e) { Err(e); }

			// Служебные задачи
			try { Images.Process(); } catch (Exception e) { Err(e); }
#endif
		}

		static void Err(Exception e)
		{
			Console.WriteLine(e.Message + "\n\t" + e.StackTrace);
			File.WriteAllText(AppContext.BaseDirectory + "err.txt", e.Message + "\n\t" + e.StackTrace);
		}
	}
}
