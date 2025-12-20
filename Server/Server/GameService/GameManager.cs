using Server.GameService.Core;
using Server.LobbyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameService
{
    public class GameManager
    {
        private readonly GameDeck _deck;
        private readonly GameNotifier _notifier;
        private readonly GameTurnTimer _turnTimer;
        private readonly object _gameLock = new object();

        private readonly List<LobbyClient> _players;
        private readonly Dictionary<string, int> _scores;
        private readonly GameSettings _settings;

        private int _currentPlayerIndex;
        private GameDeck.GameCard _firstFlippedCard;
        private bool _isProcessingMismatch;

        public bool IsGameInProgress { get; private set; }

        public event Action<string, Dictionary<string, int>> GameEnded;

        public GameManager(List<LobbyClient> players, GameSettings settings)
        {
            _players = players;
            _settings = SanitizeSettings(settings);
            _scores = players.ToDictionary(p => p.Id, p => 0);

            _deck = new GameDeck(_settings.CardCount);
            _notifier = new GameNotifier(_players);

            _turnTimer = new GameTurnTimer(_settings.TurnTimeSeconds, OnTurnTimeout);
        }

        public void StartGame()
        {
            lock (_gameLock)
            {
                if (IsGameInProgress) return;
                IsGameInProgress = true;

                _currentPlayerIndex = new Random().Next(_players.Count);
            }

            _notifier.NotifyGameStarted(_deck.GetBoardInfo());
            StartNewTurn();
        }

        public void HandleFlipCard(string playerId, int cardIndex)
        {
            string imageToSend = null;
            bool isMatch = false;
            bool isSecondCard = false;
            GameDeck.GameCard card1 = null;
            GameDeck.GameCard card2 = null;

            lock (_gameLock)
            {
                if (!IsGameInProgress || _isProcessingMismatch) return;

                var currentPlayer = _players[_currentPlayerIndex];
                if (currentPlayer.Id != playerId) return;

                var card = _deck.GetCard(cardIndex);
                if (card == null || card.IsMatched) return;
                if (_firstFlippedCard != null && _firstFlippedCard.Index == cardIndex) return;

                imageToSend = card.Info.ImageIdentifier;

                if (_firstFlippedCard == null)
                {
                    _firstFlippedCard = card;
                }
                else
                {
                    isSecondCard = true;
                    card1 = _firstFlippedCard;
                    card2 = card;
                    _firstFlippedCard = null;
                    _turnTimer.Stop();

                    if (card1.Info.CardId == card2.Info.CardId)
                    {
                        isMatch = true;
                        card1.IsMatched = true;
                        card2.IsMatched = true;
                        _scores[playerId]++;
                    }
                    else
                    {
                        _isProcessingMismatch = true;
                    }
                }
            }

            if (imageToSend != null)
            {
                _notifier.NotifyShowCard(cardIndex, imageToSend);
            }

            if (isSecondCard)
            {
                if (isMatch)
                {
                    HandleMatch(playerId, card1, card2);
                }
                else
                {
                    HandleMismatch(card1, card2);
                }
            }
        }

        private void HandleMatch(string playerId, GameDeck.GameCard c1, GameDeck.GameCard c2)
        {
            int score = 0;
            string playerName = "";
            bool isGameOver = false;
            string winnerName = "";
            string winnerId = "";

            lock (_gameLock)
            {
                score = _scores[playerId];
                playerName = _players.First(p => p.Id == playerId).Name;
                isGameOver = _deck.IsAllMatched();

                if (isGameOver)
                {
                    var maxScore = _scores.Values.Max();
                    winnerId = _scores.First(x => x.Value == maxScore).Key;
                    winnerName = _players.First(p => p.Id == winnerId).Name;
                    IsGameInProgress = false;
                }
            }

            _notifier.NotifyMatch(c1.Index, c2.Index, playerName, score);

            if (isGameOver)
            {
                _notifier.NotifyWinner(winnerName);
                _turnTimer.Dispose();
                GameEnded?.Invoke(winnerId, new Dictionary<string, int>(_scores));
            }
            else
            {
                StartNewTurn(samePlayer: true);
            }
        }

        private void HandleMismatch(GameDeck.GameCard c1, GameDeck.GameCard c2)
        {
            Task.Delay(1500).ContinueWith(_ =>
            {
                _notifier.NotifyHideCards(c1.Index, c2.Index);

                lock (_gameLock)
                {
                    _isProcessingMismatch = false;
                }

                AdvanceTurn();
            });
        }

        private void OnTurnTimeout()
        {
            lock (_gameLock)
            {
                if (!_isProcessingMismatch && IsGameInProgress)
                {
                    if (_firstFlippedCard != null)
                    {
                        var c = _firstFlippedCard;
                        _firstFlippedCard = null;
                        _notifier.NotifyHideCards(c.Index, c.Index);
                    }
                }
            }
            AdvanceTurn();
        }

        private void AdvanceTurn()
        {
            lock (_gameLock)
            {
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            }
            StartNewTurn();
        }

        private void StartNewTurn(bool samePlayer = false)
        {
            string name;
            lock (_gameLock)
            {
                if (!IsGameInProgress) return;
                name = _players[_currentPlayerIndex].Name;
            }

            _notifier.NotifyTurnChange(name, _settings.TurnTimeSeconds);
            _turnTimer.Restart();
        }

        private GameSettings SanitizeSettings(GameSettings s)
        {
            if (s.CardCount < 16 || s.CardCount % 2 != 0) s.CardCount = 16;
            if (s.TurnTimeSeconds < 5) s.TurnTimeSeconds = 5;
            return s;
        }
    }
}