using System.Linq;

namespace Server.Validator
{
    public interface IGameLobbyServiceValidator
    {
        bool IsValidGameCode(string gameCode);
        bool IsValidGuestName(string guestName);
        bool IsValidChatMessage(string message);
        bool CanJoinLobby(int currentClientCount);
        bool IsValidCardIndex(int index, int totalCards);
    }

    public class GameLobbyServiceValidator : IGameLobbyServiceValidator
    {
        private const int MAX_LOBBY_SIZE = 4;
        private const int MAX_CHAT_LENGTH = 500;
        private const int MAX_NAME_LENGTH = 30;
        private const int CODE_LENGTH = 6;
        public bool CanJoinLobby(int currentClientCount)
        {
            return currentClientCount < MAX_LOBBY_SIZE;
        }

        public bool IsValidCardIndex(int index, int totalCards)
        {
            return index >= 0 && index < totalCards;
        }

        public bool IsValidChatMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }
            return message.Length <= MAX_CHAT_LENGTH;
        }

        public bool IsValidGameCode(string gameCode)
        {
            return !string.IsNullOrEmpty(gameCode) &&
                   gameCode.Length == CODE_LENGTH &&
                   gameCode.All(char.IsDigit);
        }

        public bool IsValidGuestName(string guestName)
        {
            if (string.IsNullOrWhiteSpace(guestName))
            {
                return false;
            }
            var trimmed = guestName.Trim();
            return trimmed.Length > 0 && trimmed.Length <= MAX_NAME_LENGTH;
        }
    }
}
