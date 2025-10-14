using Client.View.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Client.View.Session
{
    /// <summary>
    /// Interaction logic for WindowLogIn.xaml
    /// </summary>
    public partial class WindowLogIn : Window
    {
        private readonly UserServiceReference.UserServiceClient _userServiceClient = new UserServiceReference.UserServiceClient();
        public WindowLogIn()
        {
            InitializeComponent();
            _userServiceClient = new UserServiceReference.UserServiceClient();
        }

        private async void ButtonLogIn(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmailInput.Text?.Trim();
            string password = PasswordBoxPasswordInput.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool success = await _userServiceClient.LoginAsync(email, password);

                if (success)
                {
                    MessageBox.Show(
                        "Login successful!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    var mainMenu = new WindowMainMenu();
                    mainMenu.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Login failed. Please check your email and password.",
                        "Login Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while trying to log in: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                if (_userServiceClient?.State == System.ServiceModel.CommunicationState.Opened)
                {
                    _userServiceClient.Close();
                }
            }
            catch
            {
                _userServiceClient.Abort();
            }
            base.OnClosed(e);
        }
    }
}
