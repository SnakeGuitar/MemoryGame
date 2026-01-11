using Client.Helpers;
using Client.Properties.Langs;
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
        private ServerConnectionMonitor _connectionMonitor;

        public event Action ServerConnectionLost;

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
                    try { Client.Close(); } catch { Client.Abort(); }
                }
                _connectionMonitor?.Stop();

                InstanceContext context = new InstanceContext(this);
                Client = new UserServiceClient(context);
                Client.Open();

                _connectionMonitor = new ServerConnectionMonitor(async () =>
                {
                    try
                    {
                        if (Client == null ||
                            Client.State == CommunicationState.Closed ||
                            Client.State == CommunicationState.Faulted)
                        {
                            return false;
                        }

                        await Client.PingAsync();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                });

                _connectionMonitor.ConnectionLost += () =>
                {
                    Application.Current?.Dispatcher?.Invoke(() =>
                    {
                        ServerConnectionLost?.Invoke();
                    });
                };

                _connectionMonitor.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UserServiceManager] Init Failed: {ex.Message}");
                Application.Current?.Dispatcher?.Invoke(() => ServerConnectionLost?.Invoke());
            }
        }

        #region Wrappers

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            if (Client == null || Client.State == CommunicationState.Faulted || Client.State == CommunicationState.Closed)
            {
                InitializeClient();
            }

            if (Client.State != CommunicationState.Opened)
            {
                return new LoginResponse
                {
                    Success = false,
                    MessageKey = "Global_Error_ServerOffline"
                };
            }

            try
            {
                return await Client.LoginAsync(email, password);
            }
            catch (Exception)
            {
                return new LoginResponse
                {
                    Success = false,
                    MessageKey = "Global_Error_ServerOffline"
                };
            }
        }

        public async Task LogoutAsync(string token)
        {
            if (EnsureConnection())
            {
                try
                {
                    await Task.Run(() =>
                    {
                        if (UserSession.IsGuest)
                        {
                            Client.LogoutGuest(token);
                        }
                        else
                        {
                            Client.Logout(token);
                        }
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Logout Error] {ex.Message}");
                }
            }
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
            if (Application.Current == null)
            {
                return;
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    var loginWindow = new Login();
                    var oldWindow = Application.Current.MainWindow;

                    Application.Current.MainWindow = loginWindow;
                    loginWindow.Show();

                    if (oldWindow != null && oldWindow != loginWindow)
                    {
                        oldWindow.Close();
                    }

                    UserSession.EndSession();

                    reason = Lang.Global_Error_SessionExpired;

                    new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Error,
                        reason,
                        loginWindow,
                        Views.Controls.CustomMessageBox.MessageBoxType.Error
                    ).ShowDialog();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[ForceLogout] Error: {ex.Message}");
                }
            });
        }

        #endregion
    }
}