using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.GameService.Core
{
    public class GameDeck
    {
        private readonly List<GameCard> _cards;
        private readonly Random _random = new Random();

        public GameDeck(int cardCount)
        {
            _cards = GenerateDeck(cardCount);
        }

        public List<CardInfo> GetBoardInfo()
        {
            return _cards.Select(c => c.Info).ToList();
        }

        public GameCard GetCard(int index)
        {
            return _cards.FirstOrDefault(c => c.Index == index);
        }

        public bool IsAllMatched()
        {
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
