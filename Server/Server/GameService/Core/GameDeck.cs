using log4net.Repository;
using Server.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.GameService.Core
{
    public class GameDeck
    {
        private readonly List<GameCard> _cards;
        private readonly Random _random;
        private readonly Shared.ILoggerManager _logger;

        public GameDeck(int cardCount, ILoggerManager logger)
        {
            _cards = GenerateDeck(cardCount);
            _random = new Random();
            _logger = logger;
        }

        public GameDeck(int cardCount) : this(
            cardCount, 
            new Logger(typeof(GameDeck)))
        {
        }

        public List<CardInfo> GetBoardInfo()
        {
            _logger.LogInfo("Retrieving board information.");
            return _cards.Select(c => c.Info).ToList();
        }

        public GameCard GetCard(int index)
        {
            _logger.LogInfo("Retrieving card.");
            return _cards.FirstOrDefault(c => c.Index == index);
        }

        public bool IsAllMatched()
        {
            _logger.LogInfo("Checking if all cards are matched.");
            return _cards.All(c => c.IsMatched);
        }

        private List<GameCard> GenerateDeck(int count)
        {
            var cardInfos = new List<CardInfo>();
            int pairCount = count / 2;

            for (int i = 0; i < pairCount; i++)
            {
                var info = new CardInfo
                {
                    CardId = i,
                    ImageIdentifier = $"card_{i}"
                };
                cardInfos.Add(info);
                cardInfos.Add(info);
            }

            _logger.LogInfo("Generated deck.");
            return cardInfos
                .OrderBy(x => _random.Next())
                .Select((info, index) => new GameCard { Index = index, Info = info, IsMatched = false })
                .ToList();
        }

        public class GameCard
        {
            public int Index { get; set; }
            public CardInfo Info { get; set; }
            public bool IsMatched { get; set; }
        }
    }
}
