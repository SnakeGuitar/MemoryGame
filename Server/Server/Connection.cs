using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using Server.SessionService;

namespace ServerHost
{
    class Program
    {
        static void Main()
        {
            // Base addresses
            Uri httpBase = new Uri("http://localhost:52000/mexHttp");
            Uri tcpBase = new Uri("net.tcp://localhost:52001/Server");

            using (ServiceHost host = new ServiceHost(typeof(UserService), httpBase, tcpBase))
            {
                // Endpoint TCP
                host.AddServiceEndpoint(typeof(IUserService), new NetTcpBinding(), "");

                // Endpoint HTTP MEX
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mexHttp");

                // Endpoint TCP MEX
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mexTcp");

                // ServiceMetadataBehavior
                if (host.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior
                    {
                        HttpGetEnabled = true,
                        HttpGetUrl = httpBase
                    });
                }

                host.Open();
                Console.WriteLine("Service running...");
                Console.WriteLine("HTTP WSDL: http://localhost:52000/mexHttp?wsdl");
                Console.WriteLine("Press ENTER to stop.");
                Console.ReadLine();
            }
        }
    }
}
