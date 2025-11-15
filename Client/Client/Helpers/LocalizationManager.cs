using Client.Properties.Langs;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.Helpers
{
    public static class LocalizationManager
    {
        public static string GetString(ValidationHelper.ValidationCode code)
        {
            switch (code)
            {
                case ValidationHelper.ValidationCode.UsernameEmpty:
                    return Lang.Global_ValidationUsername_Empty;
                case ValidationHelper.ValidationCode.UsernameTooLong:
                    return Lang.Global_ValidationUsername_TooLong;
                case ValidationHelper.ValidationCode.UsernameInvalidChars:
                    return Lang.Global_ValidationUsername_InvalidChars;

                case ValidationHelper.ValidationCode.PasswordEmpty:
                    return Lang.Global_ValidationPassword_Empty;
                case ValidationHelper.ValidationCode.PasswordLengthInvalid:
                    return Lang.Global_ValidationPassword_LengthInvalid;
                case ValidationHelper.ValidationCode.PasswordMissingUpper:
                    return Lang.Global_ValidationPassword_MissingUpper;
                case ValidationHelper.ValidationCode.PasswordMissingLower:
                    return Lang.Global_ValidationPassword_MissingLower;
                case ValidationHelper.ValidationCode.PasswordMissingDigit:
                    return Lang.Global_ValidationPassword_MissingDigit;

                case ValidationHelper.ValidationCode.EmailEmpty:
                    return Lang.Global_ValidationEmail_Empty;
                case ValidationHelper.ValidationCode.EmailInvalidFormat:
                    return Lang.Global_ValidationEmail_InvalidFormat;

                case ValidationHelper.ValidationCode.CodeEmpty:
                    return Lang.Global_ValidationCode_Empty;
                case ValidationHelper.ValidationCode.CodeLengthInvalid:
                    return Lang.Global_ValidationCode_LengthInvalid;
                case ValidationHelper.ValidationCode.CodeNotNumeric:
                    return Lang.Global_ValidationCode_NotNumeric;

                default:
                    return Lang.Global_ServiceError_Unknown;
            }
        }

        public static string GetString(Exception ex)
        {
            if (ex is EndpointNotFoundException)
            {
                return Lang.Global_ServiceError_Offline;
            }
            if (ex is CommunicationException)
            {
                return Lang.Global_ServiceError_NetworkDown;
            }

            return Lang.Global_ServiceError_Unknown;
        }
    }
}
