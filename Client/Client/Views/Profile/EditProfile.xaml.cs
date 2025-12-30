using System;
using System.Windows;
using Microsoft.Win32;
using Client.UserServiceReference;
using Client.Helpers;
using Client.Views.Controls;
using Client.Views.Session;

namespace Client.Views.Profile
{
    /// <summary>
    /// Interaction logic for EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Window
    {
        private readonly UserServiceClient _userServiceClient = new UserServiceClient();

        public EditProfile()
        {
            InitializeComponent();
            LoadCurrentAvatar();

            if (FindName("TextBoxNewUsername") is System.Windows.Controls.TextBox txt)
            {
                txt.Text = UserSession.Username;
            }
        }

        private async void LoadCurrentAvatar()
        {
            try
            {
                var bytes = await _userServiceClient.GetUserAvatarAsync(UserSession.Email);
                if (bytes != null && bytes.Length > 0)
                {
                    ImageAvatar.Source = ImageHelper.ByteArrayToImageSource(bytes);
                }
            }
            catch (Exception) {}
        }

        private async void ButtonChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    byte[] originalBytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                    byte[] resizedBytes = ImageHelper.ResizeImage(originalBytes, 200, 200);

                    var response = await _userServiceClient.UpdateUserAvatarAsync(UserSession.Email, resizedBytes);

                    if (response.Success)
                    {
                        ImageAvatar.Source = ImageHelper.ByteArrayToImageSource(resizedBytes);
                        ShowSuccess("Success", "Avatar updated successfully.");
                    }
                    else
                    {
                        ShowError("Error", "Failed to update avatar.");
                    }
                }
                catch (Exception)
                {
                    ShowError("Error", "Error processing the image.");
                }
            }
        }

        private async void ButtonUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            string currentPass = PasswordBoxCurrent.Password;
            string newPass = PasswordBoxNew.Password;

            if (string.IsNullOrWhiteSpace(currentPass) || string.IsNullOrWhiteSpace(newPass))
            {
                ShowError("Validation", "Please fill all password fields.");
                return;
            }

            try
            {
                var response = await _userServiceClient.ChangePasswordAsync(UserSession.SessionToken, currentPass, newPass);
                if (response.Success)
                {
                    ShowSuccess("Success", "Password updated.");
                    PasswordBoxCurrent.Password = "";
                    PasswordBoxNew.Password = "";
                }
                else
                {
                    ShowError("Error", "Failed to update password. Check your current password.");
                }
            }
            catch (Exception)
            {
                ShowError("Error", "Connection error.");
            }
        }

        private async void ButtonUpdateUsername_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = TextBoxNewUsername.Text.Trim();

            if (string.IsNullOrEmpty(newUsername))
            {
                ShowError("Validation", "Username cannot be empty.");
                return;
            }

            if (newUsername == UserSession.Username)
            {
                ShowError("Validation", "New username is the same as the current one.");
                return;
            }

            ButtonUpdateUsername.IsEnabled = false;

            try
            {
                var response = await _userServiceClient.ChangeUsernameAsync(UserSession.SessionToken, newUsername);

                if (response.Success)
                {
                    MessageBox.Show("Username updated successfully.\nFor security reasons, you must log in again.",
                                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    UserSession.EndSession();

                    Login loginWindow = new Login();
                    loginWindow.Show();

                    this.Close();

                    if (this.Owner != null)
                    {
                        this.Owner.Close();
                    }
                }
                else
                {
                    if (response.MessageKey == "Global_Error_UsernameInUse")
                        ShowError("Error", "This username is already taken.");
                    else
                        ShowError("Error", "Could not update username.");
                }
            }
            catch (Exception)
            {
                ShowError("Error", "Network connection error.");
            }
            finally
            {
                ButtonUpdateUsername.IsEnabled = true;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.Show();
            }
            this.Close();
        }

        private void ShowError(string title, string message)
        {
            var msgBox = new CustomMessageBox(title, message, this, CustomMessageBox.MessageBoxType.Error);
            msgBox.ShowDialog();
        }

        private void ShowSuccess(string title, string message)
        {
            var msgBox = new CustomMessageBox(title, message, this, CustomMessageBox.MessageBoxType.Success);
            msgBox.ShowDialog();
        }
    }
}