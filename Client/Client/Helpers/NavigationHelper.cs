using Client.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Helpers
{
    public static class NavigationHelper
    {
        /// <summary>
        /// Navigates from the current window to the next window, transferring visual state 
        /// (size, position, window state) and safely closing all other windows.
        /// </summary>
        /// <param name="currentWindow">The source window (can be null if starting up).</param>
        /// <param name="nextWindow">The target window to navigate to.</param>
        public static void NavigateTo(Window currentWindow, Window nextWindow)
        {
            if (nextWindow == null) return;

            TransferWindowState(currentWindow, nextWindow);

            WindowHelper.ApplySavedSetting(nextWindow);

            try
            {
                nextWindow.Show();
            }
            catch (InvalidOperationException)
            {
                System.Diagnostics.Debug.WriteLine("[NavigationHelper] CRITICAL: Attempted to show a closed or invalid window. Navigation aborted.");
                return;
            }

            Application.Current.MainWindow = nextWindow;

            CloseOtherWindows(nextWindow);
        }

        /// <summary>
        /// Copies the dimensions and state (Maximized/Normal) from the source to the target window.
        /// </summary>
        private static void TransferWindowState(Window source, Window target)
        {
            if (source == null) return;

            try
            {
                target.WindowState = source.WindowState;

                if (source.WindowState == WindowState.Normal)
                {
                    target.WindowStartupLocation = WindowStartupLocation.Manual;
                    target.Width = source.Width;
                    target.Height = source.Height;
                    target.Top = source.Top;
                    target.Left = source.Left;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NavigationHelper] Visual state transfer warning: {ex.Message}");
            }
        }

        /// <summary>
        /// Closes all open windows in the application except the one specified.
        /// </summary>
        /// <param name="keepAliveWindow">The window that should remain open.</param>
        private static void CloseOtherWindows(Window keepAliveWindow)
        {
            var windowsToClose = new List<Window>();

            foreach (Window win in Application.Current.Windows)
            {
                if (win != keepAliveWindow)
                {
                    windowsToClose.Add(win);
                }
            }

            foreach (var win in windowsToClose)
            {
                try
                {
                    win.Close();
                }
                catch
                {

                }
            }
        }

        public static bool? ShowDialog(Window parentWindow, Window dialogWindow)
        {
            try
            {
                if (parentWindow != null)
                {
                    dialogWindow.Owner = parentWindow;
                    dialogWindow.WindowState = parentWindow.WindowState;
                }
                return dialogWindow.ShowDialog();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
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
                    var logoutTask = UserServiceManager.Instance.LogoutAsync(UserSession.SessionToken);
                    var delayTask = Task.Delay(2000);
                    await Task.WhenAny(logoutTask, delayTask);
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