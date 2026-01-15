using System;
using System.ServiceModel;

namespace Client.Core
{
    /// <summary>
    /// Factory responsible for managing the lifecycle, configuration, and instantiation of WCF Service Proxies.
    /// </summary>
    public static class WcfClientFactory
    {
        /// <summary>
        /// Recreates a WCF client, handling safe closure of the previous instance and configuring timeouts.
        /// </summary>
        /// <typeparam name="TClient">The type of the WCF client (must inherit from ClientBase and implement ICommunicationObject).</typeparam>
        /// <typeparam name="TChannel">The service interface (Contract).</typeparam>
        /// <param name="currentClient">The current client instance to dispose.</param>
        /// <param name="callbackInstance">The callback implementation (InstanceContext).</param>
        /// <param name="constructor">Delegate to instantiate the new client.</param>
        /// <returns>A fully configured and opened WCF client.</returns>
        public static TClient CreateClient<TClient, TChannel>
            (TClient currentClient,
            object callbackInstance,
            Func<InstanceContext, TClient> constructor)
            where TClient : ClientBase<TChannel>, ICommunicationObject
            where TChannel : class
        {
            if (currentClient != null)
            {
                try
                {
                    if (currentClient.State == CommunicationState.Opened)
                    {
                        currentClient.Close();
                    }
                    else
                    {
                        currentClient.Abort();
                    }
                }
                catch
                {
                    currentClient.Abort();
                }
            }

            InstanceContext context = new InstanceContext(callbackInstance);
            TClient newClient = constructor(context);

            ConfigureTimeouts(newClient);

            newClient.Open();

            return newClient;
        }

        private static void ConfigureTimeouts<TChannel>(ClientBase<TChannel> client) where TChannel : class
        {
            if (client.Endpoint.Binding is System.ServiceModel.Channels.Binding binding)
            {
                binding.OpenTimeout = TimeSpan.FromSeconds(5);
                binding.CloseTimeout = TimeSpan.FromSeconds(5);
                binding.SendTimeout = TimeSpan.FromSeconds(10);
                binding.ReceiveTimeout = TimeSpan.FromMinutes(20);
            }
        }

        /// <summary>
        /// Checks if the communication object is in a usable state.
        /// </summary>
        public static bool IsConnectionActive(ICommunicationObject client)
        {
            if (client == null)
            {
                return false;
            }
            return client.State == CommunicationState.Opened;
        }
    }
}