using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModels
{
    public class Card : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int PairId { get; set; }
        private readonly string _frontimagePath;
        private readonly string _backimagePath;
        private bool isFlipped;
        private bool isMatched;

        public Card(int id, int pairId, string frontImage)
        {
            Id = id;
            PairId = pairId;
            _frontimagePath = frontImage;
            isFlipped = false;
            isMatched = false;
        }

        public bool IsFlipped
        {
            get { return isFlipped; }
            set
            {
                if (isFlipped != value)
                {
                    isFlipped = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DisplayImage));
                }
            }
        }

        public bool IsMatched
        {
            get { return isMatched; }
            set
            {
                if (isMatched != value)
                {
                    isMatched = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsMatched));
                }
            }
        }

        public string DisplayImage
        {
            get
            {
                if (IsMatched || IsFlipped)
                {
                    return _frontimagePath;
                }
                return _backimagePath;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
