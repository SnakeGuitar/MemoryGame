using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Exceptions
{
   public enum NetworkFailPolicy
    {
        ShowWarningOnly,
        AskToRetryOrExit,
        CriticalExit
    }
}
