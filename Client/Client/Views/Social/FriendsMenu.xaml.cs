using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Core;
using Client.Views.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Client.Helpers.LocalizationHelper;
using static Client.Views.Controls.ConfirmationMessageBox;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Social
{
    public partial class FriendsMenu : Window
    {
        private readonly UserServiceClient _proxy;

        public FriendsMenu()
        {
            InitializeComponent();
            _proxy = UserServiceManager.Instance.Client;
            _ = LoadSocialData();
        }

        private async Task LoadSocialData()
        {
            try
            {
                var requests = await _proxy.GetPendingRequestsAsync(UserSession.SessionToken);

                var requestsDisplay = requests.Select(r => new RequestDisplay
                {
                    RequestId = r.RequestId,
                    SenderUsername = r.SenderUsername,
                    AvatarImage = ImageHelper.ByteArrayToImageSource(r.SenderAvatar)
                }).ToList();

                ListViewRequests.ItemsSource = requestsDisplay;

                var friendsDto = await _proxy.GetFriendsListAsync(UserSession.SessionToken);

                var friendDisplayList = friendsDto.Select(f => new FriendDisplay
                {
                    Username = f.Username,
                    IsOnline = f.IsOnline,
                    AvatarImage = ImageHelper.ByteArrayToImageSource(f.Avatar)
                }).ToList();

                DataGridFriends.ItemsSource = friendDisplayList;
            }
            catch (EndpointNotFoundException ex)
            {
                HandleException(ex, Lang.Global_Title_ServerOffline);
            }
            catch (TimeoutException ex)
            {
                HandleException(ex, Lang.Global_Title_NetworkError);
            }
            catch (CommunicationException ex)
            {
                HandleException(ex, Lang.Global_Title_NetworkError);
            }
            catch (Exception ex)
            {
                HandleException(ex, Lang.Global_Title_AppError);
            }
        }

        private void HandleException(Exception ex, string titleKey)
        {
            string errorMessage = GetString(ex) ?? ex.Message;
            Debug.WriteLine($"[{ex.GetType().Name}]: {ex.ToString()}");
            var msgBox = new CustomMessageBox(
                titleKey, errorMessage,
                this, MessageBoxType.Error);
            msgBox.ShowDialog();
        }

        private async void ButtonSendRequest_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxSearchUser.Text.Trim();
            if (string.IsNullOrEmpty(username)) return;

            if (username == UserSession.Username)
            {
                ShowError(Lang.Social_Error_SelfAdd);
                return;
            }

            ButtonSendRequest.IsEnabled = false;

            try
            {
                var response = await _proxy.SendFriendRequestAsync(UserSession.SessionToken, username);

                if (response.Success)
                {
                    string message = string.Format(Lang.Friends_Label_SuccessRequest, username);
                    ShowSuccess(message);
                    TextBoxSearchUser.Text = string.Empty;
                }
                else
                {
                    string msg = GetString(response.MessageKey);
                    ShowError(msg);
                }
            }
            catch (EndpointNotFoundException)
            {
                ShowError(Lang.Global_Title_ServerOffline);
            }
            catch (TimeoutException)
            {
                ShowError(Lang.Global_Title_NetworkError);
            }
            catch (Exception)
            {
                ShowError(Lang.Friends_Error_SendRequest);
            }
            finally
            {
                ButtonSendRequest.IsEnabled = true;
            }
        }

        private async void ButtonAcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int requestId)
            {
                await ProcessRequest(requestId, true);
            }
        }

        private async void ButtonDeclineRequest_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int requestId)
            {
                await ProcessRequest(requestId, false);
            }
        }

        private async Task ProcessRequest(int requestId, bool accept)
        {
            try
            {
                var response = await _proxy.AnswerFriendRequestAsync(UserSession.SessionToken, requestId, accept);

                if (response.Success)
                {
                    _ = LoadSocialData();
                }
                else
                {
                    ShowError(GetString(response.MessageKey));
                }
            }
            catch (EndpointNotFoundException)
            {
                ShowError(Lang.Global_Title_ServerOffline);
            }
            catch (Exception)
            {
                ShowError(Lang.Global_Label_ConnectionFailed);
            }
        }

        private async void ButtonRemoveFriend_Click(object sender, RoutedEventArgs e)
        {
            
            if (sender is Button btn && btn.Tag is string username)
            {
                string message = string.Format(Lang.Friends_Message_RemoveFriend, username);

                ConfirmationMessageBox confirmationBox = new ConfirmationMessageBox(
                message, Lang.Global_Label_Confirm, this, ConfirmationBoxType.Warning);

                bool? result = confirmationBox.ShowDialog();
                if (result == true)
                {
                    try
                    {
                        var response = await _proxy.RemoveFriendAsync(UserSession.SessionToken, username);

                        if (response.Success)
                        {
                            _ = LoadSocialData();
                        }
                        else
                        {
                            ShowError(Lang.Friends_Error_RemoveFriend);
                        }
                    }
                    catch (EndpointNotFoundException)
                    {
                        ShowError(Lang.Global_Title_ServerOffline);
                    }
                    catch (Exception)
                    {
                        ShowError(Lang.Global_Label_ConnectionFailed);
                    }
                }
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

        private void ShowError(string message)
        {
            var msgBox = new CustomMessageBox(Lang.Global_Title_Error, message,
                this, MessageBoxType.Error);
            msgBox.ShowDialog();
        }

        private void ShowSuccess(string message)
        {
            var msgBox = new CustomMessageBox(Lang.Global_Title_Success, message,
                this, MessageBoxType.Success);
            msgBox.ShowDialog();
        }

        public class FriendDisplay
        {
            public string Username { get; set; }
            public bool IsOnline { get; set; }
            public ImageSource AvatarImage { get; set; }

            public string StatusText => IsOnline ? Lang.Global_Label_Online : Lang.Global_Label_Offline;
            public Brush StatusColor => IsOnline ? Brushes.Green : Brushes.Gray;
        }
        public class RequestDisplay
        {
            public int RequestId { get; set; }
            public string SenderUsername { get; set; }
            public ImageSource AvatarImage { get; set; }
        }
    }
}