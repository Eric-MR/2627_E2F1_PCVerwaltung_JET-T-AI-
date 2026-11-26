using PCVerwaltung.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für RamView.xaml
    /// </summary>
    public partial class RamView : UserControl
    {
        public event Action<HardwareKomponente>? Saved;

        public RamView()
        {
            InitializeComponent();
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
            if (!int.TryParse(txtKapazitaet.Text, out var kapazitaet) || kapazitaet <= 0)
            {
                SetError(txtKapazitaet, "Bitte eine gültige Kapazität eingeben.");
                valid = false;
            }
            if (!int.TryParse(txtTakt.Text, out var takt) || takt <= 0)
            {
                SetError(txtTakt, "Bitte einen gültigen Takt eingeben.");
                valid = false;
            }
            if (!valid) return;

            Saved?.Invoke(new HardwareKomponente
            {
                Hersteller = txtHersteller.Text.Trim(),
                Modell = txtModell.Text.Trim(),
                Beschreibung = $"Kapazität: {kapazitaet} GB; Takt: {takt} MHz",
                Aktiv = true
            });
            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => ResetFields();

        private void ResetFields()
        {
            txtHersteller.Text = "";
            txtModell.Text = "";
            txtKapazitaet.Text = "";
            txtTakt.Text = "";
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
            foreach (var control in new Control[] { txtHersteller, txtModell, txtKapazitaet, txtTakt })
            {
                control.ClearValue(Border.BorderBrushProperty);
                control.ClearValue(Border.BorderThicknessProperty);
                control.ClearValue(ToolTipProperty);
            }
        }
    }
}
