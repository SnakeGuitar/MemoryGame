using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Server.GameService.Core
{
    public class GameTurnTimer : IDisposable
    {
        private readonly Timer _timer;
        private readonly Action _onElapsed;
        private double _remainingMs;
        private DateTime _lastStartTime;
        private bool _disposed = false;

        public GameTurnTimer(int seconds, Action onElapsed)
        {
            _onElapsed = onElapsed;
            _remainingMs = seconds * 1000;
            _timer = new Timer { AutoReset = false };
            _timer.Elapsed += (s, e) => _onElapsed?.Invoke();
        }

        public void Restart(int seconds)
        {
            if (_disposed)
            {
                return;
            }
            StartInternal(seconds * 1000);
        }

        public void Pause()
        {
            if (_disposed)
            {
                return;
            }

            _timer.Stop();
            var elapsed = (DateTime.UtcNow - _lastStartTime).TotalMilliseconds;
            _remainingMs -= elapsed;
        }

        public void Resume()
        {
            if (_disposed)
            {
                return;
            }

            if (_remainingMs <= 100)
            {
                _remainingMs = 100;
            }
            StartInternal(_remainingMs);
        }

        private void StartInternal(double duration)
        {
            _timer.Stop();
            _remainingMs = duration;
            _timer.Interval = duration;
            _lastStartTime = DateTime.UtcNow;
            _timer.Start();
        }

        public void Stop()
        {
            if (!_disposed)
            {
                _timer.Stop();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _timer.Stop();
                _timer.Dispose();
                _disposed = true;
            }
        }
    }
}