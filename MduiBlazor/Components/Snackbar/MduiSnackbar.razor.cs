using MduiBlazor.Components.Snackbar;
using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiSnackbar : MduiComponentBase, IDisposable
    {
        private SnackbarOptions _options = new();

        private CountdownTimer? _countdownTimer;

        protected string Classname =>
          new ClassBuilder("mdui-snackbar")
            .AddClass($"mdui-snackbar-{_options.HorizontalPosition.ToDescriptionString()}-{_options.VerticalPosition.ToDescriptionString()}")
            .AddClass(Class)
            .Build();

        private string ContentClassname =>
            new ClassBuilder("mdui-snackbar-content")
            .AddClass($"mdui-color-{Color}", !string.IsNullOrWhiteSpace(Color))
            .Build();

        [CascadingParameter]
        private MduiSnackbarProvider? SnackbarProvider { get; set; }

        [Parameter]
        public SnackbarOptions? Options { get; set; }

        [Parameter]
        public string? Color { get; set; }

        [Parameter]
        public Guid SnackbarId { get; set; }

        protected override void OnInitialized()
        {
            if (Options is not null)
            {
                _options = Options;
            }
            else if (SnackbarProvider is not null)
            {
                _options = SnackbarProvider.Options;
            }

            _countdownTimer = new CountdownTimer()
                .OnElapsed(Close);

            _countdownTimer.Start(_options.Timeout);
        }

        public void Close() => SnackbarProvider?.CloseSnackbar(SnackbarId);

        void IDisposable.Dispose()
        {
            _countdownTimer?.Dispose();
            _countdownTimer = null;
            GC.SuppressFinalize(this);
        }
    }
}