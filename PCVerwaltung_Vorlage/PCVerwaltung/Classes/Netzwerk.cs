using System;

namespace PCVerwaltung.Classes
{
    public class Netzwerk
    {
        public long NetzwerkId { get; set; }
        public string Bezeichnung { get; set; } = string.Empty;
        public string Gateway { get; set; } = string.Empty;
        public string Subnetzmaske { get; set; } = string.Empty;
        public string Netzwerkadresse { get; set; } = string.Empty;
    }
}
