using Client.Core;
using Client.Properties.Langs;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Views.Controls
{
    public partial class InviteFriendDialog : Window
    {
        private readonly string _lobbyCode;

        public InviteFriendDialog(string lobbyCode)
        {
            InitializeComponent();
            _lobbyCode = lobbyCode;
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            string email = TextBoxEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                new CustomMessageBox(
                    Lang.Global_Title_Error,
                    Lang.InviteFriendDialog_Message_EnterEmail,
                    this, MessageBoxType.Warning).ShowDialog();
                return;
            }

            _ = SendEmail(email);
            this.Close();
        }

        private async Task SendEmail(string targetEmail)
        {
            try
            {
                string subject = Lang.InviteFriendDialog_Title_SubjectEmail;
                string body = string.Format(Lang.InviteFriendDialog_Message_BodyEmail, _lobbyCode);
                bool sent = await GameServiceManager.Instance.Client
                    .SendInvitationEmailAsync(targetEmail, subject, body);

                if (sent)
                {
                    new CustomMessageBox(
                        Lang.Global_Title_Success,
                        Lang.InviteFriendDialog_Message_EmailSentSuccess,
                        this, MessageBoxType.Success).ShowDialog();
                    this.Close();
                }
                else
                {
                    new CustomMessageBox(
                        Lang.Global_Title_Error,
                        Lang.InviteFriendDialog_Label_ErrorAppEmail,
                        this, MessageBoxType.Error).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                new CustomMessageBox(
                    Lang.Global_Title_Error,
                    Lang.Global_ServiceError_Unknown,
                    this, MessageBoxType.Error).ShowDialog();
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