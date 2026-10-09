using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace PCVerwaltung
{
    public partial class MainView : Window
    {
        private bool _isDarkMode;
        private bool _isAiChatOpen;
        private bool _isAiAnimationRunning;

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

                AiButtonLogo.Source = LoadLogo(
    "Jettgpt_NoBkg_W.png");
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

                AiButtonLogo.Source = LoadLogo(
    "Jettgpt_NoBkg_B.png");
            }
        }

        private void SetResourceColor(
            string resourceKey,
            string colorCode)
        {
            Color color = (Color)ColorConverter.ConvertFromString(colorCode);

            SolidColorBrush newBrush = new SolidColorBrush(color);

            Application.Current.Resources[resourceKey] = newBrush;
        }

        private BitmapImage? LoadLogo(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)
                || !string.Equals(
                    Path.GetFileName(fileName),
                    fileName,
                    StringComparison.Ordinal))
            {
                return LoadFallbackLogo();
            }

            string resourcePath =
                $"pack://application:,,,/PCVerwaltung;component/Images/{fileName}";

            if (!Uri.TryCreate(
                    resourcePath,
                    UriKind.Absolute,
                    out Uri? logoUri)
                || logoUri.Scheme != "pack")
            {
                return LoadFallbackLogo();
            }

            return TryLoadBitmap(logoUri) ?? LoadFallbackLogo();
        }

        private BitmapImage? LoadFallbackLogo()
        {
            const string fallbackPath =
                "pack://application:,,,/PCVerwaltung;component/Images/logo.png";

            if (!Uri.TryCreate(
                    fallbackPath,
                    UriKind.Absolute,
                    out Uri? fallbackUri))
            {
                return null;
            }

            return TryLoadBitmap(fallbackUri);
        }

        private BitmapImage? TryLoadBitmap(Uri imageUri)
        {
            try
            {
                BitmapImage bitmap = new BitmapImage();

                bitmap.BeginInit();
                bitmap.UriSource = imageUri;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();

                if (bitmap.PixelWidth <= 0 || bitmap.PixelHeight <= 0)
                {
                    return null;
                }

                bitmap.Freeze();
                return bitmap;
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (IOException)
            {
                return null;
            }
            catch (NotSupportedException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
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



        private void OpenAiButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (_isAiChatOpen || _isAiAnimationRunning)
            {
                return;
            }

            _isAiAnimationRunning = true;

            OpenAiButton.Visibility = Visibility.Collapsed;
            JettAiLayer.Visibility = Visibility.Visible;

            JettImage.Width = 250;
            JettImage.Height = 390;
            JettImage.Source = LoadLogo(
                "Jett_dash_links.png");

            DoubleAnimation dashAnimation = new DoubleAnimation
            {
                From = 610,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };

            dashAnimation.Completed += (_, _) =>
            {
                JettImage.Width = 170;
                JettImage.Height = 300;
                JettImage.Source = LoadLogo(
                    "Jett_stand.png");

                _isAiChatOpen = true;
                _isAiAnimationRunning = false;
            };

            JettAiTransform.BeginAnimation(
                TranslateTransform.XProperty,
                dashAnimation);
        }


        private void CloseAiButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (!_isAiChatOpen || _isAiAnimationRunning)
            {
                return;
            }

            _isAiAnimationRunning = true;

            JettImage.Width = 180;
            JettImage.Height = 300;

            JettImage.Source = LoadLogo(
                "Jett_dash_rechts.png");

            DoubleAnimation dashAnimation = new DoubleAnimation
            {
                From = 0,
                To = 610,
                Duration = TimeSpan.FromMilliseconds(500),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseIn
                }
            };

            dashAnimation.Completed += (_, _) =>
            {
                JettAiLayer.Visibility = Visibility.Collapsed;
                OpenAiButton.Visibility = Visibility.Visible;

                _isAiChatOpen = false;
                _isAiAnimationRunning = false;
            };

            JettAiTransform.BeginAnimation(
                TranslateTransform.XProperty,
                dashAnimation);
        }


    

    private void SendAiButton_Click(
    object sender,
    RoutedEventArgs e)
        {
            string message = AiInputBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show(
                    "Bitte zuerst eine Nachricht eingeben.",
                    "Keine Nachricht",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                return;
            }

            // Hier kannst du die Nachricht weiterverarbeiten.
            MessageBox.Show(
                message,
                "Nachricht aus dem Chat");
        }


    }
}