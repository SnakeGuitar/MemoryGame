using Client.Core;
using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Controls;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

                var safeRequests = requests ?? new FriendRequestDTO[0];

                var requestsDisplay = safeRequests.Select(r => new RequestDisplay
                {
                    RequestId = r.RequestId,
                    SenderUsername = r.SenderUsername,
                    AvatarImage = r.SenderAvatar != null
                        ? ImageHelper.ByteArrayToImageSource(r.SenderAvatar)
                        : null
                }).ToList();

                ListViewRequests.ItemsSource = requestsDisplay;

                var friendsDto = await _proxy.GetFriendsListAsync(UserSession.SessionToken);

                var safeFriends = friendsDto ?? new FriendDTO[0];

                var friendDisplayList = safeFriends.Select(f => new FriendDisplay
                {
                    Username = f.Username,
                    IsOnline = f.IsOnline,
                    AvatarImage = f.Avatar != null
                        ? ImageHelper.ByteArrayToImageSource(f.Avatar)
                        : null
                }).ToList();

                DataGridFriends.ItemsSource = friendDisplayList;
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
            }
        }

        private async void ButtonSendRequest_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxSearchUser.Text.Trim();
            if (string.IsNullOrEmpty(username)) return;

            if (username == UserSession.Username)
            {
                new CustomMessageBox(Lang.Global_Title_Error, Lang.Social_Error_SelfAdd,
                    this, MessageBoxType.Error).ShowDialog();
                return;
            }

            ButtonSendRequest.IsEnabled = false;

            try
            {
                var response = await _proxy.SendFriendRequestAsync(UserSession.SessionToken, username);

                if (response.Success)
                {
                    new CustomMessageBox(Lang.Global_Title_Success,
                        string.Format(Lang.Friends_Label_SuccessRequest, username),
                        this, MessageBoxType.Success).ShowDialog();

                    TextBoxSearchUser.Text = string.Empty;
                }
                else
                {
                    new CustomMessageBox(Lang.Global_Title_Error, GetString(response.MessageKey),
                        this, MessageBoxType.Error).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
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
                    new CustomMessageBox(Lang.Global_Title_Error, GetString(response.MessageKey),
                        this, MessageBoxType.Error).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this);
            }
        }

        private async void ButtonRemoveFriend_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string username)
            {
                var confirmationBox = new ConfirmationMessageBox(
                    string.Format(Lang.Friends_Message_RemoveFriend, username),
                    Lang.Global_Label_Confirm, this, ConfirmationBoxType.Warning);

                if (confirmationBox.ShowDialog() == true)
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
                            new CustomMessageBox(Lang.Global_Title_Error, Lang.Friends_Error_RemoveFriend,
                                this, MessageBoxType.Error).ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionManager.Handle(ex, this);
                    }
                }
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new MainMenu());
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