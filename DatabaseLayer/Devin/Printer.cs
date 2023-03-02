using LinqToDB.Mapping;

namespace DatabaseLayer.Devin
{
    [Table(Name = "Printers")]
    public class Printer
    {
        [Column, PrimaryKey, Identity, NotNull]
        public int Id { get; set; }

        [Column]
        public string Name { get; set; }

        [Column]
        public string Description { get; set; }
    }
}