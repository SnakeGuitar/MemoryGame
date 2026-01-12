using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.LobbyService
{
    public class Lobby
    {
        public string GameCode { get; set; }
        public ConcurrentDictionary<string, LobbyClient> Clients { get; set; } = new ConcurrentDictionary<string, LobbyClient>();
        public DateTime CreatedAt { get; set; }
        public object LockObject { get; } = new object();
        public bool IsPublic { get; set; } = false;
    }
}
