using Server.GameService.Core;
using Server.LobbyService;
using Server.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.GameService
{
    public class GameManager
    {
        #region Private Fields

        private readonly GameDeck _deck;
        private readonly GameNotifier _notifier;
        private readonly GameTurnTimer _turnTimer;
        private readonly object _gameLock = new object();

        private readonly List<LobbyClient> _players;
        private readonly Dictionary<string, int> _scores;
        private readonly GameSettings _settings;
        private readonly Dictionary<string, HashSet<string>> _kickVotes = new Dictionary<string, HashSet<string>>();

        private int _currentPlayerIndex;
        private GameDeck.GameCard _firstFlippedCard;
        private bool _isProcessingMismatch;

        private readonly ILoggerManager _logger;

        #endregion

        public bool IsGameInProgress { get; private set; }

        public event Action<string, Dictionary<string, int>> GameEnded;

        public GameManager(List<LobbyClient> players, GameSettings settings, ILoggerManager logger)
        {
            _players = players;
            _settings = SanitizeSettings(settings);
            _scores = players.ToDictionary(p => p.Id, p => 0);
            _logger = logger;

            _deck = new GameDeck(_settings.CardCount);
            _notifier = new GameNotifier(_players, _logger);

            _turnTimer = new GameTurnTimer(_settings.TurnTimeSeconds, OnTurnTimeout);
        }

        public void StartGame()
        {
            lock (_gameLock)
            {
                if (IsGameInProgress)
                {
                    return;
                }

                IsGameInProgress = true;
                _isProcessingMismatch = false;
                _firstFlippedCard = null;
                _currentPlayerIndex = new Random().Next(_players.Count);
                _currentPlayerIndex = (_currentPlayerIndex - 1 + _players.Count) % _players.Count;
            }

            _notifier.NotifyGameStarted(_deck.GetBoardInfo());
            StartNewTurn(samePlayer: false);
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
                if (!CanFlipCard(playerId, cardIndex, out card2))
                {
                    return;
                }

                imageToSend = card2.Info.ImageIdentifier;

                if (_firstFlippedCard == null)
                {
                    _firstFlippedCard = card2;
                }
                else
                {
                    isSecondCard = true;
                    card1 = _firstFlippedCard;
                    _firstFlippedCard = null;
                    _turnTimer.Pause();

                    if (card1.Info.CardId == card2.Info.CardId)
                    {
                        isMatch = true;
                        card1.IsMatched = card2.IsMatched = true;
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

        private bool CanFlipCard(string playerId, int cardIndex, out GameDeck.GameCard card)
        {
            card = _deck.GetCard(cardIndex);

            if (!IsGameInProgress || _isProcessingMismatch)
            {
                return false;
            }

            if (_players[_currentPlayerIndex].Id != playerId)
            {
                return false;
            }

            if (card == null || card.IsMatched)
            {
                return false;
            }

            if (_firstFlippedCard != null && _firstFlippedCard.Index == cardIndex)
            {
                return false;
            }

            return true;
        }

        private void HandleMatch(string playerId, GameDeck.GameCard c1, GameDeck.GameCard c2)
        {
            int score = 0;
            string playerName = "";
            bool isGameOver = false;

            lock (_gameLock)
            {
                if (_scores.ContainsKey(playerId))
                {
                    score = _scores[playerId];
                }

                var playerObj = _players.FirstOrDefault(p => p.Id == playerId);

                if (playerObj != null)
                {
                    playerName = playerObj.Name;
                }

                isGameOver = _deck.IsAllMatched();

                if (isGameOver)
                {
                    IsGameInProgress = false;
                }
            }

            _notifier.NotifyMatch(c1.Index, c2.Index, playerName, score);

            if (isGameOver)
            {
                EndGame();
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

                StartNewTurn(samePlayer: false);
            });
        }

        private void OnTurnTimeout()
        {
            bool shouldAdvance = false;

            lock (_gameLock)
            {
                if (!_isProcessingMismatch && IsGameInProgress)
                {
                    shouldAdvance = true;

                    if (_firstFlippedCard != null)
                    {
                        var c = _firstFlippedCard;
                        _firstFlippedCard = null;
                        _notifier.NotifyHideCards(c.Index, c.Index);
                    }
                }
            }

            if (shouldAdvance)
            {
                StartNewTurn(samePlayer: false);
            }
        }

        private void StartNewTurn(bool samePlayer = false)
        {
            string name;

            lock (_gameLock)
            {
                if (!IsGameInProgress)
                {
                    return;
                }

                if (!samePlayer)
                {
                    _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
                    _turnTimer.Restart(_settings.TurnTimeSeconds);
                }
                else
                {
                    _turnTimer.Resume();
                }

                if (_currentPlayerIndex >= _players.Count) _currentPlayerIndex = 0;
                name = _players[_currentPlayerIndex].Name;
            }
            _notifier.NotifyTurnChange(name, _settings.TurnTimeSeconds);
        }

        private static GameSettings SanitizeSettings(GameSettings s)
        {
            if (s.CardCount < 16 || s.CardCount % 2 != 0)
            {
                s.CardCount = 16;
            }

            if (s.CardCount > 40)
            {
                s.CardCount = 40;
            }

            if (s.TurnTimeSeconds < 5)
            {
                s.TurnTimeSeconds = 5;
            }
            return s;
        }

        public void HandleVoteKick(string voterId, string targetId)
        {
            lock (_gameLock)
            {
                if (!IsGameInProgress)
                {
                    return;
                }

                if (voterId == targetId)
                {
                    return;
                }

                if (!_players.Any(p => p.Id == voterId) || !_players.Any(p => p.Id == targetId))
                {
                    return;
                }

                if (!_kickVotes.ContainsKey(targetId))
                {
                    _kickVotes[targetId] = new HashSet<string>();
                }

                if (_kickVotes[targetId].Add(voterId))
                {
                    var voterName = _players.First(p => p.Id == voterId).Name;
                    var targetName = _players.First(p => p.Id == targetId).Name;
                    int required = GetRequiredVotes();

                    _notifier.NotifyChatMessage("System", $"{voterName} voted to kick {targetName}. ({_kickVotes[targetId].Count}/{GetRequiredVotes()})", true);

                    if (_kickVotes[targetId].Count >= required)
                    {
                        KickPlayer(targetId);
                    }
                }
            }
        }

        private int GetRequiredVotes()
        {
            return (_players.Count / 2) + 1;
        }

        private void KickPlayer(string playerId)
        {
            int playerIndex = _players.FindIndex(p => p.Id == playerId);
            if (playerIndex == -1)
            {
                return;
            }

            var playerToRemove = _players[playerIndex];
            bool wasHisTurn = (playerIndex == _currentPlayerIndex);

            _notifier.NotifyChatMessage("System", $"{playerToRemove.Name} has been kicked by majority vote.", true);
            _notifier.NotifyPlayerLeft(playerToRemove.Name);

            if (playerIndex < _currentPlayerIndex)
            {
                _currentPlayerIndex--;
            }

            _players.RemoveAt(playerIndex);
            _kickVotes.Remove(playerId);

            foreach (var key in _kickVotes.Keys.ToList())
            {
                _kickVotes[key].Remove(playerId);
            }

            if (_players.Count < 2)
            {
                EndGame();
            }
            else if (wasHisTurn)
            {
                if (_currentPlayerIndex >= _players.Count)
                {
                    _currentPlayerIndex = 0;
                }

                StartNewTurn(samePlayer: false);
            }
            else
            {
                if (_currentPlayerIndex >= _players.Count)
                {
                    _currentPlayerIndex = 0;
                }
            }
        }

        private void EndGame()
        {
            IsGameInProgress = false;
            _turnTimer.Dispose();

            string winnerId = null;
            string winnerName = "PlayGameMultiplayer_Label_Tie";

            lock (_gameLock)
            {
                if (_players.Count == 1)
                {
                    var survivor = _players[0];
                    winnerId = survivor.Id;
                    winnerName = survivor.Name;
                }
                else if (_scores.Count > 0)
                {
                    var maxScore = _scores.Values.Max();
                    var winners = _scores.Where(x => x.Value == maxScore).Select(x => x.Key).ToList();

                    if (winners.Count == 1)
                    {
                        winnerId = winners[0];
                        winnerName = _players.First(p => p.Id == winnerId).Name;
                    }
                }
            }

            _notifier.NotifyWinner(winnerName);
            GameEnded?.Invoke(winnerId, new Dictionary<string, int>(_scores));
        }
    }
}