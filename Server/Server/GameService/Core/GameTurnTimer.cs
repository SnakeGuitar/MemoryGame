using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Server.GameService.Core
{
    internal class GameTurnTimer : IDisposable
    {
        private readonly Timer _timer;
        private readonly Action _onElapsed;

        public GameTurnTimer(int seconds, Action onElapsed)
        {
            _onElapsed = onElapsed;
            _timer = new Timer(seconds * 1000)
            {
                AutoReset = false
            };
            _timer.Elapsed += (s, e) => _onElapsed?.Invoke();
        }

        public void Restart()
        {
            try
            {
                _timer.Stop();
                _timer.Start();
            }
            catch
            {
            }
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
