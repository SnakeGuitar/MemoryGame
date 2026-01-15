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

        private UserServiceManager()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            Client = WcfClientFactory.CreateClient<UserServiceClient, IUserService>
                (Client, this, context => new UserServiceClient(context));
        }

        private void EnsureConnection()
        {
            if (!WcfClientFactory.IsConnectionActive(Client))
            {
                InitializeClient();
            }
        }

        #region Wrappers

        public async Task<ResponseDTO> StartRegistrationAsync(string email, string password)
        {
            EnsureConnection();
            return await Client.StartRegistrationAsync(email, password);
        }

        public async Task<ResponseDTO> InitiateGuestRegistrationAsync(int userId, string email, string password)
        {
            EnsureConnection();
            return await Client.InitiateGuestRegistrationAsync(userId, email, password);
        }

        public async Task<ResponseDTO> VerifyRegistrationAsync(string email, string code)
        {
            EnsureConnection();
            return await Client.VerifyRegistrationAsync(email, code);
        }

        public async Task<ResponseDTO> VerifyGuestRegistrationAsync(int userId, string email, string code)
        {
            EnsureConnection();
            return await Client.VerifyGuestRegistrationAsync(userId, email, code);
        }

        public async Task<ResponseDTO> ResendVerificationCodeAsync(string email)
        {
            EnsureConnection();
            return await Client.ResendVerificationCodeAsync(email);
        }

        public async Task<LoginResponse> FinalizeRegistrationAsync(string email, string username, byte[] avatar)
        {
            EnsureConnection();
            return await Client.FinalizeRegistrationAsync(email, username, avatar);
        }

        public async Task<LoginResponse> LoginAsGuestAsync(string username)
        {
            EnsureConnection();
            return await Client.LoginAsGuestAsync(username);
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            EnsureConnection();
            return await Client.LoginAsync(email, password);
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
                        if (UserSession.IsGuest)
                        {
                            Client.LogoutGuest(token);
                        }
                        else
                        {
                            Client.Logout(token);
                        }
                    }
                    catch
                    {

                    }
                });
            }
            catch
            {

            }
        }

        public async Task<byte[]> GetUserAvatarAsync(string email)
        {
            EnsureConnection();
            return await Client.GetUserAvatarAsync(email);
        }

        public async Task<ResponseDTO> UpdateUserAvatarAsync(string token, byte[] avatar)
        {
            EnsureConnection();
            return await Client.UpdateUserAvatarAsync(token, avatar);
        }

        public async Task<ResponseDTO> ChangePasswordAsync(string token, string currentPass, string newPass)
        {
            EnsureConnection();
            return await Client.ChangePasswordAsync(token, currentPass, newPass);
        }

        public async Task<ResponseDTO> ChangeUsernameAsync(string token, string newUsername)
        {
            EnsureConnection();
            return await Client.ChangeUsernameAsync(token, newUsername);
        }

        public async Task<List<MatchHistoryDTO>> GetMatchHistoryAsync(string token)
        {
            EnsureConnection();
            var resultArray = await Client.GetMatchHistoryAsync(token);
            return resultArray.ToList();
        }

        public async Task<List<FriendDTO>> GetFriendsListAsync(string token)
        {
            EnsureConnection();
            var result = await Client.GetFriendsListAsync(token);
            return result.ToList();
        }

        public async Task<List<FriendRequestDTO>> GetPendingRequestsAsync(string token)
        {
            EnsureConnection();
            var result = await Client.GetPendingRequestsAsync(token);
            return result.ToList();
        }

        public async Task<ResponseDTO> UpdatePersonalInfoAsync(string email, string name, string lastName)
        {
            EnsureConnection();
            return await Client.UpdatePersonalInfoAsync(email, name, lastName);
        }

        public async Task<LoginResponse> RenewSessionAsync(string token)
        {
            EnsureConnection();
            return await Client.RenewSessionAsync(token);
        }

        public async Task<AddSocialNetworkResponse> AddSocialNetworkAsync(string token, string account)
        {
            EnsureConnection();
            return await Client.AddSocialNetworkAsync(token, account);
        }

        public async Task<ResponseDTO> RemoveSocialNetworkAsync(string token, int socialId)
        {
            EnsureConnection();
            return await Client.RemoveSocialNetworkAsync(token, socialId);
        }

        #endregion

        #region IUserServiceCallback Implementation

        public void ForceLogout(string reason)
        {
            if (Application.Current == null) return;

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

                    new Views.Controls.CustomMessageBox(
                        Lang.Global_Title_Error,
                        Lang.Global_Error_SessionExpired,
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