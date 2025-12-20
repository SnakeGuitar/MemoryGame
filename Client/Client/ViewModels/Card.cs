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
        private string _frontimagePath;
        private readonly string _backimagePath = "/Client;component/Resources/Images/Cards/Backs/default.png";
        private bool isFlipped;
        private bool isMatched;

        public Card(int id, int pairId, string frontImage)
        {
            Id = id;
            PairId = pairId;
            _frontimagePath = frontImage;
            IsFlipped = false;
            IsMatched = false;
        }

        public void SetFrontImage(string imagePath)
        {
            _frontimagePath = imagePath;
            OnPropertyChanged(nameof(DisplayImage));
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
                    OnPropertyChanged(nameof(DisplayImage));
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
