using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Client.Core
{
    public class ServerConnectionMonitor
    {
        private readonly DispatcherTimer _timer;
        private readonly Func<Task<bool>> _checkConnectionAsync;
        private bool _isCheckInProgress;

        public event Action ConnectionLost;

        public ServerConnectionMonitor(Func<Task<bool>> checkCallback, int intervalSeconds = 3)
        {
            _checkConnectionAsync = checkCallback ?? throw new ArgumentNullException(nameof(checkCallback));

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

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (_isCheckInProgress)
            {
                _isCheckInProgress = false;
            }

            try
            {

                bool isAlive = await _checkConnectionAsync();

                if (!isAlive)
                {
                    Stop();
                    ConnectionLost?.Invoke();
                }
            }
            catch 
            {
                Stop();
                ConnectionLost?.Invoke();
            }
            finally
            {
                _isCheckInProgress = false;
            }
        }
    }
}