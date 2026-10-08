using System;
using System.Collections.Generic;

namespace PCVerwaltung.Classes
{
    public class Rechnung
    {
        public long RechnungId { get; set; }
        public string Rechnungsnummer { get; set; } = string.Empty;
        public DateTime Rechnungsdatum { get; set; }
        public string Status { get; set; } = string.Empty;
        public long ZahlungsartId { get; set; }
        public decimal BruttoBetrag { get; set; }
        public decimal SteuerBetrag { get; set; }
        public decimal RabattBetrag { get; set; }
        public decimal RabattProzent { get; set; }
        public decimal Gesamtbetrag { get; set; }
        public decimal NettoBetrag { get; set; }
        public List<Rechnungsposition> Positionen { get; set; } = new();
    }
}
