using Server.LobbyService;
using Server.SessionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        static void Main(string[] args)
        {
            ServiceHost userHost = new ServiceHost(typeof(UserService));
            ServiceHost lobbyHost = new ServiceHost(typeof(GameLobbyService));

            try
            {
                userHost.Open();
                lobbyHost.Open();

                Console.WriteLine("Server started succesfully.");
                Console.WriteLine("Service running. Type [ENTER] to stop.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong.");
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                if (userHost.State == CommunicationState.Opened)
                    userHost.Close();

                if (lobbyHost.State == CommunicationState.Opened)
                    lobbyHost.Close();
            }
        }
    }
}
