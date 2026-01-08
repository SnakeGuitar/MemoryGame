using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server.SessionService
{
    public interface IUserCallback
    {
        [OperationContract(IsOneWay = true)]
        void ForceLogout(string reason);
    }
}
