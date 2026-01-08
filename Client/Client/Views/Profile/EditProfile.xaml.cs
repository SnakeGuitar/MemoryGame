using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Utilities;
using Client.Views.Controls;
using Client.Views.Session;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static Client.Helpers.LocalizationHelper;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Profile
{
    /// <summary>
    /// Interaction logic for EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Window
    {
        public ObservableCollection<SocialNetworkDTO> SocialNetworksList { get; set; }

        public EditProfile()
        {
            InitializeComponent();
            SocialNetworksList = new ObservableCollection<SocialNetworkDTO>();
            ItemsControlSocials.ItemsSource = SocialNetworksList;
            _ = LoadCurrentAvatar();
            LoadExistingData();

        }

        private void LoadExistingData()
        {
            if (FindName("TextBoxNewUsername") is System.Windows.Controls.TextBox txtUser)
            {
                txtUser.Text = UserSession.Username;
            }

            if (FindName("TextBoxName") is System.Windows.Controls.TextBox txtName)
            {
                txtName.Text = UserSession.Name ?? "";
            }
            if (FindName("TextBoxLastName") is System.Windows.Controls.TextBox txtLast)
            {
                txtLast.Text = UserSession.LastName ?? "";
            }

            if (UserSession.SocialNetworks != null)
            {
                foreach (var social in UserSession.SocialNetworks)
                {
                    SocialNetworksList.Add(social);
                }
            }
        }

        private async Task LoadCurrentAvatar()
        {
            try
            {
                var bytes = await UserServiceManager.Instance.Client.GetUserAvatarAsync(UserSession.Email);
                if (bytes != null && bytes.Length > 0)
                {
                    ImageAvatar.Source = ImageHelper.ByteArrayToImageSource(bytes);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (Exception ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
        }

        private async void ButtonChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = Lang.SetupProfile_Dialog_Title,
                Filter = Lang.SetupProfile_Dialog_Filter,
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    byte[] originalBytes = System.IO.File.ReadAllBytes(openFileDialog.FileName);
                    byte[] resizedBytes = ImageHelper.ResizeImage(originalBytes, 200, 200);

                    if (resizedBytes.Length == 0)
                    {
                        ShowError(Lang.Global_Title_Error, Lang.Global_Error_ImageEmpty);
                        return;
                    }

                    var response = await UserServiceManager.Instance.Client.UpdateUserAvatarAsync(UserSession.SessionToken, resizedBytes);

                    if (response.Success)
                    {
                        ImageAvatar.Source = ImageHelper.ByteArrayToImageSource(resizedBytes);
                        UserSession.OnProfileUpdated();

                        ShowSuccess(Lang.Global_Title_Success, Lang.EditProfile_Label_AvatarUpdated);
                    }
                    else
                    {
                        ShowError(Lang.Global_Title_Error, GetString(response.MessageKey));
                    }
                }
                catch(InvalidOperationException ex) when (ex.Message == "ImageTooLarge")
                {
                    ShowError(Lang.Global_Title_Error, Lang.Global_Error_ImageTooLarge);
                }
                catch (EndpointNotFoundException ex)
                {
                    string errorMessage = GetString(ex);
                    Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_NetworkError, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();
                }
                catch (CommunicationException ex)
                {
                    string errorMessage = GetString(ex);
                    Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_NetworkError, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();
                }
                catch (Exception ex)
                {
                    string errorMessage = GetString(ex);
                    Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_AppError, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();
                }
            }
        }

        private async void ButtonUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            string currentPass = PasswordBoxCurrent.Password;
            string newPass = PasswordBoxNew.Password;

            if (string.IsNullOrWhiteSpace(currentPass) || string.IsNullOrWhiteSpace(newPass))
            {
                CustomMessageBox messageBox = new CustomMessageBox(
                    Lang.Global_Title_Warning, Lang.EditProfile_Label_ErrorPasswordFields, 
                    this, MessageBoxType.Warning);
                return;
            }

            try
            {
                var response = await UserServiceManager.Instance.Client.ChangePasswordAsync(UserSession.SessionToken, currentPass, newPass);
                if (response.Success)
                {
                    ShowSuccess(Lang.Global_Title_Success, Lang.EditProfile_Label_PasswordUpdated);
                    PasswordBoxCurrent.Password = "";
                    PasswordBoxNew.Password = "";
                }
                else
                {
                    ShowError(Lang.Global_Title_Error, Lang.EditProfile_Label_ErrorPasswordUpdate);
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (Exception ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
        }

        private async void ButtonUpdateUsername_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = TextBoxNewUsername.Text.Trim();

            if (string.IsNullOrEmpty(newUsername))
            {
                ShowError(Lang.Global_Title_Error, Lang.EditProfile_Label_ErrorUsernameEmpty);
                return;
            }

            if (newUsername == UserSession.Username)
            {
                ShowError(Lang.Global_Title_Error, Lang.EditProfile_Label_ErrorSameUsername);
                return;
            }

            ButtonUpdateUsername.IsEnabled = false;

            try
            {
                var response = await UserServiceManager.Instance.Client.ChangeUsernameAsync(UserSession.SessionToken, newUsername);

                if (response.Success)
                {
                    CustomMessageBox messageBox = new CustomMessageBox(
                        Lang.Global_Title_Success, Lang.EditProfile_Label_SuccesUpdateUsername,
                        this, MessageBoxType.Information);

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
                        ShowError(Lang.Global_Title_Error, Lang.Global_Error_UsernameInUse);
                    else
                        ShowError(Lang.Global_Title_Error, "Could not update username.");
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (Exception ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            finally
            {
                ButtonUpdateUsername.IsEnabled = true;
            }
        }

        private async void ButtonUpdatePersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            string name = TextBoxName.Text.Trim();
            string lastName = TextBoxLastName.Text.Trim();

            try
            {
                var response = await UserServiceManager.Instance.Client.UpdatePersonalInfoAsync(UserSession.SessionToken, name, lastName);
                if (response.Success)
                {
                    UserSession.Name = name;
                    UserSession.LastName = lastName;

                    ShowSuccess(Lang.Global_Title_Success, Lang.EditProfile_Label_SuccesUpdateInfo);
                }
                else
                {
                    ShowError(Lang.Global_Title_Error, GetString(response.MessageKey));
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (Exception ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
        }

        private async void ButtonRemoveSocial_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int socialId)
            {
                try
                {
                    var response = await UserServiceManager.Instance.Client.RemoveSocialNetworkAsync(UserSession.SessionToken, socialId);

                    if (response.Success)
                    {
                        var itemToRemove = SocialNetworksList.FirstOrDefault(s => s.SocialNetworkId == socialId);
                        if (itemToRemove != null) SocialNetworksList.Remove(itemToRemove);

                        var sessionItem = UserSession.SocialNetworks.FirstOrDefault(s => s.SocialNetworkId == socialId);
                        if (sessionItem != null) UserSession.SocialNetworks.Remove(sessionItem);
                    }
                    else
                    {
                        ShowError(Lang.Global_Title_Error, GetString(response.MessageKey));
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    string errorMessage = GetString(ex);
                    Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_NetworkError, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();
                }
                catch (CommunicationException ex)
                {
                    string errorMessage = GetString(ex);
                    Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_NetworkError, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();
                }
                catch (Exception ex)
                {
                    string errorMessage = GetString(ex);
                    Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_AppError, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();
                }
            }
        }

        private async void ButtonAddSocial_Click(object sender, RoutedEventArgs e)
        {
            string account = TextBoxNewSocial.Text.Trim();
            if (string.IsNullOrEmpty(account)) return;

            try
            {
                var response = await UserServiceManager.Instance.Client.AddSocialNetworkAsync(UserSession.SessionToken, account);

                if (response.Success)
                {
                    SocialNetworksList.Add(new SocialNetworkDTO { Account = account });

                    UserSession.SocialNetworks.Add(new SocialNetworkDTO { Account = account });

                    TextBoxNewSocial.Text = string.Empty;
                }
                else
                {
                    ShowError(Lang.Global_Title_Error, GetString(response.MessageKey));
                }
            }
            catch (EndpointNotFoundException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[EndpointNotFoundException]: {errorMessage}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (CommunicationException ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[CommunicationException]: {ex.Message}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_NetworkError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
            catch (Exception ex)
            {
                string errorMessage = GetString(ex);
                Debug.WriteLine($"[Unexpected Error]: {ex.ToString()}");
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_AppError, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            if (Owner != null) Owner.Show();
            this.Close();
        }

        private void ShowError(string title, string message)
        {
            var msgBox = new CustomMessageBox(title, message, this, MessageBoxType.Error);
            msgBox.ShowDialog();
        }

        private void ShowSuccess(string title, string message)
        {
            var msgBox = new CustomMessageBox(title, message, this, MessageBoxType.Success);
            msgBox.ShowDialog();
        }
    }
}