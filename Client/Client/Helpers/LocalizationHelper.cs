using Client.Properties.Langs;
using System;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media;

namespace Client.Helpers
{
    public static class LocalizationHelper
    {
        public static class ServerKeys
        {
            #region Authentication & Session
            public const string InvalidCredentials = "Global_Error_InvalidCredentials";
            public const string UserAlreadyLoggedIn = "Global_Error_UserAlreadyLoggedIn";
            public const string AccountPenalized = "Global_Error_AccountPenalized";
            public const string SessionExpired = "Global_Error_SessionExpired";
            public const string InvalidToken = "Global_Error_InvalidToken";
            public const string Unauthorized = "Global_Error_Unauthorized";
            #endregion

            #region Registration & User Account
            public const string PasswordInvalid = "Global_Error_PasswordInvalid";
            public const string EmailInUse = "Global_Error_EmailInUse";
            public const string EmailSendFailed = "Global_Error_EmailSendFailed";
            public const string RegistrationNotFound = "Global_Error_RegistrationNotFound";
            public const string CodeInvalid = "Global_Error_CodeInvalid";
            public const string CodeExpired = "Global_Error_CodeExpired";
            public const string InvalidUsername = "Global_Error_InvalidUsername";
            public const string UsernameInvalidChars = "Global_ValidationUsername_InvalidChars";
            public const string UsernameInUse = "Global_Error_UsernameInUse";
            public const string UserNotFound = "Global_Error_UserNotFound";
            public const string AlreadyRegistered = "Global_Error_AlreadyRegistered";
            #endregion

            #region Profile & Settings
            public const string ImageInvalid = "Global_Error_ImageInvalid";
            public const string PasswordNull = "Global_Error_PasswordNull";
            public const string PasswordUpdateFailed = "Global_Error_PasswordUpdate";
            public const string NewUsernameNull = "Global_Error_NewUsernameIsNull";
            public const string SameUsername = "Global_Error_SameUsername";
            public const string SocialDuplicate = "Profile_Error_SocialDuplicate";
            public const string NotFound = "Global_Error_NotFound";
            #endregion

            #region Social
            public const string SelfAddFriend = "Social_Error_SelfAdd";
            public const string AlreadyFriends = "Social_Error_AlreadyFriends";
            public const string RequestExists = "Social_Error_RequestExists";
            public const string RequestNotFound = "Social_Error_RequestNotFound";
            public const string NotFriends = "Social_Error_NotFriends";
            #endregion

            #region Kowloon Generic Errors (Server inconsistencies)
            public const string ServiceErrorDatabase = "Global_ServiceError_Database";
            public const string ServiceErrorUnknown = "Global_ServiceError_Unknown";
            public const string ErrorDatabase = "Global_Error_Database";
            public const string ErrorUnknown = "Global_Error_Unknown";
            public const string ErrorDatabaseError = "Global_Error_DatabaseError";
            public const string ErrorUnexpectedError = "Global_Error_UnexpectedError";
            public const string ErrorUnknownError = "Global_Error_UnknownError";
            #endregion
        }

        public static string GetString(string serverMessageKey)
        {
            if (string.IsNullOrEmpty(serverMessageKey))
                return Lang.Global_ServiceError_Unknown;

            switch (serverMessageKey)
            {
                // --- AUTH & SESSION ---
                case ServerKeys.InvalidCredentials:
                    return Lang.Global_Error_InvalidCredentials;

                case ServerKeys.UserAlreadyLoggedIn:
                    return Lang.Global_Error_InvalidCredentials;

                case ServerKeys.SessionExpired:
                case ServerKeys.InvalidToken:
                    return Lang.Global_Error_InvalidToken;

                case ServerKeys.AccountPenalized:
                    return "Your account has been penalized.";

                // --- REGISTRATION ---
                case ServerKeys.PasswordInvalid:
                    return Lang.Global_Error_PasswordInvalid;
                case ServerKeys.EmailInUse:
                    return Lang.Global_Error_EmailInUse;
                case ServerKeys.EmailSendFailed:
                    return Lang.Global_Error_EmailSendFailed;
                case ServerKeys.RegistrationNotFound:
                    return Lang.Global_Error_RegistrationNotFound;
                case ServerKeys.CodeInvalid:
                    return Lang.Global_Error_CodeInvalid;
                case ServerKeys.CodeExpired:
                    return Lang.Global_Error_CodeExpired;
                case ServerKeys.InvalidUsername:
                    return Lang.Global_Error_InvalidUsername;
                case ServerKeys.UsernameInUse:
                    return Lang.Global_Error_UsernameInUse;

                // --- PERFIL ---
                case ServerKeys.UserNotFound:
                    return Lang.Global_Error_UserNotFound;
                case ServerKeys.ImageInvalid:
                    return Lang.Global_Error_ImageInvalid;
                case ServerKeys.PasswordUpdateFailed:
                    return Lang.EditProfile_Label_ErrorPasswordUpdate;
                case ServerKeys.SameUsername:
                    return Lang.EditProfile_Label_ErrorSameUsername;

                // --- SOCIAL ---
                case ServerKeys.SelfAddFriend:
                    return Lang.Social_Error_SelfAdd;
                case ServerKeys.AlreadyFriends:
                    return Lang.Social_Error_AlreadyFriends;
                case ServerKeys.RequestExists:
                    return Lang.Social_Error_RequestExists;

                // --- KOWLOON GENERIC ERRORS ---
                case ServerKeys.ServiceErrorDatabase:
                case "Global_Error_Database":
                case "Global_Error_DatabaseError":
                    return Lang.Global_ServiceError_Unknown;

                case ServerKeys.ServiceErrorUnknown:
                default:
                    return Lang.Global_ServiceError_Unknown;
            }
        }

        public static string GetString(ValidationHelper.ValidationCode code)
        {
            switch (code)
            {
                case ValidationHelper.ValidationCode.Success:
                    return string.Empty;

                // --- EMAIL ---
                case ValidationHelper.ValidationCode.EmailEmpty:
                    return Lang.Global_ValidationEmail_Empty;
                case ValidationHelper.ValidationCode.EmailInvalidFormat:
                    return Lang.Global_ValidationEmail_InvalidFormat;

                // --- PASSWORD ---
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

                // --- USERNAME ---
                case ValidationHelper.ValidationCode.UsernameEmpty:
                    return Lang.Global_ValidationUsername_Empty;

                case ValidationHelper.ValidationCode.UsernameTooSmall:
                    return Lang.Global_ValidationUsername_TooSmall;

                case ValidationHelper.ValidationCode.UsernameTooLong:
                    return Lang.Global_ValidationUsername_TooLong;

                case ValidationHelper.ValidationCode.UsernameInvalidChars:
                    return Lang.Global_ValidationUsername_InvalidChars;

                // --- VERIFICATION CODE ---
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
            if (ex is TimeoutException)
            {
                return Lang.Global_ServiceError_NetworkDown;
            }
            return Lang.Global_ServiceError_Unknown;
        }

        public static void ApplyLanguageFont()
        {
            string titleFontName = Lang.Global_TitleFont;
            string bodyFontName = Lang.Global_BodyFont;

            string titleFontPath = $"pack://application:,,,/Resources/Fonts/{titleFontName}";
            string bodyFontPath = $"pack://application:,,,/Resources/Fonts/{bodyFontName}";

            FontFamily newTitleFont = new FontFamily(titleFontPath);
            FontFamily newBodyFont = new FontFamily(bodyFontPath);

            Application.Current.Resources["TitleFont"] = newTitleFont;
            Application.Current.Resources["BodyFont"] = newBodyFont;
        }
    }
}
