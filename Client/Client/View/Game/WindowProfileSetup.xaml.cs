using Client.UserServiceReference;
using Microsoft.Win32;
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

namespace Client.View.Game
{
    /// <summary>
    /// Interaction logic for WindowProfileSetup.xaml
    /// </summary>
    public partial class WindowProfileSetup : Window
    {
        private readonly string _email;
        private byte[] _profileImageBytes;
        private readonly UserServiceClient _userServiceClient;
        public WindowProfileSetup(string email)
        {
            InitializeComponent();
            _email = email ?? throw new ArgumentNullException(nameof(email));
            _userServiceClient = new UserServiceClient();
        }

        private async void ButtonContinue(object sender, RoutedEventArgs e)
        {
            string username = TextBoxUsername.Text?.Trim();
            if (string.IsNullOrEmpty(username))
            {
                username = _email.Split('@')[0];
                if (string.IsNullOrEmpty(username))
                {
                    username = "User" + new Random().Next(1000, 9999);
                }
            }

            try
            {
                bool success = await _userServiceClient.UpdateUserProfileAsync(
                    _email,
                    username,
                    _profileImageBytes);

                if (success)
                {
                    MessageBox.Show("Profile setup complete!",
                        "Success",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    // Proceed to the main application window or next step
                    new WindowMainMenu().Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Failed to update profile. Please try again.",
                        "Update Failed",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while updating profile: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

            }
        }

        private void ButtonSelectAvatar(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select Profile Image",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp",
                Multiselect = false
            };

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(dialog.FileName);
                    bitmap.DecodePixelWidth = 200; // Resize for performance
                    bitmap.EndInit();
                    bitmap.Freeze(); // Freeze for cross-thread operations
                    ProfilePicture.Source = bitmap;

                    // Convert image to byte array
                    _profileImageBytes = System.IO.File.ReadAllBytes(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Failed to load image: {ex.Message}",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    _profileImageBytes = null;
                    ProfilePicture.Source = null;
                }
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
                _userServiceClient?.Abort();
            }
            base.OnClosed(e);
        }
    }
}
