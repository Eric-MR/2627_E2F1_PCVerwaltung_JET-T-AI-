using System;

namespace PCVerwaltung.Classes
{
    public class Kunde
    {
        public long KundeId { get; set; }
        public string Kundennummer { get; set; } = string.Empty;
        public string Nachname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Ranking { get; set; } = string.Empty; // map to enum or string
        public long ZahlungsartId { get; set; }
        public bool Aktiv { get; set; }
    }
}
