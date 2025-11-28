using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Server.SessionService;
using Server.LobbyService;
using Server.GameService;
using Test.Helpers;
using System.ServiceModel.Security;

namespace Test.InfraestructureTest
{
    [TestClass]
    public class WcfConnectivityTest
    {
        // CHANGE WHEN CONNECTED TO HOTSPOT NETWORK
        private const string TARGET_IP = "localhost";

        private const string USERSERVICE_PORT = "52001";
        private const string LOBBYSERVICE_PORT = "53001";

        private ChannelFactory<IUserService> _userServiceFactory;
        private ChannelFactory<IGameLobbyService> _lobbyServiceFactory;
        private WcfConnectivityTestHelper _helper = new WcfConnectivityTestHelper();

        [TestCleanup]
        public void Cleanup()
        {
            _userServiceFactory?.Close();
            _lobbyServiceFactory?.Close();
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Verify_UserService_Connection()
        {
            var binding = _helper.CreateRobustBinding();
            var address = new EndpointAddress($"net.tcp://{TARGET_IP}:{USERSERVICE_PORT}");

            _userServiceFactory = new ChannelFactory<IUserService>(binding, address);
            IUserService proxy = null;

            try
            {
                Console.WriteLine($"Connecting to UserService at {address.Uri}...");
                proxy = _userServiceFactory.CreateChannel();

                var response = proxy.Login("diana_best_fox_girl@test.com", "ping");

                Assert.IsNotNull(response, "Service response was null");
                Assert.AreEqual(CommunicationState.Opened, ((IClientChannel)proxy).State, "Channel is not opened");

                Console.WriteLine("UserService connection successful. :3");
            }
            catch (EndpointNotFoundException)
            {
                Assert.Fail($"Couldn't find server at {TARGET_IP}." +
                    $"\nPossible causes:" +
                    $"\n1. Firewasll is blocking port {USERSERVICE_PORT}." +
                    $"\n2. Server is offline." +
                    $"\n3. IP is incorrect.");
            }
            catch (SecurityNegotiationException)
            {
                Assert.Fail("Security Error (SSPI). Check that both client and server have the same security settings.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection established, but internal logic failed: {ex.Message}");
            }
            finally
            {
                _helper.CloseChannel((IClientChannel)proxy);
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void Verify_LobbyService_Connection()
        {
            var binding = _helper.CreateRobustBinding();
            var address = new EndpointAddress($"net.tcp://{TARGET_IP}:{LOBBYSERVICE_PORT}");

            var mockCallback = new MockLobbyCallback();
            var context = new InstanceContext(mockCallback);

            _lobbyServiceFactory = new DuplexChannelFactory<IGameLobbyService>(context, binding, address);
            IGameLobbyService proxy = null;

            try
            {
                Console.WriteLine($"Connecting to LobbyService at {address.Uri}...");
                proxy = _lobbyServiceFactory.CreateChannel();

                proxy.JoinLobby("test_token_connection", "000000", true, "NetTester");

                Assert.AreEqual(CommunicationState.Opened, ((IClientChannel)proxy).State, "Lobby Channel is not opened");
                Console.WriteLine("LobbyService connection successful. :3");
            }
            catch (EndpointNotFoundException)
            {
                Assert.Fail($"Couldn't find Lobby Server at {TARGET_IP}:{LOBBYSERVICE_PORT}. Check Firewall/Port.");
            }
            catch (SecurityNegotiationException)
            {
                Assert.Fail("Security Error (SSPI) on Lobby. Check <security mode='None'>.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection established, logical error expected: {ex.Message}");
            }
            finally
            {
                _helper.CloseChannel((IClientChannel)proxy);
            }
        }
    }
}
