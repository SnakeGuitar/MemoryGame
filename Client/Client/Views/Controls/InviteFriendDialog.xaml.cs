using Client.Properties.Langs;
using System;
using System.Diagnostics;
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

            SendEmail(email);
            this.Close();
        }

        private void SendEmail(string targetEmail)
        {
            try
            {
                string subject = Lang.InviteFriendDialog_Title_SubjectEmail;
                string body = Lang.InviteFriendDialog_Message_BodyEmail + $"{_lobbyCode}";
                string mailtoUri = $"mailto:{targetEmail}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";
                Process.Start(new ProcessStartInfo(mailtoUri) { UseShellExecute = true });
            }
            catch (System.ComponentModel.Win32Exception)
            {
                new CustomMessageBox(
                    Lang.Global_Title_Error,
                    Lang.InviteFriendDialog_Label_ErrorAppEmail,
                    this, MessageBoxType.Error).ShowDialog();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SendEmail Error]: {ex.Message}");
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