using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    internal class UserSession
    {
        public static string SessionToken { get; set; }
        public static string Username { get; set; }
        public static bool IsGuest { get; set; }

        public static void StartSession(string token, string username)
        {
            SessionToken = token;
            Username = username;
            IsGuest = false;
        }

        public static void StartGuestSession(string token, string username)
        {
            SessionToken = token;
            Username = username;
            IsGuest = true;
        }

        public static void EndSession()
        {
            SessionToken = null;
            Username = null;
            IsGuest = false;
        }
    }
}
