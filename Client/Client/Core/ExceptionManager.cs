using Client.Helpers;
using Client.Properties.Langs;
using Client.Views;
using Client.Views.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using static Client.Views.Controls.CustomMessageBox;

namespace Client.Core
{
    public static class ExceptionManager
    {
        private static readonly object _logLock = new object();
        private const string LogFileName = "client_crash_log.txt";

        private static bool _isAskingUser = false;

        public static async Task<bool> ExecuteNetworkCallAsync(Func<Task> action, Window owner)
        {
            int retryCount = 0;
            const int maxSilentRetries = 3;

            while (true)
            {
                try
                {
                    await action();
                    return true;
                }
                catch (Exception ex)
                {
                    if (_isAskingUser)
                    {
                        return false;
                    }

                    if (IsRecoverableError(ex))
                    {
                        retryCount++;
                        if (retryCount <= maxSilentRetries)
                        {
                            Debug.WriteLine($"[Red] Silent retry {retryCount}/{maxSilentRetries}...");
                            await Task.Delay(500 * retryCount);
                            continue;
                        }
                    }
                    if (IsRecoverableError(ex) || IsCriticalServerError(ex))
                    {
                        bool userWantsToWait = AskUserToRetry(ex, owner);

                        if (userWantsToWait)
                        {
                            retryCount = 0;
                            Debug.WriteLine("[User] chose to retry.");
                            continue;
                        }
                        else
                        {
                            Debug.WriteLine("[User] chose to leave.");
                            NavigateToTitleScreen(owner);
                            return false;
                        }
                    }

                    Handle(ex, owner);
                    return false;
                }
            }
        }

        private static bool AskUserToRetry(Exception ex, Window owner)
        {
            if (_isAskingUser)
            {
                return false;
            }
            _isAskingUser = true;

            bool result = false;

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (owner == null || !owner.IsLoaded)
                        owner = Application.Current.MainWindow;

                    string title = Lang.Global_Title_ConnectionError;
                    string msgError = GetFriendlyErrorMessage(ex).Message;
                    string msgQuestion = "\n\n" + (Lang.Global_Message_RetryConnection ?? "¿Would you like to try to reconnect? \n(Select 'No' to go back to menu)");

                    var dialog = new ConfirmationMessageBox(
                        title,
                        msgError + msgQuestion,
                        owner,
                        ConfirmationMessageBox.ConfirmationBoxType.Question);

                    result = dialog.ShowDialog() == true;
                }
                finally
                {
                    _isAskingUser = false;
                }
            });

            return result;
        }

        private static void NavigateToTitleScreen(Window owner)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NavigationHelper.NavigateTo(owner, new TitleScreen());
                UserSession.EndSession();
            });
        }


        private static bool IsRecoverableError(Exception ex)
        {
            return ex is TimeoutException ||
                   ex is EndpointNotFoundException ||
                   ex is CommunicationException;
        }

        private static bool IsCriticalServerError(Exception ex)
        {
            string msg = ex.Message ?? "";
            return msg.Contains("Global_ServiceError_Database") ||
                   msg.Contains("EntityException") ||
                   msg.Contains("provider failed");
        }

        public static void Handle(Exception ex, Window owner = null, Action onHandled = null)
        {
            if (ex == null || ex is TaskCanceledException)
            {
                return;
            }
            LogException(ex);

            var info = GetFriendlyErrorMessage(ex);

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (owner == null || !owner.IsLoaded) owner = Application.Current.MainWindow;
                new CustomMessageBox(info.Title, info.Message, owner, MessageBoxType.Warning).ShowDialog();
                onHandled?.Invoke();
            });
        }

        private static (string Title, string Message) GetFriendlyErrorMessage(Exception ex)
        {
            if (IsRecoverableError(ex))
            {
                return (Lang.Global_Title_NetworkError, Lang.Global_Error_ConnectionLost);
            }

            if (IsCriticalServerError(ex))
            {
                return (Lang.Global_Title_DatabaseDown, Lang.Global_Error_DatabaseCritical);
            }
            if (ex is FaultException faultEx)
            {
                string msg = LocalizationHelper.GetString(faultEx.Message);
                if (msg == faultEx.Message)
                {
                    return (Lang.Global_Title_Warning, Lang.Global_Error_GenericValidation);
                }
                return (Lang.Global_Title_Warning, msg);
            }

            return (Lang.Global_Title_Error, Lang.Global_ServiceError_Unknown);
        }

        private static void LogException(Exception ex)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFileName);
                lock (_logLock)
                {
                    File.AppendAllText(logPath, $"[{DateTime.Now}] {ex.GetType().Name}: {ex.Message}\n");
                }
            }
            catch 
            {
                Debug.WriteLine("Ignored log exception error");
            }
        }
    }
}