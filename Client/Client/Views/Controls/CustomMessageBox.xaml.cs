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

namespace Client.Views.Controls
{
    /// <summary>
    /// Lógica de interacción para CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        public enum MessageBoxType
        {
            Information,
            Success,
            Warning,
            Error
        }
        public CustomMessageBox(String title, string message, Window owner, MessageBoxType type)
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

        private void SetStyle(MessageBoxType type)
        {
            SolidColorBrush borderBrush;
            SolidColorBrush textMessageBrush = (SolidColorBrush)Application.Current.FindResource("AccentForegroundColor") ?? Brushes.Black;

            switch (type)
            {
                case MessageBoxType.Success:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("EasyColor");
                    break;
                case MessageBoxType.Warning:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("AccentHoverColor");
                    break;
                case MessageBoxType.Error:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("HardColor");
                    break;
                case MessageBoxType.Information:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("InformationColor");
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
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
