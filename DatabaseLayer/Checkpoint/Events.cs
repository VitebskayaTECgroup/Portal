using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Checkpoint
{
	[Table]
    public class EVENTS
    {
        [Column, Identity]
        public int EV_ID { get; set; }

        [Column]
        public DateTime EV_TSTAMP { get; set; }

        [Column]
        public int EV_TYPE { get; set; }

        [Column]
        public int EV_ADDR { get; set; }

        [Column]
        public int? EV_OW_ID { get; set; }

        [Column]
        public int? EV_CA_VALUE { get; set; }

        [Column]
        public string EV_SECTION_PIN { get; set; }

        [Column]
        public int EV_FLAGS { get; set; }
    }
}