using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
    [Table(Name = "AutosDrivers")]
    public class Driver
    {
        [Column(Name = "A_ID"), Identity]
        public int Id { get; set; }

        [Column(Name = "A_driver")]
        public string Name { get; set; }

        [Column(Name = "A_phone_number")]
        public string PhoneNumber { get; set; }

        [Column(Name = "A_defaultauto")]
        public int? DefaultCarId { get; set; }
    }
}