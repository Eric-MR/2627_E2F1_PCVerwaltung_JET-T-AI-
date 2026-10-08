using System;

namespace PCVerwaltung.Classes
{
    public class SSD
    {
        public long KomponenteId { get; set; }
        public long KomponententypId { get; set; }
        public string Hersteller { get; set; } = string.Empty;
        public string Modell { get; set; } = string.Empty;
        public string Seriennummer { get; set; } = string.Empty;
        public string Beschreibung { get; set; } = string.Empty;
        public decimal Preis { get; set; }
        public int Lagerbestand { get; set; }
        public bool Aktiv { get; set; }
    }
}
