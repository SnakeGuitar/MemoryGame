using Client.Core;
using NUnit.Framework;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Client.Tests.Core
{
    [TestFixture]
    public class UserServiceManagerTests
    {
        [TearDown]
        public void ResetSingleton()
        {
            var field = typeof(UserServiceManager).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
            field?.SetValue(null, null);
        }

        [Test]
        public void Instance_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var instance = UserServiceManager.Instance;
            });
        }

        [Test]
        public void StartRegistrationAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.StartRegistrationAsync("email", "pass"));
        }

        [Test]
        public void LoginAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.LoginAsync("user", "pass"));
        }

        [Test]
        public void LoginAsGuestAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.LoginAsGuestAsync("guest"));
        }

        [Test]
        public void LogoutAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.LogoutAsync("token"));
        }

        [Test]
        public void GetMatchHistoryAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.GetMatchHistoryAsync("token"));
        }

        [Test]
        public void GetFriendsListAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.GetFriendsListAsync("token"));
        }

        [Test]
        public void GetPendingRequestsAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.GetPendingRequestsAsync("token"));
        }

        [Test]
        public void VerifyRegistrationAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.VerifyRegistrationAsync("email", "code"));
        }

        [Test]
        public void ResendVerificationCodeAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.ResendVerificationCodeAsync("email"));
        }

        [Test]
        public void RecoverPasswordAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.ChangePasswordAsync("token", "currentPassword", "newPassword"));
        }

        [Test]
        public void ChangePasswordAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.ChangePasswordAsync("token", "old", "new"));
        }

        [Test]
        public void UpdateUserProfileInfo_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.UpdatePersonalInfoAsync("token", "name", "lastname"));
        }

        [Test]
        public void AddSocialNetworkAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.AddSocialNetworkAsync("token", "account"));
        }

        [Test]
        public void RemoveSocialNetworkAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.RemoveSocialNetworkAsync("token", 1));
        }

        [Test]
        public void ForceLogout_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                UserServiceManager.Instance.ForceLogout("reason"));
        }

        [Test]
        public void GetMatchsAsync_ServerOffline_ThrowsInvalidOperationException()
        {
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await UserServiceManager.Instance.ChangePasswordAsync("token", "currentPass", "newPass"));
        }
    }
}