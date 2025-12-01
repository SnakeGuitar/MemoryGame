using Server.SessionService.Core;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Server.SessionService
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        ResponseDTO StartRegistration(string email, string password);

        [OperationContract]
        ResponseDTO VerifyRegistration(string email, string pin);

        [OperationContract]
        ResponseDTO ResendVerificationCode(string email);

        [OperationContract]
        LoginResponse FinalizeRegistration(string email, string username, byte[] avatar);

        [OperationContract]
        LoginResponse Login(string email, string password);

        [OperationContract]
        byte[] GetUserAvatar(string email);

        [OperationContract]
        ResponseDTO UpdateUserAvatar(string email, byte[] avatar);

        [OperationContract]
        ResponseDTO ChangePassword(string email, string currentPassword, string newPassword);

        [OperationContract]
        ResponseDTO ChangeUsername(string email, string newUsername);

        [OperationContract]
        LoginResponse LoginAsGuest(string guestUsername);

        [OperationContract]
        void LogoutGuest(string sessionToken);

        [OperationContract]
        ResponseDTO SendFriendRequest(string token, string username);

        [OperationContract]
        List<FriendRequestDTO> GetPendingRequests(string token);

        [OperationContract]
        ResponseDTO AnswerFriendRequest(string token, int requestId, bool accept);

        [OperationContract]
        List<MatchHistoryDTO> GetMatchHistory(string token);

        [OperationContract]
        ResponseDTO ReportUser(string token, string targetUser, int matchId);
    }

    [DataContract]
    public class ResponseDTO
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string MessageKey { get; set; }
    }

    [DataContract]
    public class LoginResponse : ResponseDTO
    {
        [DataMember]
        public string SessionToken { get; set; }
        [DataMember]
        public UserDTO User { get; set; }
    }

    [DataContract]
    public class UserDTO
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Email { get; set; }
    }

    [DataContract]
    public class FriendDTO
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public byte[] Avatar { get; set; }
        [DataMember]
        public bool IsOnline { get; set; }
    }

    [DataContract]
    public class FriendRequestDTO
    {
        [DataMember]
        public int RequestId { get; set; }
        [DataMember]
        public string SenderUsername { get; set; }
        [DataMember]
        public byte[] SenderAvatar { get; set; }
    }

    [DataContract]
    public class MatchHistoryDTO
    {
        [DataMember] public int MatchId { get; set; }
        [DataMember] public DateTime Date { get; set; }
        [DataMember] public int Score { get; set; }
        [DataMember] public string WinnerName { get; set; }
    }

    [DataContract]
    public class PenaltyDTO
    {
        [DataMember] public int Type { get; set; }
        [DataMember] public DateTime EndDate { get; set; }
        [DataMember] public string Reason { get; set; }
    }
}
