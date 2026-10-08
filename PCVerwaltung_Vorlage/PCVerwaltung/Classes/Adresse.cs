using System;

namespace PCVerwaltung.Classes
{
    public class Adresse
    {
        public long AdresseId { get; set; }
        public long KundeId { get; set; }
        public string AdresseTyp { get; set; } = string.Empty;
        public string Strasse { get; set; } = string.Empty;
        public string Hausnummer { get; set; } = string.Empty;
        public string PLZ { get; set; } = string.Empty;
        public string Ort { get; set; } = string.Empty;
        public string Land { get; set; } = string.Empty;
    }
}
