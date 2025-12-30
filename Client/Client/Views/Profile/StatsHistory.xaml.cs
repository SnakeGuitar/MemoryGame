using Client.Helpers;
using Client.UserServiceReference;
using Client.Views.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Client.Views.Profile
{
    /// <summary>
    /// Interaction logic for StatsHistory.xaml
    /// </summary>
    public partial class StatsHistory : Window
    {
        public StatsHistory()
        {
            InitializeComponent();
            LoadMatchHistory();
        }

        private async void LoadMatchHistory()
        {
            try
            {
                using (var client = new UserServiceClient())
                {
                    var history = await client.GetMatchHistoryAsync(UserSession.SessionToken);
                    DataGridHistory.ItemsSource = history;
                }
            }
            catch (Exception)
            {
                var msgBox = new CustomMessageBox("Error", "Could not load match history.", this, CustomMessageBox.MessageBoxType.Error);
                msgBox.ShowDialog();
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

        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is MatchHistoryDTO matchInfo)
            {
                var reportDialog = new Client.Views.Controls.ReportUserDialog(matchInfo.WinnerName, matchInfo.MatchId);
                reportDialog.Owner = this;
                reportDialog.ShowDialog();
            }
        }
    }
}