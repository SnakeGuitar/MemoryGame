using Client.Core;
using Client.Helpers;
using Client.UserServiceReference;
using Client.Views.Controls;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
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
            _ = LoadMatchHistory();
        }

        private async Task LoadMatchHistory()
        {
            await ExceptionManager.ExecuteSafeAsync(async () =>
            {
                var sessionCheck = await UserServiceManager.Instance.RenewSessionAsync(UserSession.SessionToken);
                if (!sessionCheck.Success)
                {
                    throw new FaultException(sessionCheck.MessageKey);
                }
                var history = await UserServiceManager.Instance.GetMatchHistoryAsync(UserSession.SessionToken);
                DataGridHistory.ItemsSource = history;
            });
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationHelper.NavigateTo(this, this.Owner ?? new PlayerProfile());
        }

        private void ButtonReport_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is MatchHistoryDTO matchInfo)
            {
                string userToReport = matchInfo.WinnerName;
                var reportDialog = new ReportUserDialog(userToReport, matchInfo.MatchId);
                NavigationHelper.ShowDialog(this, reportDialog);
            }
        }
    }
}