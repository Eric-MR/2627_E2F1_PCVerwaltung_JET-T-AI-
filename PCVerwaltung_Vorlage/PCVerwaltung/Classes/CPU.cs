using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class CPU
    {
        public long Id { get; set; }
        public double Taktfrequenz { get; set; } = 0.0;
        public string Modell { get; set; } = "";

        public CPU(string modell, double taktfrequenz)
        {
            Id = 0;
            Modell = modell;
            Taktfrequenz = taktfrequenz;
        }

        // used when loading from DB
        public CPU(long id, string modell, double taktfrequenz)
        {
            Id = id;
            Modell = modell;
            Taktfrequenz = taktfrequenz;
        }
    }
}
