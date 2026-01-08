using Server.SessionService.Core;
using Server.Shared;
using Server.Validator;
using System;
using System.Collections.Generic;

namespace Server.SessionService
{
    /// <summary>
    /// Implements the user service that orchestrates authentication, user profile, and social logic.
    /// Acts as a facade between presentation layers and core business components 
    /// (<see cref="AuthenticationCore"/>, <see cref="UserProfileCore"/>, and <see cref="SocialCore"/>).
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AuthenticationCore _authenticationCore;
        private readonly UserProfileCore _userProfileCore;
        private readonly SocialCore _socialCore;
        private readonly StatisticsCore _statisticsCore;
        private readonly PenaltyCore _penaltyCore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// Instantiates and configures essential system components including database context factory,
        /// security, session, notification, and logging services.
        /// </summary>
        public UserService()
        {
            var dbFactory = new DbContextFactory();
            var security = new SecurityService();
            var session = new SessionManager();
            var notification = new NotificationService();

            _authenticationCore = new AuthenticationCore(dbFactory, security, session, notification, new Logger(typeof(AuthenticationCore)));
            _userProfileCore = new UserProfileCore(dbFactory, new Logger(typeof(UserProfileCore)), session, security);
            _socialCore = new SocialCore(dbFactory, session, new Logger(typeof(SocialCore)));
            _statisticsCore = new StatisticsCore(dbFactory, session, new Logger(typeof(StatisticsCore)));
            _penaltyCore = new PenaltyCore(dbFactory, session, new Logger(typeof(PenaltyCore)));
        }

        // === Authentication ===

        /// <summary>
        /// Initiates the user registration process using an email address and password.
        /// A verification code is generated and sent to the provided email.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The password provided by the user.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating the result of the operation.</returns>
        public ResponseDTO StartRegistration(string email, string password)
        {
            return _authenticationCore.StartRegistration(email, password);
        }

        /// <summary>
        /// Verifies a user's registration using the one-time PIN previously sent to their email.
        /// </summary>
        /// <param name="email">The email address associated with the registration.</param>
        /// <param name="pin">The one-time verification code.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating whether verification succeeded.</returns>
        public ResponseDTO VerifyRegistration(string email, string pin)
        {
            return _authenticationCore.VerifyRegistration(email, pin);
        }

        /// <summary>
        /// Resends the verification code to the specified email address.
        /// </summary>
        /// <param name="email">The user's email address requesting retransmission.</param>
        /// <returns>A <see cref="ResponseDTO"/> with the operation status.</returns>
        public ResponseDTO ResendVerificationCode(string email)
        {
            return _authenticationCore.ResendVerificationCode(email);
        }

        /// <summary>
        /// Completes user registration after email verification by setting a username and avatar.
        /// </summary>
        /// <param name="email">The verified email address of the user.</param>
        /// <param name="username">The chosen username.</param>
        /// <param name="avatar">Profile image as a byte array.</param>
        /// <returns>A <see cref="LoginResponse"/> containing session data if registration succeeds.</returns>
        public LoginResponse FinalizeRegistration(string email, string username, byte[] avatar)
        {
            return _authenticationCore.FinalizeRegistration(email, username, avatar);
        }

        /// <summary>
        /// Authenticates a registered user using email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>A <see cref="LoginResponse"/> with session data if credentials are valid.</returns>
        public LoginResponse Login(string email, string password)
        {
            return _authenticationCore.Login(email, password);
        }

        /// <summary>
        /// Allows guest login using a temporary username.
        /// </summary>
        /// <param name="guestUsername">Temporary username for guest mode.</param>
        /// <returns>A <see cref="LoginResponse"/> containing guest session data.</returns>
        public LoginResponse LoginAsGuest(string guestUsername)
        {
            return _authenticationCore.LoginAsGuest(guestUsername);
        }

        /// <summary>
        /// Logs out a guest user by invalidating their session token.
        /// </summary>
        /// <param name="sessionToken">The guest session token to invalidate.</param>
        public void LogoutGuest(string sessionToken)
        {
            _authenticationCore.LogoutGuest(sessionToken);
        }

        /// <summary>
        /// Initiates the user registration process using an email address and password.
        /// A verification code is generated and sent to the provided email.
        /// </summary>
        /// <param name="guestUserId">The guest user ID from the current session.param>
        /// <param name="newEmail">The user's new email address that will replace the temporary registration.param>
        /// <param name="newPassword">The new user password that will replace the temporary registration.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating the result of the operation.</returns>
        public ResponseDTO InitiateGuestRegistration(int guestUserId, string newEmail, string newPassword)
        {
            return _authenticationCore.InitiateGuestRegistration(guestUserId, newEmail, newPassword);
        }

        /// <summary>
        /// Complete the registration of a guest user after email verification.
        /// </summary>
        /// 
        /// <param name="guestUserId">The guest user ID from the current session.param>
        /// <param name="email">The email address you entered earlier will be verified.</param>
        /// <param name="pin">The one-time verification code.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating whether verification succeeded</returns>
        public ResponseDTO VerifyGuestRegistration(int guestUserId, string email, string pin)
        {
            return _authenticationCore.VerifyGuestRegistration(guestUserId, email, pin);
        }

        // === User Profile ===


        /// <summary>
        /// Retrieves the user's avatar by email address.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>The user's profile image as a byte array.</returns>
        public byte[] GetUserAvatar(string email)
        {
            return _userProfileCore.GetUserAvatar(email);
        }

        /// <summary>
        /// Updates the authenticated user's avatar.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="avatar">New profile image as a byte array.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating the result of the update.</returns>
        public ResponseDTO UpdateUserAvatar(string email, byte[] avatar)
        {
            return _userProfileCore.UpdateUserAvatar(email, avatar);
        }

        /// <summary>
        /// Changes the authenticated user's password.
        /// </summary>
        /// <param name="token">The user's authentication token.</param>
        /// <param name="currentPassword">The user's current password.</param>
        /// <param name="newPassword">The desired new password.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating whether the password change succeeded.</returns>
        public ResponseDTO ChangePassword(string token, string currentPassword, string newPassword)
        {
            return _userProfileCore.ChangePassword(token, currentPassword, newPassword);
        }

        /// <summary>
        /// Changes the authenticated user's username.
        /// </summary>
        /// <param name="token">The user's authentication token.</param>
        /// <param name="newUsername">The desired new username.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating whether the username change succeeded.</returns>
        public ResponseDTO ChangeUsername(string token, string newUsername)
        {
            return _userProfileCore.ChangeUsername(token, newUsername);
        }

        public ResponseDTO UpdatePersonalInfo(string email, string name, string lastName)
        {
            return _userProfileCore.UpdatePersonalInfo(email, name, lastName);
        }
        public ResponseDTO AddSocialNetwork(string token, string accountName)
        {
            return _userProfileCore.AddSocialNetwork(token, accountName);
        }
        public ResponseDTO RemoveSocialNetwork(string token, int socialNetworkId)
        {
            return _userProfileCore.RemoveSocialNetwork(token, socialNetworkId);
        }


        // === Social Features ===


        /// <summary>
        /// Sends a friend request to another user.
        /// </summary>
        /// <param name="token">Authentication token of the requesting user.</param>
        /// <param name="username">Target user's username.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating the result of the operation.</returns>
        public ResponseDTO SendFriendRequest(string token, string username)
        {
            return _socialCore.SendFriendRequest(token, username);
        }

        /// <summary>
        /// Retrieves pending friend requests for the authenticated user.
        /// </summary>
        /// <param name="token">Authentication token of the user.</param>
        /// <returns>A list of <see cref="FriendRequestDTO"/> objects representing pending requests.</returns>
        public List<FriendRequestDTO> GetPendingRequests(string token)
        {
            return _socialCore.GetPendingRequests(token);
        }

        /// <summary>
        /// Accepts or declines a specific friend request.
        /// </summary>
        /// <param name="token">Authentication token of the responding user.</param>
        /// <param name="requestId">Unique identifier of the friend request.</param>
        /// <param name="accept"><c>true</c> to accept the request; <c>false</c> to decline it.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating the result of the operation.</returns>
        public ResponseDTO AnswerFriendRequest(string token, int requestId, bool accept)
        {
            return _socialCore.AnswerFriendRequest(token, requestId, accept);
        }

        /// <summary>
        /// Retrieves the authenticated user's friend list.
        /// </summary>
        /// <param name="token">Authentication token of the user.</param>
        /// <returns>A list of <see cref="FriendDTO"/> objects representing the user's friends.</returns>
        public List<FriendDTO> GetFriendsList(string token)
        {
            return _socialCore.GetFriendsList(token);
        }

        /// <summary>
        /// Removes a friend from the authenticated user's contact list.
        /// </summary>
        /// <param name="token">Authentication token of the user.</param>
        /// <param name="username">Username of the friend to remove.</param>
        /// <returns>A <see cref="ResponseDTO"/> indicating the result of the operation.</returns>
        public ResponseDTO RemoveFriend(string token, string username)
        {
            return _socialCore.RemoveFriend(token, username);
        }

        public List<MatchHistoryDTO> GetMatchHistory(string token)
        {
            return _statisticsCore.GetMatchHistory(token);
        }

        public ResponseDTO ReportUser(string token, string targetUser, int matchId)
        {
            return _penaltyCore.ReportUser(token, targetUser, matchId);
        }

        public LoginResponse RenewSession(string token)
        {
            return _authenticationCore.RenewSession(token);
        }
    }
}