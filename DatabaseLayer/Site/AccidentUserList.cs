using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
    [Table(Name = "AccidentsUsersLists")]
	public class AccidentUserList
	{
		[Column]
		public int ListId { get; set; }

		[Column]
		public int UserId { get; set; }

		[Column]
		public AccidentListRoles Role { get; set; }

		public static string RoleDescription(AccidentListRoles role)
        {
			if (role == AccidentListRoles.Reader) return "Подписан на сообщения списка";
			if (role == AccidentListRoles.Writer) return "Может рассылать сообщения по списку";
			return "Нет связи";
		}
	}

	public enum AccidentListRoles
    {
		NonSet = 0,
		Reader = 1,
		Writer = 2,
    }
}