using PCVerwaltung.Classes;
using System.Windows;
using System.Windows.Controls;

namespace PCVerwaltung
{
    public partial class PcBuilderView : UserControl
    {
        public PcBuilderView()
        {
            InitializeComponent();
            // App stellt Cases, CPUs, Mainboards, PCs bereit
            DataContext = (App)Application.Current;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var selCase = CmbCase.SelectedItem as Case;
            var selMb = CmbMainboard.SelectedItem as Mainboard;
            var selCpu = CmbCpu.SelectedItem as CPU;

            if (selCase is null || selMb is null || selCpu is null)
            {
                MessageBox.Show("Bitte Gehäuse, Mainboard und CPU auswählen.",
                                "Auswahl unvollständig",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Optional: einfache Kompatibilitätsprüfung (Case ↔ Mainboard Formfaktor)
            bool fits = selCase.Formfaktor switch
            {
                Formfaktor.ATX => true,
                Formfaktor.MicroATX => selMb.Formfaktor == Formfaktor.MicroATX || selMb.Formfaktor == Formfaktor.MiniITX,
                Formfaktor.MiniITX => selMb.Formfaktor == Formfaktor.MiniITX,
                _ => false
            };

            if (!fits)
            {
                MessageBox.Show($"Formfaktor nicht kompatibel:\nGehäuse {selCase.Formfaktor} ↔ Board {selMb.Formfaktor}",
                                "Inkompatibel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Optional: CPU↔Mainboard-Sockel prüfen, falls CPU.Sockel existiert
            var cpuSockelProp = typeof(CPU).GetProperty("Sockel");
            if (cpuSockelProp != null)
            {
                var cpuSockel = cpuSockelProp.GetValue(selCpu);
                if (!Equals(cpuSockel, selMb.Sockel))
                {
                    MessageBox.Show($"Sockel nicht kompatibel:\nCPU {cpuSockel} ↔ Board {selMb.Sockel}",
                                    "Inkompatibel", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            // Speichern
            var pc = new PC(selCase, selCpu, selMb);
            App.PCs.Add(pc);

            //Damit die PC-Liste aktualisiert wird.
            dgPCs.Items.Refresh();

            MessageBox.Show("PC zur Liste hinzugefügt.", "Gespeichert",
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
