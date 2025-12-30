using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Shared;

namespace Test.SharedTest
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void Constructor_WhenCalled_DoesNotThrowException()
        {
            try
            {
                var logger = new Logger(typeof(LoggerTest));
            }
            catch
            {
                Assert.Fail("Logger constructor threw an exception.");
            }
            Assert.IsTrue(true);
        }
    }
}