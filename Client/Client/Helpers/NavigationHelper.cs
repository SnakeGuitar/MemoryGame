using Client.Core;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Helpers
{
    public static class NavigationHelper
    {
        public static void NavigateTo(Window currentWindow, Window nextWindow)
        {
            if (currentWindow == null || nextWindow == null)
            {
                return;
            }
            if (currentWindow.WindowState == WindowState.Maximized)
            {
                nextWindow.WindowState = WindowState.Maximized;
            }
            else if (currentWindow.WindowState == WindowState.Normal)
            {
                nextWindow.WindowState = currentWindow.WindowState;
                nextWindow.Width = currentWindow.Width;
                nextWindow.Height = currentWindow.Height;
                nextWindow.Top = currentWindow.Top;
                nextWindow.Left = currentWindow.Left;
            }

            WindowHelper.ApplySavedSetting(nextWindow);
            Application.Current.MainWindow = nextWindow;
            nextWindow.Show();
            var windowsToClose = Application.Current.Windows
                .Cast<Window>()
                .Where(window => window != nextWindow)
                .ToList();

            foreach (var window in windowsToClose)
            {
                try
                {
                    window.Close();
                }
                catch
                {
                }
            }
        }

        public static bool? ShowDialog(Window parentWindow, Window dialogWindow)
        {
            dialogWindow.Owner = parentWindow;
            dialogWindow.WindowState = parentWindow.WindowState;
            return dialogWindow.ShowDialog();
        }

        public static OpenFileDialog GetOpenFileDialog(string title, string filter, bool isMultiSelect)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = title;
            dialog.Filter = filter;
            dialog.Multiselect = isMultiSelect;
            return dialog;
        }

        public static async void ExitApplication()
        {
            try
            {
                if (!string.IsNullOrEmpty(UserSession.SessionToken))
                {
                    await UserServiceManager.Instance.LogoutAsync(UserSession.SessionToken);
                }
                UserSession.EndSession();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Exit Error]: {ex.Message}");
            }
            finally
            {
                Application.Current.Shutdown();
            }
        }
    }
}