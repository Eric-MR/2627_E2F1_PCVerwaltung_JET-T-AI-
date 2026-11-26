using PCVerwaltung.Classes;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für MainboardView.xaml
    /// </summary>
    public partial class MainboardView : UserControl
    {
        public event Action<Mainboard>? Saved;

        public MainboardView()
        {
            InitializeComponent();
            cmbFormfaktor.ItemsSource = Enum.GetValues(typeof(Formfaktor)).Cast<Formfaktor>();
            cmbSockel.ItemsSource = Enum.GetValues(typeof(SockelTyp)).Cast<SockelTyp>();
            cmbFormfaktor.SelectedItem = Formfaktor.ATX;
            cmbSockel.SelectedItem = SockelTyp.LGA1700;
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            ClearErrors();
            var valid = true;

            if (string.IsNullOrWhiteSpace(txtHersteller.Text))
            {
                SetError(txtHersteller, "Bitte Hersteller angeben.");
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(txtModell.Text))
            {
                SetError(txtModell, "Bitte Modell angeben.");
                valid = false;
            }
            if (!valid) return;

            var data = new Mainboard(
                txtHersteller.Text.Trim(),
                txtModell.Text.Trim(),
                (Formfaktor)cmbFormfaktor.SelectedItem,
                (SockelTyp)cmbSockel.SelectedItem);
            Saved?.Invoke(data);
            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => ResetFields();

        private void ResetFields()
        {
            txtHersteller.Text = "";
            txtModell.Text = "";
            cmbFormfaktor.SelectedItem = Formfaktor.ATX;
            cmbSockel.SelectedItem = SockelTyp.LGA1700;
            ClearErrors();
        }

        private void SetError(Control control, string message)
        {
            control.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 20, 60));
            control.BorderThickness = new Thickness(1.5);
            control.ToolTip = message;
        }

        private void ClearErrors()
        {
            foreach (var control in new Control[] { txtHersteller, txtModell, cmbFormfaktor, cmbSockel })
            {
                control.ClearValue(Border.BorderBrushProperty);
                control.ClearValue(Border.BorderThicknessProperty);
                control.ClearValue(ToolTipProperty);
            }
        }
    }
}
