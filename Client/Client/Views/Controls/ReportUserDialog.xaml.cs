using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Core;
using System;
using System.Windows;
using System.Windows.Input;
using static Client.Helpers.LocalizationHelper;
using static Client.Views.Controls.CustomMessageBox;

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
                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_Error, Lang.ReportUserDialog_Error_AutoReport,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();
                this.Close();
                return;
            }

            ButtonReport.IsEnabled = false;
            var client = UserServiceManager.Instance.Client;

            try
            {
                var response = await client.ReportUserAsync(UserSession.SessionToken, _targetUsername, _matchId);

                if (response.Success)
                {
                    var msgBox = new CustomMessageBox(
                        Lang.ReportUserDialog_Title_ReportSuccess, Lang.ReportUserDialog_Message_Report,
                        this, MessageBoxType.Success);
                    msgBox.ShowDialog();
                    this.Close();
                }
                else
                {
                    string errorMessage = GetString(response.MessageKey);
                    var msgBox = new CustomMessageBox(
                        Lang.Global_Title_Error, errorMessage,
                        this, MessageBoxType.Error);
                    msgBox.ShowDialog();

                    ButtonReport.IsEnabled = true;
                }

                client.Close();
            }
            catch (Exception ex)
            {
                client.Abort();
                string errorMessage = GetString(ex);

                var msgBox = new CustomMessageBox(
                    Lang.Global_Title_Error, errorMessage,
                    this, MessageBoxType.Error);
                msgBox.ShowDialog();

                ButtonReport.IsEnabled = true;
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}