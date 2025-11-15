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

            this.Owner = owner;
            TextBlockTitle.Text = title;
            TextBlockMessage.Text = message;

            SetStyle(type);
        }

        private void SetStyle(MessageBoxType type)
        {
            SolidColorBrush borderBrush;
            SolidColorBrush textBrush;

            switch (type)
            {
                case MessageBoxType.Success:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("EasyColor");
                    textBrush = (SolidColorBrush)Application.Current.FindResource("PrimaryTextColor");
                    break;
                case MessageBoxType.Warning:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("AccentHoverColor");
                    textBrush = (SolidColorBrush)Application.Current.FindResource("AccentForegroundColor");
                    break;
                case MessageBoxType.Error:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("HardColor");
                    textBrush = (SolidColorBrush)Application.Current.FindResource("PrimaryTextColor");
                    break;
                case MessageBoxType.Information:
                default:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("AccentColor");
                    textBrush = (SolidColorBrush)Application.Current.FindResource("AccentForegroundColor");
                    break;
            }

            MessageBorder.Background = borderBrush;
            TextBlockMessage.Foreground = textBrush;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
