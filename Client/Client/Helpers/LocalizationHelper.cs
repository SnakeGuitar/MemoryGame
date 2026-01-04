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
    public static class LocalizationHelper
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

        public static string GetString(string serverMessageKey)
        {
            if (string.IsNullOrEmpty(serverMessageKey))
                return Lang.Global_ServiceError_Unknown;

            switch (serverMessageKey)
            {
                case "Global_Error_CodeInvalid":
                    return Lang.Global_Error_CodeInvalid;
                case "Global_Error_CodeExpired":
                    return Lang.Global_Error_CodeExpired;

                case "Global_Error_RegistrationNotFound":
                    return Lang.Global_Error_RegistrationNotFound;
                case "Global_Error_EmailInUse":
                    return Lang.Global_Error_EmailInUse;
                case "Global_Error_PasswordInvalid":
                    return Lang.Global_Error_PasswordInvalid;
                case "Global_Error_EmailSendFailed":
                    return Lang.Global_Error_EmailSendFailed;

                case "Global_Error_InvalidCredentials":
                    return Lang.Global_Error_InvalidCredentials;
                case "Global_Error_InvalidUsername":
                case "Glocal_Error_GuestUsernameInvalid":
                    return Lang.Global_Error_InvalidUsername;

                case "Global_Error_UsernameInUse":
                    return Lang.Global_Error_UsernameInUse;

                case "Global_ValidationUsername_InvalidChars":
                    return Lang.Global_ValidationUsername_InvalidChars;

                default:
                    return Lang.Global_ServiceError_Unknown;
            }
        }
    }
}
