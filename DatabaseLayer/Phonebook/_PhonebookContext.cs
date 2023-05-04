using LinqToDB;
using LinqToDB.Data;

namespace DatabaseLayer.Phonebook
{
	public class PhonebookContext : DataConnection
	{
		public PhonebookContext() : base("Phonebook") { }

		public ITable<Phone> Phones
			=> this.GetTable<Phone>();
	}
}