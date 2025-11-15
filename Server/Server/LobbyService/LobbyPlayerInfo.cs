using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.LobbyService
{
    [DataContract]
    public class LobbyPlayerInfo
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool IsGuest { get; set; }
        [DataMember]
        public DateTime JoinedAt { get; set; }
    }
}