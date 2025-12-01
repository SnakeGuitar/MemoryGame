using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    internal class DifficultyPresets
    {
        public static GameConfiguration Easy => new GameConfiguration
        {
            NumberOfCards = 16,
            TimeLimitSeconds = 60,
            NumberRows = 4,
            NumberColumns = 4,
            DifficultyLevel = "Easy"
        };

        public static GameConfiguration Normal => new GameConfiguration
        {
            NumberOfCards = 24,
            TimeLimitSeconds = 90,
            NumberRows = 4,
            NumberColumns = 6,
            DifficultyLevel = "Normal"
        };

        public static GameConfiguration Hard => new GameConfiguration
        {
            NumberOfCards = 30,
            TimeLimitSeconds = 120,
            NumberRows = 5,
            NumberColumns = 6,
            DifficultyLevel = "Hard"
        };

        public static (int Rows, int Columns) CalculateLayout(int numberOfCards)
        {
            switch (numberOfCards)
            {
                case 40:
                    return (5, 8);

                case 30:
                    return (5, 6);

                case 24:
                    return (4, 6);

                case 16:
                default:
                    return (4, 4);
            }
        }
    }
}
