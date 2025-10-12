using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Server.AuthenticationService
{
    [ServiceContract]
    internal interface IUserService
    {
        [OperationContract]
        bool RequestRegistration(string email, string password);

        [OperationContract]
        bool VerifyRegistration(string email, string code);

        [OperationContract]
        bool Login(string email, string password);
    }
}
