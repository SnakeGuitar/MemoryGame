using Client.Properties.Langs;
using Client.Views;
using Client.Views.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Core.Exceptions
{
    public static class DialogManager
    {
        private static bool _isDialogOpen = false;

        public static void ShowWarning(string title, string message, Window owner = null)
        {
            Show(title, message, owner, MessageBoxType.Warning);
        }

        public static void ShowError(string title, string message, Window owner = null)
        {
            Show(title, message, owner, MessageBoxType.Error);
        }

        private static void Show(string title, string message, Window owner, MessageBoxType type)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (owner == null || !owner.IsLoaded)
                        owner = Application.Current.MainWindow;

                    new CustomMessageBox(title, message, owner, type).ShowDialog();
                }
                catch
                {
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        public static bool AskToRetry(string title, string message, Window owner = null)
        {
            if (_isDialogOpen) return false;
            _isDialogOpen = true;

            bool result = false;

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (owner == null || !owner.IsLoaded)
                        owner = Application.Current.MainWindow;

                    string question = Lang.Global_Message_RetryConnection ?? "\n¿Would you like to try to reconnect?";
                    string fullMessage = $"{message}\n\n{question}";

                    var dialog = new ConfirmationMessageBox(
                        title,
                        fullMessage,
                        owner,
                        ConfirmationMessageBox.ConfirmationBoxType.Question);

                    result = dialog.ShowDialog() == true;
                }
                finally
                {
                    _isDialogOpen = false;
                }
            });

            return result;
        }

        public static void ForceNavigateToTitle(Window owner = null)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (owner == null || !owner.IsLoaded)
                    owner = Application.Current.MainWindow;

                try
                {
                    UserSession.EndSession();
                }
                catch
                {
                    Debug.WriteLine("Ignored ForceNavigateToTitle error");
                }

                Helpers.NavigationHelper.NavigateTo(owner, new TitleScreen());
            });
        }
    }
}