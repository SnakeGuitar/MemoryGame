using Client.UserServiceReference;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core
{
    internal static class UserSession
    {
        public static string SessionToken { get; set; }
        public static int UserId { get; private set; }
        public static string Username { get; set; }
        public static bool IsGuest { get; set; }
        public static string Email { get; private set; }
        public static string Name { get; set; } = string.Empty;
        public static string LastName { get; set; } = string.Empty;
        public static DateTime RegistrationDate { get; set; }
        public static List<SocialNetworkDTO> SocialNetworks { get; set; } = new List<SocialNetworkDTO>();
        

        public static void StartSession(string token, UserDTO user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            SessionToken = token;
            UserId = user.UserId;
            Username = user.Username;
            IsGuest = user.IsGuest;
            Email = user.Email;
            Name = user.Name;
            LastName = user.LastName;
            RegistrationDate = user.RegistrationDate;
            SocialNetworks = user.SocialNetworks != null
                ? new List<SocialNetworkDTO>(user.SocialNetworks)
                : new List<SocialNetworkDTO>();
        }

        public static void EndSession()
        {
            SessionToken = null;
            UserId = 0;
            Username = null;
            Email = null;
            IsGuest = false;
        }

        public static event Action ProfileUpdated;
        public static void OnProfileUpdated()
        {
            ProfileUpdated?.Invoke();
        }
    }
}
