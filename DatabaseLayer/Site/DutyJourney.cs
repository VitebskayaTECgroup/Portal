using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
    [Table(Name = "DutyJourney")]
    public class DutyJourney
    {
        [Column, NotNull]
        public int TabId { get; set; }

        [Column]
        public DateTime DateStart { get; set; }

        [Column]
        public DateTime DateEnd { get; set; }

        public bool IsValid => TabId > 0
            && DateStart > DateTime.MinValue
            && DateEnd > DateTime.MinValue
            && DateEnd >= DateStart;
    }
}
