using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.GameService
{
    [DataContract]
    public class GameSettings
    {
        [DataMember]
        public int CardCount { get; set; }
        [DataMember]
        public int TurnTimeSeconds { get; set; }
    }
}