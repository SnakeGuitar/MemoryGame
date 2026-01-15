using Client.Helpers;
using Client.Properties.Langs;
using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Core.Exceptions
{
    public static class ExceptionManager
    {
        private static readonly object _logLock = new object();
        private const string LogFileName = "client_crash_log.txt";
        private static bool _isInteractingWithUser = false;

        public static async Task<bool> ExecuteNetworkCallAsync(
            Func<Task> action,
            Window owner,
            NetworkFailPolicy policy = NetworkFailPolicy.AskToRetryOrExit)
        {
            if (_isInteractingWithUser)
            {
                return false;
            }

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
                    if (_isInteractingWithUser)
                    {
                        return false;
                    }

                    if (IsTransientNetworkError(ex))
                    {
                        retryCount++;
                        if (retryCount <= maxSilentRetries)
                        {
                            Debug.WriteLine($"[Network] Retry {retryCount}/{maxSilentRetries}...");
                            await Task.Delay(500 * retryCount);
                            continue;
                        }
                    }

                    var errorInfo = GetDistinguishedErrorMessage(ex);
                    bool isConnectionError = IsTransientNetworkError(ex) || IsCriticalServerError(ex);

                    if (!isConnectionError)
                    {
                        Handle(ex, owner);
                        return false;
                    }

                    switch (policy)
                    {
                        case NetworkFailPolicy.ShowWarningOnly:
                            Handle(ex, owner);
                            return false;

                        case NetworkFailPolicy.AskToRetryOrExit:
                            bool retry = AskUserWithLock(errorInfo.Title, errorInfo.Message, owner);
                            if (retry)
                            {
                                retryCount = 0;
                                await Task.Delay(1000);
                                continue;
                            }
                            else
                            {
                                DialogManager.ForceNavigateToTitle(owner);
                                return false;
                            }

                        case NetworkFailPolicy.CriticalExit:
                            ShowErrorWithLock(errorInfo.Title, errorInfo.Message, owner);
                            DialogManager.ForceNavigateToTitle(owner);
                            return false;
                    }
                }
            }
        }

        public static void Handle(Exception ex, Window owner = null, Action onHandled = null)
        {
            if (ex == null || ex is TaskCanceledException)
            {
                return;
            }
            LogException(ex);

            var info = GetDistinguishedErrorMessage(ex);
            DialogManager.ShowWarning(info.Title, info.Message, owner);
            onHandled?.Invoke();
        }

        private static bool AskUserWithLock(string title, string msg, Window owner)
        {
            if (_isInteractingWithUser)
            {
                return false;
            }
            _isInteractingWithUser = true;
            try
            {
                return DialogManager.AskToRetry(title, msg, owner);
            }
            finally
            {
                _isInteractingWithUser = false;
            }
        }

        private static void ShowErrorWithLock(string title, string msg, Window owner)
        {
            if (_isInteractingWithUser)
            {
                return;
            }
            _isInteractingWithUser = true;
            try
            {
                DialogManager.ShowError(title, msg, owner);
            }
            finally
            {
                _isInteractingWithUser = false;
            }
        }

        private static bool IsTransientNetworkError(Exception ex)
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
                   msg.Contains("SqlException") ||
                   msg.Contains("provider failed");
        }

        private static (string Title, string Message) GetDistinguishedErrorMessage(Exception ex)
        {
            if (IsCriticalServerError(ex))
            {
                return (Lang.Global_Title_DatabaseDown, Lang.Global_Error_DatabaseCritical);
            }
            if (ex is EndpointNotFoundException)
            {
                return (Lang.Global_Title_ServerOffline, Lang.Global_ServiceError_NetworkDown);
            }
            if (ex is TimeoutException)
            {
                return (Lang.Global_Title_NetworkError, Lang.Global_Error_Timeout);
            }
            if (ex is CommunicationException)
            {
                return (Lang.Global_Title_NetworkError, Lang.Global_Error_ConnectionLost);
            }
            if (ex is FaultException faultEx)
            {
                string translated = LocalizationHelper.GetString(faultEx.Message);
                if (translated == faultEx.Message)
                {
                    return (Lang.Global_Title_Warning, Lang.Global_Error_GenericValidation);
                }
                return (Lang.Global_Title_Warning, translated);
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
            catch { }
        }
    }
}