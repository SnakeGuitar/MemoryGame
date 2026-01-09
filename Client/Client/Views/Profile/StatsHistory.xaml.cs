using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Core;
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
            try
            {
                    var history = await UserServiceManager.Instance.Client.GetMatchHistoryAsync(UserSession.SessionToken);
                    DataGridHistory.ItemsSource = history;
            }
            catch (EndpointNotFoundException ex)
            {
                ShowError(Lang.Global_Title_NetworkError, LocalizationHelper.GetString(ex));
            }
            catch (TimeoutException ex)
            {
                ShowError(Lang.Global_Title_NetworkError, LocalizationHelper.GetString(ex));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                ShowError(Lang.Global_Title_Error, Lang.MatchHistory_Label_ErrorHistory);
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
                string userToReport = matchInfo.WinnerName;

                var reportDialog = new ReportUserDialog(userToReport, matchInfo.MatchId);
                reportDialog.Owner = this;
                reportDialog.ShowDialog();
            }
        }

        private void ShowError(string title, string msg)
        {
            var msgBox = new CustomMessageBox(title, msg, this, CustomMessageBox.MessageBoxType.Error);
            msgBox.ShowDialog();
        }
    }
}