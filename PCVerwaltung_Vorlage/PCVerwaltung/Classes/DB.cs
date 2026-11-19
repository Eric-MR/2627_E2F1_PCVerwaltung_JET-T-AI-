using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace PCVerwaltung.Classes
{
    public class DB
    {
        private MySqlConnection? _myConnection;
        private readonly string _connectionString = "SERVER=localhost;DATABASE=pc_verwaltung;UID=root;PASSWORD=;SslMode=Disabled;";

        public void Initialize()
        {
            _myConnection = new MySqlConnection(_connectionString);
            _myConnection.Open();
        }

        public void ValidateHardwareSchema()
        {
            if (_myConnection?.State != System.Data.ConnectionState.Open)
                throw new InvalidOperationException("The database connection is not open.");

            using (var cmd = new MySqlCommand("SELECT komponententyp_id, bezeichnung FROM komponententyp LIMIT 0", _myConnection))
            using (cmd.ExecuteReader())
            {
            }

            using (var cmd = new MySqlCommand("SELECT komponente_id, komponententyp_id, hersteller, modell, seriennummer, beschreibung, preis, lagerbestand, aktiv FROM hardwarekomponente LIMIT 0", _myConnection))
            using (cmd.ExecuteReader())
            {
            }
        }

        public long GetKomponententypIdByName(string name)
        {
            if (_myConnection == null) throw new InvalidOperationException("DB connection is not initialized.");
            using var cmd = new MySqlCommand("SELECT komponententyp_id FROM komponententyp WHERE LOWER(bezeichnung) = @name LIMIT 1", _myConnection);
            cmd.Parameters.AddWithValue("@name", name.ToLower());
            var res = cmd.ExecuteScalar();
            if (res == null || res == DBNull.Value) return -1;
            return Convert.ToInt64(res);
        }

        public long EnsureKomponententyp(string name)
        {
            var id = FindKomponententypId(name);
            if (id > 0) return id;
            if (_myConnection == null) throw new InvalidOperationException("DB connection is not initialized.");
            using (var cmd = new MySqlCommand("INSERT INTO komponententyp (bezeichnung) VALUES (@name)", _myConnection))
            {
                cmd.Parameters.AddWithValue("@name", name.ToLower());
                cmd.ExecuteNonQuery();
            }
            using (var cmd2 = new MySqlCommand("SELECT LAST_INSERT_ID()", _myConnection))
            {
                var res2 = cmd2.ExecuteScalar();
                return res2 == null ? -1 : Convert.ToInt64(res2);
            }
        }

        public long CountHardwareByTypeName(string typeName)
        {
            var (names, placeholders) = BuildTypeNameParameters(typeName);
            using var cmd = new MySqlCommand($"SELECT COUNT(*) FROM hardwarekomponente h JOIN komponententyp kt ON kt.komponententyp_id = h.komponententyp_id WHERE LOWER(TRIM(kt.bezeichnung)) IN ({placeholders})", _myConnection);
            AddTypeNameParameters(cmd, names);
            var res = cmd.ExecuteScalar();
            return res == null ? 0 : Convert.ToInt64(res);
        }

        public List<HardwareKomponente> GetHardwareKomponentenByTypeName(string typeName)
        {
            var list = new List<HardwareKomponente>();
            var (names, placeholders) = BuildTypeNameParameters(typeName);
            using var cmd = new MySqlCommand($"SELECT h.komponente_id, h.komponententyp_id, h.hersteller, h.modell, h.seriennummer, h.beschreibung, h.preis, h.lagerbestand, h.aktiv FROM hardwarekomponente h JOIN komponententyp kt ON kt.komponententyp_id = h.komponententyp_id WHERE LOWER(TRIM(kt.bezeichnung)) IN ({placeholders})", _myConnection);
            AddTypeNameParameters(cmd, names);
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var hk = new HardwareKomponente
                {
                    KomponenteId = rdr.GetInt64(0),
                    KomponententypId = rdr.GetInt64(1),
                    Hersteller = rdr.IsDBNull(2) ? string.Empty : rdr.GetString(2),
                    Modell = rdr.IsDBNull(3) ? string.Empty : rdr.GetString(3),
                    Seriennummer = rdr.IsDBNull(4) ? string.Empty : rdr.GetString(4),
                    Beschreibung = rdr.IsDBNull(5) ? string.Empty : rdr.GetString(5),
                    Preis = rdr.IsDBNull(6) ? 0m : rdr.GetDecimal(6),
                    Lagerbestand = rdr.IsDBNull(7) ? 0 : rdr.GetInt32(7),
                    Aktiv = !rdr.IsDBNull(8) && rdr.GetInt32(8) != 0
                };
                list.Add(hk);
            }
            return list;
        }

        private long FindKomponententypId(string name)
        {
            foreach (var alias in GetTypeAliases(name))
            {
                var id = GetKomponententypIdByName(alias);
                if (id > 0) return id;
            }
            return -1;
        }

        private static string[] GetTypeAliases(string typeName)
        {
            return typeName.Trim().ToLowerInvariant() switch
            {
                "case" or "gehäuse" or "gehaeuse" => new[] { "case", "gehäuse", "gehaeuse" },
                "cpu" or "prozessor" => new[] { "cpu", "prozessor" },
                "mainboard" or "motherboard" => new[] { "mainboard", "motherboard" },
                _ => new[] { typeName.Trim().ToLowerInvariant() }
            };
        }

        private static (string[] Names, string Placeholders) BuildTypeNameParameters(string typeName)
        {
            var names = GetTypeAliases(typeName);
            var placeholdersList = new List<string>();
            for (var i = 0; i < names.Length; i++)
            {
                placeholdersList.Add($"@type{i}");
            }
            var placeholders = string.Join(", ", placeholdersList);
            return (names, placeholders);
        }

        private static void AddTypeNameParameters(MySqlCommand command, string[] names)
        {
            for (var i = 0; i < names.Length; i++)
            {
                command.Parameters.AddWithValue($"@type{i}", names[i]);
            }
        }

        public long InsertHardwareKomponente(HardwareKomponente h)
        {
            if (_myConnection == null) throw new InvalidOperationException("DB connection is not initialized.");
            using (var cmd = new MySqlCommand("INSERT INTO hardwarekomponente (komponententyp_id, hersteller, modell, seriennummer, beschreibung, preis, lagerbestand, aktiv) VALUES (@kt,@hersteller,@modell,@sn,@desc,@preis,@lager,@aktiv)", _myConnection))
            {
                cmd.Parameters.AddWithValue("@kt", h.KomponententypId);
                cmd.Parameters.AddWithValue("@hersteller", h.Hersteller);
                cmd.Parameters.AddWithValue("@modell", h.Modell);
                cmd.Parameters.AddWithValue("@sn", h.Seriennummer);
                cmd.Parameters.AddWithValue("@desc", h.Beschreibung);
                cmd.Parameters.AddWithValue("@preis", h.Preis);
                cmd.Parameters.AddWithValue("@lager", h.Lagerbestand);
                cmd.Parameters.AddWithValue("@aktiv", h.Aktiv ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
            using var cmd2 = new MySqlCommand("SELECT LAST_INSERT_ID()", _myConnection);
            var res = cmd2.ExecuteScalar();
            return res == null ? -1 : Convert.ToInt64(res);
        }

        public void SeedIfEmpty()
        {
            var caseType = EnsureKomponententyp("case");
            var cpuType = EnsureKomponententyp("cpu");
            var mbType = EnsureKomponententyp("mainboard");

            if (CountHardwareByTypeName("case") == 0)
            {
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = caseType, Hersteller = "Fractal Design", Modell = "Meshify 2", Beschreibung = "Formfaktor: ATX", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = caseType, Hersteller = "Cooler Master", Modell = "NR400", Beschreibung = "Formfaktor: MicroATX", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = caseType, Hersteller = "NZXT", Modell = "H1", Beschreibung = "Formfaktor: MiniITX", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = caseType, Hersteller = "be quiet!", Modell = "Pure Base 500DX", Beschreibung = "Formfaktor: ATX", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = caseType, Hersteller = "Lian Li", Modell = "O11 Dynamic", Beschreibung = "Formfaktor: ATX", Preis = 0m, Lagerbestand = 0, Aktiv = true });
            }

            if (CountHardwareByTypeName("cpu") == 0)
            {
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = cpuType, Hersteller = "AMD", Modell = "Ryzen 7 7800X3D", Beschreibung = "Takt: 4.2 GHz", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = cpuType, Hersteller = "Intel", Modell = "Core i5-13600K", Beschreibung = "Takt: 3.5 GHz", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = cpuType, Hersteller = "AMD", Modell = "Ryzen 5 5600", Beschreibung = "Takt: 3.5 GHz", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = cpuType, Hersteller = "Intel", Modell = "Core i7-12700F", Beschreibung = "Takt: 2.1 GHz", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = cpuType, Hersteller = "AMD", Modell = "Ryzen 7 5700G", Beschreibung = "Takt: 3.8 GHz", Preis = 0m, Lagerbestand = 0, Aktiv = true });
            }

            if (CountHardwareByTypeName("mainboard") == 0)
            {
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = mbType, Hersteller = "ASUS", Modell = "TUF GAMING B650-PLUS", Beschreibung = "Formfaktor: ATX; Sockel: AM5", Preis = 189.99m, Lagerbestand = 4, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = mbType, Hersteller = "MSI", Modell = "PRO B760M-A", Beschreibung = "Formfaktor: MicroATX; Sockel: LGA1700", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = mbType, Hersteller = "Gigabyte", Modell = "B550I AORUS PRO AX", Beschreibung = "Formfaktor: MiniITX; Sockel: AM4", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = mbType, Hersteller = "ASRock", Modell = "B650M Pro RS", Beschreibung = "Formfaktor: MicroATX; Sockel: AM5", Preis = 0m, Lagerbestand = 0, Aktiv = true });
                InsertHardwareKomponente(new HardwareKomponente { KomponententypId = mbType, Hersteller = "ASUS", Modell = "ROG Strix Z690-A", Beschreibung = "Formfaktor: ATX; Sockel: LGA1700", Preis = 0m, Lagerbestand = 0, Aktiv = true });
            }
        }

        public List<Case> GetCases()
        {
            var list = new List<Case>();
            var hks = GetHardwareKomponentenByTypeName("case");
            foreach (var hk in hks)
            {
                var ff = ParseFormfaktorFromDescription(hk.Beschreibung);
                list.Add(new Case(hk.KomponenteId, hk.Hersteller, hk.Modell, ff));
            }
            return list;
        }

        public List<CPU> GetCPUs()
        {
            var list = new List<CPU>();
            var hks = GetHardwareKomponentenByTypeName("cpu");
            foreach (var hk in hks)
            {
                var takt = ParseTaktFromDescription(hk.Beschreibung);
                var modell = string.IsNullOrWhiteSpace(hk.Hersteller) || hk.Modell.StartsWith(hk.Hersteller, StringComparison.OrdinalIgnoreCase)
                    ? hk.Modell
                    : $"{hk.Hersteller} {hk.Modell}";
                list.Add(new CPU(hk.KomponenteId, modell, takt));
            }
            return list;
        }

        public List<Mainboard> GetMainboards()
        {
            var list = new List<Mainboard>();
            var hks = GetHardwareKomponentenByTypeName("mainboard");
            foreach (var hk in hks)
            {
                var ff = ParseFormfaktorFromDescription(hk.Beschreibung);
                var sockel = ParseSockelFromDescription(hk.Beschreibung);
                list.Add(new Mainboard(hk.KomponenteId, hk.Hersteller, hk.Modell, ff, sockel));
            }
            return list;
        }

        private Formfaktor ParseFormfaktorFromDescription(string desc)
        {
            if (string.IsNullOrWhiteSpace(desc)) return Formfaktor.ATX;
            desc = desc.ToLower();
            if (desc.Contains("microatx") || desc.Contains("micro-atx")) return Formfaktor.MicroATX;
            if (desc.Contains("miniitx") || desc.Contains("mini-itx")) return Formfaktor.MiniITX;
            return Formfaktor.ATX;
        }

        private SockelTyp ParseSockelFromDescription(string desc)
        {
            if (string.IsNullOrWhiteSpace(desc)) return SockelTyp.LGA1151;
            desc = desc.ToUpper();
            if (desc.Contains("AM4")) return SockelTyp.AM4;
            if (desc.Contains("AM5")) return SockelTyp.AM5;
            if (desc.Contains("LGA1700")) return SockelTyp.LGA1700;
            if (desc.Contains("LGA1200")) return SockelTyp.LGA1200;
            if (desc.Contains("TR4")) return SockelTyp.TR4;
            return SockelTyp.LGA1151;
        }

        private double ParseTaktFromDescription(string desc)
        {
            if (string.IsNullOrWhiteSpace(desc)) return 0.0;
            var m = Regex.Match(desc, "(\\d+(?:[.,]\\d+)?)\\s*GHz", RegexOptions.IgnoreCase);
            if (!m.Success) m = Regex.Match(desc, "(\\d+(?:[.,]\\d+)?)\\s*ghz", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                if (double.TryParse(m.Groups[1].Value.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var v))
                    return v;
            }
            return 0.0;
        }
    }
}
