using System;

namespace PCVerwaltung.Classes
{
    public class PcSystemKomponente
    {
        public long PcSystemKomponenteId { get; set; }
        public long PcId { get; set; }
        public long KomponenteId { get; set; }
        public decimal PreisZumZeitpunkt { get; set; }
        public int Anzahl { get; set; }
    }
}
