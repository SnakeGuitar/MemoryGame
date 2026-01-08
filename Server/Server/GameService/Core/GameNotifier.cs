using Server.LobbyService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.GameService.Core
{
    public class GameNotifier
    {
        private readonly List<LobbyClient> _players;

        public GameNotifier(List<LobbyClient> players)
        {
            _players = players;
        }

        public void NotifyGameStarted(List<CardInfo> board)
        {
            Broadcast(c => c.Callback.GameStarted(board));
        }

        public void NotifyTurnChange(string playerName, int seconds)
        {
            Broadcast(c => c.Callback.UpdateTurn(playerName, seconds));
        }

        public void NotifyShowCard(int index, string imageId)
        {
            Broadcast(c => c.Callback.ShowCard(index, imageId));
        }

        public void NotifyMatch(int idx1, int idx2, string playerName, int score)
        {
            Broadcast(c =>
            {
                c.Callback.SetCardsAsMatched(idx1, idx2);
                c.Callback.UpdateScore(playerName, score);
            });
        }

        public void NotifyHideCards(int idx1, int idx2)
        {
            Broadcast(c => c.Callback.HideCards(idx1, idx2));
        }

        public void NotifyWinner(string winnerName)
        {
            Broadcast(c => c.Callback.GameFinished(winnerName));
        }

        public void NotifyChatMessage(string sender, string message, bool isNotification)
        {
            Broadcast(c => c.Callback.ReceiveChatMessage(sender, message, isNotification));
        }

        public void NotifyPlayerLeft(string playerName)
        {
            Broadcast(c => c.Callback.PlayerLeft(playerName));
        }

        private void Broadcast(Action<LobbyClient> action)
        {
            foreach (var player in _players)
            {
                Task.Run(() =>
                {
                    try
                    {
                        action(player);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Game broadcast error to {player.Name}: {ex.Message}");
                    }
                });
            }
        }
    }
}