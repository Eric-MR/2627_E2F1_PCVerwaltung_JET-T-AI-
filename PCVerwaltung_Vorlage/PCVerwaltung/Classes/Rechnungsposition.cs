using System;

namespace PCVerwaltung.Classes
{
    public class Rechnungsposition
    {
        public long RechnungspositionId { get; set; }
        public long RechnungId { get; set; }
        public string Beschreibung { get; set; } = string.Empty;
        public int Menge { get; set; }
        public decimal Einzelpreis { get; set; }
        public decimal Gesamtpreis { get; set; }
        public decimal Steuersatz { get; set; }
    }
}
