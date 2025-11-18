using Server.LobbyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Server.GameService
{
    public class GameManager
    {
        // GAME STATE
        private readonly List<LobbyClient> _players;
        private string _currentPlayerTurnId;
        private readonly Dictionary<string, int> _scores = new Dictionary<string, int>();
        private List<GameCard> _gameDeck;
        private GameCard _firstFlippedCard;
        private readonly GameSettings _settings;
        private readonly Random _random = new Random();

        // CONCURRENCY AND TIMING
        private bool _isCheckingPair = false;
        private readonly object _gameLock = new object();
        private Timer _turnTimer;

        public bool IsGameInProgress { get; private set; } = false;

        public GameManager(List<LobbyClient> players, GameSettings settings)
        {
            _players = players;
            _settings = settings;
            if (_settings.CardCount < 16 || settings.CardCount > 40 || _settings.CardCount % 2 != 0)
            {
                _settings.CardCount = 16; // Default to 16 if invalid
            }
            if (_settings.TurnTimeSeconds < 5 || _settings.TurnTimeSeconds > 40)
            {
                _settings.TurnTimeSeconds = 20; // Default to 15 seconds if invalid
            }
        }

        public void StartGame()
        {
            lock (_gameLock)
            {
                if (IsGameInProgress)
                {
                    return;
                }

                var cardInfos = new List<CardInfo>();
                int pairCount = _settings.CardCount / 2;

                for (int i = 0; i < pairCount; i++)
                {
                    var info = new CardInfo
                    {
                        CardId = i,
                        ImageIdentifier = $"card{i}"
                    };
                    cardInfos.Add(info);
                    cardInfos.Add(info); // Add a pair
                }

                var shuffledDeck = cardInfos.OrderBy(x => _random.Next()).ToList();

                _gameDeck = shuffledDeck.Select((info, index) => new GameCard
                {
                    Index = index,
                    Info = info,
                    IsMatched = false
                }).ToList();

                _scores.Clear();
                foreach (var player in _players)
                {
                    _scores[player.Id] = 0;
                }
                IsGameInProgress = true;
                _firstFlippedCard = null;
                _isCheckingPair = false;

                _turnTimer = new Timer(_settings.TurnTimeSeconds * 1000);
                _turnTimer.Elapsed += OnTurnTimerElapsed;
                _turnTimer.AutoReset = false;

                _currentPlayerTurnId = _players[_random.Next(_players.Count)].Id;

                BroadcastToPlayers(client =>
                {
                    client.Callback.GameStarted(shuffledDeck);
                    foreach (var p in _players)
                    {
                        client.Callback.UpdateScore(p.Name, 0);
                    }
                });

                StartNewTurn();
            }
        }

        private void OnTurnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (_gameLock)
            {
                if (!_isCheckingPair && IsGameInProgress)
                {
                    if (_firstFlippedCard != null)
                    {
                        BroadcastToPlayers(c => c.Callback.HideCards(_firstFlippedCard.Index, _firstFlippedCard.Index));
                        _firstFlippedCard = null;
                    }

                    PassTurnToNextPlayer();
                }
            }
        }

        private void StartNewTurn()
        {
            var currentPlayer = _players.First(p => p.Id == _currentPlayerTurnId);

            BroadcastToPlayers(client =>
            {
                client.Callback.UpdateTurn(currentPlayer.Name, _settings.TurnTimeSeconds);
            });

            _turnTimer.Stop();
            _turnTimer.Start();
        }

        private void PassTurnToNextPlayer()
        {
            _turnTimer.Stop();
            _isCheckingPair = false;
            _firstFlippedCard = null;

            int currentIndex = _players.FindIndex(p => p.Id == _currentPlayerTurnId);

            int nextIndex = (currentIndex + 1) % _players.Count;

            _currentPlayerTurnId = _players[nextIndex].Id;

            StartNewTurn();
        }

        public void HandleFlipCard(string playerId, int cardIndex)
        {
            lock (_gameLock)
            {
                if (_isCheckingPair || !IsGameInProgress || playerId != _currentPlayerTurnId)
                {
                    return;
                }

                var card = _gameDeck.FirstOrDefault(c => c.Index == cardIndex);

                if (card == null || card.IsMatched || _firstFlippedCard != null && card.Index == _firstFlippedCard.Index)
                {
                    return;
                }

                BroadcastToPlayers(client =>
                {
                    client.Callback.ShowCard(card.Index, card.Info.ImageIdentifier);
                });

                if (_firstFlippedCard == null)
                {
                    _firstFlippedCard = card;
                }
                else
                {
                    _isCheckingPair = true;
                    var secondCard = card;
                    var firstCard = _firstFlippedCard;
                    _firstFlippedCard = null;

                    _turnTimer.Stop();

                    if (firstCard.Info.CardId ==secondCard.Info.CardId)
                    {
                        firstCard.IsMatched = true;
                        secondCard.IsMatched = true;
                        _scores[playerId]++;

                        BroadcastToPlayers(client =>
                        {
                            client.Callback.SetCardsAsMatched(firstCard.Index, secondCard.Index);
                            client.Callback.UpdateScore(_players.First(p => p.Id == playerId).Name, _scores[playerId]);
                        });

                        if (_gameDeck.All(c => c.IsMatched))
                        {
                            IsGameInProgress = false;
                            var winnerScore = _scores.Values.Max();
                            var winnerId = _scores.First(kv => kv.Value == winnerScore).Key;
                            var winnerName = _players.First(p => p.Id == winnerId).Name;

                            BroadcastToPlayers(client =>
                            {
                                client.Callback.GameFinished(winnerName);
                            });
                        }
                        else
                        {
                            _isCheckingPair = false;
                            StartNewTurn();
                        }
                    }
                    else
                    {
                        Task.Delay(1500).ContinueWith(_ =>
                        {
                            lock (_gameLock)
                            {
                                BroadcastToPlayers(client =>
                                {
                                    client.Callback.HideCards(firstCard.Index, secondCard.Index);
                                });
                                PassTurnToNextPlayer();
                            }
                        });
                    }
                }
            }

        }

        private void BroadcastToPlayers(Action<LobbyClient> action)
        {
            Parallel.ForEach(_players, client =>
            {
                try
                {
                    action(client);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Sending error to {client.Name}: {ex.Message}");
                }
            });
        }

        private class GameCard
        {
            public int Index { get; set; }
            public CardInfo Info { get; set; }
            public bool IsMatched { get; set; }
        }
    }
}
