using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    public class GameConfiguration
    {
        public int NumberOfCards { get; set; }
        public int TimeLimitSeconds { get; set; }
        public string DifficultyLevel { get; set; }
        public int NumberRows { get; set; }
        public int NumberColumns { get; set; }

        public GameConfiguration() { }

        public GameConfiguration(int cards, int seconds, int rows, int columns, string difficulty)
        {
            NumberOfCards = cards;
            TimeLimitSeconds = seconds;
            NumberRows = rows;
            NumberColumns = columns;
            DifficultyLevel = difficulty;

        }
    }
}
