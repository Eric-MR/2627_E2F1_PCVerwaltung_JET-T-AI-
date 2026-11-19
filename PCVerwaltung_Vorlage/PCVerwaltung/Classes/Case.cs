using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class Case
    {
        // DB uses bigint auto-increment -> map to long
        public long Id { get; set; }
        public string Hersteller { get; set; } = "";
        public string Modell { get; set; } = "";
        public Formfaktor Formfaktor { get; set; } = Formfaktor.ATX;

        // constructor for creating a new case (Id will be assigned by DB)
        public Case(string hersteller, string modell, Formfaktor formfaktor)
        {
            Id = 0;
            Hersteller = hersteller;
            Modell = modell;
            Formfaktor = formfaktor;
        }

        // used when loading from DB with an existing id
        public Case(long id, string hersteller, string modell, Formfaktor formfaktor)
        {
            Id = id;
            Hersteller = hersteller;
            Modell = modell;
            Formfaktor = formfaktor;
        }
    }
}
