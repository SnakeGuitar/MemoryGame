using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace Client.Core
{
    public class MusicManager
    {
        private static MusicManager _instance;
        public static MusicManager Instance => _instance ?? (_instance = new MusicManager());

        private MediaPlayer _mediaPlayer;
        private List<string> _playlist;
        private int _currentIndex;
        private bool _isShuffled;
        private Random _random;

        public event Action<int> TrackChanged;

        public double Volume
        {
            get => _mediaPlayer.Volume;
            set => _mediaPlayer.Volume = value;
        }

        public int CurrentIndex => _currentIndex;

        private MusicManager()
        {
            _mediaPlayer = new MediaPlayer();
            _playlist = new List<string>();
            _random = new Random();
            _currentIndex = -1;

            _mediaPlayer.MediaEnded += (s, e) => PlayNext();
        }

        public void Initialize(List<string> filePaths, bool shuffle = true)
        {
            if (filePaths == null || filePaths.Count == 0)
            {
                return;
            }

            _playlist = new List<string>(filePaths);
            _isShuffled = shuffle;

            if (_isShuffled)
            {
                ShufflePlaylist();
            }
            PlayTrackIndex(0);
        }

        public List<string> GetTrackList()
        {
            return _playlist.Select(path => Path.GetFileNameWithoutExtension(path)).ToList();
        }

        public void PlayTrackIndex(int index)
        {
            if (index >= 0 && index < _playlist.Count)
            {
                _currentIndex = index;
                PlayTrack(_playlist[_currentIndex]);
                TrackChanged?.Invoke(_currentIndex);
            }
        }

        private void ShufflePlaylist()
        {
            int n = _playlist.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                string value = _playlist[k];
                _playlist[k] = _playlist[n];
                _playlist[n] = value;
            }
        }

        public void PlayNext()
        {
            if (_playlist.Count == 0)
            {
                return;
            }

            int nextIndex = _currentIndex + 1;
            if (nextIndex >= _playlist.Count)
            {
                nextIndex = 0;
            }

            PlayTrackIndex(nextIndex);
        }

        private void PlayTrack(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    _mediaPlayer.Open(new Uri(filePath));
                    _mediaPlayer.Play();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[MusicManager] Error: {ex.Message}");
            }
        }
    }
}