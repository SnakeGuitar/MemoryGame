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
using static Client.Helpers.ValidationHelper;

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
                var bytes = await UserServiceManager.Instance.GetUserAvatarAsync(UserSession.Email);
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

        private bool IsInputValid(ValidationCode code)
        {
            if (code == ValidationCode.Success) return true;

            string message = string.Empty;

            switch (code)
            {
                case ValidationCode.UsernameEmpty:
                    message = Lang.EditProfile_Label_ErrorUsernameEmpty;
                    break;
                case ValidationCode.UsernameTooLong:
                    message = Lang.Global_Error_UsernameTooLong ?? "Username is too long.";
                    break;
                case ValidationCode.UsernameInvalidChars:
                    message = Lang.Global_Error_UsernameInvalidChars ?? "Username contains invalid characters.";
                    break;

                case ValidationCode.PasswordEmpty:
                    message = Lang.EditProfile_Label_ErrorPasswordFields;
                    break;
                case ValidationCode.PasswordLengthInvalid:
                    message = Lang.Global_Error_PasswordLength ?? "Password must be between 8 and 100 characters.";
                    break;
                case ValidationCode.PasswordMissingUpper:
                    message = Lang.Global_Error_PasswordUpper ?? "Password must contain an uppercase letter.";
                    break;
                case ValidationCode.PasswordMissingLower:
                    message = Lang.Global_Error_PasswordLower ?? "Password must contain a lowercase letter.";
                    break;
                case ValidationCode.PasswordMissingDigit:
                    message = Lang.Global_Error_PasswordDigit ?? "Password must contain a digit.";
                    break;
                case ValidationCode.PasswordMissingSymbol:
                    message = Lang.Global_Error_PasswordSymbol ?? "Password must contain a symbol.";
                    break;

                default:
                    message = Lang.Global_Error_GenericValidation ?? "Invalid input.";
                    break;
            }

            ShowError(Lang.Global_Title_Error, message);
            return false;
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

                    var response = await UserServiceManager.Instance.UpdateUserAvatarAsync(UserSession.SessionToken, resizedBytes);

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

            if (string.IsNullOrWhiteSpace(currentPass))
            {
                ShowError(Lang.Global_Title_Warning, Lang.EditProfile_Label_ErrorPasswordFields);
                return;
            }

            if (!IsInputValid(ValidatePassword(newPass)))
            {
                return;
            }

            try
            {
                var response = await UserServiceManager.Instance.ChangePasswordAsync(UserSession.SessionToken, currentPass, newPass);
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

            if (!IsInputValid(ValidateUsername(newUsername)))
            {
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
                var response = await UserServiceManager.Instance.ChangeUsernameAsync(UserSession.SessionToken, newUsername);

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

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName))
            {
                ShowError(Lang.Global_Title_Error, Lang.Global_Error_EmptyFields ?? "Name fields cannot be empty.");
                return;
            }

            var button = sender as Button;
            if (button != null)
            {
                button.IsEnabled = false;
                Mouse.OverrideCursor = Cursors.Wait;
            }

            try
            {
                var response = await UserServiceManager.Instance.UpdatePersonalInfoAsync(UserSession.SessionToken, name, lastName);
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
            if (!(sender is Button button) || !(button.Tag is int socialId))
            {
                return;
            }

            if (socialId <= 0)
            {
                RemoveFromUiList(socialId);
                return;
            }

            await RemoveRemoteSocialNetworkAsync(socialId);
        }

        private async Task RemoveRemoteSocialNetworkAsync(int socialId)
        {
            try
            {
                var response = await UserServiceManager.Instance.RemoveSocialNetworkAsync(UserSession.SessionToken, socialId);

                if (response.Success)
                {
                    RemoveFromUiList(socialId);
                    RemoveFromSession(socialId);
                }
                else
                {
                    string errorMessage = GetString(response.MessageKey);
                    ShowError(Lang.Global_Title_Error, errorMessage);
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
            }
        }

        private void RemoveFromUiList(int socialId)
        {
            var item = SocialNetworksList.FirstOrDefault(s => s.SocialNetworkId == socialId);
            if (item != null)
            {
                SocialNetworksList.Remove(item);
            }
        }

        private static void RemoveFromSession(int socialId)
        {
            var sessionItem = UserSession.SocialNetworks.FirstOrDefault(s => s.SocialNetworkId == socialId);
            if (sessionItem != null)
            {
                UserSession.SocialNetworks.Remove(sessionItem);
            }
        }

        private async void ButtonAddSocial_Click(object sender, RoutedEventArgs e)
        {
            string account = TextBoxNewSocial.Text.Trim();
            if (string.IsNullOrEmpty(account))
            {
                return;
            }

            var button = sender as Button;
            if (button != null) button.IsEnabled = false;

            try
            {
                var response = await UserServiceManager.Instance.AddSocialNetworkAsync(UserSession.SessionToken, account);

                if (response.Success)
                {
                    TextBoxNewSocial.Text = string.Empty;

                    var newSocial = new SocialNetworkDTO
                    {
                        Account = account,
                        SocialNetworkId = response.NewSocialNetworkId
                    };

                    SocialNetworksList.Add(newSocial);
                    UserSession.SocialNetworks.Add(newSocial);

                    new CustomMessageBox(Lang.Global_Title_Success, "(˶ˆᗜˆ˵)", this, MessageBoxType.Success).ShowDialog();
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
                if (button != null) button.IsEnabled = true;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new PlayerProfile());
        }
        private void ShowError(string title, string message)
        {
            new CustomMessageBox(title, message, this, MessageBoxType.Error).ShowDialog();
        }

        private void ShowSuccess(string title, string message)
        {
            new CustomMessageBox(title, message, this, MessageBoxType.Success).ShowDialog();
        }
    }
}