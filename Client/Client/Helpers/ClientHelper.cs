using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    internal class ClientHelper
    {
        public static string GenerateGameCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

    }
}
