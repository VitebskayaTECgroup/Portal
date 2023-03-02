using LinqToDB.Mapping;

namespace DatabaseLayer.Site
{
    [Table(Name = "AutosCars")]
    public class Car
    {
        [Column(Name = "A_ID"), Identity]
        public int Id { get; set; }

        [Column(Name = "A_auto")]
        public string Model { get; set; }

        [Column(Name = "A_auto_number")]
        public string Number { get; set; }

        [Column(Name = "A_auto_weight")]
        public string Type { get; set; }
    }
}