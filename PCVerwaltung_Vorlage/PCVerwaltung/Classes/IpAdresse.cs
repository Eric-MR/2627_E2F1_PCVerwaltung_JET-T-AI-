using System;

namespace PCVerwaltung.Classes
{
    public class IpAdresse
    {
        public long IpId { get; set; }
        public long NetzwerkId { get; set; }
        public long PcId { get; set; }
        public string IpAdresseWert { get; set; } = string.Empty;
    }
}
