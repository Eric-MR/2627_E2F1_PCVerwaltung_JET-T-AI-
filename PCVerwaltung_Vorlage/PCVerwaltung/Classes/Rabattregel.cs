using System;

namespace PCVerwaltung.Classes
{
    public class Rabattregel
    {
        public long RabattregelId { get; set; }
        public string Ranking { get; set; } = string.Empty;
        public decimal RabattProzent { get; set; }
        public DateTime GueltigAb { get; set; }
    }
}
