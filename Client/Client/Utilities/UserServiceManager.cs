using Client.Helpers;
using Client.UserServiceReference;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Utilities
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class UserServiceManager : IUserServiceCallback
    {
        #region Singleton
        private static UserServiceManager _instance;
        public static UserServiceManager Instance => _instance ?? (_instance = new UserServiceManager());
        #endregion

        public UserServiceClient Client { get; private set; }

        private UserServiceManager()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            InstanceContext context = new InstanceContext(this);
            Client = new UserServiceClient(context);
        }

        #region Wrappers

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            if (EnsureConnection())
            {
                return await Client.LoginAsync(email, password);
            }
            return new LoginResponse { Success = false, MessageKey = "Global_Error_ConnectionLost" };
        }

        public async Task<LoginResponse> RenewSessionAsync(string token)
        {
            if (EnsureConnection())
            {
                return await Client.RenewSessionAsync(token);
            }
            return new LoginResponse { Success = false };
        }

        private bool EnsureConnection()
        {
            if (Client == null || Client.State == CommunicationState.Closed || Client.State == CommunicationState.Faulted)
            {
                InitializeClient();
            }
            return Client.State == CommunicationState.Opened || Client.State == CommunicationState.Created;
        }

        #endregion

        #region IUserServiceCallback Implementation

        public void ForceLogout(string reason)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (Application.Current.MainWindow is Views.Session.Login)
                {
                    return;
                }

                MessageBox.Show(
                    $"You have been disconnected: {reason}",
                    "Session Ended",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

                UserSession.EndSession();

                var loginWindow = new Views.Session.Login();
                loginWindow.Show();
                Application.Current.MainWindow = loginWindow;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window != loginWindow)
                    {
                        window.Close();
                    }
                }
            });
        }

        #endregion
    }
}