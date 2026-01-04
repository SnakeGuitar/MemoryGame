using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.GameService;

namespace Test.GameServiceTest
{
    [TestClass]
    public class GameModelsTest
    {
        [TestMethod]
        public void GameSettings_CanSetAndGetCardCount()
        {
            var settings = new GameSettings();
            settings.CardCount = 20;
            Assert.AreEqual(20, settings.CardCount);
        }

        [TestMethod]
        public void GameSettings_CanSetAndGetTurnTime()
        {
            var settings = new GameSettings();
            settings.TurnTimeSeconds = 30;
            Assert.AreEqual(30, settings.TurnTimeSeconds);
        }

        [TestMethod]
        public void CardInfo_CanSetAndGetCardId()
        {
            var info = new CardInfo();
            info.CardId = 5;
            Assert.AreEqual(5, info.CardId);
        }

        [TestMethod]
        public void CardInfo_CanSetAndGetImageIdentifier()
        {
            var info = new CardInfo();
            info.ImageIdentifier = "cat.png";
            Assert.AreEqual("cat.png", info.ImageIdentifier);
        }
    }
}