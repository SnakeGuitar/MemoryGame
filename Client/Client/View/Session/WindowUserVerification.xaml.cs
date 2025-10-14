using Client.UserServiceReference;
using Client.View.Game;
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
    /// Interaction logic for WindowUserVerification.xaml
    /// </summary>
    public partial class WindowUserVerification : Window
    {
        private readonly string _email;
        private readonly UserServiceClient _userServiceClient;
        private const int PIN_LENGTH = 6;
        public WindowUserVerification(string email)
        {
            InitializeComponent();
            _email = email ?? throw new ArgumentNullException(nameof(email));
            _userServiceClient = new UserServiceClient();
        }

        private async void ButtonVerify(object sender, RoutedEventArgs e)
        {
            string pin = TextBoxPinInput.Text?.Trim();
            if (string.IsNullOrEmpty(pin) || pin.Length != PIN_LENGTH )
            {
                MessageBox.Show("Please enter the verification code.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                bool success = await _userServiceClient.VerifyRegistrationAsync(_email, pin);

                if (success)
                {
                    MessageBox.Show("Your account has been successfully verified!", 
                        "Success", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Information);
                    var profileWindow = new WindowProfileSetup(_email);
                    profileWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Verification failed. Please check the code and try again.", "Verification Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred during verification: {ex.Message}", 
                    "Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }

        }

        private void ButtonSendAgain(object sender, RoutedEventArgs e)
        {

        }

        private bool IsNumeric(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => char.IsDigit(c));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _userServiceClient.Close();
        }
    }
}
