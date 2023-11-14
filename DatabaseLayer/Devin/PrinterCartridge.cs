using LinqToDB.Mapping;

namespace DatabaseLayer.Devin
{
	[Table(Name = "PrintersCartridges")]
	public class PrinterCartridge
	{
		[Column]
		public int PrinterId { get; set; }

		[Column]
		public int CartridgeId { get; set; }
	}
}