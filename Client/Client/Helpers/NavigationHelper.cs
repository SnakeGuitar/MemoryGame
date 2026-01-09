using Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Helpers
{
    public static class NavigationHelper
    {
        public static void NavigateTo(Window currentWindow, Window nextWindow)
        {
            if (nextWindow == null)
            {
                return;
            }

            nextWindow.WindowState = currentWindow.WindowState;
            nextWindow.Width = currentWindow.Width;
            nextWindow.Height = currentWindow.Height;
            nextWindow.Top = currentWindow.Top;
            nextWindow.Left = currentWindow.Left;
            nextWindow.Owner = currentWindow.Owner;
            
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

        public static void ExitApplication()
        {
            UserSession.EndSession();
            Application.Current.Shutdown();
        }
    }
}
