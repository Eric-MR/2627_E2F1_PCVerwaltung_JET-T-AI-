using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace PCVerwaltung.Controls
{
    public partial class JettGptControl : System.Windows.Controls.UserControl
    {
        private bool _isAiChatOpen;
        private bool _isAiAnimationRunning;

        public static readonly DependencyProperty IsDarkModeProperty =
            DependencyProperty.Register(
                nameof(IsDarkMode),
                typeof(bool),
                typeof(JettGptControl),
                new PropertyMetadata(false, OnIsDarkModeChanged));

        public JettGptControl()
        {
            InitializeComponent();
        }

        public bool IsDarkMode
        {
            get => (bool)GetValue(IsDarkModeProperty);
            set => SetValue(IsDarkModeProperty, value);
        }

        private static void OnIsDarkModeChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            ((JettGptControl)dependencyObject).UpdateTheme((bool)e.NewValue);
        }

        private void UpdateTheme(bool darkMode)
        {
            AiButtonLogo.Source = LoadLogo(
                darkMode
                    ? "Jettgpt_NoBkg_W.png"
                    : "Jettgpt_NoBkg_B.png");
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
            JettImage.Source = LoadLogo("Jett_dash_links.png");

            DoubleAnimation dashAnimation = new()
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
                JettImage.Source = LoadLogo("Jett_stand.png");

                _isAiChatOpen = true;
                _isAiAnimationRunning = false;
            };

            JettAiTransform.BeginAnimation(
                System.Windows.Media.TranslateTransform.XProperty,
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
            JettImage.Source = LoadLogo("Jett_dash_rechts.png");

            DoubleAnimation dashAnimation = new()
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
                System.Windows.Media.TranslateTransform.XProperty,
                dashAnimation);
        }

        private void ChatWindow_CloseRequested(
            object sender,
            RoutedEventArgs e)
        {
            CloseAiButton_Click(sender, e);
        }

        private static BitmapImage LoadLogo(string fileName)
        {
            BitmapImage bitmap = new();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(
                $"pack://application:,,,/PCVerwaltung;component/Images/{fileName}",
                UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }
    }
}
