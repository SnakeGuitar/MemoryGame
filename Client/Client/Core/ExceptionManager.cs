using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Core
{
    /// <summary>
    /// Manages centralized exception handling, logging, and UI alerts for the application.
    /// Acts as a "Tank" to absorb errors and prevent crashes.
    /// </summary>
    public static class ExceptionManager
    {
        private static readonly object _logLock = new object();
        private const string LogFileName = "client_crash_log.txt";

        /// <summary>
        /// Handles an exception by logging it and displaying a user-friendly alert on the UI thread.
        /// </summary>
        /// <param name="ex">The exception to process.</param>
        /// <param name="owner">The parent window for the alert (optional). If null, attempts to find the MainWindow.</param>
        /// <param name="onHandled">Callback to execute after the alert is closed.</param>
        /// <param name="isFatal">If set to true, the application will shut down after handling.</param>
        public static void Handle(Exception ex, Window owner = null, Action onHandled = null, bool isFatal = false)
        {
            if (ex == null)
            {
                return;
            }

            LogException(ex);

            var details = GetExceptionDetails(ex);

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (owner == null && Application.Current.MainWindow != null && Application.Current.MainWindow.IsVisible)
                    {
                        owner = Application.Current.MainWindow;
                    }

                    ShowSafeMessageBox(details.Title, details.Message, owner, isFatal);

                    onHandled?.Invoke();

                    if (isFatal)
                    {
                        UserSession.EndSession();
                        Application.Current.Shutdown();
                    }
                }
                catch (Exception dispatchEx)
                {
                    Debug.WriteLine($"[CRITICAL UI ERROR]: {dispatchEx.Message}");
                }
            });
        }

        /// <summary>
        /// Safely attempts to show a CustomMessageBox, falling back to native MessageBox if visual tree issues occur.
        /// </summary>
        private static void ShowSafeMessageBox(string title, string message, Window owner, bool isFatal)
        {
            try
            {
                var type = isFatal ? CustomMessageBox.MessageBoxType.Error : CustomMessageBox.MessageBoxType.Warning;
                var msgBox = new CustomMessageBox(title, message, owner, type);
                msgBox.ShowDialog();
            }
            catch (Exception uiEx)
            {
                Debug.WriteLine($"CustomMessageBox failed: {uiEx.Message}");
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Analyzes the exception type to determine the appropriate localized Title and Message.
        /// </summary>
        private static (string Title, string Message) GetExceptionDetails(Exception ex)
        {
            string title = Lang.Global_Title_AppError;
            string message = ex.Message;

            if (ex is EndpointNotFoundException || ex is ServerTooBusyException)
            {
                title = Lang.Global_Title_ServerOffline;
                message = Lang.Global_Label_ConnectionFailed;
            }
            else if (ex is FaultException faultEx)
            {
                message = LocalizationHelper.GetString(faultEx.Message);
                title = DetermineTitleFromKey(faultEx.Message);
            }
            else if (ex is TimeoutException || ex is CommunicationException)
            {
                title = Lang.Global_Title_NetworkError;
                message = LocalizationHelper.GetString(ex);
            }
            else if (ex is SqlException || ex is EntityException)
            {
                title = Lang.Global_Title_DatabaseDown;
                message = LocalizationHelper.GetString(ex);
            }
            else
            {
                string translated = LocalizationHelper.GetString(ex.Message);

                if (translated != Lang.Global_ServiceError_Unknown)
                {
                    message = translated;
                    title = DetermineTitleFromKey(ex.Message);
                }

                if (ex is UnauthorizedAccessException)
                {
                    title = Lang.Global_Title_Error;
                    message = Lang.Global_Error_AccessDenied;
                }
            }

            return (title, message);
        }

        /// <summary>
        /// Determines the appropriate UI Title based on the server error key.
        /// </summary>
        private static string DetermineTitleFromKey(string key)
        {
            switch (key)
            {
                case LocalizationHelper.ServerKeys.ServiceErrorDatabase:
                case LocalizationHelper.ServerKeys.ErrorDatabase:
                case LocalizationHelper.ServerKeys.ErrorDatabaseError:
                    return Lang.Global_Title_DatabaseDown;

                case LocalizationHelper.ServerKeys.InvalidCredentials:
                case LocalizationHelper.ServerKeys.UserAlreadyLoggedIn:
                case LocalizationHelper.ServerKeys.AccountPenalized:
                case LocalizationHelper.ServerKeys.SessionExpired:
                case LocalizationHelper.ServerKeys.InvalidToken:
                    return Lang.Global_Title_LoginFailed;

                case LocalizationHelper.ServerKeys.EmailInUse:
                case LocalizationHelper.ServerKeys.UsernameInUse:
                case LocalizationHelper.ServerKeys.UserNotFound:
                case LocalizationHelper.ServerKeys.AlreadyFriends:
                case LocalizationHelper.ServerKeys.SelfAddFriend:
                    return Lang.Global_Title_Warning;

                default:
                    return Lang.Global_Title_Error;
            }
        }

        /// <summary>
        /// Writes the exception details to a local text file for debugging purposes.
        /// </summary>
        private static void LogException(Exception ex)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFileName);
                string logContent = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{ex.GetType().Name}]: {ex.Message}\n" +
                                    $"Stack Trace:\n{ex.StackTrace}\n" +
                                    $"Inner: {ex.InnerException?.Message ?? "None"}\n" +
                                    "--------------------------------------------------\n";

                lock (_logLock)
                {
                    File.AppendAllText(logPath, logContent);
                }

                Debug.WriteLine($"[EXCEPTION MANAGER] Logged: {ex.Message}");
            }
            catch (Exception logEx)
            {
                Debug.WriteLine($"[CRITICAL] Log failed: {logEx.Message}");
            }
        }

        /// <summary>
        /// Executes an asynchronous task safely. If an exception occurs, it handles the UI alert and logging.
        /// </summary>
        /// <param name="action">The async function to execute.</param>
        /// <param name="onFailed">Optional action to execute if the task fails.</param>
        /// <returns>True if the action succeeded; otherwise, false.</returns>
        public static async Task<bool> ExecuteSafeAsync(Func<Task> action, Action onFailed = null)
        {
            try
            {
                await action();
                return true;
            }
            catch (Exception ex)
            {
                Handle(ex);
                onFailed?.Invoke();
                return false;
            }
        }
    }
}