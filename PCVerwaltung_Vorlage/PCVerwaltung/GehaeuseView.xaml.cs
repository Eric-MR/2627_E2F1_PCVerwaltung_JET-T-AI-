using PCVerwaltung.Classes;
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
    /// Interaktionslogik für GehaeuseView.xaml
    /// </summary>


    public partial class GehaeuseView : UserControl
    {
        // Optional: Event, damit der Host (HardwareWindow) das Ergebnis bekommt
        public event Action<Case>? Saved;



        public GehaeuseView()
        {
            InitializeComponent();
            cmbFormfaktor.ItemsSource = Enum.GetValues(typeof(Formfaktor)).Cast<Formfaktor>();
            cmbFormfaktor.SelectedItem = Formfaktor.ATX;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            ClearAllErrors();

            bool ok = true;

            if (string.IsNullOrWhiteSpace(txtHersteller.Text))
            {
                SetError(txtHersteller, "Bitte Hersteller angeben.");
                ok = false;
            }

            if (string.IsNullOrWhiteSpace(txtModell.Text))
            {
                SetError(txtModell, "Bitte Modell angeben.");
                ok = false;
            }


            if (!ok) return;

            var data = new Case(txtHersteller.Text.Trim(), txtModell.Text.Trim(), (Formfaktor)cmbFormfaktor.SelectedItem);


            // Optional: an Host signalisieren
            Saved?.Invoke(data);

            // Demo: Anzeige
            MessageBox.Show(
                $"Gespeichert:\nHersteller: {data.Hersteller}\nModell: {data.Modell}\nFormfaktor: {data.Formfaktor}",
                "Gehäuse", MessageBoxButton.OK, MessageBoxImage.Information);

            // Felder zurücksetzen (optional)
            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => ResetFields();

        private void ResetFields()
        {
            txtHersteller.Text = "";
            txtModell.Text = "";
            cmbFormfaktor.SelectedIndex = 0;
            ClearAllErrors();
        }

        #region Simple-Validation-Helpers
        private static readonly Brush ErrorBrush = new SolidColorBrush(Color.FromRgb(220, 20, 60)); // Crimson
        private static readonly Brush NormalBrush = SystemColors.ControlDarkBrush;

        private void SetError(Control c, string msg)
        {
            c.BorderBrush = ErrorBrush;
            c.BorderThickness = new Thickness(1.5);
            c.ToolTip = msg;
        }

        private void ClearError(Control c)
        {
            c.ClearValue(Border.BorderBrushProperty);
            c.ClearValue(Border.BorderThicknessProperty);
            c.ClearValue(ToolTipProperty);
        }

        private void ClearAllErrors()
        {
            ClearError(txtHersteller);
            ClearError(txtModell);
            ClearError(cmbFormfaktor);
        }
        #endregion
    }



}
