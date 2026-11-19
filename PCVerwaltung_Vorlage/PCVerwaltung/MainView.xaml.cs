using PCVerwaltung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            // Optional: Startansicht
            ContentHost.Content = new AllListsView();
        }



        private void OnMenuGehaeuse(object sender, RoutedEventArgs e)
        {
            var view = new GehaeuseView();
            view.Saved += data =>
            {
                try
                {
                    var typeId = App.Database.EnsureKomponententyp("case");
                    data.Id = App.Database.InsertHardwareKomponente(new PCVerwaltung.Classes.HardwareKomponente
                    {
                        KomponententypId = typeId,
                        Hersteller = data.Hersteller,
                        Modell = data.Modell,
                        Beschreibung = $"Formfaktor: {data.Formfaktor}",
                        Aktiv = true
                    });
                    App.Cases.Add(data);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Gehäuse konnte nicht in der Datenbank gespeichert werden.\n\n{ex.Message}",
                        "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };
            ContentHost.Content = view;
        }

     
        private void OnMenuPcSystemAssemble(object sender, RoutedEventArgs e) => ContentHost.Content = new PcBuilderView();

        private void OnMenuGenerateInvoice(object sender, RoutedEventArgs e) => ContentHost.Content = new PcBuilderView();

        private void OnMenuMainboard(object sender, RoutedEventArgs e) => ContentHost.Content = new MainboardView();
        private void OnMenuCpu(object sender, RoutedEventArgs e) => ContentHost.Content = new CpuView();
        private void OnMenuRam(object sender, RoutedEventArgs e) => ContentHost.Content = new RamView();

        private void OnMenuAllLists(object sender, RoutedEventArgs e) => ContentHost.Content = new AllListsView();


    }
}
