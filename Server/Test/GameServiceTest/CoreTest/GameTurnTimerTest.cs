using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.GameService.Core;
using System.Threading;

namespace Test.GameServiceTest.CoreTest
{
    [TestClass]
    public class GameTurnTimerTest
    {
        [TestMethod]
        public void Restart_StartsTimer_AndCallbackFires()
        {
            var signal = new ManualResetEvent(false);

            var timer = new GameTurnTimer(1, () =>
            {
                signal.Set();
            });

            timer.Restart();

            bool received = signal.WaitOne(2000);

            timer.Dispose();
            Assert.IsTrue(received, "Timer callback wasn't executed after calling Restart().");
        }

        [TestMethod]
        public void Stop_PreventsCallback()
        {
            bool called = false;
            var timer = new GameTurnTimer(1, () => called = true);

            timer.Restart();
            timer.Stop();

            Thread.Sleep(1500);

            Assert.IsFalse(called, "Callback should not be executed if timer was stopped.");
            timer.Dispose();
        }

        [TestMethod]
        public void Dispose_DoesNotThrow()
        {
            var timer = new GameTurnTimer(1, () => { });
            try
            {
                timer.Dispose();
            }
            catch
            {
                Assert.Fail("Dispose should not throw an exception.");
            }
            Assert.IsTrue(true);
        }
    }
}