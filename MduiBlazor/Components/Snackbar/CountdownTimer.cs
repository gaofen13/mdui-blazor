namespace MduiBlazor.Components.Snackbar
{
    internal class CountdownTimer : IDisposable
    {
        private readonly Timer _timer;
        private Action? _elapsedDelegate;

        internal CountdownTimer()
        {
            _timer = new Timer(DoWork);
        }

        internal CountdownTimer OnElapsed(Action elapsedDelegate)
        {
            _elapsedDelegate = elapsedDelegate;
            return this;
        }

        internal void Start(int countdownTime)
        {
            _timer.Change(countdownTime, Timeout.Infinite);
        }

        internal void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void DoWork(object? state)
        {
            _elapsedDelegate?.Invoke();
        }

        public void Dispose()
        {
            Stop();
            _timer.Dispose();
        }
    }
}
