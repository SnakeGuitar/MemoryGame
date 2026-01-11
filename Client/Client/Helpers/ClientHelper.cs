using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    internal static class ClientHelper
    {
        public static string GenerateGameCode()
        {
            byte[] randomBytes = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            int value = BitConverter.ToInt32(randomBytes, 0) & int.MaxValue;
            return (value % 900000 + 100000).ToString();
        }

    }
}
