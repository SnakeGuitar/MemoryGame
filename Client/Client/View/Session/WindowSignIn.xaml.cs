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

namespace Client.View.Session
{
    /// <summary>
    /// Interaction logic for WindowSignIn.xaml
    /// </summary>
    public partial class WindowSignIn : Window
    {
        private readonly UserServiceReference.UserServiceClient _userServiceClient = new UserServiceReference.UserServiceClient();
        public WindowSignIn()
        {
            InitializeComponent();
            _userServiceClient = new UserServiceReference.UserServiceClient();
        }

        private async void ButtonSignIn(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmailInput?.Text;
            string password = PasswordBoxPasswordInput?.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool success = await _userServiceClient.RequestRegistrationAsync(email, password);

                if (success)
                {
                    MessageBox.Show(
                        "Sign-in successful!", 
                        "Success", 
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                        );

                    var UserVerificationWindow = new WindowUserVerification(email);
                    UserVerificationWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Sign-in failed. Please check your credentials and try again.", 
                        "Sign-In Failed", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error
                        );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred during sign-in: {ex.Message}", 
                    "Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error
                    );
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            try
                {
                if (_userServiceClient.State != System.ServiceModel.CommunicationState.Faulted)
                {
                    _userServiceClient.Close();
                }
                else
                {
                    _userServiceClient.Abort();
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
