using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared
{
    public interface IDbContextFactory
    {
        memoryGameDBEntities Create();
    }

    public class DbContextFactory : IDbContextFactory
    {
        public memoryGameDBEntities Create()
        {
            return new memoryGameDBEntities();
        }
    }
}
