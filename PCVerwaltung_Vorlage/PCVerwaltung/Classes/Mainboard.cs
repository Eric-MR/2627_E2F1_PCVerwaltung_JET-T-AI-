using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    /// <summary>
    /// Reines DTO für den Transport/Binding – ohne Logik.
    /// </summary>
    public class Mainboard
    {
        public Guid Id { get; }
        public string Hersteller { get; set; }
        public string Modell { get; set; }
        public Formfaktor Formfaktor { get; set; }
        public SockelTyp Sockel { get; set; }  

        public Mainboard(string hersteller, string modell, Formfaktor formfaktor, SockelTyp sockel)
        {
            if (string.IsNullOrWhiteSpace(hersteller)) throw new Exception("Board-Hersteller ist Pflicht.");
            if (string.IsNullOrWhiteSpace(modell)) throw new Exception("Board-Modell ist Pflicht.");

            Id = Guid.NewGuid();
            Hersteller = hersteller.Trim();
            Modell = modell.Trim();
            Formfaktor = formfaktor;
            Sockel = sockel;
        }

        public override string ToString() => $"{Hersteller} {Modell} ({Formfaktor}, {Sockel})";
    }
}
