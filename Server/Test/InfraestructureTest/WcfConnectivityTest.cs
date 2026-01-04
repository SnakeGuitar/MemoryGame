using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ServiceModel;
using Test.Helpers;

namespace Test.InfraestructureTest
{
    [TestClass]
    public class WcfConnectivityTest
    {
        private WcfConnectivityTestHelper _helper;

        [TestInitialize]
        public void Setup()
        {
            _helper = new WcfConnectivityTestHelper();
        }

        [TestMethod]
        public void CreateRobustBinding_SetsSecurityModeToNone()
        {
            var binding = _helper.CreateRobustBinding();

            Assert.AreEqual(SecurityMode.None, binding.Security.Mode);
        }

        [TestMethod]
        public void CreateRobustBinding_SetsCorrectMaxReceivedMessageSize()
        {
            var binding = _helper.CreateRobustBinding();

            Assert.AreEqual(52428800, binding.MaxReceivedMessageSize);
        }

        [TestMethod]
        public void CreateRobustBinding_SetsCorrectMaxBufferSize()
        {
            var binding = _helper.CreateRobustBinding();

            Assert.AreEqual(52428800, binding.MaxBufferSize);
        }

        [TestMethod]
        public void CreateRobustBinding_SetsCorrectMaxArrayLength()
        {
            var binding = _helper.CreateRobustBinding();

            Assert.AreEqual(52428800, binding.ReaderQuotas.MaxArrayLength);
        }

        [TestMethod]
        public void CreateRobustBinding_SetsCorrectOpenTimeout()
        {
            var binding = _helper.CreateRobustBinding();

            Assert.AreEqual(TimeSpan.FromSeconds(15), binding.OpenTimeout);
        }

        [TestMethod]
        public void CloseChannel_NullChannel_DoesNotThrow()
        {
            try
            {
                _helper.CloseChannel(null);
            }
            catch
            {
                Assert.Fail("CloseChannel should not throw on null input.");
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CloseFactory_NullFactory_DoesNotThrow()
        {
            try
            {
                _helper.CloseFactory(null);
            }
            catch
            {
                Assert.Fail("CloseFactory should not throw on null input.");
            }

            Assert.IsTrue(true);
        }
    }
}