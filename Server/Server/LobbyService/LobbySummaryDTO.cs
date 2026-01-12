using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.LobbyService
{
    [DataContract]
    public class LobbySummaryDTO
    {
        [DataMember]
        public string GameCode { get; set; }

        [DataMember]
        public int CurrentPlayers { get; set; }

        [DataMember]
        public bool IsFull { get; set; }
    }
}
