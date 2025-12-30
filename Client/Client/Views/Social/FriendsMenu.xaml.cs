using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Client.UserServiceReference;
using Client.Helpers;
using Client.Views.Controls;

namespace Client.Views.Social
{
    public partial class FriendsMenu : Window
    {
        private readonly UserServiceClient _proxy;

        public FriendsMenu()
        {
            InitializeComponent();
            _proxy = new UserServiceClient();
            LoadSocialData();
        }

        private async void LoadSocialData()
        {
            try
            {
                var requests = await _proxy.GetPendingRequestsAsync(UserSession.SessionToken);
                ListViewRequests.ItemsSource = requests;

                var friendsDto = await _proxy.GetFriendsListAsync(UserSession.SessionToken);

                var friendDisplayList = friendsDto.Select(f => new FriendDisplay
                {
                    Username = f.Username,
                    IsOnline = f.IsOnline,
                    AvatarImage = ImageHelper.ByteArrayToImageSource(f.Avatar)
                }).ToList();

                DataGridFriends.ItemsSource = friendDisplayList;
            }
            catch (CommunicationException)
            {
                ShowError("Network Error", "Could not connect to the server.");
            }
            catch (Exception ex)
            {
                ShowError("Error", "An unexpected error occurred: " + ex.Message);
            }
        }

        private async void ButtonSendRequest_Click(object sender, RoutedEventArgs e)
        {
            string username = TextBoxSearchUser.Text.Trim();
            if (string.IsNullOrEmpty(username)) return;

            if (username == UserSession.Username)
            {
                ShowError("Error", "You cannot add yourself.");
                return;
            }

            ButtonSendRequest.IsEnabled = false;

            try
            {
                var response = await _proxy.SendFriendRequestAsync(UserSession.SessionToken, username);

                if (response.Success)
                {
                    ShowSuccess("Success", $"Friend request sent to {username}!");
                    TextBoxSearchUser.Text = string.Empty;
                }
                else
                {
                    string msg = GetFriendlyErrorMessage(response.MessageKey);
                    ShowError("Error", msg);
                }
            }
            catch (Exception)
            {
                ShowError("Error", "Failed to send request due to network issues.");
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

        private async System.Threading.Tasks.Task ProcessRequest(int requestId, bool accept)
        {
            try
            {
                var response = await _proxy.AnswerFriendRequestAsync(UserSession.SessionToken, requestId, accept);

                if (response.Success)
                {
                    LoadSocialData();
                }
                else
                {
                    ShowError("Error", GetFriendlyErrorMessage(response.MessageKey));
                }
            }
            catch (Exception)
            {
                ShowError("Error", "Connection failed.");
            }
        }

        private async void ButtonRemoveFriend_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string username)
            {
                var result = MessageBox.Show($"Are you sure you want to remove {username}?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var response = await _proxy.RemoveFriendAsync(UserSession.SessionToken, username);

                        if (response.Success)
                        {
                            LoadSocialData();
                        }
                        else
                        {
                            ShowError("Error", "Could not remove friend.");
                        }
                    }
                    catch (Exception)
                    {
                        ShowError("Error", "Connection failed.");
                    }
                }
            }
        }

        private string GetFriendlyErrorMessage(string key)
        {
            switch (key)
            {
                case "Global_Error_UserNotFound": return "User not found.";
                case "Social_Error_SelfAdd": return "You cannot add yourself.";
                case "Social_Error_AlreadyFriends": return "You are already friends.";
                case "Social_Error_RequestExists": return "Friend request already pending.";
                default: return "An error occurred.";
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

        public class FriendDisplay
        {
            public string Username { get; set; }
            public bool IsOnline { get; set; }
            public ImageSource AvatarImage { get; set; }

            public string StatusText => IsOnline ? "Online" : "Offline";
            public Brush StatusColor => IsOnline ? Brushes.Green : Brushes.Gray;
        }
    }
}