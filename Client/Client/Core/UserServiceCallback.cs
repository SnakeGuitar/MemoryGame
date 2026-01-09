using Client.UserServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Core
{
    public class UserServiceCallback : IUserServiceCallback
    {
        public void ForceLogout(string reason)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    $"You have been disconnected: {reason}",
                    "Session Ended",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                
                UserSession.EndSession();

                var loginWindow = new Views.Session.Login();
                loginWindow.Show();

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != loginWindow)
                    {
                        window.Close();
                    }
                }
            });
        }
    }
}
