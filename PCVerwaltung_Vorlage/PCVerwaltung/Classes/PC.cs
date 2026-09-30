using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public class PC
    {
        public Case Case { get; }
        public CPU Cpu { get; }
        public Mainboard Mainboard { get; }

        public PC(Case @case, CPU cpu, Mainboard mainboard)
        {
            Case = @case ?? throw new ArgumentNullException(nameof(@case));
            Cpu = cpu ?? throw new ArgumentNullException(nameof(cpu));
            Mainboard = mainboard ?? throw new ArgumentNullException(nameof(mainboard));
        }

        public override string ToString()
            => $"{Cpu.Modell} | {Mainboard.Hersteller} {Mainboard.Modell} ({Mainboard.Formfaktor}) | {Case.Hersteller} {Case.Modell}";
    }
}
