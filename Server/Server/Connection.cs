using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Server;

namespace Server
{
    class Connection
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Server.AuthenticationService.UserService)))
            {
                try
                {
                    host.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.ReadLine();
                }
                Console.WriteLine("Service is running...");
                Console.WriteLine("Press Enter to terminate service.");
                Console.ReadLine();
            }
        }
    }
}