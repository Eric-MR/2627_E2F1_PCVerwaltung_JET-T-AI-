using System;

namespace PCVerwaltung.Classes
{
    public class Benutzer
    {
        public long BenutzerId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswortHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Aktiv { get; set; }
    }
}
