using LinqToDB;
using LinqToDB.Data;

namespace DatabaseLayer.Checkpoint
{
	public class CheckpointContext : DataConnection
	{
		public CheckpointContext() : base("Checkpoint") { }

		public ITable<OWNERS> OWNERS
			=> this.GetTable<OWNERS>();

		public ITable<EVENTS> EVENTS
			=> this.GetTable<EVENTS>();
	}
}