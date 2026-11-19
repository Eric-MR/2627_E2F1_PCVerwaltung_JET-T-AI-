using System;
using System.Collections.Generic;

namespace PCVerwaltung.Classes
{
    public class PcSystem
    {
        public long PcId { get; set; }
        public long KundeId { get; set; }
        public string Bezeichnung { get; set; } = string.Empty;
        public decimal Gesamtpreis { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ErstelltAm { get; set; }
        public List<PcSystemKomponente> Komponenten { get; set; } = new();
    }
}
