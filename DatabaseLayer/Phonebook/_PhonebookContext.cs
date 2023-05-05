using LinqToDB;
using LinqToDB.Data;

namespace DatabaseLayer.Phonebook
{
	public class PhonebookContext : DataConnection
	{
		#if DEBUG
		public PhonebookContext() : base("PhonebookDebug") { }
		#else
		public PhonebookContext() : base("Phonebook") { }
		#endif

		public ITable<Phone> Phones
			=> this.GetTable<Phone>();
	}
}