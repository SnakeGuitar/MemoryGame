using System;

namespace Server.LobbyService
{
    public class LobbyClient
    {
        public string Id { get; set; }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public bool IsGuest { get; set; }
        public IGameLobbyCallback Callback { get; set; }
        public DateTime JoinedAt { get; set; }
        public string SessionId { get; set; }
    }
}
