using Client.Helpers;
using Client.UserServiceReference;
using Client.Views.Session;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Core
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
            try
            {
                if (Client != null)
                {
                    try 
                    { 
                        Client.Close(); 
                    } 
                    catch 
                    { 
                        Client.Abort(); 
                    }
                }

                InstanceContext context = new InstanceContext(this);
                Client = new UserServiceClient(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UserServiceManager] Init Failed: {ex.Message}");
            }
        }

        #region Wrappers

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            if (EnsureConnection())
            {
                try
                {
                    return await Client.LoginAsync(email, password);
                }
                catch (Exception)
                {
                    throw;
                }
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
            if (Client == null || 
                Client.State == CommunicationState.Closed || 
                Client.State == CommunicationState.Faulted)
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
                if (Application.Current.MainWindow is Login)
                {
                    return;
                }

                MessageBox.Show(
                    $"You have been disconnected: {reason}",
                    "Session Ended",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

                UserSession.EndSession();

                if (Application.Current.MainWindow != null)
                {
                    NavigationHelper.NavigateTo(Application.Current.MainWindow, new Login());
                }
                else
                {
                    // Manual window opening or fallback reasons
                    var login = new Login();
                    Application.Current.MainWindow = login;
                    login.Show();
                }
            });
        }

        #endregion
    }
}