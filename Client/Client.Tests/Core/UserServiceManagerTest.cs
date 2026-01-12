using Client.Core;
using Client.UserServiceReference;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace Client.Test.Core
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

        #region Singleton Tests

        [Test]
        public void Instance_IsNotNull()
        {
            var instance = UserServiceManager.Instance;
            Assert.That(instance, Is.Not.Null);
        }

        [Test]
        public void Instance_AlwaysReturnsSameObject()
        {
            var instance1 = UserServiceManager.Instance;
            var instance2 = UserServiceManager.Instance;

            Assert.That(instance1, Is.SameAs(instance2));
        }

        #endregion

        #region Registration Tests

        // --- StartRegistrationAsync ---

        [Test]
        public async Task StartRegistrationAsync_ReturnsNotNull_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.StartRegistrationAsync("test@email.com", "pass");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task StartRegistrationAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.StartRegistrationAsync("test@email.com", "pass");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task StartRegistrationAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.StartRegistrationAsync("test@email.com", "pass");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- InitiateGuestRegistrationAsync ---

        [Test]
        public async Task InitiateGuestRegistrationAsync_ReturnsNotNull_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.InitiateGuestRegistrationAsync(1, "test@email.com", "pass");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task InitiateGuestRegistrationAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.InitiateGuestRegistrationAsync(1, "test@email.com", "pass");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task InitiateGuestRegistrationAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.InitiateGuestRegistrationAsync(1, "test@email.com", "pass");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- VerifyRegistrationAsync ---

        [Test]
        public async Task VerifyRegistrationAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.VerifyRegistrationAsync("test@email.com", "123456");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task VerifyRegistrationAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.VerifyRegistrationAsync("test@email.com", "123456");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- VerifyGuestRegistrationAsync ---

        [Test]
        public async Task VerifyGuestRegistrationAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.VerifyGuestRegistrationAsync(1, "test@email.com", "123456");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task VerifyGuestRegistrationAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.VerifyGuestRegistrationAsync(1, "test@email.com", "123456");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- ResendVerificationCodeAsync ---

        [Test]
        public async Task ResendVerificationCodeAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.ResendVerificationCodeAsync("test@email.com");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task ResendVerificationCodeAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.ResendVerificationCodeAsync("test@email.com");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- FinalizeRegistrationAsync ---

        [Test]
        public async Task FinalizeRegistrationAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.FinalizeRegistrationAsync("test@email.com", "user", new byte[0]);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task FinalizeRegistrationAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.FinalizeRegistrationAsync("test@email.com", "user", new byte[0]);
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        #endregion

        #region Session Tests

        // --- LoginAsGuestAsync ---

        [Test]
        public async Task LoginAsGuestAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.LoginAsGuestAsync("GuestUser");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task LoginAsGuestAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.LoginAsGuestAsync("GuestUser");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- LoginAsync ---

        [Test]
        public async Task LoginAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.LoginAsync("test@email.com", "pass");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task LoginAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.LoginAsync("test@email.com", "pass");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- LogoutAsync ---

        [Test]
        public void LogoutAsync_DoesNotCrash_WhenServerOffline()
        {
            Assert.DoesNotThrowAsync(async () => await UserServiceManager.Instance.LogoutAsync("token"));
        }

        #endregion

        #region Profile Tests

        // --- GetUserAvatarAsync ---

        [Test]
        public async Task GetUserAvatarAsync_ReturnsNull_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.GetUserAvatarAsync("test@email.com");
            Assert.That(result, Is.Null);
        }

        // --- UpdateUserAvatarAsync ---

        [Test]
        public async Task UpdateUserAvatarAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.UpdateUserAvatarAsync("token", new byte[0]);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task UpdateUserAvatarAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.UpdateUserAvatarAsync("token", new byte[0]);
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- ChangePasswordAsync ---

        [Test]
        public async Task ChangePasswordAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.ChangePasswordAsync("token", "old", "new");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task ChangePasswordAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.ChangePasswordAsync("token", "old", "new");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- ChangeUsernameAsync ---

        [Test]
        public async Task ChangeUsernameAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.ChangeUsernameAsync("token", "newUser");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task ChangeUsernameAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.ChangeUsernameAsync("token", "newUser");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- UpdatePersonalInfoAsync ---

        [Test]
        public async Task UpdatePersonalInfoAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.UpdatePersonalInfoAsync("email", "name", "last");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task UpdatePersonalInfoAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.UpdatePersonalInfoAsync("email", "name", "last");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        #endregion

        #region Social & History Tests

        // --- GetMatchHistoryAsync ---

        [Test]
        public async Task GetMatchHistoryAsync_ReturnsNotNull_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.GetMatchHistoryAsync("token");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetMatchHistoryAsync_ReturnsEmptyList_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.GetMatchHistoryAsync("token");
            Assert.That(result, Is.Empty);
        }

        // --- GetFriendsListAsync ---

        [Test]
        public async Task GetFriendsListAsync_ReturnsNotNull_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.GetFriendsListAsync("token");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetFriendsListAsync_ReturnsEmptyList_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.GetFriendsListAsync("token");
            Assert.That(result, Is.Empty);
        }

        // --- GetPendingRequestsAsync ---

        [Test]
        public async Task GetPendingRequestsAsync_ReturnsNotNull_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.GetPendingRequestsAsync("token");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetPendingRequestsAsync_ReturnsEmptyList_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.GetPendingRequestsAsync("token");
            Assert.That(result, Is.Empty);
        }

        // --- AddSocialNetworkAsync ---

        [Test]
        public async Task AddSocialNetworkAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.AddSocialNetworkAsync("token", "account");
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task AddSocialNetworkAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.AddSocialNetworkAsync("token", "account");
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        // --- RemoveSocialNetworkAsync ---

        [Test]
        public async Task RemoveSocialNetworkAsync_ReturnsFailure_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.RemoveSocialNetworkAsync("token", 1);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task RemoveSocialNetworkAsync_ReturnsCorrectMessage_WhenServerOffline()
        {
            var result = await UserServiceManager.Instance.RemoveSocialNetworkAsync("token", 1);
            Assert.That(result.MessageKey, Is.EqualTo("Global_Error_ServerOffline"));
        }

        #endregion
    }
}