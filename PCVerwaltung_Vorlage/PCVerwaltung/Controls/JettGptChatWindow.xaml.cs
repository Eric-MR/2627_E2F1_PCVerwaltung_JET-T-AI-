using PCVerwaltung.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCVerwaltung.Controls
{
    public partial class JettGptChatWindow : System.Windows.Controls.UserControl
    {
        private const string Greeting = "Hallo! Ich bin dein KI-Assistent.";
        private readonly DeepSeekChatService _chatService = new();
        private readonly List<DeepSeekChatMessage> _conversation = new()
        {
            new DeepSeekChatMessage("assistant", Greeting)
        };
        private bool _isSending;

        public JettGptChatWindow()
        {
            InitializeComponent();
            DataContext = this;
            Messages = new ObservableCollection<string> { Greeting };
            Messages.CollectionChanged += (_, _) => ChatScrollViewer.ScrollToEnd();
        }

        public event RoutedEventHandler? CloseRequested;

        public ObservableCollection<string> Messages { get; }

        private void CloseButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            CloseRequested?.Invoke(this, e);
        }

        private async void SendButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            await SendMessageAsync();
        }

        private async void AiInputBox_KeyDown(
            object sender,
            KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.Modifiers == ModifierKeys.None)
            {
                e.Handled = true;
                await SendMessageAsync();
            }
        }

        private async Task SendMessageAsync()
        {
            string userMessage = AiInputBox.Text.Trim();
            if (_isSending || string.IsNullOrWhiteSpace(userMessage))
            {
                return;
            }

            _isSending = true;
            SendButton.IsEnabled = false;
            AiInputBox.IsEnabled = false;
            AiInputBox.Clear();
            Messages.Add($"Du: {userMessage}");
            _conversation.Add(new DeepSeekChatMessage("user", userMessage));

            try
            {
                string response = await _chatService.SendMessageAsync(_conversation);
                _conversation.Add(new DeepSeekChatMessage("assistant", response));
                Messages.Add($"Jett.gpt: {response}");
            }
            catch (Exception exception)
            {
                Messages.Add($"Jett.gpt: {exception.Message}");
            }
            finally
            {
                _isSending = false;
                SendButton.IsEnabled = true;
                AiInputBox.IsEnabled = true;
                AiInputBox.Focus();
            }
        }
    }
}
