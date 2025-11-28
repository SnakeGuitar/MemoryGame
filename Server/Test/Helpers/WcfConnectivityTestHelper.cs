using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Test.Helpers
{
    public class WcfConnectivityTestHelper
    {
        public NetTcpBinding CreateRobustBinding()
        {
            var binding = new NetTcpBinding(SecurityMode.None)
            {
                MaxReceivedMessageSize = 52428800,
                MaxBufferSize = 52428800
            };

            binding.ReaderQuotas.MaxArrayLength = 52428800;
            binding.OpenTimeout = TimeSpan.FromSeconds(15);
            binding.SendTimeout = TimeSpan.FromSeconds(15);

            return binding;
        }

        public void CloseChannel(IClientChannel channel)
        {
            try
            {
                if (channel == null)
                {
                    return;
                }

                if (channel.State == CommunicationState.Faulted)
                {
                    channel.Abort();
                }
                else
                {
                    channel.Close();
                }
            }
            catch
            {
                channel?.Abort();
            }
        }

        public void CloseFactory(ChannelFactory factory)
        {
            try
            {
                if (factory == null)
                {
                    return;
                }

                if (factory.State == CommunicationState.Faulted)
                {
                    factory.Abort();
                }
                else
                {
                    factory.Close();
                }
            }
            catch
            {
                factory?.Abort();
            }
        }
    }
}
