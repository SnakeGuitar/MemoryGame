using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.LobbyService
{
    public interface ILobbyCallbackProvider
    {
        IGameLobbyCallback GetCallback();
    }

    public class WcfLobbyCallbackProvider : ILobbyCallbackProvider
    {
        public IGameLobbyCallback GetCallback()
        {
            return OperationContext.Current.GetCallbackChannel<IGameLobbyCallback>();
        }
    }
}
