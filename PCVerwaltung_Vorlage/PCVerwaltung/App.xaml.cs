using PCVerwaltung.Classes;
using System.Configuration;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static DB Database { get; } = new DB();
        public static List<Case> Cases { get; private set; } = new();
        public static List<CPU> CPUs { get; private set; } = new();
        public static List<Mainboard> Mainboards { get; private set; } = new();
        public static List<PC> PCs { get; private set; } = new();
        public List<Case> CasesData => Cases;
        public List<CPU> CPUsData => CPUs;
        public List<Mainboard> MainboardsData => Mainboards;
        public List<PC> PCsData => PCs;

        protected override void OnStartup(StartupEventArgs e)
        {
            var stage = "opening database connection";
            try
            {
                Database.Initialize();
                stage = "checking database tables and columns";
                Database.ValidateHardwareSchema();
                stage = "writing missing base hardware data";
                Database.SeedIfEmpty();
                stage = "reading hardware data";
                Cases = Database.GetCases();
                CPUs = Database.GetCPUs();
                Mainboards = Database.GetMainboards();

                PCs = new List<PC>();
                var count = Math.Min(Math.Min(Cases.Count, CPUs.Count), Mainboards.Count);
                for (int i = 0; i < count; i++)
                {
                    PCs.Add(new PC(Cases[i], CPUs[i], Mainboards[i]));
                }

                if (Cases.Count == 0 || CPUs.Count == 0 || Mainboards.Count == 0)
                {
                    MessageBox.Show(
                        $"The database connection and queries succeeded, but one or more hardware lists are empty.\nCases: {Cases.Count}; CPUs: {CPUs.Count}; Mainboards: {Mainboards.Count}.\nCheck komponententyp.bezeichnung and matching rows in hardwarekomponente.",
                        "Database connected; data missing",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                var rootException = ex.GetBaseException();
                var errorDetails = rootException is MySqlException mysqlException
                    ? $"MySQL error {mysqlException.Number} (SQL state {mysqlException.SqlState}): {mysqlException.Message}"
                    : rootException.Message;
                MessageBox.Show(
                    $"Failed while {stage}.\n\n{errorDetails}",
                    "Database error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            base.OnStartup(e);
        }

    }

}
