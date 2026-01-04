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
        public static string RegistrationDate { get; set; }

        public static void StartSession(string token, int id, string username, string email)
        {
            SessionToken = token;
            UserId = id;
            Username = username;
            Email = email;
            IsGuest = false;
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
    }
}
