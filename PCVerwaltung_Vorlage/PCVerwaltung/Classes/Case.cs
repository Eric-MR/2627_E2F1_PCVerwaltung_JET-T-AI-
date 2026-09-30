using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class Case
    {
        public string Hersteller { get; set; } = "";
        public string Modell { get; set; } = "";
        public Formfaktor Formfaktor { get; set; } = Formfaktor.ATX;

        public Case(string hersteller, string modell, Formfaktor formfaktor)
        {
            Hersteller = hersteller;
            Modell = modell;
            Formfaktor = formfaktor;
        }
    }
}
