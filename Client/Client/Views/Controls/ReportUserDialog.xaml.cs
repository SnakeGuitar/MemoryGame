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
            TextTargetUser.Text = $"{Lang.Global_Label_Username}: {_targetUsername}";
        }

        private async void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            if (_targetUsername == UserSession.Username)
            {
                new CustomMessageBox(
                    Lang.Global_Title_Error, Lang.ReportUserDialog_Error_AutoReport,
                    this, MessageBoxType.Error).ShowDialog();
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
                    new CustomMessageBox(
                        Lang.ReportUserDialog_Title_ReportSuccess, Lang.ReportUserDialog_Message_ReportSuccess,
                        this, MessageBoxType.Success).ShowDialog();
                    this.Close();
                }
                else
                {
                    new CustomMessageBox(
                        Lang.Global_Title_Error, GetString(response.MessageKey),
                        this, MessageBoxType.Error).ShowDialog();

                    ButtonReport.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex, this, () => ButtonReport.IsEnabled = true);
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