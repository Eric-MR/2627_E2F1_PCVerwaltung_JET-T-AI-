using System;

namespace PCVerwaltung.Classes
{
    public class FinanzierungPlan
    {
        public long FinanzierungId { get; set; }
        public long RechnungId { get; set; }
        public int AnzahlRaten { get; set; }
        public decimal Zinssatz { get; set; }
        public int LaufzeitMonate { get; set; }
        public decimal Ratenbetrag { get; set; }
        public decimal ZuZahlenderGesamtbetrag { get; set; }
    }
}
