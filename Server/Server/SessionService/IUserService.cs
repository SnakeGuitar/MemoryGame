using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.SessionService
{
    [ServiceContract]
    internal interface IUserService
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
        LoginResponse LoginAsGuest(string guestUsername);

        [OperationContract]
        void LogoutGuest(string sessionToken);
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
}
