using Client.Helpers;
using Client.Properties.Langs;
using Client.UserServiceReference;
using Client.Views.Session;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    try 
                    { 
                        Client.Close(); 
                    } 
                    catch 
                    { 
                        Client.Abort(); 
                    }
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

                        var pingTask = Client.PingAsync();
                        var timeoutTask = Task.Delay(4000);
                        var completedTask = await Task.WhenAny(pingTask, timeoutTask);

                        if (completedTask == timeoutTask)
                        {
                            return false;
                        }

                        await pingTask;
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UserServiceManager] Init Failed: {ex.Message}");
                Application.Current?.Dispatcher?.Invoke(() => ServerConnectionLost?.Invoke());
            }
        }

        #region Wrappers

        public async Task<ResponseDTO> StartRegistrationAsync(string email, string password)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.StartRegistrationAsync(email, password);
                }
                catch (Exception) 
                { 
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" }; 
                }
                finally { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<ResponseDTO> InitiateGuestRegistrationAsync(int userId, string email, string password)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.InitiateGuestRegistrationAsync(userId, email, password);
                }
                catch (Exception) 
                { 
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" }; 
                }
                finally 
                {
                    _connectionMonitor?.Start(); 
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<ResponseDTO> VerifyRegistrationAsync(string email, string code)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.VerifyRegistrationAsync(email, code);
                }
                catch (Exception) 
                { 
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" }; 
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<ResponseDTO> VerifyGuestRegistrationAsync(int userId, string email, string code)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.VerifyGuestRegistrationAsync(userId, email, code);
                }
                catch (Exception) 
                { 
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" }; 
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<ResponseDTO> ResendVerificationCodeAsync(string email)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.ResendVerificationCodeAsync(email);
                }
                catch (Exception) 
                { 
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<LoginResponse> FinalizeRegistrationAsync(string email, string username, byte[] avatar)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    var result = await Client.FinalizeRegistrationAsync(email, username, avatar);

                    if (result.Success)
                    {
                        _connectionMonitor?.Start();
                    }
                    return result;
                }
                catch (Exception) { return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" }; }
            }
            return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<LoginResponse> LoginAsGuestAsync(string username)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();

                try
                {
                    var result = await Client.LoginAsGuestAsync(username);

                    if (result.Success)
                    {
                        _connectionMonitor?.Start();
                    }

                    return result;
                }
                catch (Exception)
                {
                    return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
                }
            }
            return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            if (Client == null || Client.State != CommunicationState.Opened)
            {
                InitializeClient();
            }

            try
            {
                _connectionMonitor?.Stop();

                var pingTask = Client.PingAsync();
                var timeoutTask = Task.Delay(4000);
                if (await Task.WhenAny(pingTask, timeoutTask) == timeoutTask)
                {
                    return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
                }
                await pingTask;
            }
            catch
            {
                return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
            }

            try
            {
                var result = await Client.LoginAsync(email, password);

                if (result.Success)
                {
                    _connectionMonitor?.Start();
                }
                else
                {
                    _connectionMonitor?.Stop();
                }

                return result;
            }
            catch (Exception)
            {
                return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
            }
        }

        public async Task LogoutAsync(string token)
        {
            if (Client == null || Client.State != CommunicationState.Opened)
            {
                return;
            }

            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        if (UserSession.IsGuest) Client.LogoutGuest(token);
                        else Client.Logout(token);
                    }
                    catch 
                    {
                        // Intentional: Fire-and-forget strategy. 
                        // We suppress exceptions here to ensure the local logout process 
                        // completes even if the server is unreachable.
                    }
                });
            }
            catch
            {
                // Intentional: Suppress any task scheduling errors during logout.
            }
        }

        public async Task<byte[]> GetUserAvatarAsync(string email)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.GetUserAvatarAsync(email);
                }
                catch (Exception)
                {
                    return null;
                }
                finally
                {
                    _connectionMonitor?.Start();
                }
            }
            return null;
        }

        public async Task<ResponseDTO> UpdateUserAvatarAsync(string token, byte[] avatar)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.UpdateUserAvatarAsync(token, avatar);
                }
                catch (Exception) 
                { 
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" }; 
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<ResponseDTO> ChangePasswordAsync(string token, string currentPass, string newPass)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.ChangePasswordAsync(token, currentPass, newPass);
                }
                catch (Exception) 
                { 
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" }; 
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<ResponseDTO> ChangeUsernameAsync(string token, string newUsername)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.ChangeUsernameAsync(token, newUsername);
                }
                catch (Exception) { return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" }; }
                finally { _connectionMonitor?.Start(); }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<List<MatchHistoryDTO>> GetMatchHistoryAsync(string token)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    var resultArray = await Client.GetMatchHistoryAsync(token);
                    return resultArray.ToList();
                }
                catch (Exception)
                {
                    return new List<MatchHistoryDTO>();
                }
                finally
                {
                    _connectionMonitor?.Start();
                }
            }
            return new List<MatchHistoryDTO>();
        }

        public async Task<List<FriendDTO>> GetFriendsListAsync(string token)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    var result = await Client.GetFriendsListAsync(token);
                    return result.ToList();
                }
                catch (Exception) 
                { 
                    return new List<FriendDTO>(); 
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new List<FriendDTO>();
        }

        public async Task<List<FriendRequestDTO>> GetPendingRequestsAsync(string token)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    var result = await Client.GetPendingRequestsAsync(token);
                    return result.ToList();
                }
                catch (Exception) 
                { 
                    return new List<FriendRequestDTO>(); 
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new List<FriendRequestDTO>();
        }

        public async Task<ResponseDTO> UpdatePersonalInfoAsync(string email, string name, string lastName)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.UpdatePersonalInfoAsync(email, name, lastName);
                }
                catch (Exception)
                {
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
                }
                finally
                {
                    _connectionMonitor?.Start();
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<LoginResponse> RenewSessionAsync(string token)
        {
            if (EnsureConnection())
            {
                try
                {
                    return await Client.RenewSessionAsync(token);
                }
                catch (Exception)
                {
                    return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
                }
            }
            return new LoginResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        private bool EnsureConnection()
        {
            if (Client == null || 
                Client.State == CommunicationState.Closed || 
                Client.State == CommunicationState.Faulted)
            {
                InitializeClient();
            }

            if (Client == null)
            {
                return false;
            }

            return Client.State == CommunicationState.Opened || Client.State == CommunicationState.Created;
        }

        public async Task<AddSocialNetworkResponse> AddSocialNetworkAsync(string token, string account)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.AddSocialNetworkAsync(token, account);
                }
                catch (Exception)
                {
                    return new AddSocialNetworkResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new AddSocialNetworkResponse { Success = false, MessageKey = "Global_Error_ServerOffline" };
        }

        public async Task<ResponseDTO> RemoveSocialNetworkAsync(string token, int socialId)
        {
            if (EnsureConnection())
            {
                _connectionMonitor?.Stop();
                try
                {
                    return await Client.RemoveSocialNetworkAsync(token, socialId);
                }
                catch (Exception) 
                { 
                    return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" }; 
                }
                finally 
                { 
                    _connectionMonitor?.Start(); 
                }
            }
            return new ResponseDTO { Success = false, MessageKey = "Global_Error_ServerOffline" };
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