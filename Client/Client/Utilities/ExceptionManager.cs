using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Windows;
using System.Windows.Threading;

namespace Client.Utilities
{
    public static class ExceptionManager
    {
        private static readonly object _logLock = new object();
        private const string LogFileName = "client_crash_log.txt";

        /// <summary>
        /// Centralized exception handling. Safely logs and shows the error on the UI thread.
        /// </summary>
        /// <param name="ex">The exception to handle.</param>
        /// <param name="owner">The owner window (optional). If null, attempts to use MainWindow.</param>
        /// <param name="onHandled">Action to execute after the user closes the alert.</param>
        /// <param name="isFatal">If true, shuts down the application after handling.</param>
        public static void Handle(Exception ex, Window owner = null, Action onHandled = null, bool isFatal = false)
        {
            if (ex == null)
            {
                return;
            }

            LogException(ex);

            var details = GetExceptionDetails(ex);
            string title = details.Title;
            string message = details.Message;

            if (isFatal)
            {
                message += $"\n\n{Lang.Global_Error_FatalCrash}";
            }

            if (Application.Current == null)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    if (owner == null && Application.Current.MainWindow != null && Application.Current.MainWindow.IsVisible)
                    {
                        owner = Application.Current.MainWindow;
                    }

                    var type = isFatal ? CustomMessageBox.MessageBoxType.Error : CustomMessageBox.MessageBoxType.Warning;

                    CustomMessageBox messageBox;
                    try
                    {
                        messageBox = new CustomMessageBox(title, message, owner, type);
                    }
                    catch
                    {
                        messageBox = new CustomMessageBox(title, message, null, type);
                    }

                    messageBox.ShowDialog();

                    onHandled?.Invoke();

                    if (isFatal)
                    {
                        UserSession.EndSession();
                        Application.Current.Shutdown();
                    }
                }
                catch (Exception dispatchEx)
                {
                    Debug.WriteLine($"Failed to show exception dialog: {dispatchEx.Message}");
                }
            });
        }

        private static (string Title, string Message) GetExceptionDetails(Exception ex)
        {
            string title = Lang.Global_Title_AppError;
            string message = LocalizationHelper.GetString(ex);

            if (ex is EndpointNotFoundException || ex is ServerTooBusyException)
            {
                title = Lang.Global_Title_ServerOffline;
                message = Lang.Global_Label_ConnectionFailed;
            }
            else if (ex is TimeoutException || ex is CommunicationException)
            {
                title = Lang.Global_Title_NetworkError;
                message = LocalizationHelper.GetString(ex);
            }
            else if (ex is FaultException faultEx)
            {
                title = Lang.Global_Title_Error;
                message = LocalizationHelper.GetString(faultEx.Message); 
            }
            else if (ex is UnauthorizedAccessException)
            {
                title = Lang.Global_Title_Error;
                message = Lang.Global_Error_AccessDenied;
            }

            return (title, message);
        }

        private static void LogException(Exception ex)
        {
            try
            {
                string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LogFileName);
                string logContent = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{ex.GetType().Name}]: {ex.Message}\n" +
                                    $"Stack Trace:\n{ex.StackTrace}\n" +
                                    $"Inner Exception: {ex.InnerException?.Message ?? "None"}\n" +
                                    "--------------------------------------------------\n";

                lock (_logLock)
                {
                    File.AppendAllText(logPath, logContent);
                }

                Debug.WriteLine($"[EXCEPTION MANAGER] Logged to {logPath}");
            }
            catch (Exception logEx)
            {
                Debug.WriteLine($"[CRITICAL] Failed to write log file: {logEx.Message}");
            }
        }
    }
}