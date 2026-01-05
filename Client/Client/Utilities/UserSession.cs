using Client.UserServiceReference;
using Client.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    internal static class UserSession
    {
        public static string SessionToken { get; set; }
        public static int UserId { get; private set; }
        public static string Username { get; set; }
        public static string Email { get; private set; }
        public static bool IsGuest { get; set; }
        public static DateTime RegistrationDate { get; set; }
        public static string Name { get; set; } = string.Empty;
        public static string LastName { get; set; } = string.Empty;
        public static List<SocialNetworkDTO> SocialNetworks { get; set; } = new List<SocialNetworkDTO>();

        public static void StartSession(string token, int id, string username, string email, string name, string lastname,
            DateTime registrationDate, List<SocialNetworkDTO> socialNetworks)
        {
            SessionToken = token;
            UserId = id;
            Username = username;
            Email = email;
            IsGuest = false;
            Name = name;
            LastName = lastname;
            RegistrationDate = registrationDate;
            SocialNetworks = socialNetworks;
        }

        public static void StartGuestSession(string token, int id, string username)
        {
            SessionToken = token;
            UserId = id;
            Username = username;
            IsGuest = true;
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
