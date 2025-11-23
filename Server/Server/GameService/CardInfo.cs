using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.GameService
{
    [DataContract]
    public class CardInfo
    {
        [DataMember]
        public int CardId { get; set; }
        [DataMember]
        public string ImageIdentifier { get; set; }
    }
}