using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


namespace PCVerwaltung
{
    public partial class MainView : Window
    {
        private bool _isDarkMode;

        public MainView()
        {
            InitializeComponent();

            ContentHost.Content = new AllListsView();

            ApplyTheme(false);
        }

        private void TitleBar_MouseLeftButtonDown(
            object sender,
            MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                ToggleMaximize();
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            ToggleMaximize();
        }

        private void CloseButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }

        private void ToggleMaximize()
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaximizeButton.Content = "□";
                MaximizeButton.ToolTip = "Maximieren";
            }
            else
            {
                WindowState = WindowState.Maximized;
                MaximizeButton.Content = "❐";
                MaximizeButton.ToolTip = "Wiederherstellen";
            }
        }

        private void ThemeToggleButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            _isDarkMode = !_isDarkMode;

            ApplyTheme(_isDarkMode);
        }

        private void ApplyTheme(bool darkMode)
        {
            PaletteHelper paletteHelper = new PaletteHelper();

            Theme theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(
                darkMode
                    ? BaseTheme.Dark
                    : BaseTheme.Light);

            theme.SetPrimaryColor(
                Color.FromRgb(59, 130, 246));

            theme.SetSecondaryColor(
                Color.FromRgb(147, 197, 253));

            paletteHelper.SetTheme(theme);

            if (darkMode)
            {
                SetResourceColor("AppBackgroundBrush", "#0F172A");
                SetResourceColor("AppSurfaceBrush", "#172033");
                SetResourceColor("AppTextBrush", "#F8FAFC");
                SetResourceColor("AppAccentBrush", "#60A5FA");
                SetResourceColor("AppBorderBrush", "#334155");

                ThemeToggleButton.Content = "☀";
                ThemeToggleButton.ToolTip = "Light Mode aktivieren";

            }
            else
            {
                SetResourceColor("AppBackgroundBrush", "#F7FAFC");
                SetResourceColor("AppSurfaceBrush", "#FFFFFF");
                SetResourceColor("AppTextBrush", "#172033");
                SetResourceColor("AppAccentBrush", "#3B82F6");
                SetResourceColor("AppBorderBrush", "#D9E2EC");

                ThemeToggleButton.Content = "☾";
                ThemeToggleButton.ToolTip = "Dark Mode aktivieren";

            }

            JettGptControl.IsDarkMode = darkMode;
        }

        private void SetResourceColor(
            string resourceKey,
            string colorCode)
        {
            if (Resources[resourceKey] is SolidColorBrush brush)
            {
                // Statt brush.Color = ...
                Resources[resourceKey] = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(colorCode));
            }
        }

        private void OnMenuGehaeuse(
            object sender,
            RoutedEventArgs e)
        {
            GehaeuseView view = new GehaeuseView();

            view.Saved += data =>
            {
                App.Cases.Add(data);
            };

            ContentHost.Content = view;
        }

        private void OnMenuPcSystemAssemble(
            object sender,
            RoutedEventArgs e)
        {
            ContentHost.Content = new PcBuilderView();
        }

        private void OnMenuGenerateInvoice(
            object sender,
            RoutedEventArgs e)
        {
            ContentHost.Content = new InvoiceView();
        }

        private void OnMenuMainboard(
            object sender,
            RoutedEventArgs e)
        {
            ContentHost.Content = new MainboardView();
        }

        private void OnMenuCpu(
            object sender,
            RoutedEventArgs e)
        {
            ContentHost.Content = new CpuView();
        }

        private void OnMenuRam(
            object sender,
            RoutedEventArgs e)
        {
            ContentHost.Content = new RamView();
        }

        private void OnMenuAllLists(
            object sender,
            RoutedEventArgs e)
        {
            ContentHost.Content = new AllListsView();
        }
    }



}