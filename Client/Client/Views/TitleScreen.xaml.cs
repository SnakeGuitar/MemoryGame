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
using Client.Views.Session;

namespace Client.Views
{
    /// <summary>
    /// Lógica de interacción para TitleScreen.xaml
    /// </summary>
    public partial class TitleScreen : Window
    {
        public TitleScreen()
        {
            InitializeComponent();
        }

        private void ButtonLoginAsGuest_Click(object sender, RoutedEventArgs e)
        {
            var enterUsernameGuestWindow = new EnterUsernameGuest();
            enterUsernameGuestWindow.WindowState = this.WindowState;
            enterUsernameGuestWindow.Owner = this;
            enterUsernameGuestWindow.Show();
            this.Hide();
        }

        private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new Login();
            loginWindow.WindowState = this.WindowState;
            loginWindow.Owner = this;
            loginWindow.Show();
            this.Hide();
        }
        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            var registerAccountWindow = new RegisterAccount();
            registerAccountWindow.WindowState = this.WindowState;
            registerAccountWindow.Owner = this;
            registerAccountWindow.Show();
            this.Hide();
        }
    }
}
