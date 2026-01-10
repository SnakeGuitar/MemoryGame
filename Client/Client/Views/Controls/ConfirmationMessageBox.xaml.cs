using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Client.Views.Controls
{
    /// <summary>
    /// Lógica de interacción para ConfirmationMessageBox.xaml
    /// </summary>
    public partial class ConfirmationMessageBox : Window
    {
        public enum ConfirmationBoxType
        {
            Information,
            Warning,
            Critic,
            Question
        }
        public ConfirmationMessageBox(string title, string message, Window owner, ConfirmationBoxType type)
        {
            InitializeComponent();

            if (owner != null && owner.IsVisible)
            {
                this.Owner = owner;
            }
            else
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }

            TextBlockTitle.Text = title;
            TextBlockMessage.Text = message;

            SetStyle(type);
        }

        private void SetStyle(ConfirmationBoxType type)
        {
            SolidColorBrush borderBrush;
            SolidColorBrush textMessageBrush = (SolidColorBrush)Application.Current.FindResource("AccentForegroundColor");

            switch (type)
            {
                case ConfirmationBoxType.Warning:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("AccentHoverColor");
                    break;
                case ConfirmationBoxType.Information:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("InformationColor");
                    break;
                case ConfirmationBoxType.Critic:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("HardColor");
                    break;
                case ConfirmationBoxType.Question:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("AccentColor");
                    break;
                default:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("AccentColor");
                    break;
            }
            MessageBorder.Background = borderBrush;
            TextBlockMessage.Foreground = textMessageBrush;
        }

        private void ButtonAccept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}