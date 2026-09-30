using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCVerwaltung.Classes
{
    public enum Formfaktor { ATX, MicroATX, MiniITX }
    public enum SockelTyp
    {
        AM4,
        AM5,
        LGA1200,
        LGA1700,
        LGA1151,
        TR4
    }

    public enum Finanzierung
    {
        Rechnung,
        Bar,
        Ratenzahlung,
        Leasing
    }
}
