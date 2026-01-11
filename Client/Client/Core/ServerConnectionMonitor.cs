using System;
using System.Windows.Threading;

namespace Client.Core
{
    public class ServerConnectionMonitor
    {
        private DispatcherTimer _timer;
        private readonly Func<bool> _checkConnectionStatus;

        public event Action ConnectionLost;

        public ServerConnectionMonitor(Func<bool> checkCallback, int intervalSeconds = 3)
        {
            _checkConnectionStatus = checkCallback ?? throw new ArgumentNullException(nameof(checkCallback));

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(intervalSeconds);
            _timer.Tick += Timer_Tick;
        }

        public void Start()
        {
            if (!_timer.IsEnabled)
            {
                _timer.Start();
            }
        }

        public void Stop()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            bool isAlive = _checkConnectionStatus.Invoke();

            if (!isAlive)
            {
                Stop();
                ConnectionLost?.Invoke();
            }
        }
    }
}
