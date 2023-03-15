using Dapper;
using System;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;

namespace Schreduler.Actions
{
	public class Users
    {
        public static void Load()
        {
            Console.WriteLine("Users => start");

            using (var conn = new SqlConnection(Program.Site))
            {
                var names = conn.Query<string>("SELECT UName FROM Users ORDER BY UName").ToList();

                foreach (var name in names)
                {
                    Console.WriteLine("\n" + name);

                    using (var context = new PrincipalContext(ContextType.Domain))
                    {
                        var usr = UserPrincipal.FindByIdentity(context, "vst\\" + name);
                        if (usr != null)
                        {
                            var groups = usr.GetGroups().Select(x => x.Name).ToArray();

                            conn.Execute("UPDATE Users SET DisplayName = @DisplayName WHERE UName = @name", new { name, usr.DisplayName });
                            Console.WriteLine("\tDisplayName = " + usr.DisplayName);

                            conn.Execute("UPDATE Users SET Groups = @Groups WHERE UName = @name", new { name, Groups = string.Join("|", groups) });
                            Console.WriteLine("\tGroups = " + string.Join("|", groups));
                        }
                        else
                        {
                            //conn.Execute("DELETE FROM Users WHERE UName = @name", new { name });
                            Console.WriteLine("\tNot found!");
                        }
                    }
                }
            }

            Console.WriteLine("Users => stop");
        }
    }
}