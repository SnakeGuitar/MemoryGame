using Client.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Helpers
{
    public static class WindowHelper
    {
        public static void SetWindowMode(Window window, bool isFullscreen)
        {
            if (window == null)
            {
                return;
            }

            if (window is CustomMessageBox || window is InviteFriendDialog)
            {
                return;
            }

            if (isFullscreen)
            {
                if (window.WindowState == WindowState.Maximized && window.WindowStyle == WindowStyle.None)
                    return;

                window.WindowStyle = WindowStyle.None;
                window.ResizeMode = ResizeMode.NoResize;
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                if (window.AllowsTransparency)
                {
                    window.WindowStyle = WindowStyle.None;
                }
                else
                {
                    window.WindowStyle = WindowStyle.SingleBorderWindow;
                }

                window.ResizeMode = ResizeMode.CanResize;
                window.WindowState = WindowState.Normal;
            }
        }

        public static void ApplySavedSetting(Window window)
        {
            bool isFull = Properties.Settings.Default.IsFullscreen;
            SetWindowMode(window, isFull);
        }
    }
}