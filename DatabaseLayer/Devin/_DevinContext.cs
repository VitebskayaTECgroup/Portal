using LinqToDB;
using LinqToDB.Data;

namespace DatabaseLayer.Devin
{
	public class DevinContext : DataConnection
	{
		public DevinContext() : base("Devin") { }

		public ITable<Activity> Activity
			=> this.GetTable<Activity>();

		public ITable<Elm> Elm
			=> this.GetTable<Elm>();

		public ITable<AidaDescription> AidaDescriptions
			=> this.GetTable<AidaDescription>();

		public ITable<Report> Report
			=> this.GetTable<Report>();

		public ITable<Item> Item
			=> this.GetTable<Item>();

		public ITable<Cartridge> Cartridges
			=> this.GetTable<Cartridge>();

		public ITable<Device> Devices
			=> this.GetTable<Device>();

		public ITable<Folder> Folders
			=> this.GetTable<Folder>();

		public ITable<Hardware> Hardware
		   => this.GetTable<Hardware>();

		public ITable<Object1C> Objects1C
			=> this.GetTable<Object1C>();

		public ITable<Printer> Printers
			=> this.GetTable<Printer>();

		public ITable<Repair> Repairs
			=> this.GetTable<Repair>();

		public ITable<Storage> Storages
			=> this.GetTable<Storage>();

		public ITable<WorkPlace> WorkPlaces
			=> this.GetTable<WorkPlace>();

		public ITable<Writeoff> Writeoffs
			=> this.GetTable<Writeoff>();

		public ITable<WriteoffMark> WriteoffsMarks
			=> this.GetTable<WriteoffMark>();

		public ITable<Software> Software
			=> this.GetTable<Software>();

		public ITable<Employee> Employees
			=> this.GetTable<Employee>();

		public ITable<Official> Officials
			=> this.GetTable<Official>();

		// relationship tables

		public ITable<PrinterCartridge> _PrinterCartridge
			=> this.GetTable<PrinterCartridge>();

		public ITable<TypeCartridge> _CartridgeTypes
			=> this.GetTable<TypeCartridge>();

		public ITable<TypeDevice> _DeviceTypes
			=> this.GetTable<TypeDevice>();

		public ITable<TypeSystem> _SystemTypes
			=> this.GetTable<TypeSystem>();

		public ITable<TypeWriteoff> _WriteoffTypes
			=> this.GetTable<TypeWriteoff>();

		public ITable<DeviceSoftware> _DeviceSoftware
			=> this.GetTable<DeviceSoftware>();

		public ITable<Relation_Official_Employee> Relation_Officials_Employees
			=> this.GetTable<Relation_Official_Employee>();
	}
}