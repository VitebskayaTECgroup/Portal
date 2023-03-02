using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
    [Table(Name = "Appeals")]
    public class Appeal
    {
        [Column]
        public DateTime Date { get; set; }

        [Column]
        public string Ip { get; set; }

        [Column]
        public string Text { get; set; }
    }
}