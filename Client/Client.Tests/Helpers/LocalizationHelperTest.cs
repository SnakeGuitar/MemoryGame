using Client.Helpers;
using Client.Properties.Langs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;

namespace Client.Tests.Helpers
{
    [TestClass]
    public class LocalizationHelperTests
    {
        #region GetString(string serverMessageKey)

        [TestMethod]
        public void GetString_KeyIsNull_ReturnsUnknownError()
        {
            var result = LocalizationHelper.GetString((string)null);
            Assert.AreEqual(Lang.Global_ServiceError_Unknown, result);
        }

        [TestMethod]
        public void GetString_KeyIsEmpty_ReturnsUnknownError()
        {
            var result = LocalizationHelper.GetString(string.Empty);
            Assert.AreEqual(Lang.Global_ServiceError_Unknown, result);
        }

        [TestMethod]
        public void GetString_KnownKey_InvalidCredentials_ReturnsCorrectResource()
        {
            var result = LocalizationHelper.GetString(LocalizationHelper.ServerKeys.InvalidCredentials);
            Assert.AreEqual(Lang.Global_Error_InvalidCredentials, result);
        }

        [TestMethod]
        public void GetString_KnownKey_EmailInUse_ReturnsCorrectResource()
        {
            var result = LocalizationHelper.GetString(LocalizationHelper.ServerKeys.EmailInUse);
            Assert.AreEqual(Lang.Global_Error_EmailInUse, result);
        }

        [TestMethod]
        public void GetString_KnownKey_AccountPenalized_ReturnsHardcodedString()
        {
            var result = LocalizationHelper.GetString(LocalizationHelper.ServerKeys.AccountPenalized);
            Assert.AreEqual(Lang.Global_Message_AccountPenalized, result);
        }

        [TestMethod]
        public void GetString_HardcodedDatabaseKey_ReturnsUnknownError()
        {
            var result = LocalizationHelper.GetString("Global_Error_Database");
            Assert.AreEqual(Lang.Global_ServiceError_Database, result);
        }

        [TestMethod]
        public void GetString_UnknownRandomKey_ReturnsUnknownError()
        {
            var result = LocalizationHelper.GetString("Some_Random_Error_That_Does_Not_Exist");
            Assert.AreEqual(Lang.Global_ServiceError_Unknown, result);
        }

        #endregion

        #region GetString(ValidationCode)

        [TestMethod]
        public void GetString_ValidationSuccess_ReturnsEmptyString()
        {
            var result = LocalizationHelper.GetString(ValidationHelper.ValidationCode.Success);
            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void GetString_ValidationEmailInvalid_ReturnsCorrectResource()
        {
            var result = LocalizationHelper.GetString(ValidationHelper.ValidationCode.EmailInvalidFormat);
            Assert.AreEqual(Lang.Global_ValidationEmail_InvalidFormat, result);
        }

        [TestMethod]
        public void GetString_ValidationPasswordMissingDigit_ReturnsCorrectResource()
        {
            var result = LocalizationHelper.GetString(ValidationHelper.ValidationCode.PasswordMissingDigit);
            Assert.AreEqual(Lang.Global_ValidationPassword_MissingDigit, result);
        }

        #endregion

        #region GetString(Exception)

        [TestMethod]
        public void GetString_ExceptionEndpointNotFound_ReturnsOfflineMessage()
        {
            var ex = new EndpointNotFoundException();
            var result = LocalizationHelper.GetString(ex);

            Assert.AreEqual(Lang.Global_ServiceError_Offline, result);
        }

        [TestMethod]
        public void GetString_ExceptionTimeout_ReturnsNetworkDownMessage()
        {
            var ex = new TimeoutException();
            var result = LocalizationHelper.GetString(ex);

            Assert.AreEqual(Lang.Global_ServiceError_NetworkDown, result);
        }

        [TestMethod]
        public void GetString_ExceptionCommunication_ReturnsNetworkDownMessage()
        {
            var ex = new CommunicationException();
            var result = LocalizationHelper.GetString(ex);

            Assert.AreEqual(Lang.Global_ServiceError_NetworkDown, result);
        }

        [TestMethod]
        public void GetString_ExceptionGeneric_ReturnsUnknownMessage()
        {
            var ex = new Exception("Something random happened");
            var result = LocalizationHelper.GetString(ex);

            Assert.AreEqual(Lang.Global_ServiceError_Unknown, result);
        }

        #endregion
    }
}
