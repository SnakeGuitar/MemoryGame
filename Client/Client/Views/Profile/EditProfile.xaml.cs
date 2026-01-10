using Client.Core;
using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Controls;
using Client.Views.Session;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            LoadExistingData();
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            await LoadCurrentAvatar();
        }

        private void LoadExistingData()
        {
            if (FindName("TextBoxNewUsername") is TextBox txtUser)
            {
                txtUser.Text = UserSession.Username;
            }

            if (FindName("TextBoxName") is TextBox txtName)
            {
                txtName.Text = UserSession.Name ?? "";
            }
            if (FindName("TextBoxLastName") is TextBox txtLast)
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
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
            }
        }

        private async void ButtonChangeAvatar_Click(object sender, RoutedEventArgs e)
        {
            var avatarDialog = NavigationHelper.GetOpenFileDialog(
                Lang.SetupProfile_Dialog_Title,
                Lang.SetupProfile_Dialog_Filter,
                false);

            if (avatarDialog.ShowDialog() == true)
            {
                try
                {
                    byte[] originalBytes = File.ReadAllBytes(avatarDialog.FileName);
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
                catch (InvalidOperationException ex) when (ex.Message == "ImageTooLarge")
                {
                    ShowError(Lang.Global_Title_Error, Lang.Global_Error_ImageTooLarge);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Handle(ex, this);
                }
            }
        }

        private async void ButtonUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            string currentPass = PasswordBoxCurrent.Password;
            string newPass = PasswordBoxNew.Password;

            if (string.IsNullOrWhiteSpace(currentPass) || string.IsNullOrWhiteSpace(newPass))
            {
                new CustomMessageBox(Lang.Global_Title_Warning, Lang.EditProfile_Label_ErrorPasswordFields,
                    this, MessageBoxType.Warning).ShowDialog();
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
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
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
                    new CustomMessageBox(Lang.Global_Title_Success, Lang.EditProfile_Label_SuccesUpdateUsername,
                        this, MessageBoxType.Information).ShowDialog();

                    try
                    {
                        await UserServiceManager.Instance.Client.LogoutAsync(UserSession.SessionToken);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error during logout: {ex.Message}");
                    }

                    UserSession.EndSession();
                    NavigationHelper.NavigateTo(this, new Login());
                }
                else
                {
                    string msg = (response.MessageKey == ServerKeys.UsernameInUse)
                        ? Lang.Global_Error_UsernameInUse
                        : GetString(response.MessageKey);

                    ShowError(Lang.Global_Title_Error, msg);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
            }
            finally
            {
                if (ButtonUpdateUsername != null && !ButtonUpdateUsername.IsEnabled && Application.Current.MainWindow == this)
                {
                    ButtonUpdateUsername.IsEnabled = true;
                }
            }
        }

        private async void ButtonUpdatePersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            string name = TextBoxName.Text.Trim();
            string lastName = TextBoxLastName.Text.Trim();

            var button = sender as Button;

            if (button != null)
            {
                button.IsEnabled = false;
                Mouse.OverrideCursor = Cursors.Wait;
            }

            try
            {
                var response = await UserServiceManager.Instance.Client.UpdatePersonalInfoAsync(UserSession.SessionToken, name, lastName);
                if (response.Success)
                {
                    UserSession.Name = name;
                    UserSession.LastName = lastName;
                    new CustomMessageBox(Lang.Global_Title_Success, Lang.EditProfile_Label_SuccesUpdateInfo,
                        this, MessageBoxType.Success).ShowDialog();
                }
                else
                {
                    ShowError(Lang.Global_Title_Error, GetString(response.MessageKey));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
            }
            finally
            {
                if (button != null)
                {
                    button.IsEnabled = true;
                    Mouse.OverrideCursor = null;
                }
            }
        }

        private async void ButtonRemoveSocial_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int socialId)
            {
                try
                {
                    var response = await UserServiceManager.Instance.Client.RemoveSocialNetworkAsync(UserSession.SessionToken, socialId);

                    if (response.Success)
                    {
                        var itemToRemove = SocialNetworksList.FirstOrDefault(s => s.SocialNetworkId == socialId);
                        if (itemToRemove != null)
                        {
                            SocialNetworksList.Remove(itemToRemove);
                        }
                        var sessionItem = UserSession.SocialNetworks.FirstOrDefault(s => s.SocialNetworkId == socialId);
                        if (sessionItem != null)
                        {
                            UserSession.SocialNetworks.Remove(sessionItem);
                        }
                    }
                    else
                    {
                        ShowError(Lang.Global_Title_Error, GetString(response.MessageKey));
                    }
                }
                catch (Exception ex)
                {
                    ExceptionManager.Handle(ex, this);
                }
            }
        }

        private async void ButtonAddSocial_Click(object sender, RoutedEventArgs e)
        {
            string account = TextBoxNewSocial.Text.Trim();
            if (string.IsNullOrEmpty(account))
            {
                return;
            }

            try
            {
                var response = await UserServiceManager.Instance.Client.AddSocialNetworkAsync(UserSession.SessionToken, account);

                if (response.Success)
                {
                    var newSocial = new SocialNetworkDTO { Account = account };
                    SocialNetworksList.Add(newSocial);
                    UserSession.SocialNetworks.Add(new SocialNetworkDTO { Account = account });
                    TextBoxNewSocial.Text = string.Empty;
                }
                else
                {
                    ShowError(Lang.Global_Title_Error, GetString(response.MessageKey));
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner as Window ?? new MainMenu());
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