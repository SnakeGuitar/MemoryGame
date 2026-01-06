using Client.Helpers;
using Client.Properties.Langs;
using Client.Views.Controls;
using System;
using System.Diagnostics;
using System.ServiceModel;
using System.Windows;

namespace Client.Utilities
{
    public static class ExceptionManager
    {
        public static void Handle(Exception ex, Window owner, Action onHandled = null)
        {
            LogException(ex);

            var details = GetExceptionDetails(ex);
            string title = details.Item1;
            string message = details.Item2;

            Application.Current.Dispatcher.Invoke(() =>
            {
                var messageBox = new CustomMessageBox(title, message, owner, CustomMessageBox.MessageBoxType.Error);
                messageBox.ShowDialog();

                if (onHandled != null)
                {
                    onHandled.Invoke();
                }
            });
        }

        private static (string, string) GetExceptionDetails(Exception ex)
        {
            string title = Lang.Global_Title_AppError;
            string message = LocalizationHelper.GetString(ex);

            if (ex is EndpointNotFoundException)
            {
                title = Lang.Global_Title_ServerOffline;
            }
            else if (ex is TimeoutException)
            {
                title = Lang.Global_Title_NetworkError;
            }
            else if (ex is CommunicationException)
            {
                title = Lang.Global_Title_NetworkError;
            }
            else if (ex is FaultException)
            {
                title = Lang.Global_Title_Error;
                message = ((FaultException)ex).Message;
            }
            else if (ex is UnauthorizedAccessException)
            {
                title = Lang.Global_Title_Error;
            }

            return (title, message);
        }

        private static void LogException(Exception ex)
        {
            Debug.WriteLine($"[EXCEPTION MANAGER] {ex.GetType().Name}: {ex.Message}");
            Debug.WriteLine(ex.StackTrace);
        }
    }
}