namespace MduiBlazor.Components.Snackbar
{
    internal class CountdownTimer : IDisposable
    {
        private readonly PeriodicTimer _timer;
        private readonly CancellationToken _cancellationToken;
        private Action? _elapsedDelegate;

        internal CountdownTimer(int timeout, CancellationToken cancellationToken = default)
        {
            _timer = new PeriodicTimer(TimeSpan.FromMilliseconds(timeout));
            _cancellationToken = cancellationToken;
        }

        internal CountdownTimer OnElapsed(Action elapsedDelegate)
        {
            _elapsedDelegate = elapsedDelegate;
            return this;
        }

        internal async Task StartAsync()
        {
            await DoWorkAsync();
        }

        private async Task DoWorkAsync()
        {
            while (await _timer.WaitForNextTickAsync(_cancellationToken) && !_cancellationToken.IsCancellationRequested)
            {
                _elapsedDelegate?.Invoke();
            }
        }

        public void Dispose() => _timer.Dispose();
    }
}
