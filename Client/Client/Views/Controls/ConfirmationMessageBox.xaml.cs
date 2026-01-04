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
    /// Lógica de interacción para ConfirmationMessageBox.xaml
    /// </summary>
    public partial class ConfirmationMessageBox : Window
    {
        public enum ConfirmationBoxType
        {
            Information,
            Warning,
            Critic
        }
        public ConfirmationMessageBox(string title, string message, Window owner, ConfirmationBoxType type)
        {
            InitializeComponent();
            this.Owner = owner;
            TextBlockTitle.Text = title;
            TextBlockMessage.Text = message;

            SetStyle(type);
        }

        private void SetStyle(ConfirmationBoxType type)
        {
            SolidColorBrush borderBrush;
            SolidColorBrush textMessageBrush;
            string lightTextColor = "AccentForegroundColor";

            switch (type)
            {
                case ConfirmationBoxType.Warning:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("AccentHoverColor");
                    textMessageBrush = (SolidColorBrush)Application.Current.FindResource(lightTextColor);
                    break;
                case ConfirmationBoxType.Information:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("InformationColor");
                    textMessageBrush = (SolidColorBrush)Application.Current.FindResource(lightTextColor);
                    break;
                case ConfirmationBoxType.Critic:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("HardColor");
                    textMessageBrush = (SolidColorBrush)Application.Current.FindResource(lightTextColor);
                    break;
                default:
                    borderBrush = (SolidColorBrush)Application.Current.FindResource("AccentColor");
                    textMessageBrush = (SolidColorBrush)Application.Current.FindResource(lightTextColor);
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
