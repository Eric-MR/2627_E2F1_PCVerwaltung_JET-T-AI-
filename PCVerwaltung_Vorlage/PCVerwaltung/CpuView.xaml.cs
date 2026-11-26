using PCVerwaltung.Classes;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PCVerwaltung
{
    /// <summary>
    /// Interaktionslogik für CpuView.xaml
    /// </summary>
    public partial class CpuView : UserControl
    {
        public event Action<CPU>? Saved;

        public CpuView()
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
            if (!double.TryParse(txtTakt.Text.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var takt) || takt <= 0)
            {
                SetError(txtTakt, "Bitte eine gültige Taktfrequenz eingeben.");
                valid = false;
            }
            if (!valid) return;

            var data = new CPU(
                $"{txtHersteller.Text.Trim()} {txtModell.Text.Trim()}",
                takt);
            Saved?.Invoke(data);
            ResetFields();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e) => ResetFields();

        private void ResetFields()
        {
            txtHersteller.Text = "";
            txtModell.Text = "";
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
            foreach (var control in new Control[] { txtHersteller, txtModell, txtTakt })
            {
                control.ClearValue(Border.BorderBrushProperty);
                control.ClearValue(Border.BorderThicknessProperty);
                control.ClearValue(ToolTipProperty);
            }
        }
    }
}
