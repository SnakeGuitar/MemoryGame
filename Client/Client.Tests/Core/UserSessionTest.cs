using NUnit.Framework;
using System;
using Client.UserServiceReference;
using System.Collections.Generic;
using Client.Core;

namespace Client.Test.Core
{
    [TestFixture]
    public class UserSessionTests
    {
        [TearDown]
        public void LimpiarEstado()
        {
            UserSession.EndSession();

            UserSession.Name = string.Empty;
            UserSession.LastName = string.Empty;
            UserSession.SocialNetworks = new List<SocialNetworkDTO>();
        }

        #region StartSession Valid User Tests

        [Test]
        public void StartSession_SetsSessionToken()
        {
            string token = "token_abc_123";
            var userDto = CreateValidUserDTO();

            UserSession.StartSession(token, userDto);

            Assert.That(UserSession.SessionToken, Is.EqualTo(token));
        }

        [Test]
        public void StartSession_SetsUserId()
        {
            var userDto = CreateValidUserDTO();
            userDto.UserId = 99;

            UserSession.StartSession("token", userDto);

            Assert.That(UserSession.UserId, Is.EqualTo(99));
        }

        [Test]
        public void StartSession_SetsUsername()
        {
            var userDto = CreateValidUserDTO();
            userDto.Username = "tester";

            UserSession.StartSession("token", userDto);

            Assert.That(UserSession.Username, Is.EqualTo("tester"));
        }

        [Test]
        public void StartSession_SetsIsGuest()
        {
            var userDto = CreateValidUserDTO();
            userDto.IsGuest = true;

            UserSession.StartSession("token", userDto);

            Assert.That(UserSession.IsGuest, Is.True);
        }

        [Test]
        public void StartSession_SetsEmail()
        {
            var userDto = CreateValidUserDTO();
            userDto.Email = "test@email.com";

            UserSession.StartSession("token", userDto);

            Assert.That(UserSession.Email, Is.EqualTo("test@email.com"));
        }

        [Test]
        public void StartSession_SetsName()
        {
            var userDto = CreateValidUserDTO();
            userDto.Name = "Juan";

            UserSession.StartSession("token", userDto);

            Assert.That(UserSession.Name, Is.EqualTo("Juan"));
        }

        [Test]
        public void StartSession_SetsSocialNetworksCount()
        {
            var userDto = CreateValidUserDTO();
            userDto.SocialNetworks = new[] { new SocialNetworkDTO() };

            UserSession.StartSession("token", userDto);

            Assert.That(UserSession.SocialNetworks.Count, Is.EqualTo(1));
        }

        #endregion

        #region StartSession Null/Edge Cases

        [Test]
        public void StartSession_UserIsNull_ThrowsArgumentNullException()
        {
            string token = "token_test";
            UserDTO user = null;

            Assert.Throws<ArgumentNullException>(() => UserSession.StartSession(token, user));
        }

        [Test]
        public void StartSession_SocialNetworksNull_InitializesListNotNull()
        {
            var userDto = new UserDTO { SocialNetworks = null };

            UserSession.StartSession("token", userDto);

            Assert.That(UserSession.SocialNetworks, Is.Not.Null);
        }

        [Test]
        public void StartSession_SocialNetworksNull_InitializesListEmpty()
        {
            var userDto = new UserDTO { SocialNetworks = null };

            UserSession.StartSession("token", userDto);

            Assert.That(UserSession.SocialNetworks, Is.Empty);
        }

        #endregion

        #region EndSession Tests

        [Test]
        public void EndSession_ClearsSessionToken()
        {
            UserSession.StartSession("token_sucio", CreateValidUserDTO());

            UserSession.EndSession();

            Assert.That(UserSession.SessionToken, Is.Null);
        }

        [Test]
        public void EndSession_ClearsUserId()
        {
            var user = CreateValidUserDTO();
            user.UserId = 55;
            UserSession.StartSession("token", user);

            UserSession.EndSession();

            Assert.That(UserSession.UserId, Is.EqualTo(0));
        }

        [Test]
        public void EndSession_ClearsUsername()
        {
            var user = CreateValidUserDTO();
            user.Username = "sucio";
            UserSession.StartSession("token", user);

            UserSession.EndSession();

            Assert.That(UserSession.Username, Is.Null);
        }

        [Test]
        public void EndSession_ClearsIsGuest()
        {
            var user = CreateValidUserDTO();
            user.IsGuest = true;
            UserSession.StartSession("token", user);

            UserSession.EndSession();

            Assert.That(UserSession.IsGuest, Is.False);
        }

        #endregion

        #region Event Tests

        [Test]
        public void OnProfileUpdated_InvokesEvent()
        {
            bool eventWasRaised = false;
            Action handler = () => eventWasRaised = true;
            UserSession.ProfileUpdated += handler;

            try
            {
                UserSession.OnProfileUpdated();
                Assert.That(eventWasRaised, Is.True, "El evento ProfileUpdated debería haberse disparado.");
            }
            finally
            {
                UserSession.ProfileUpdated -= handler;
            }
        }

        #endregion

        private UserDTO CreateValidUserDTO()
        {
            return new UserDTO
            {
                UserId = 1,
                Username = "default",
                IsGuest = false,
                Email = "default@test.com",
                Name = "Default",
                LastName = "User",
                RegistrationDate = new DateTime(2023, 1, 1),
                SocialNetworks = new SocialNetworkDTO[0]
            };
        }
    }
}