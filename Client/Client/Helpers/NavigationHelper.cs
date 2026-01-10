using Client.Core;
using Microsoft.Win32;
using System;
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
            
            Application.Current.MainWindow = nextWindow;
            nextWindow.Show();
            currentWindow.Close();
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

        public static void ExitApplication()
        {
            try
            {
                if (UserSession.IsGuest)
                {
                    UserServiceManager.Instance.Client.LogoutGuestAsync(UserSession.SessionToken);
                }
                UserSession.EndSession();
            }
            catch (Exception)
            {

            }
            finally
            {
                Application.Current.Shutdown();
            }
        }
    }
}