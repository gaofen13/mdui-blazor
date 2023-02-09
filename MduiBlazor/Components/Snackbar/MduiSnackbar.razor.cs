
using MduiBlazor.Components.Snackbar;
using MduiBlazor.Extensions;
using MduiBlazor.Utilities;
using Microsoft.AspNetCore.Components;

namespace MduiBlazor
{
    public partial class MduiSnackbar : MduiComponentBase
    {
        private SnackbarOptions _options = new SnackbarOptions();

        private CountdownTimer? _countdownTimer;

        protected string Classname =>
          new ClassBuilder("mdui-snackbar")
            .AddClass($"mdui-snackbar-{_options.PositionX.ToDescriptionString()}-{_options.PositionY.ToDescriptionString()}")
            .AddClass(Class)
            .Build();

        [CascadingParameter]
        private MduiSnackbarProvider? SnackbarProvider { get; set; }

        [Parameter]
        public SnackbarOptions? Options { get; set; }

        [Parameter]
        public Guid SnackbarId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Options is not null)
            {
                _options = Options;
            }
            else if (SnackbarProvider is not null)
            {
                _options = SnackbarProvider.Options;
            }

            _countdownTimer = new CountdownTimer(_options.TimeOut)
                .OnElapsed(Close);

            await _countdownTimer.StartAsync();
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