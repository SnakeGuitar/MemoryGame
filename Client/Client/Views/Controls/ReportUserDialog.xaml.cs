using System;
using System.Windows;
using Client.UserServiceReference;
using Client.Helpers;

namespace Client.Views.Controls
{
    public partial class ReportUserDialog : Window
    {
        private readonly string _targetUsername;
        private readonly int _matchId;

        public ReportUserDialog(string targetUsername, int matchId)
        {
            InitializeComponent();
            _targetUsername = targetUsername;
            _matchId = matchId;
            TextTargetUser.Text = $"Target: {_targetUsername}";
        }

        private async void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            if (_targetUsername == UserSession.Username)
            {
                MessageBox.Show("You cannot report yourself.");
                this.Close();
                return;
            }

            try
            {
                using (var client = new UserServiceClient())
                {
                    var response = await client.ReportUserAsync(UserSession.SessionToken, _targetUsername, _matchId);

                    if (response.Success)
                    {
                        MessageBox.Show("User reported successfully. Admins will review the case.", "Report Sent", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Could not report user: {response.MessageKey}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Connection error while trying to report.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}