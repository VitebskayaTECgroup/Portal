using LinqToDB.Mapping;

namespace DatabaseLayer.Devin
{
    [Table(Name = "TypesDevices")]
    public class TypeDevice
    {
        [Column]
        public string Id { get; set; }

        [Column]
        public string DeviceText { get; set; }

        [Column]
        public string StorageText { get; set; }

        [Column]
        public string Icon { get; set; }
    }
}